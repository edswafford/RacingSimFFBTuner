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

## Architectural Overview

The system follows a **hybrid Clean Architecture** with four core layers:

1. **Domain Layer** – Core business rules and domain models
2. **Application Layer** – Use cases and orchestration logic
3. **Adapters / Infrastructure Layer** – External concerns (simulators, hardware, persistence)
4. **Presentation Layer** – WPF MVVM user interface and application startup

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

#### Entities

[Entities](Domain/Entities/Entities.md)

*Capability:* Defines domain objects with identity and lifecycle that represent core business concepts.

Entities are objects that have a unique identity and lifecycle. They represent the core business concepts that persist over time and have state that changes.

#### Value Objects

[Value Objects](Domain/ValueObjects/ValueObjects.md)

*Capability:* Immutable, simulator‑agnostic data types representing telemetry, configuration, and force feedback output.

For comprehensive analysis and design details, see [Value Objects Analysis and Design](../../docs_old/Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md).

The Value Objects component includes immutable data structures representing:
- Telemetry measurements (SteeringWheelAngle, YawRate, Velocity, Speed, GForce, EngineRPM, ShockVelocity, TireLoad)
- Hardware input (WheelPosition, WheelVelocity)
- Force feedback output (ForceFeedbackVector)
- Configuration (Scale, PidConfig, YawRateFactor)
- Composite telemetry (TelemetryDataPoint)

All value objects are generic and simulator-agnostic, following TDD principles with comprehensive validation and testing.

#### Interfaces (Gateways / Ports)

[Interfaces](Domain/Interfaces/Interfaces.md)

*Capability:* Defines contracts for telemetry input, hardware output, configuration, and logging.

Interfaces (also called Ports in Hexagonal Architecture) define the contracts that external systems must satisfy. These are implemented by adapters in the Infrastructure Layer.

Key interfaces include:
- **ISimulatorTelemetryGateway** – Contract for reading telemetry from simulators
- **IForceFeedbackPort** – Contract for sending FFB commands to hardware
- **IConfigurationRepository** – Contract for persisting and retrieving configuration
- **ILoggingService** – Contract for logging events

#### Domain Services

[Domain Services](Domain/DomainServices/DomainServices.md)

*Capability:* Encapsulates complex business logic that does not belong to a single entity.

Domain Services contain business logic that doesn't naturally fit within a single entity or value object. They operate on domain objects and enforce business rules.

Key services include:
- **Force Feedback Calculation Service** – Calculates force feedback vectors from telemetry
- **Telemetry Processing Service** – Processes raw telemetry data

#### Connection Management Service

[Connection Management Service](Domain/ConnectionManagementService/ConnectionManagementService.md)

*Capability:* Coordinates detection and management of simulator connections.

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

- **RealTimeTelemetryAndForceFeedback**  
  [RealTimeTelemetryAndForceFeedback](Application/UseCases/RealTimeTelemetryAndForceFeedback/RealTimeTelemetryAndForceFeedback.md)  
  *Capability:* Ingests telemetry, applies force feedback algorithms, and outputs real‑time force commands.

- **StopForceFeedback**  
  [StopForceFeedback](Application/UseCases/StopForceFeedback/StopForceFeedback.md)  
  *Capability:* Immediately halts force output to the wheel under error or user‑initiated conditions.

- **LoadForceFeedbackProfile**  
  [LoadForceFeedbackProfile](Application/UseCases/LoadForceFeedbackProfile/LoadForceFeedbackProfile.md)  
  *Capability:* Loads and applies a stored force feedback profile.

- **SaveForceFeedbackProfile**  
  [SaveForceFeedbackProfile](Application/UseCases/SaveForceFeedbackProfile/SaveForceFeedbackProfile.md)  
  *Capability:* Persists user‑defined force feedback configurations.

- **AnalyzeLapTimes**  
  [AnalyzeLapTimes](Application/UseCases/AnalyzeLapTimes/AnalyzeLapTimes.md)  
  *Capability:* Analyzes recorded session data to compute lap and sector times.

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

#### Gateways

[Gateways](Infrastructure/Gateways/Gateways.md)

*Capability:* Connects to simulator telemetry sources and adapts raw data into domain value objects.

Gateways implement the `ISimulatorTelemetryGateway` interface and handle the protocol-specific details of communicating with different simulators (iRacing, Assetto Corsa, etc.).

#### Persistence

[Persistence](Infrastructure/Persistence/Persistence.md)

*Capability:* Stores and retrieves configuration, profiles, and session data.

Persistence implementations handle all data storage concerns, implementing interfaces like `IConfigurationRepository` and `ITelemetryRepository`.

#### Hardware Output

[Hardware Output](Infrastructure/HardwareOutput/HardwareOutput.md)

