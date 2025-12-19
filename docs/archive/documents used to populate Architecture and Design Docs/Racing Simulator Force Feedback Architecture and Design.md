Racing Simulator Force FeedBack Design

# Overview

This document is a complete design and step-by-step plan to build **Racing Sim FFB**: a Windows desktop application that reads real-time telemetry from racing simulators (iRacing, Assetto Corsa, Automobilista, etc.) and outputs high-frequency force feedback (FFB) to wheels (Simucube Pro, Fanatec, etc.). The project is organized for maintainability, testability, and high-performance real-time loops.

The first milestone is a **prototype shell** (mockup) — a minimal WPF app plus project skeleton that can built out to the full application.

**Target stack:** C# (.NET 10.0), WPF (MVVM), Clean Architecture, Test-Driven Development (TDD)

**Coding Standard:** IDesign C# Coding Standard Guidelines and Best Practices (v3.01)

**IDE:** Visual Studio 2026

**Solution and Project files:** Visual Studio 2026, target .net 10.0

**Assemblies**: Separate per layer for strong separation and testability.

# Goals

* Reliable telemetry ingestion (shared memory, UDP, SDKs)
* Deterministic FFB output with support for high update rates (360 Hz+)
* User-facing UI for tuning profiles and live telemetry visualization
* Testable codebase following Clean Architecture and DI
* Cross-adapter device abstraction for simulator driving wheels

Key Principles

* **TDD First:** Write failing tests for behavior before implementation. Keep tests small and fast.
* **Clean Architecture:** Core contains no framework references.
* **DI everywhere:** Constructor-inject dependencies; avoid service locators.
* **Dependency Rule:** Dependencies always point inward. The Infrastructure and Presentation layers can reference the Application and Domain layers, but the inner layers must know nothing about the outer layers.
* **Separation of Concerns:** Each layer has a specific, well-defined responsibility, making the system easier to maintain, test, and scale.
* **Testability:** Business logic in the Domain and Application layers can be tested in isolation from external infrastructure concerns.
* Real-time FFB loop runs in an isolated, high-priority service that consumes telemetry and emits commands to device adapte
* **Minimize allocations on hot paths:** Use Span<T>, ArrayPool<T>, struct DTOs where appropriate.
* **Real-time loop isolation:** Separate the high-priority FFB loop from UI thread — no blocking calls on that thread.
* **Safety & Robustness:** Use cancellation tokens, graceful shutdown, and telemetry watch-dog timers.

# High-level Architecture

Follow Clean Architecture (Onion/Hexagonal):

High level Assemblies

* Domain Layer
* Application Layer
* Adapters/Infrastructure Layer
* Presentation Layer

## Domain layer Components (Used by the Application Layer)

These components live in the **Domain Layer** and define the core business rules and data structures, but they are crucial for the **Application Layer** logic (the Use Cases).

### Entities

**Entities are objects that have an identity that persists over time, and their state changes often.**

#### Simulator

Represents a specific racing simulator (e.g., iRacing, Assetto Corsa), identified by a unique ID or name. Its state changes (e.g., connected/disconnected, running/paused), but it remains the same simulator entity.

#### Force Feedback Wheel

Represents the physical FFB wheel hardware (e.g., Simucube Pro, Fanatec), identified by a device ID. Its state might include calibration status, connection status, and current FFB strength settings.

#### Telemetry Feed

Represents the source connection to a specific simulator. It has a unique identity tied to the connection.**ID:** Simulator.iRacing; **State:** Connected, Connecting, Disconnected; **Behavior:** Connect(), Disconnect().

#### Car

Represents the vehicle in the simulation, with attributes like weight, suspension settings, and tire properties. It has a persistent identity within a session or the application's configuration.

**Driver**: Represents the user with an identity that transcends specific sessions or cars.

#### Driver Session

Represents a single, continuous driving session (e.g., a qualifying session or a race). Its identity is crucial. identified by a **ID:** Guid (Session ID); **State:** InProgress, Paused, Completed; **Behavior:** StartSession(), EndSession().

### Value Objects (Data Without Identity)

Value objects describe characteristics and have no conceptual identity; they are defined entirely by their attributes and are typically immutable. If two value objects have the same attributes, they are considered equal.

#### Telemetry Data Point

An immutable snapshot of data at a specific moment. Its value is the data it holds, not a unique ID. Containing values like:

