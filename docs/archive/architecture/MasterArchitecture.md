# Racing Simulator Force Feedback Master Architecture

## Executive Overview

This document is the **unified, comprehensive architectural overview** for the Racing Simulator Force Feedback project. It combines architecture, design, and implementation details into a single reference document. It is intended to be readable by:

- Engineers new to the project
- Engineers new to Clean Architecture
- Reviewers who want to understand *what the system does and why*, without diving into code
- Developers implementing features who need both high-level and detailed guidance

This document explains the system's intent, structure, responsibilities, and detailed design, serving as the complete architectural reference.

### Documentation Philosophy

This document is **comprehensive and stable**, combining high-level overview with detailed design guidance. Lower‑level documents contain capabilities, behavior descriptions, and tests. Changes are driven by **new or failing tests**, not speculative design. Architecture evolves incrementally but intentionally.

---

## Project Goal

The goal of this project is to provide a **high‑performance, simulator‑agnostic force feedback system** for racing simulators on Windows. The system:

- Ingests high‑frequency telemetry from a user-selected racing simulator
- Applies domain‑specific force feedback algorithms
- Outputs precise, low‑latency force commands to user-selected physical steering wheel hardware
- Allows configuration, profiling, logging, and analysis without restarting the simulator

The architecture prioritizes:

- Deterministic behavior at high update rates (up to 360 Hz)
- Test‑driven development (TDD)
- Simulator and hardware independence
- Long‑term maintainability

---

## Key Architectural Principles

### Architectural Principles

- **Clean Architecture (Onion / Hexagonal)** with strict dependency rules
- **Dependencies always point inward**
- **Business logic is framework‑agnostic**
- **Use cases drive design**, not UI or infrastructure
- **Separation of Concerns:** Each layer has a specific, well-defined responsibility, making the system easier to maintain, test, and scale.

### Development Practices

- **TDD First:** Write failing tests for behavior before implementation. Keep tests small and fast.
- **Test-Driven Development acts as a continuous design refinement process**
- **Documentation evolves incrementally alongside tests**
- **DI everywhere:** Constructor-inject dependencies; avoid service locators.
- **Testability:** Business logic in the Domain and Application layers can be tested in isolation from external infrastructure concerns.

### Performance Principles

- **Real-time FFB loop isolation:** The real-time FFB loop runs in an isolated, high-priority service that consumes telemetry and emits commands to device adapters. It is separated from the UI thread with no blocking calls on that thread.
- **Minimize allocations on hot paths:** Use Span<T>, ArrayPool<T>, struct DTOs where appropriate.
- **Safety & Robustness:** Use cancellation tokens, graceful shutdown, and telemetry watch-dog timers.

---

## High-Level Architecture

The following sections describe the system's layered structure, starting with the high-level organization and then diving into each layer's responsibilities and components.

The system follows **Clean Architecture (Onion/Hexagonal)** principles with four main layers:

1. **Domain Layer** – Core business rules and domain models, independent of external frameworks
2. **Application Layer** – Use cases and orchestration logic
3. **Infrastructure Layer** – External concerns: simulators, hardware, persistence, logging (contains adapters)
4. **Presentation Layer** – WPF MVVM user interface

### Dependency Rule

The dependency rule is fundamental to Clean Architecture: **dependencies always point inward**. The outer layers can reference inner layers, but inner layers must remain independent of outer layer implementations.

- **Source code dependencies can only point inward**
- Nothing in an inner layer can know anything about an outer layer
- Inner layers define interfaces (ports), outer layers implement them (gateways/adapters)

This ensures that:
- Business logic remains independent of frameworks, databases, and UI
- The system can be tested in isolation
- Technology choices can be changed without affecting core business logic
- The system is maintainable and adaptable over time

---

## Domain Layer

The Domain Layer is the innermost layer of the architecture, containing the core business logic that is independent of all external concerns. The following sections detail its components and responsibilities.

### Clean Architecture Perspective

The Domain Layer contains the **core business logic** of the system. It has **no dependencies** on frameworks, UI, hardware SDKs, or persistence technologies. This is the innermost layer and the most stable part of the system.

This layer answers the question:

> *What is force feedback, independent of any simulator or wheel?*

### Dependency Rules

- **No dependencies on outer layers** – The Domain Layer cannot reference Application, Infrastructure, or Presentation layers
- **No framework dependencies** – No references to WPF, Entity Framework, or any external libraries
- **Pure business logic** – Contains only domain concepts, rules, and contracts

### Project Domain Overview

The Domain Layer defines:

- The fundamental data structures used throughout the system
- The rules governing force feedback behavior
- The contracts that external systems must satisfy

