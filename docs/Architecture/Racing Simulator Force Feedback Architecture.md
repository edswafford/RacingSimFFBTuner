# Racing Simulator Force Feedback Architecture

## Executive Overview

This document is the **single, top-level architectural overview** for the Racing Simulator Force Feedback project. It is intended to be readable by:

- Engineers new to the project
- Engineers new to Clean Architecture
- Reviewers who want to understand *what the system does and why*, without diving into code

It deliberately avoids implementation detail. Its purpose is to explain **intent, structure, and responsibilities**, and to act as the stable entry point into the rest of the documentation set.

This document should change **rarely**. When it changes, it is because the *fundamental shape or goals of the system* have changed.

---

## Project Goal

The goal of this project is to provide a **high‑performance, simulator‑agnostic force feedback system** for racing simulators on Windows. The system:

- Ingests high‑frequency telemetry from a user selected racing simulator
- Applies domain‑specific force feedback algorithms
- Outputs precise, low‑latency force commands to user selected physical steering wheel hardware
- Allows configuration, profiling, logging, and analysis without restarting the simulator

The architecture prioritizes:

- Deterministic behavior at high update rates (up to 360 Hz)
- Test‑driven development (TDD)
- Simulator and hardware independence
- Long‑term maintainability

---

## Key Architectural Principles

- **Clean Architecture (Onion / Hexagonal)** with strict dependency rules
- **Dependencies always point inward**
- **Business logic is framework‑agnostic**
- **Use cases drive design**, not UI or infrastructure
- **Test-Driven Development acts as a continuous design refinement process**
- **Documentation evolves incrementally alongside tests**

---

## Clean Architecture Overview

The system follows **Clean Architecture** (also known as Onion Architecture or Hexagonal Architecture), which organizes code into concentric layers with strict dependency rules. The fundamental principle is that **dependencies point inward**—outer layers depend on inner layers, but inner layers never depend on outer layers.

### Dependency Rule

The dependency rule states that:

- **Source code dependencies can only point inward**
- Nothing in an inner layer can know anything about an outer layer
- Inner layers define interfaces (ports), outer layers implement them (adapters)

This ensures that:

- Business logic remains independent of frameworks, databases, and UI
- The system can be tested in isolation
- Technology choices can be changed without affecting core business logic
- The system is maintainable and adaptable over time

### Layer Structure

The architecture consists of four main layers:

1. **Domain Layer** (innermost) – Core business rules and domain models
2. **Application Layer** – Use cases and orchestration logic
3. **Infrastructure Layer** – External concerns (simulators, hardware, persistence)
4. **Presentation Layer** (outermost) – WPF MVVM user interface

Each layer has a clearly defined responsibility and communicates only through stable interfaces.

---

## Domain Layer

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

### Domain Elements

[Entities](Domain/Entities/Entities.md)

Entities are objects that have a unique identity and lifecycle. They represent the core business concepts that persist over time and have state that changes.

[Value Objects](Domain/ValueObjects/ValueObjects.md)

The Value Objects component includes immutable data structures representing:

- Telemetry measurements (SteeringWheelAngle, YawRate, Velocity, Speed, GForce, EngineRPM, ShockVelocity, TireLoad)
- Hardware input (WheelPosition, WheelVelocity)
- Force feedback output (ForceFeedbackVector)
- Configuration (Scale, PidConfig, YawRateFactor)
- Composite telemetry (TelemetryDataPoint)

All value objects are generic and simulator-agnostic, following TDD principles with comprehensive validation and testing.

[Interfaces](Domain/Interfaces/Interfaces.md)

Interfaces (also called Ports in Hexagonal Architecture) define the contracts that external systems must satisfy. These are implemented by adapters in the Infrastructure Layer.

Key interfaces include:

- **ISimulatorTelemetryGateway** – Contract for reading telemetry from simulators
- **IForceFeedbackPort** – Contract for sending FFB commands to hardware
- **IConfigurationRepository** – Contract for persisting and retrieving configuration
- **ILoggingService** – Contract for logging events

[Domain Services](Domain/DomainServices/DomainServices.md)

Domain Services contain business logic that doesn't naturally fit within a single entity or value object. They operate on domain objects and enforce business rules.

Key services include:

- **Force Feedback Calculation Service** – Calculates force feedback vectors from telemetry
- **Telemetry Processing Service** – Processes raw telemetry data

[Connection Management Service](Domain/ConnectionManagementService/ConnectionManagementService.md)

This service manages the complex process of detecting running simulators and establishing connections to their telemetry streams.

---

## Application Layer

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

### Application Elements

#### Use Cases 

[RealTimeTelemetryAndForceFeedback](Application/UseCases/RealTimeTelemetryAndForceFeedback/RealTimeTelemetryAndForceFeedback.md)  

[StopForceFeedback](Application/UseCases/StopForceFeedback/StopForceFeedback.md)  

[LoadForceFeedbackProfile](Application/UseCases/LoadForceFeedbackProfile/LoadForceFeedbackProfile.md)  

[SaveForceFeedbackProfile](Application/UseCases/SaveForceFeedbackProfile/SaveForceFeedbackProfile.md)  

[AnalyzeLapTimes](Application/UseCases/AnalyzeLapTimes/AnalyzeLapTimes.md)  



---

## Adapters / Infrastructure Layer

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

### Infrastructure Elements

[Gateways](Infrastructure/Gateways/Gateways.md)

Gateways implement the `ISimulatorTelemetryGateway` interface and handle the protocol-specific details of communicating with different simulators (iRacing, Assetto Corsa, etc.).

[Persistence](Infrastructure/Persistence/Persistence.md)

Persistence implementations handle all data storage concerns, implementing interfaces like `IConfigurationRepository` and `ITelemetryRepository`.

[Hardware Output](Infrastructure/HardwareOutput/HardwareOutput.md)

Hardware output implementations send force feedback vectors to physical hardware using manufacturer-specific SDKs or APIs (Simucube, Fanatec, etc.).

#### Utilities & Logging

[Utilities & Logging](Infrastructure/Utilities/Utilities.md)

This includes concrete logging implementations, file system utilities, and other cross-cutting infrastructure concerns.

---

## Presentation Layer

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

### Presentation Elements

[User Interface](Presentation/UserInterface/UserInterface.md).

The UI consists of WPF XAML views that bind to ViewModels and display information to users.

[ViewModels](Presentation/ViewModels/ViewModels.md).

ViewModels implement the MVVM pattern, exposing properties and commands that the UI binds to, and invoking use cases from the Application Layer.

[Composition Root](Presentation/CompositionRoot/CompositionRoot.md)

The Composition Root (typically in `App.xaml.cs`) is where all dependencies are wired together, infrastructure implementations are registered, and the application's object graph is assembled.

---