*Capability:* Sends force feedback commands to physical steering wheel hardware.

Hardware output implementations send force feedback vectors to physical hardware using manufacturer-specific SDKs or APIs (Simucube, Fanatec, etc.).

#### Utilities & Logging

[Utilities & Logging](Infrastructure/Utilities/Utilities.md)

*Capability:* Provides logging and supporting infrastructure concerns.

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

#### User Interface

[User Interface](Presentation/UserInterface/UserInterface.md)

*Capability:* Presents force feedback state, configuration, and diagnostics to the user.

The UI consists of WPF XAML views that bind to ViewModels and display information to users.

#### Adapter / Controllers (ViewModels)

[ViewModels](Presentation/ViewModels/ViewModels.md)

*Capability:* Translates user input into application use case invocations.

ViewModels implement the MVVM pattern, exposing properties and commands that the UI binds to, and invoking use cases from the Application Layer.

#### Composition Root

[Composition Root](Presentation/CompositionRoot/CompositionRoot.md)

*Capability:* Assembles the application object graph and configures dependency injection.

The Composition Root (typically in `App.xaml.cs`) is where all dependencies are wired together, infrastructure implementations are registered, and the application's object graph is assembled.

---

## Documentation Philosophy

- This document is **stable and high‑level**
- Lower‑level documents contain capabilities, behavior descriptions, and tests
- Changes are driven by **new or failing tests**, not speculative design
- Architecture evolves incrementally but intentionally

This document is the map. The rest of the documentation is the territory.

---

## Capabilities Template

> **Purpose**  
> This document defines *why this component exists* and *what it must do*, independent of implementation details.  
> It is the primary **TDD driver document** for this component.
>
> This document should change **infrequently**. If it changes, it is usually because:
> - A requirement was misunderstood
> - A new legitimate capability was discovered
> - Tests revealed an invalid assumption
>
> Do **not** document implementation details here.

---

### 1. Capability

**One sentence describing why this component exists.**

- Focus on *intent*, not mechanics
- Avoid framework, technology, or class names
- Phrase it so a non-UI consumer (test, service, batch process) could invoke it

**Example:**  
> Calculates and outputs real-time force feedback commands from simulator telemetry.

---

### 2. Context

**Describe where this capability fits in the system.**

Include:
- Which layer this component belongs to
- Who/what calls it
- What it collaborates with (at a high level)

Avoid:
- Method names
- Class diagrams
- Control flow details

**Guidance questions:**
- Is this triggered by a user action, a system event, or a continuous process?
- Is this synchronous or asynchronous from the caller's perspective?

---

### 3. Inputs

**Describe the information this capability requires.**

- Use domain language, not DTO or API names
- Specify constraints and expectations
- If input is optional or conditional, say so

**Example format:**
- Telemetry data point (validated, time-ordered)
- Active force feedback profile
- Current simulator connection state

---

### 4. Outputs

**Describe what this capability produces or affects.**

Outputs may be:
- Returned values
- State changes
- Commands sent through ports

Be explicit about:
- Guarantees (e.g., always produced, best-effort, conditional)
- Observable effects

---

### 5. Behaviors

**List the externally observable behaviors that define correctness.**

Each behavior:
- Is testable
- Is phrased in business/domain terms
- Avoids algorithmic detail

Use numbered statements.

**Example:**
1. When telemetry is received, a force feedback command is produced within the same update cycle.
2. When telemetry input is invalid, no force is sent to the hardware.
3. When the system is paused, force output is zeroed.

> If a behavior cannot be tested, it does not belong here.

---

### 6. Invariants

**List conditions that must always be true, regardless of implementation.**

These guide refactoring and prevent accidental rule erosion.

**Examples:**
- Force feedback output must never exceed configured safety limits.
- Domain objects remain immutable.
- No infrastructure concern leaks into the domain.

---

### 7. Failure Modes

**Describe how this capability behaves when things go wrong.**

Include:
- Invalid input
- Missing dependencies
- External system failures

State:
- What happens
- What does *not* happen

Avoid exception-class-level detail.

---

### 8. Non-Goals

**Explicitly state what this capability does *not* do.**

This prevents scope creep and accidental coupling.

**Examples:**
- Does not manage simulator discovery
- Does not persist configuration
- Does not perform UI updates

---

### 9. Change Policy

**Define how and when this document is allowed to change.**

Typical rules:
- Adding a behavior requires a new failing test
- Removing a behavior requires explicit review
- Design changes do *not* require changes here unless behavior changes

---

### 10. Related Artifacts

**Link to relevant documents and tests.**

- Related capabilities
- High-level tests
- Architecture overview

Keep this section lightweight and navigational.

---

> **Rule of Thumb**  
> If you are tempted to explain *how*, you are in the wrong document.
> Explain *why* and *what*, then write tests.