### Domain Components

#### Entities

**Entities are objects that have an identity that persists over time, and their state changes often.**

**Status:** TBD

**Simulator**  
Represents a specific racing simulator (e.g., iRacing, Assetto Corsa), identified by a unique ID or name. Its state changes (e.g., connected/disconnected, running/paused), but it remains the same simulator entity.

**Force Feedback Wheel**  
Represents the physical FFB wheel hardware (e.g., Simucube Pro, Fanatec), identified by a device ID. Its state might include calibration status, connection status, and current FFB strength settings.

**Telemetry Feed**  
Represents the source connection to a specific simulator. It has a unique identity tied to the connection. Its state transitions between Connected, Connecting, and Disconnected states. Provides operations to connect to and disconnect from the simulator's telemetry stream.

**Car**  
Represents the vehicle in the simulation, with attributes like weight, suspension settings, and tire properties. It has a persistent identity within a session or the application's configuration.

**Driver**  
Represents the user with an identity that transcends specific sessions or cars. The Driver entity persists across multiple sessions and maintains user preferences, historical data, and profile associations. (TBD: Detailed attributes and behaviors to be defined during implementation)

**Driver Session**  
Represents a single, continuous driving session (e.g., a qualifying session or a race). Its identity is crucial. **ID:** Guid (Session ID); **State:** InProgress, Paused, Completed; **Behavior:** StartSession(), EndSession().

#### Value Objects

Value objects describe characteristics and have no conceptual identity; they are defined entirely by their attributes and are typically immutable. If two value objects have the same attributes, they are considered equal.

**Status:** ✅ **COMPLETED**

The Value Objects component includes immutable data structures representing:

- **Telemetry measurements:** SteeringWheelAngle, YawRate, Velocity, Speed, GForce, EngineRPM, ShockVelocity, TireLoad
- **Hardware input:** WheelPosition, WheelVelocity
- **Force feedback output:** ForceFeedbackVector
- **Configuration:** Scale, PidConfig, YawRateFactor
- **Composite telemetry:** TelemetryDataPoint

All value objects are generic and simulator-agnostic, following TDD principles with comprehensive validation and testing.

**Telemetry Data Point**  
An immutable snapshot of data at a specific moment. Its value is the data it holds, not a unique ID. Containing values like:

- **TireLoad:** Represents the current force/load on a single tire. Defined by the Load (Newtons); Slip Ratio (unitless).
- **Wheel Angle:** Represents the current steering angle (e.g., in degrees or radians), a simple value with specific invariants (e.g., within the range of the wheel's rotation limits).
- **Engine RPM:** A value object representing engine rotational speed, possibly including max RPM limits for validation.

**Force Feedback Vector**  
The actual high-frequency force feedback signal. Represents a 3D force vector (X, Y, Z components) to be applied to the wheel. Immutable force data. **Data:** Force (Newtons), Damper (N·s/m), Inertia (kg·m²). Two Force Vector objects with the same X, Y, and Z values are considered equal.

**Pid Config**  
Configuration parameters for the FFB algorithm (e.g., Proportional, Integral, Derivative gains).

#### Interfaces (Ports)

**Terminology clarification:**
- **Port** (or Interface): A contract defined in the Domain layer that specifies what operations are needed without defining how they are implemented. The term "port" emphasizes the hexagonal architecture perspective where ports are the boundaries of the application.
- **Gateway** (or Adapter): A concrete implementation of a port in the Infrastructure layer that translates between the domain's abstract interface and external systems (simulators, hardware, databases, etc.).

Interfaces in the Domain layer define contracts (ports) that the outer (Infrastructure) layers must implement (as gateways/adapters). This adheres to the Dependency Rule, ensuring the Core remains independent of implementation details.

**ISimulatorTelemetryGateway**  
Defines the contract for an adapter that can read real-time telemetry data streams from various simulators. The implementation in the Infrastructure layer handles the specific UDP/shared memory protocols of iRacing, Assetto Corsa, etc.

**IForceFeedbackPort**  
Defines the contract for a gateway that sends FFB commands to the physical hardware. The Infrastructure implementation uses the specific SDKs or APIs for Simucube Pro, Fanatec, etc.

**IConfigurationRepository**  
Defines methods for saving and retrieving user configurations (e.g., FFB strength preferences, supported simulators). The Infrastructure layer implements this using file storage or a database.

**ILoggingService**  
An interface for logging events, which is implemented by a concrete logging framework (like Serilog) in the Infrastructure layer.

#### Domain Services

These services contain business logic that involves multiple entities or value objects, or complex calculations that are not suitable for an Entity's method.

**Force Feedback Calculation Service**  
A service that takes raw TelemetryDataPoint value objects and, using complex algorithms, calculates the appropriate ForceFeedbackVector to send to the wheel. This logic is crucial to the core business of the application but doesn't belong to any single entity.

**Telemetry Processing Service**  
Coordinates the retrieval of raw data via the ISimulatorTelemetryGateway, processes it, and passes it to the Force Feedback Calculation Service.

**Connection Management Service**  
Manages the complex process of detecting running simulators and connecting to their telemetry streams, which involves operations spanning multiple system concerns beyond a single entity's scope.

---

## Application Layer

Moving outward from the Domain Layer, the Application Layer orchestrates use cases and coordinates between domain logic and infrastructure, without depending on concrete implementations.

### Clean Architecture Perspective

The Application Layer contains **use cases**. It orchestrates domain logic and communicates with the outside world exclusively through interfaces defined in the Domain Layer.

This layer answers the question:

> *What can the system do?*

### Dependency Rules

- **Depends only on Domain Layer** – Application Layer can reference Domain entities, value objects, and interfaces
- **No Infrastructure dependencies** – Does not know about concrete implementations
- **Orchestration logic** – Coordinates domain services and infrastructure through interfaces

### Project Application Overview

The Application Layer defines all system behaviors as explicit use cases. Each use case is independently testable and represents a user or system‑level capability.

Use cases are organized into:
- **Commands** – Operations that change system state
- **Queries** – Operations that retrieve information without side effects

### Application Use Cases

#### Real-Time Telemetry and Force Feedback

**Generate Force Feedback Command**  
The core real-time use case that orchestrates telemetry ingestion, FFB profile loading, force calculation, and hardware output.

**Input:** Raw telemetry data (TelemetryDataPoint)  
**Output:** FFB command (ForceFeedbackVector)

**Orchestration:**
1. ISimulatorTelemetryGateway (input port) - Listens for and receives raw telemetry from the simulator
2. IConfigurationRepository (Domain port, implemented by Infrastructure) - Loads the user's currently active ForceFeedbackProfile
3. ForceFeedbackCalculationService (Domain Service) - Calls the Domain Service to perform the complex force calculation
4. IForceFeedbackPort (output port) - Sends the final, calculated FFB command to the wheel hardware

**Stop Force Feedback Command**  
Immediately sends a zero-force signal to the wheel (e.g., in case of a crash, connection loss, or user pause).

**Orchestration:**
1. Creates a zeroed ForceFeedbackVector
2. Sends the zeroed ForceFeedbackVector to the IForceFeedbackPort
3. Calls the Disconnect() method on the ISimulatorTelemetryGateway if needed

#### Configuration and Profile Management

**Get Available Simulator List Query**  
Retrieves the list of simulators the application supports.

**Orchestration:**
1. Queries the ISimulatorTelemetryGateway implementations in the Infrastructure layer (perhaps via a factory) to discover available connections
2. Returns a simple list of simulator names and status (e.g., "iRacing - Ready," "Assetto Corsa - Not Installed")

**Save Force Feedback Profile Command**  
Persists a user-defined set of FFB settings for a specific car/wheel combination.

**Orchestration:**
1. Receives input data for a new or updated ForceFeedbackProfile Entity
2. Applies any necessary validation rules (Domain rules) to the Entity
3. Passes the valid Entity to the IConfigurationRepository (Infrastructure Layer) for persistence

**Load Force Feedback Profile Query**  
Retrieves an existing FFB profile to apply to the active session.

**Orchestration:**
1. Receives the unique ID of the desired ForceFeedbackProfile
2. Queries the IConfigurationRepository to retrieve the corresponding Entity
3. Returns the ForceFeedbackProfile Entity or a corresponding Data Transfer Object (DTO) to the Presentation Layer

#### Data Logging and Analysis

**Log Session Data Command**  
Saves all telemetry and application state data for an entire driving session.

**Orchestration:**
1. Receives a finalized DriverSession Entity containing all logged high-frequency data (a stream of TelemetryDataPoint and other Value Objects)
2. Passes the DriverSession Entity to the IConfigurationRepository for storage (Infrastructure Layer)

**Analyze Lap Times Query**  
Calculates lap times and sector times for a given session log.

**Orchestration:**
1. Retrieves a DriverSession Entity from the IConfigurationRepository
2. Passes the session data to the LapTimeAnalyzer (Domain Service)
3. Receives calculated lap/sector times from the Domain Service
4. Returns the analysis results to the Presentation Layer

---

## Infrastructure Layer

The Infrastructure Layer is the outermost technical layer, providing concrete implementations of the interfaces defined by inner layers and handling all interactions with external systems and frameworks.

### Clean Architecture Perspective

The Infrastructure Layer provides **concrete implementations** of the interfaces defined in the Domain and Application layers. This is where all external concerns live.

This layer answers the question:

> *How does the system talk to real hardware, simulators, and storage?*

### Dependency Rules

- **Depends on Domain and Application Layers** – Infrastructure implements interfaces defined in inner layers
- **Framework-specific code** – Contains all simulator SDKs, hardware drivers, database code, etc.
- **No business logic** – Only contains adaptation and translation logic

### Project Infrastructure Overview

All simulator‑specific, hardware‑specific, and framework‑specific logic lives here. Nothing in this layer contains business rules.

### Infrastructure Components

#### Gateways (Implementations of ISimulatorTelemetryGateway)

This component implements the logic to connect and receive real-time data from the different racing simulator APIs/SDKs. It translates raw, external simulator data into the common **Domain Entities** and **Value Objects** the Application Layer expects.

**iRacing Socket Gateway**  
Uses iRacing's telemetry socket protocol to connect, listen for data, and push it to the Application Layer. Translates raw iRacing data into Domain Entities and Value Objects.

**Assetto Corsa API Gateway**  
Uses Assetto Corsa's shared memory implementation to poll for game data at high frequency. Translates raw Assetto Corsa data into Domain Entities and Value Objects.

#### Persistence (Implementations of IConfigurationRepository)

This component handles storing and retrieving persistent data (like session logs and user profiles).

**SqliteTelemetryRepository**  
Uses an ORM (like Entity Framework Core) with an SQLite database file to save historical DriverSession data and ForceFeedbackProfile Entities. Handles storing and retrieving persistent data like session logs and user profiles.

#### Hardware Output (Implementations of IForceFeedbackPort)

This component handles the critical, low-latency communication with the specific wheel hardware APIs.

**SimucubePro Driver**  
Calls the manufacturer's SDK or low-level USB API to send the final ForceFeedbackVector (Value Object) as a high-frequency command directly to the wheel motor. Handles critical, low-latency communication with Simucube hardware.

**Fanatec Wheel API**  
Uses the Fanatec proprietary driver API to send force, damper, and friction values. Handles critical, low-latency communication with Fanatec hardware.

#### Utilities & Logging

**SerilogLogger**  
Concrete implementation of ILoggingService (defined in Domain Layer). Sends logging output to a file or the console using the Serilog logging framework.

---

## Presentation Layer

The Presentation Layer provides the user interface and coordinates application startup, depending on the Application Layer to execute use cases while remaining decoupled from infrastructure details.

### Clean Architecture Perspective

The Presentation Layer contains all UI and application startup logic. It depends on the Application Layer but is unaware of Infrastructure implementations.

This layer answers the question:

> *How do users interact with the system?*

### Dependency Rules

- **Depends on Application Layer** – Presentation invokes use cases from Application Layer
- **No Domain or Infrastructure dependencies** – Should not directly reference Domain entities or Infrastructure implementations
- **UI framework code** – Contains all WPF-specific code

### Project Presentation Overview

The system uses **WPF with MVVM**. ViewModels invoke Application use cases and expose state to the UI.

### Presentation Components

#### User Interface

**WPF View (MainWindow.xaml)**  
XAML files defining the UI structure. Binds data and commands directly to the ViewModel. Provides the visual interface for user interaction.

#### Adapter/Controller

**WPF ViewModel (MainViewModel)**  
Receives user input (e.g., button clicks) and calls the Use Cases (Commands/Queries) in the Application Layer using a dependency like IMediator. Coordinates between the View and Application Layer use cases.

#### Composition Root

**Dependency Injection Setup**  
The main application startup logic (App.xaml.cs) that ties all layers together: configures infrastructure implementations, registers Use Cases, and injects the dependencies into the View Models. This is where the dependency injection container is configured and the application's object graph is assembled.

---

## Testing Strategy

The architecture's layered design and dependency rules enable comprehensive testing at multiple levels. The following sections outline the testing approach and principles.

### Test Types

- **Unit Tests** – Test individual components in isolation
- **Integration Tests** – Test interactions between layers
- **End-to-End Tests** – Test complete workflows (hardware-in-loop optional)

### Testing Principles

- **TDD First:** Write failing tests for behavior before implementation
- **Keep tests small and fast**
- **Test business logic in isolation** from external infrastructure concerns
- **Use mocks and stubs** for infrastructure dependencies in unit tests

---

## Capabilities Template

For detailed information about creating capability documentation for components, see the [Capabilities Template](documents%20used%20to%20populate%20Architecture%20and%20Design%20Docs/Capabilities%20Template.md) document.