##### TireLoad

**Represents t**he current force/load on a single tire. Defined by the Load (Newtons); Slip Ratio (unitless).

##### Wheel Angle

Represents the current steering angle (e.g., in degrees or radians), a simple value with specific invariants (e.g., within the range of the wheel's rotation limits).

##### Engine RPM

A value object representing engine rotational speed, possibly including max RPM limits for validation.

#### Force Feedback Vector

The actual high-frequency force feedback signal. Represents a 3D force vector (X, Y, Z components) to be applied to the wheel. Immutable force data.**Data:** Force (Newtons), Damper (N·s/m), Inertia (kg·m²). Two Force Vector objects with the same X, Y, and Z values are considered equal.

#### Pid Config

Configuration parameters for the FFB algorithm (e.g., Proportional, Integral, Derivative gains

### Interfaces (Gateways/Ports)

Interfaces in the Domain layer define contracts that the outer (Infrastructure) layers must implement. This adheres to the Dependency Rule, ensuring the Core remains independent of implementation details.

#### ISimulator Telemetry Gateway

Defines the contract for an adapter that can read real-time telemetry data streams from various simulators. The implementation in the Infrastructure layer would handle the specific UDP/shared memory protocols of iRacing, Assetto Corsa, etc.

#### IForce Feedback Port

Defines the contract for a gateway that sends FFB commands to the physical hardware. The Infrastructure implementation would use the specific SDKs or APIs for Simucube Pro, Fanatec, etc.

#### IConfiguration Repository

Defines methods for saving and retrieving user configurations (e.g., FFB strength preferences, supported simulators). The Infrastructure layer would implement this using file storage or a database.

#### ILogging Service

An interface for logging events, which might be implemented by a concrete logging framework (like log4net or Serilog) in the Infrastructure layer

### Domain Services (Business Logic that Doesn't Belong to a Single Entity)

These services contain business logic that involves multiple entities or value objects, or complex calculations that are not suitable for an Entity's method..

#### Force Feedback Calculation Service

A service that takes raw TelemetryDataPoint value objects and, using complex algorithms, calculates the appropriate ForceVector to send to the wheel. This logic is crucial to the core business of the application but doesn't belong to any single entity.

#### Telemetry Processing Service

Coordinates the retrieval of raw data via the ISimulatorTelemetryGateway, processes it, and passes it to the FFBCalculationService

#### Connection Management Service

**Manages the complex process of detecting running simulators and connecting to their telemetry streams, which involves operations spanning multiple system concerns beyond a single entity's scope.**

## Application Layer (Use Cases)

## The Application Layer is where the actual use cases (the application's features) are defined. They depend on the Domain Layer components above and the Interfaces.

### Real-Time Telemetry and Force Feedback

These are the high-frequency, continuous use cases essential for the application's core function.

#### Generate Force Feedback Command

This Use Case is the heart of your application's real-time functionality.

1. **Input:** Raw telemetry data **TelemetryData** (Value Object)
2. **Output:** Success/Failure, and the FFB command **FFBVector** (Value Object)

**Orchestration:**

1. ISimulatorGateway (input port) - Listens for and receives raw telemetry from the simulator.
2. FFBProfileRepository (Infrastructure interface/port) -- Loads the user's currently active ForceFeedbackProfile.
3. ForceFeedbackCalculator (Domain Service) - Calls the Domain Service to perform the complex force calculation.
4. IWheelHardwareOutput (output port) - Sends the final, calculated FFB command to the wheel hardware.

#### Stop Force Feedback Command

**Purpose:** Immediately sends a zero-force signal to the wheel (e.g., in case of a crash, connection loss, or user pause).

**Orchestration:**

1. Creates a zeroed FFBVector.
2. Sends the zeroed FFBVector to the **IWheelHardwareOutput**.
3. Calls the Disconnect() method on the **ISimulatorGateway** if needed.

### Configuration and Profile Management

These use cases handle the less frequent, user-driven interactions related to setup.

#### Get Available Simulator List Query

**Purpose:** Retrieves the list of simulators the application supports.

**Orchestration:**

1. Queries the **ISimulatorGateway implementations** in the Infrastructure layer (perhaps via a factory) to discover available connections.
2. Returns a simple list of simulator names and status (e.g., "iRacing - Ready," "Assetto Corsa - Not Installed").

#### Save Force Feed Back Profile Command

**Purpose:** Persists a user-defined set of FFB settings for a specific car/wheel combination.

**Orchestration:**

1. Receives input data for a new or updated ForceFeedbackProfile Entity.
2. Applies any necessary validation rules (Domain rules) to the Entity.
3. Passes the valid Entity to the **ITelemetryRepository** (Infrastructure Layer) for persistence.

#### Load Force Feed Back Profile Query

**Purpose:** Retrieves an existing FFB profile to apply to the active session.

**Orchestration:**

1. Receives the unique ID of the desired ForceFeedbackProfile.
2. Queries the **ITelemetryRepository** to retrieve the corresponding Entity.
3. Returns the ForceFeedbackProfile Entity or a corresponding Data Transfer Object (DTO) to the Presentation Layer.

### Data Logging and Analysis

These use cases handle saving, retrieving, and processing data for offline review.

#### Log Session Data Command

**Purpose:** To save all telemetry and application state data for an entire driving session.

**Orchestration:**

1. Receives a finalized DriverSession Entity containing all logged high-frequency data (a stream of CarMotion and other Value Objects).
2. Passes the DriverSession Entity to the **ITelemetryRepository** for storage (Infrastructure Layer).

#### Analyze Lap Times Query

**Purpose:** Calculates lap times and sector times for a given session log.

**Orchestration:**

1. Retrieves a DriverSession Entity from the **ITelemetryRepository**.
2. Passes the session data to the **LapTimeAnalyzer** (Domain Service).
3. Receives calculated lap/sector times from the Domain Service.
4. Returns the analysis results to the Presentation Layer.

## Adapters / Infrastructure Layer

Telemetry adapters (iRacing SDK, UDP parsers), Device Drivers / Simucube bindings, persistence, logging

This layer is responsible for implementing the interfaces (**Ports**) defined in the Application and Domain Layers. This is where external concerns like databases, logging, and specific hardware drivers live.

### Gateways (Implementations of ISimulatorGateway)

This component implements the logic to connect and receive real-time data from the different racing simulator APIs/SDKs. It translates raw, external simulator data into the common **Domain Entities** and **Value Objects** the Application Layer expects.

#### ISimulator Gateway

##### IRacing Socket Gateway

Uses iRacing's telemetry socket protocol to connect, listen for data, and push it to the Application Layer.

##### Assetto Corsa API Gateway

Uses Assetto Corsa's shared memory implementation to poll for game data at high frequency.

Persistence (Implementations of ITelemetryRepository**)**

This component handles storing and retrieving persistent data (like session logs and user profiles).

#### ITelemetry Repository

**SqliteTelemetryRepository**

Uses an ORM (like Entity Framework Core) with an SQLite database file to save historical DriverSession data and ForceFeedbackProfile Entities.

### Hardware Output (Implementations of IWheelHardwareOutput)

This component handles the critical, low-latency communication with the specific wheel hardware APIs.

#### IWheel Hardware Output

SimucubePro Driver Calls the manufacturer's SDK or low-level USB API to send the final FFBVector (Value Object) as a high-frequency command directly to the wheel motor.

Fanatec Whee lAPI Uses the Fanatec proprietary driver API to send force, damper, and friction values.

### Utilities & Logging

SerilogLogger ILoggingService (defined in Application Layer)

Concrete implementation of a simple logging interface, sending output to a file or the console.

## Presentation Layer (WPF MVVM Views and ViewModels)

This layer contains the **Views** and **ViewModels** that consume the Use Cases (Commands and Queries) from the Application Layer. It is completely unaware of the Infrastructure layer details.

Since we are using WPF with the MVVM pattern, the Presentation layer structure looks like this:

### **User Interface** (The outermost layer)

#### WPF View (MainWindow.xaml)

XAML files defining the UI structure. It binds data and commands directly to the ViewModel.

### Adapter/Controller

WPF View Model (MainViewModel)

Receives user input (e.g., button clicks) and **calls the Use Cases** (Commands/Queries) in the Application Layer using a dependency like IMediator.

### Composition Root

#### Dependency Injection Setup

The main application startup logic (App.xaml.cs) that ties all layers together: configures infrastructure implementations, registers Use Cases, and injects the dependencies into the View Models.

## Tests

#### Unit, Integration, End to End (hardware-in-loop optional)