# Racing Simulator Force Feedback Detail Design

This document provides a high-level overview of the RacingSimFFBTuner project architecture, organized by layer. For detailed design information, refer to [Racing Simulator Force Feedback Design.md](../../Racing%20Simulator%20Force%20Feedback%20Design.md).

## Architecture

The project follows Clean Architecture (Onion/Hexagonal) principles with four main layers:

- **Domain Layer** - Core business rules and entities, independent of external frameworks
- **Application Layer** - Use cases and orchestration logic
- **Adapters/Infrastructure Layer** - External concerns: simulators, hardware, persistence, logging
- **Presentation Layer** - WPF MVVM user interface

Dependencies always point inward. The outer layers can reference inner layers, but inner layers must remain independent of outer layer implementations.

---

## Domain Layer

The Domain Layer contains the core business logic and domain models. It has no dependencies on external frameworks or infrastructure concerns. This layer defines the fundamental concepts and rules that govern the application's behavior.

### Components

#### Entities

** TBD **

#### Value Objects

Value objects describe characteristics and have no conceptual identity; they are defined entirely by their attributes and are typically immutable. If two value objects have the same attributes, they are considered equal.

**Status:** ✅ **COMPLETED**

For comprehensive analysis and design details, see [Value Objects Analysis and Design](Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md).

The Value Objects component includes immutable data structures representing:
- Telemetry measurements (SteeringWheelAngle, YawRate, Velocity, Speed, GForce, EngineRPM, ShockVelocity, TireLoad)
- Hardware input (WheelPosition, WheelVelocity)
- Force feedback output (ForceFeedbackVector)
- Configuration (Scale, PidConfig, YawRateFactor)
- Composite telemetry (TelemetryDataPoint)

All value objects are generic and simulator-agnostic, following TDD principles with comprehensive validation and testing.

#### Interfaces (Gateways/Ports)

**ISimulatorTelemetryGateway**  
Defines the contract for an adapter that can read real-time telemetry data streams from various simulators. The implementation in the Infrastructure layer handles the specific UDP/shared memory protocols of iRacing, Assetto Corsa, etc.

**IForceFeedbackPort**  
Defines the contract for a gateway that sends FFB commands to the physical hardware. The Infrastructure implementation uses the specific SDKs or APIs for Simucube Pro, Fanatec, etc.

**IConfigurationRepository**  
Defines methods for saving and retrieving user configurations (e.g., FFB strength preferences, supported simulators). The Infrastructure layer implements this using file storage or a database.

**ILoggingService**  
An interface for logging events, which is implemented by a concrete logging framework (like Serilog) in the Infrastructure layer.

#### Domain Services

**Force Feedback Calculation Service**  
A service that takes raw TelemetryDataPoint value objects and, using complex algorithms, calculates the appropriate ForceFeedbackVector to send to the wheel. This logic is crucial to the core business of the application but doesn't belong to any single entity.

**Telemetry Processing Service**  
Coordinates the retrieval of raw data via the ISimulatorTelemetryGateway, processes it, and passes it to the Force Feedback Calculation Service.

**Connection Management Service**  
Manages the complex process of detecting running simulators and connecting to their telemetry streams, which involves operations spanning multiple system concerns beyond a single entity's scope.

---

## Application Layer

The Application Layer contains the use cases (application features) that orchestrate domain logic and coordinate with infrastructure through interfaces. This layer depends on the Domain Layer but remains independent of infrastructure implementations.

### Components

#### Real-Time Telemetry and Force Feedback

**Generate Force Feedback Command**  
The core real-time use case that orchestrates telemetry ingestion, FFB profile loading, force calculation, and hardware output. Input: Raw telemetry data (TelemetryDataPoint). Output: FFB command (ForceFeedbackVector).

**Stop Force Feedback Command**  
Immediately sends a zero-force signal to the wheel (e.g., in case of a crash, connection loss, or user pause). Creates a zeroed ForceFeedbackVector and sends it to the hardware output.

#### Configuration and Profile Management

**Get Available Simulator List Query**  
Retrieves the list of simulators the application supports. Queries the ISimulatorGateway implementations in the Infrastructure layer to discover available connections and returns simulator names and status.

**Save Force Feedback Profile Command**  
Persists a user-defined set of FFB settings for a specific car/wheel combination. Receives input data for a new or updated ForceFeedbackProfile Entity, applies validation rules, and persists via the ITelemetryRepository.

**Load Force Feedback Profile Query**  
Retrieves an existing FFB profile to apply to the active session. Receives the unique ID of the desired ForceFeedbackProfile, queries the ITelemetryRepository, and returns the profile Entity or DTO to the Presentation Layer.

#### Data Logging and Analysis

**Log Session Data Command**  
Saves all telemetry and application state data for an entire driving session. Receives a finalized DriverSession Entity containing all logged high-frequency data and passes it to the ITelemetryRepository for storage.

**Analyze Lap Times Query**  
Calculates lap times and sector times for a given session log. Retrieves a DriverSession Entity from the ITelemetryRepository, passes the session data to the LapTimeAnalyzer Domain Service, and returns the analysis results to the Presentation Layer.

---

## Adapters/Infrastructure Layer

The Infrastructure Layer implements the interfaces (Ports) defined in the Application and Domain Layers. This is where external concerns like databases, logging, and specific hardware drivers live. This layer depends on both the Application and Domain layers.

### Components

#### Gateways (Implementations of ISimulatorTelemetryGateway)

**IRacing Socket Gateway**  
Uses iRacing's telemetry socket protocol to connect, listen for data, and push it to the Application Layer. Translates raw iRacing data into Domain Entities and Value Objects.

**Assetto Corsa API Gateway**  
Uses Assetto Corsa's shared memory implementation to poll for game data at high frequency. Translates raw Assetto Corsa data into Domain Entities and Value Objects.

#### Persistence (Implementations of ITelemetryRepository)

**SqliteTelemetryRepository**  
Uses an ORM (like Entity Framework Core) with an SQLite database file to save historical DriverSession data and ForceFeedbackProfile Entities. Handles storing and retrieving persistent data like session logs and user profiles.

#### Hardware Output (Implementations of IWheelHardwareOutput)

**SimucubePro Driver**  
Calls the manufacturer's SDK or low-level USB API to send the final ForceFeedbackVector (Value Object) as a high-frequency command directly to the wheel motor. Handles critical, low-latency communication with Simucube hardware.

**Fanatec Wheel API**  
Uses the Fanatec proprietary driver API to send force, damper, and friction values. Handles critical, low-latency communication with Fanatec hardware.

#### Utilities & Logging

**SerilogLogger**  
Concrete implementation of ILoggingService (defined in Domain Layer). Sends logging output to a file or the console using the Serilog logging framework.

---

## Presentation Layer

The Presentation Layer contains the Views and ViewModels that consume the Use Cases (Commands and Queries) from the Application Layer. It is completely unaware of the Infrastructure layer details. This layer uses WPF with the MVVM pattern.

### Components

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

## References

- [Racing Simulator Force Feedback Design.md](../../Racing%20Simulator%20Force%20Feedback%20Design.md) - Complete design and step-by-step plan
- [Value Objects Analysis and Design](Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md) - Comprehensive value objects documentation
