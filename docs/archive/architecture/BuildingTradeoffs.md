# Building Tradeoffs: Approaches for Racing Sim FFB Development

This document analyzes different approaches to building the Racing Sim FFB application, considering:

- Test-Driven Development (TDD) as the core methodology
- Domain Layer Value Objects already completed
- Support for multiple racing simulators (starting with iRacing)
- Support for multiple steering wheel hardware (Simucube, Fanatec, etc.)
- Goal: Working mockup that can be incrementally transformed into the full application

## Current State

**Completed:**

- Domain Layer Value Objects (EngineRPM, ForceFeedbackVector, GForce, PidConfig, Scale, ShockVelocity, Speed, SteeringWheelAngle, TelemetryDataPoint, TireLoad, Torque, Velocity, WheelPosition, WheelVelocity, YawRate, YawRateFactor)
- Project structure with Clean Architecture layers
- Unit tests for all Value Objects

**Reference Implementation:**

- Legacy SimRacingFFB application (forked from Marvin's AIRA)
- GitHub: https://github.com/edswafford/SimRacingFFB

---

## Approach 1: Vertical Slice (Feature-First with iRacing)

### Description

Build complete vertical slices through all layers for one feature at a time. Start with the core FFB loop for iRacing only, then expand horizontally to add more simulators and features.

**Build Order:**

1. **Minimal FFB Loop Slice**

   - Domain: `ISimulatorTelemetryGateway`, `IForceFeedbackPort` interfaces
   - Domain: `TelemetryFeed` entity (basic)
   - Application: `GenerateForceFeedbackCommand` use case (simplified)
   - Infrastructure: `IRacingTelemetryGateway` (read shared memory)
   - Infrastructure: `DirectInputForceFeedbackPort` (write constant force)
   - Presentation: Minimal WPF window showing connection status
1. **Expand FFB Calculation**

   - Domain: `ForceFeedbackCalculationService` with real algorithms
   - Application: Complete `GenerateForceFeedbackCommand` with all processing
1. **Add UI for Tuning**

   - Presentation: ViewModels for FFB scales, profiles
   - Application: `SaveForceFeedbackProfileCommand`, `LoadForceFeedbackProfileQuery`
1. **Add Second Simulator**

   - Infrastructure: `AssettoCorseGateway` (demonstrates abstraction works)

### Pros

| Benefit                         | Description                                                           |
| ------------------------------- | --------------------------------------------------------------------- |
| **Early Working Software**      | Real FFB output within days, not weeks                                |
| **Validates Architecture**      | Proves abstractions work end-to-end before investing in them          |
| **User Feedback Loop**          | Can test with real hardware/simulator immediately                     |
| **Reduces Risk**                | Discovers integration issues early (DirectInput quirks, timing, etc.) |
| **TDD Friendly**                | Each slice has clear acceptance criteria: "telemetry in → FFB out"    |
| **Mockup Transforms Naturally** | The "mockup" IS the application from day one                          |

### Cons

| Drawback                         | Description                                                   |
| -------------------------------- | ------------------------------------------------------------- |
| **Refactoring Overhead**         | May need to refactor early code as patterns emerge            |
| **Domain Purity Risk**           | Temptation to add iRacing-specific logic to domain layer      |
| **Parallel Development Harder**  | Difficult to split work among multiple developers             |
| **Infrastructure-Coupled Tests** | Early tests may be more integration-focused than unit-focused |
| **Tight Coupling Risk**          | Without discipline, layers may become entangled               |

### Best For

- Solo developers or small teams
- Projects with high technical uncertainty (hardware/SDK integration)
- When stakeholder feedback is critical early
- When you have working reference code (legacy SimRacingFFB)

---

## Approach 2: Inside-Out (Domain-Driven with Mockups)

### Description

Continue building outward from the completed Value Objects. Build complete domain layer first, then application layer with mock infrastructure, then real infrastructure, and finally presentation. Each layer is tested in isolation before integration.

**Build Order:**

1. **Complete Domain Layer**

   - Domain Entities: `Simulator`, `ForceFeedbackWheel`, `TelemetryFeed`, `Car`, `DriverSession`
   - Domain Services: `ForceFeedbackCalculationService`, `TelemetryProcessingService`
   - Domain Interfaces: `ISimulatorTelemetryGateway`, `IForceFeedbackPort`, `IConfigurationRepository`
   - *All with unit tests using mocks*
1. **Complete Application Layer**

   - Use Cases: `GenerateForceFeedbackCommand`, `StopForceFeedbackCommand`, `SaveProfileCommand`, etc.
   - Application Services orchestrating domain services
   - *Tests using mock infrastructure implementations*
1. **Build Infrastructure Layer**

   - Real implementations: `IRacingTelemetryGateway`, `DirectInputForceFeedbackPort`
   - Integration tests against real hardware/simulators
   - *Mockup UI can be built in parallel*
1. **Build Presentation Layer**

   - WPF Views and ViewModels
   - Wire to real use cases
   - End-to-end testing

### Pros

| Benefit                       | Description                                                           |
| ----------------------------- | --------------------------------------------------------------------- |
| **Clean Architecture Purity** | Strict adherence to dependency rule throughout                        |
| **Highly Testable**           | Each layer can be tested in complete isolation                        |
| **Domain Model Excellence**   | Time to think through domain concepts without infrastructure pressure |
| **Parallel Development**      | Different developers can work on different layers                     |
| **Documentation Value**       | Domain layer serves as executable specification                       |
| **Refactoring Safety**        | Changes to infrastructure don't affect core logic                     |

### Cons

| Drawback                  | Description                                                       |
| ------------------------- | ----------------------------------------------------------------- |
| **Delayed Integration**   | Real hardware testing comes late, integration issues surface late |
| **Over-Engineering Risk** | May build abstractions that don't fit real requirements           |
| **Speculative Design**    | Domain design without integration feedback may miss the mark      |
| **Late User Feedback**    | No working software until all layers complete                     |
| **Mock Maintenance**      | Large investment in mock objects that are discarded               |
| **Analysis Paralysis**    | Temptation to perfect domain before moving on                     |

### Best For

- Teams with Clean Architecture experience
- Projects with well-understood domains (this has legacy reference)
- When you need strong test coverage guarantees
- Larger teams that can parallelize layer development

---

## Approach 3: UI Shell First (Outside-In with Walking Skeleton)

### Description

Build a "walking skeleton" — a minimal end-to-end implementation with the UI shell first, using mock/simulated data. Then progressively replace mocks with real implementations. The UI drives what needs to be built.

**Build Order:**

1. **WPF Shell with Mock Data**

   - Presentation: Complete UI layout with all planned features
   - ViewModels with hardcoded/simulated data
   - Visualizes what the app will look like
   - *Manual testing, possibly automated UI tests*
1. **Add Application Layer Interface**

   - Define use case interfaces that ViewModels call
   - Mock implementations return simulated data
   - ViewModels bind to use case results
1. **Implement Domain & Infrastructure for Each Feature**

   - Pick a UI feature (e.g., "Show Current FFB Strength")
   - Implement the complete stack: Domain → Application → Infrastructure
   - Replace mock with real implementation
   - *TDD for each feature slice*
1. **Iterate Until All Features Real**

   - Systematically replace mocks with real implementations
   - Each iteration adds working functionality

### Pros

| Benefit                        | Description                                               |
| ------------------------------ | --------------------------------------------------------- |
| **Immediate Visual Prototype** | Stakeholder feedback on UI/UX before backend work         |
| **UI-Driven Design**           | Implementation driven by actual UI needs, not speculation |
| **Mockup IS the App**          | Shell becomes the real application incrementally          |
| **Clear Progress Visibility**  | Easy to see what's working vs. mocked                     |
| **Feature Prioritization**     | Can implement high-value features first                   |
| **User Testing Early**         | Can do usability testing with mock data                   |

### Cons

| Drawback                         | Description                                                 |
| -------------------------------- | ----------------------------------------------------------- |
| **UI Churn**                     | UI may need redesign as backend realities emerge            |
| **Presentation Layer Pollution** | Risk of business logic leaking into ViewModels              |
| **Mock Explosion**               | Many mocks needed for complex UI scenarios                  |
| **Domain Afterthought**          | Domain model may be shaped by UI rather than business rules |
| **Integration Testing Late**     | Real hardware/simulator testing comes last                  |
| **WPF Overhead**                 | Significant UI framework knowledge needed upfront           |

### Best For

- Consumer applications where UX is critical
- Projects with non-technical stakeholders who need to see progress
- When UI requirements are uncertain
- Teams with strong WPF/MVVM experience

---

## Approach 4: Hybrid — Domain Core + Vertical Slices (Recommended)

### Description

A pragmatic hybrid approach: Complete the domain core (entities, services, interfaces) with mocks, then build vertical slices for each simulator/hardware combination. Leverages the completed Value Objects and provides early integration feedback.

**Build Order:**

### Phase 1: Domain Core Completion (1-2 weeks)

Build the domain layer foundation that all features will use:

1. **Domain Entities** (TDD with mocks)

   - `TelemetryFeed` - connection state machine
   - `ForceFeedbackWheel` - wheel identity and state
   - `Simulator` - simulator identity and capabilities
   - `DriverSession` - session lifecycle
1. **Domain Interfaces** (contracts only)

   - `ISimulatorTelemetryGateway` - port for simulator adapters
   - `IForceFeedbackPort` - port for wheel output
   - `IConfigurationRepository` - port for persistence
1. **Domain Services** (TDD with mocks)

   - `ForceFeedbackCalculationService` - core FFB algorithm
   - `TelemetryProcessingService` - telemetry normalization

### Phase 2: iRacing Vertical Slice (2-3 weeks)

Complete end-to-end for iRacing with real hardware:

4. **Infrastructure: iRacing Adapter**

   - `IRacingTelemetryGateway` implementing `ISimulatorTelemetryGateway`
   - Reads shared memory, emits `TelemetryDataPoint` value objects
   - *Integration tests with real/simulated iRacing data*
5. **Infrastructure: DirectInput FFB**

   - `DirectInputForceFeedbackPort` implementing `IForceFeedbackPort`
   - Constant force effect with multimedia timer (360Hz)
   - *Integration tests with real hardware*
6. **Application Use Cases**

   - `GenerateForceFeedbackCommand` - main FFB loop
   - `StopForceFeedbackCommand` - safety stop
   - *Integration tests with domain services*
7. **Minimal WPF UI**

   - Connection status, FFB strength display
   - Basic scale adjustment controls
   - *Manual testing with real simulator*

### Phase 3: Profile Management + Settings

Add persistence and user configuration:

8. **Application Use Cases**

   - `SaveForceFeedbackProfileCommand`
   - `LoadForceFeedbackProfileQuery`
   - `GetAvailableSimulatorsQuery`
9. **Infrastructure: Persistence**

   - XML/SQLite repository for profiles
10. **Presentation: Settings UI**

    - Profile selection, FFB tuning panels

### Phase 4: Expand Hardware/Simulators

Add additional adapters demonstrating abstraction:

11. **Additional Gateways**

    - `AssettoCorsaGateway` (or other simulator)
    - Additional wheel protocols if needed
12. **Advanced Features**

    - Session logging, telemetry visualization
    - Voice announcements, LFE integration

### Pros

| Benefit                    | Description                                                 |
| -------------------------- | ----------------------------------------------------------- |
| **Best of Both Worlds**    | Clean domain model + early integration feedback             |
| **Proven Abstractions**    | Interfaces designed with real implementation in mind        |
| **Early Risk Mitigation**  | Hardware/timing issues discovered in Phase 2                |
| **Incremental Value**      | Working iRacing FFB before expanding to other sims          |
| **TDD Throughout**         | Unit tests for domain, integration tests for infrastructure |
| **Parallel Work Possible** | After Phase 1, UI and backend can proceed in parallel       |
| **Legacy Code Reference**  | Can port algorithms from SimRacingFFB as you go             |

### Cons

| Drawback                  | Description                                       |
| ------------------------- | ------------------------------------------------- |
| **Requires Discipline**   | Must resist shortcuts in Phase 2 integration work |
| **Phase 1 Investment**    | 1-2 weeks before any visible output               |
| **Domain Refactoring**    | May need to adjust domain after Phase 2 learnings |
| **More Complex Planning** | Need to manage phase transitions                  |

### Best For

- This project specifically, given:
    - Domain Value Objects already complete
    - Working legacy reference code available
    - Real hardware available for testing
    - TDD workflow established
    - Solo/small team development

---

## Comparison Matrix

| Criterion                        | Vertical Slice      | Inside-Out   | UI Shell First | Hybrid (Recommended) |
| -------------------------------- | ------------------- | ------------ | -------------- | -------------------- |
| **Time to First Working FFB**    | 🟢 Days              | 🔴 Weeks      | 🟡 Days (mock)  | 🟡 1-2 weeks          |
| **Clean Architecture Adherence** | 🟡 Medium            | 🟢 High       | 🔴 Low          | 🟢 High               |
| **TDD Effectiveness**            | 🟡 Integration-heavy | 🟢 Unit-heavy | 🟡 UI-focused   | 🟢 Balanced           |
| **Refactoring Safety**           | 🟡 Medium            | 🟢 High       | 🔴 Low          | 🟢 High               |
| **Risk of Over-Engineering**     | 🟢 Low               | 🔴 High       | 🟢 Low          | 🟡 Medium             |
| **Integration Issue Discovery**  | 🟢 Early             | 🔴 Late       | 🔴 Late         | 🟢 Early (Phase 2)    |
| **Mockup → Full App Transition** | 🟢 Seamless          | 🟡 Medium     | 🟢 Seamless     | 🟢 Seamless           |
| **Multi-Simulator Abstraction**  | 🟡 Reactive          | 🟢 Proactive  | 🔴 Afterthought | 🟢 Proactive          |
| **Stakeholder Visibility**       | 🟢 High              | 🔴 Low        | 🟢 High         | 🟡 Medium             |

---

## Recommendation

**Use Approach 4: Hybrid (Domain Core + Vertical Slices)** for this project because:

1. **Builds on Completed Work** - Value Objects are done; natural next step is entities and services
2. **Leverages Legacy Code** - Can port `ForceFeedbackCalculationService` algorithms from SimRacingFFB
3. **Early Integration Testing** - Phase 2 validates DirectInput/iRacing integration before building more
4. **TDD Workflow Matches** - Phase 1 is pure TDD with mocks; Phase 2 adds integration tests
5. **Multi-Sim Ready** - Interfaces designed before implementation prevents iRacing-specific leakage
6. **Incremental Transformation**Building Tradeoffs: Approaches for Racing Sim FFB Development

   This document analyzes different approaches to building the Racing Sim FFB application, considering:

   - Test-Driven Development (TDD) as the core methodology
   - Domain Layer Value Objects already completed
   - Support for multiple racing simulators (starting with iRacing)
   - Support for multiple steering wheel hardware (Simucube, Fanatec, etc.)
   - Goal: Working mockup that can be incrementally transformed into the full application

   ## Current State

   **Completed:**

   - Domain Layer Value Objects (EngineRPM, ForceFeedbackVector, GForce, PidConfig, Scale, ShockVelocity, Speed, SteeringWheelAngle, TelemetryDataPoint, TireLoad, Torque, Velocity, WheelPosition, WheelVelocity, YawRate, YawRateFactor)
   - Project structure with Clean Architecture layers
   - Unit tests for all Value Objects

   **Reference Implementation:**

   - Legacy SimRacingFFB application (forked from Marvin's AIRA)
   - GitHub: https://github.com/edswafford/SimRacingFFB

   ---

   ## Approach 1: Vertical Slice (Feature-First with iRacing)

   ### Description

   Build complete vertical slices through all layers for one feature at a time. Start with the core FFB loop for iRacing only, then expand horizontally to add more simulators and features.

   **Build Order:**

   1. **Minimal FFB Loop Slice**

      - Domain: `ISimulatorTelemetryGateway`, `IForceFeedbackPort` interfaces
      - Domain: `TelemetryFeed` entity (basic)
      - Application: `GenerateForceFeedbackCommand` use case (simplified)
      - Infrastructure: `IRacingTelemetryGateway` (read shared memory)
      - Infrastructure: `DirectInputForceFeedbackPort` (write constant force)
      - Presentation: Minimal WPF window showing connection status
   1. **Expand FFB Calculation**

      - Domain: `ForceFeedbackCalculationService` with real algorithms
      - Application: Complete `GenerateForceFeedbackCommand` with all processing
   1. **Add UI for Tuning**

      - Presentation: ViewModels for FFB scales, profiles
      - Application: `SaveForceFeedbackProfileCommand`, `LoadForceFeedbackProfileQuery`
   1. **Add Second Simulator**

      - Infrastructure: `AssettoCorseGateway` (demonstrates abstraction works)

   ### Pros

   | Benefit                         | Description                                                           |
   | ------------------------------- | --------------------------------------------------------------------- |
   | **Early Working Software**      | Real FFB output within days, not weeks                                |
   | **Validates Architecture**      | Proves abstractions work end-to-end before investing in them          |
   | **User Feedback Loop**          | Can test with real hardware/simulator immediately                     |
   | **Reduces Risk**                | Discovers integration issues early (DirectInput quirks, timing, etc.) |
   | **TDD Friendly**                | Each slice has clear acceptance criteria: "telemetry in → FFB out"    |
   | **Mockup Transforms Naturally** | The "mockup" IS the application from day one                          |

   ### Cons

   | Drawback                         | Description                                                   |
   | -------------------------------- | ------------------------------------------------------------- |
   | **Refactoring Overhead**         | May need to refactor early code as patterns emerge            |
   | **Domain Purity Risk**           | Temptation to add iRacing-specific logic to domain layer      |
   | **Parallel Development Harder**  | Difficult to split work among multiple developers             |
   | **Infrastructure-Coupled Tests** | Early tests may be more integration-focused than unit-focused |
   | **Tight Coupling Risk**          | Without discipline, layers may become entangled               |

   ### Best For

   - Solo developers or small teams
   - Projects with high technical uncertainty (hardware/SDK integration)
   - When stakeholder feedback is critical early
   - When you have working reference code (legacy SimRacingFFB)

   ---

   ## Approach 2: Inside-Out (Domain-Driven with Mockups)

   ### Description

   Continue building outward from the completed Value Objects. Build complete domain layer first, then application layer with mock infrastructure, then real infrastructure, and finally presentation. Each layer is tested in isolation before integration.

   **Build Order:**

   1. **Complete Domain Layer**

      - Domain Entities: `Simulator`, `ForceFeedbackWheel`, `TelemetryFeed`, `Car`, `DriverSession`
      - Domain Services: `ForceFeedbackCalculationService`, `TelemetryProcessingService`
      - Domain Interfaces: `ISimulatorTelemetryGateway`, `IForceFeedbackPort`, `IConfigurationRepository`
      - *All with unit tests using mocks*
   1. **Complete Application Layer**

      - Use Cases: `GenerateForceFeedbackCommand`, `StopForceFeedbackCommand`, `SaveProfileCommand`, etc.
      - Application Services orchestrating domain services
      - *Tests using mock infrastructure implementations*
   1. **Build Infrastructure Layer**

      - Real implementations: `IRacingTelemetryGateway`, `DirectInputForceFeedbackPort`
      - Integration tests against real hardware/simulators
      - *Mockup UI can be built in parallel*
   1. **Build Presentation Layer**

      - WPF Views and ViewModels
      - Wire to real use cases
      - End-to-end testing

   ### Pros

   | Benefit                       | Description                                                           |
   | ----------------------------- | --------------------------------------------------------------------- |
   | **Clean Architecture Purity** | Strict adherence to dependency rule throughout                        |
   | **Highly Testable**           | Each layer can be tested in complete isolation                        |
   | **Domain Model Excellence**   | Time to think through domain concepts without infrastructure pressure |
   | **Parallel Development**      | Different developers can work on different layers                     |
   | **Documentation Value**       | Domain layer serves as executable specification                       |
   | **Refactoring Safety**        | Changes to infrastructure don't affect core logic                     |

   ### Cons

   | Drawback                  | Description                                                       |
   | ------------------------- | ----------------------------------------------------------------- |
   | **Delayed Integration**   | Real hardware testing comes late, integration issues surface late |
   | **Over-Engineering Risk** | May build abstractions that don't fit real requirements           |
   | **Speculative Design**    | Domain design without integration feedback may miss the mark      |
   | **Late User Feedback**    | No working software until all layers complete                     |
   | **Mock Maintenance**      | Large investment in mock objects that are discarded               |
   | **Analysis Paralysis**    | Temptation to perfect domain before moving on                     |

   ### Best For

   - Teams with Clean Architecture experience
   - Projects with well-understood domains (this has legacy reference)
   - When you need strong test coverage guarantees
   - Larger teams that can parallelize layer development

   ---

   ## Approach 3: UI Shell First (Outside-In with Walking Skeleton)

   ### Description

   Build a "walking skeleton" — a minimal end-to-end implementation with the UI shell first, using mock/simulated data. Then progressively replace mocks with real implementations. The UI drives what needs to be built.

   **Build Order:**

   1. **WPF Shell with Mock Data**

      - Presentation: Complete UI layout with all planned features
      - ViewModels with hardcoded/simulated data
      - Visualizes what the app will look like
      - *Manual testing, possibly automated UI tests*
   1. **Add Application Layer Interface**

      - Define use case interfaces that ViewModels call
      - Mock implementations return simulated data
      - ViewModels bind to use case results
   1. **Implement Domain & Infrastructure for Each Feature**

      - Pick a UI feature (e.g., "Show Current FFB Strength")
      - Implement the complete stack: Domain → Application → Infrastructure
      - Replace mock with real implementation
      - *TDD for each feature slice*
   1. **Iterate Until All Features Real**

      - Systematically replace mocks with real implementations
      - Each iteration adds working functionality

   ### Pros

   | Benefit                        | Description                                               |
   | ------------------------------ | --------------------------------------------------------- |
   | **Immediate Visual Prototype** | Stakeholder feedback on UI/UX before backend work         |
   | **UI-Driven Design**           | Implementation driven by actual UI needs, not speculation |
   | **Mockup IS the App**          | Shell becomes the real application incrementally          |
   | **Clear Progress Visibility**  | Easy to see what's working vs. mocked                     |
   | **Feature Prioritization**     | Can implement high-value features first                   |
   | **User Testing Early**         | Can do usability testing with mock data                   |

   ### Cons

   | Drawback                         | Description                                                 |
   | -------------------------------- | ----------------------------------------------------------- |
   | **UI Churn**                     | UI may need redesign as backend realities emerge            |
   | **Presentation Layer Pollution** | Risk of business logic leaking into ViewModels              |
   | **Mock Explosion**               | Many mocks needed for complex UI scenarios                  |
   | **Domain Afterthought**          | Domain model may be shaped by UI rather than business rules |
   | **Integration Testing Late**     | Real hardware/simulator testing comes last                  |
   | **WPF Overhead**                 | Significant UI framework knowledge needed upfront           |

   ### Best For

   - Consumer applications where UX is critical
   - Projects with non-technical stakeholders who need to see progress
   - When UI requirements are uncertain
   - Teams with strong WPF/MVVM experience

   ---

   ## Approach 4: Hybrid — Domain Core + Vertical Slices (Recommended)

   ### Description

   A pragmatic hybrid approach: Complete the domain core (entities, services, interfaces) with mocks, then build vertical slices for each simulator/hardware combination. Leverages the completed Value Objects and provides early integration feedback.

   **Build Order:**

   ### Phase 1: Domain Core Completion (1-2 weeks)

   Build the domain layer foundation that all features will use:

   1. **Domain Entities** (TDD with mocks)

      - `TelemetryFeed` - connection state machine
      - `ForceFeedbackWheel` - wheel identity and state
      - `Simulator` - simulator identity and capabilities
      - `DriverSession` - session lifecycle
   1. **Domain Interfaces** (contracts only)

      - `ISimulatorTelemetryGateway` - port for simulator adapters
      - `IForceFeedbackPort` - port for wheel output
      - `IConfigurationRepository` - port for persistence
   1. **Domain Services** (TDD with mocks)

      - `ForceFeedbackCalculationService` - core FFB algorithm
      - `TelemetryProcessingService` - telemetry normalization

   ### Phase 2: iRacing Vertical Slice (2-3 weeks)

   Complete end-to-end for iRacing with real hardware:

   4. **Infrastructure: iRacing Adapter**

      - `IRacingTelemetryGateway` implementing `ISimulatorTelemetryGateway`
      - Reads shared memory, emits `TelemetryDataPoint` value objects
      - *Integration tests with real/simulated iRacing data*
   5. **Infrastructure: DirectInput FFB**

      - `DirectInputForceFeedbackPort` implementing `IForceFeedbackPort`
      - Constant force effect with multimedia timer (360Hz)
      - *Integration tests with real hardware*
   6. **Application Use Cases**

      - `GenerateForceFeedbackCommand` - main FFB loop
      - `StopForceFeedbackCommand` - safety stop
      - *Integration tests with domain services*
   7. **Minimal WPF UI**

      - Connection status, FFB strength display
      - Basic scale adjustment controls
      - *Manual testing with real simulator*

   ### Phase 3: Profile Management + Settings

   Add persistence and user configuration:

   8. **Application Use Cases**

      - `SaveForceFeedbackProfileCommand`
      - `LoadForceFeedbackProfileQuery`
      - `GetAvailableSimulatorsQuery`
   9. **Infrastructure: Persistence**

      - XML/SQLite repository for profiles
   10. **Presentation: Settings UI**

       - Profile selection, FFB tuning panels

   ### Phase 4: Expand Hardware/Simulators

   Add additional adapters demonstrating abstraction:

   11. **Additional Gateways**

       - `AssettoCorsaGateway` (or other simulator)
       - Additional wheel protocols if needed
   12. **Advanced Features**

       - Session logging, telemetry visualization
       - Voice announcements, LFE integration

   ### Pros

   | Benefit                    | Description                                                 |
   | -------------------------- | ----------------------------------------------------------- |
   | **Best of Both Worlds**    | Clean domain model + early integration feedback             |
   | **Proven Abstractions**    | Interfaces designed with real implementation in mind        |
   | **Early Risk Mitigation**  | Hardware/timing issues discovered in Phase 2                |
   | **Incremental Value**      | Working iRacing FFB before expanding to other sims          |
   | **TDD Throughout**         | Unit tests for domain, integration tests for infrastructure |
   | **Parallel Work Possible** | After Phase 1, UI and backend can proceed in parallel       |
   | **Legacy Code Reference**  | Can port algorithms from SimRacingFFB as you go             |

   ### Cons

   | Drawback                  | Description                                       |
   | ------------------------- | ------------------------------------------------- |
   | **Requires Discipline**   | Must resist shortcuts in Phase 2 integration work |
   | **Phase 1 Investment**    | 1-2 weeks before any visible output               |
   | **Domain Refactoring**    | May need to adjust domain after Phase 2 learnings |
   | **More Complex Planning** | Need to manage phase transitions                  |

   ### Best For

   - This project specifically, given:
       - Domain Value Objects already complete
       - Working legacy reference code available
       - Real hardware available for testing
       - TDD workflow established
       - Solo/small team development

   ---

   ## Comparison Matrix

   | Criterion                        | Vertical Slice      | Inside-Out   | UI Shell First | Hybrid (Recommended) |
   | -------------------------------- | ------------------- | ------------ | -------------- | -------------------- |
   | **Time to First Working FFB**    | 🟢 Days              | 🔴 Weeks      | 🟡 Days (mock)  | 🟡 1-2 weeks          |
   | **Clean Architecture Adherence** | 🟡 Medium            | 🟢 High       | 🔴 Low          | 🟢 High               |
   | **TDD Effectiveness**            | 🟡 Integration-heavy | 🟢 Unit-heavy | 🟡 UI-focused   | 🟢 Balanced           |
   | **Refactoring Safety**           | 🟡 Medium            | 🟢 High       | 🔴 Low          | 🟢 High               |
   | **Risk of Over-Engineering**     | 🟢 Low               | 🔴 High       | 🟢 Low          | 🟡 Medium             |
   | **Integration Issue Discovery**  | 🟢 Early             | 🔴 Late       | 🔴 Late         | 🟢 Early (Phase 2)    |
   | **Mockup → Full App Transition** | 🟢 Seamless          | 🟡 Medium     | 🟢 Seamless     | 🟢 Seamless           |
   | **Multi-Simulator Abstraction**  | 🟡 Reactive          | 🟢 Proactive  | 🔴 Afterthought | 🟢 Proactive          |
   | **Stakeholder Visibility**       | 🟢 High              | 🔴 Low        | 🟢 High         | 🟡 Medium             |

   ---

   ## Recommendation

   **Use Approach 4: Hybrid (Domain Core + Vertical Slices)** for this project because:

   1. **Builds on Completed Work** - Value Objects are done; natural next step is entities and services
   2. **Leverages Legacy Code** - Can port `ForceFeedbackCalculationService` algorithms from SimRacingFFB
   3. **Early Integration Testing** - Phase 2 validates DirectInput/iRacing integration before building more
   4. **TDD Workflow Matches** - Phase 1 is pure TDD with mocks; Phase 2 adds integration tests
   5. **Multi-Sim Ready** - Interfaces designed before implementation prevents iRacing-specific leakage
   6. **Incremental Transformation** - Each phase produces working, shippable software

   ### Suggested First Steps

   1. Create Domain Entities with TDD (start with `TelemetryFeed` as it's well-defined in design doc)
   2. Define `ISimulatorTelemetryGateway` interface based on legacy `OnTelemetryData()` method
   3. Implement `ForceFeedbackCalculationService` by porting from legacy `UpdateForceFeedback()`
   4. Build `IRacingTelemetryGateway` with integration tests against iRacing

   ---

   ## References

   - [Racing Simulator Force Feedback Design](../Racing%20Simulator%20Force%20Feedback%20Design.md)
   - [Legacy SimRacingFFB Analysis](../LegacySimRacingFFB.md)
   - [GitHub: edswafford/SimRacingFFB](https://github.com/edswafford/SimRacingFFB)
   - [IDesign C# Coding Standard](../IDesign%20C%23%20Coding%20Standard.md)

    - Each phase produces working, shippable software

### Suggested First Steps

1. Create Domain Entities with TDD (start with `TelemetryFeed` as it's well-defined in design doc)
2. Define `ISimulatorTelemetryGateway` interface based on legacy `OnTelemetryData()` method
3. Implement `ForceFeedbackCalculationService` by porting from legacy `UpdateForceFeedback()`
4. Build `IRacingTelemetryGateway` with integration tests against iRacing

---

## References

- [Racing Simulator Force Feedback Design](../Racing%20Simulator%20Force%20Feedback%20Design.md)
- [Legacy SimRacingFFB Analysis](../LegacySimRacingFFB.md)
- [GitHub: edswafford/SimRacingFFB](https://github.com/edswafford/SimRacingFFB)
- [IDesign C# Coding Standard](../IDesign%20C%23%20Coding%20Standard.md)