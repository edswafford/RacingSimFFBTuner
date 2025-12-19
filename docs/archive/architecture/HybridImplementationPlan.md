# Hybrid Implementation Plan: Domain Core + Vertical Slices

This document outlines the implementation plan for building the Racing Sim FFB application using a hybrid approach that combines Clean Architecture domain modeling with vertical feature slices.

## Overview

**Strategy:** Complete the domain core (entities, services, interfaces) with mocks using TDD, then build vertical slices for each simulator/hardware combination. This leverages the completed Value Objects and provides early integration feedback.

## Current State

**Completed:**
- Domain Layer Value Objects (EngineRPM, ForceFeedbackVector, GForce, PidConfig, Scale, ShockVelocity, Speed, SteeringWheelAngle, TelemetryDataPoint, TireLoad, Torque, Velocity, WheelPosition, WheelVelocity, YawRate, YawRateFactor)
- Project structure with Clean Architecture layers
- Unit tests for all Value Objects

**Reference Implementation:**
- Legacy SimRacingFFB application (forked from Marvin's AIRA)
- GitHub: https://github.com/edswafford/SimRacingFFB

## Why This Approach

1. **Builds on Completed Work** - Value Objects are done; natural next step is entities and services
2. **Leverages Legacy Code** - Can port `ForceFeedbackCalculationService` algorithms from SimRacingFFB
3. **Early Integration Testing** - Phase 2 validates DirectInput/iRacing integration before building more
4. **TDD Workflow Matches** - Phase 1 is pure TDD with mocks; Phase 2 adds integration tests
5. **Multi-Sim Ready** - Interfaces designed before implementation prevents iRacing-specific leakage
6. **Incremental Transformation** - Each phase produces working, shippable software

---

## Phase 1: Domain Core Completion (1-2 weeks)

Build the domain layer foundation that all features will use. All work in this phase uses TDD with mocks.

### 1.1 Domain Entities (TDD with mocks)

**TelemetryFeed**
- Represents the source connection to a specific simulator
- **Identity:** Simulator identifier (e.g., "iRacing")
- **State:** Connected, Connecting, Disconnected
- **Behavior:** `Connect()`, `Disconnect()`, `IsConnected`
- **Tests:** State transitions, connection lifecycle

**ForceFeedbackWheel**
- Represents the physical FFB wheel hardware
- **Identity:** Device GUID or unique identifier
- **State:** Connected, Calibrated, ConnectionStatus
- **Behavior:** `Connect()`, `Disconnect()`, `GetCapabilities()`
- **Tests:** Device identification, state management

**Simulator**
- Represents a specific racing simulator
- **Identity:** Unique ID or name (e.g., "iRacing", "Assetto Corsa")
- **State:** Installed, Running, Paused
- **Behavior:** `Detect()`, `GetStatus()`
- **Tests:** Simulator detection, status tracking

**DriverSession**
- Represents a single, continuous driving session
- **Identity:** Guid (Session ID)
- **State:** InProgress, Paused, Completed
- **Behavior:** `StartSession()`, `EndSession()`, `Pause()`, `Resume()`
- **Tests:** Session lifecycle, state transitions

**Car** (optional for Phase 1)
- Represents the vehicle in the simulation
- **Identity:** Car identifier (e.g., "Dallara F3")
- **State:** Current car attributes (weight, suspension, etc.)
- **Tests:** Car identification, attribute management

### 1.2 Domain Interfaces (contracts only)

**ISimulatorTelemetryGateway**
- Port for simulator adapters to implement
- Methods: `ConnectAsync()`, `DisconnectAsync()`, `ReadTelemetryAsync()`, `IsConnected`
- Returns: `TelemetryDataPoint` value objects
- **Reference:** Legacy `OnTelemetryData()` method signature

**IForceFeedbackPort**
- Port for wheel output adapters to implement
- Methods: `ConnectAsync()`, `DisconnectAsync()`, `SendForceFeedback(ForceFeedbackVector)`, `IsConnected`
- **Reference:** Legacy `UpdateConstantForce()` and DirectInput interaction

**IConfigurationRepository**
- Port for persistence adapters
- Methods: `SaveProfile(ForceFeedbackProfile)`, `LoadProfile(Guid)`, `GetAvailableProfiles()`
- **Reference:** Legacy `SaveForceFeedbackProfileCommand`, `LoadForceFeedbackProfileQuery`

**ILoggingService** (optional for Phase 1)
- Port for logging framework
- Methods: `LogInfo()`, `LogError()`, `LogWarning()`

### 1.3 Domain Services (TDD with mocks)

**ForceFeedbackCalculationService**
- Core FFB algorithm that processes telemetry and calculates force
- **Input:** `TelemetryDataPoint` value objects
- **Output:** `ForceFeedbackVector` value objects
- **Algorithm:** Port from legacy `UpdateForceFeedback()` method
- **Key Features:**
  - Yaw rate factor processing
  - G-force tracking
  - Shock velocity processing
  - Understeer/oversteer detection
  - Delta torque calculation
  - Steady-state torque smoothing
  - Overall and detail scale application
- **Tests:** Unit tests with mock telemetry data, verify force calculations

**TelemetryProcessingService**
- Coordinates telemetry retrieval and normalization
- **Input:** Raw telemetry from `ISimulatorTelemetryGateway`
- **Output:** Normalized `TelemetryDataPoint` value objects
- **Behavior:** Handles telemetry validation, missing data, rate limiting
- **Tests:** Unit tests with mock gateway, verify normalization

**ConnectionManagementService** (optional for Phase 1)
- Manages simulator detection and connection lifecycle
- **Behavior:** Detects running simulators, manages `TelemetryFeed` entities
- **Tests:** Unit tests with mock gateways, verify connection state

### Phase 1 Deliverables

- ✅ All domain entities with unit tests
- ✅ All domain interfaces defined
- ✅ Core domain services with unit tests
- ✅ Clean Architecture dependency rule enforced
- ✅ No infrastructure dependencies in domain layer

---

## Phase 2: iRacing Vertical Slice (2-3 weeks)

Complete end-to-end for iRacing with real hardware. This phase validates the architecture and discovers integration issues early.

### 2.1 Infrastructure: iRacing Adapter

**IRacingTelemetryGateway**
- Implements `ISimulatorTelemetryGateway`
- Reads iRacing shared memory (IRSDK)
- Emits `TelemetryDataPoint` value objects
- **Key Features:**
  - Shared memory mapping
  - Telemetry data parsing
  - Connection state management
  - Error handling and reconnection
- **Reference:** Legacy `App.IRacingSDK.cs`, `OnTelemetryData()` method
- **Tests:** Integration tests with real/simulated iRacing data

**Implementation Steps:**
1. Create `IRacingTelemetryGateway` class in Infrastructure layer
2. Implement shared memory reading (IRSDK wrapper)
3. Map iRacing data to `TelemetryDataPoint` value objects
4. Implement connection/disconnection logic
5. Write integration tests (can use recorded telemetry data)

### 2.2 Infrastructure: DirectInput FFB

**DirectInputForceFeedbackPort**
- Implements `IForceFeedbackPort`
- Uses DirectInput constant force effect
- Multimedia timer for 360Hz updates
- **Key Features:**
  - Device enumeration and selection
  - Constant force effect creation
  - High-frequency force updates (360Hz)
  - Force interpolation between samples
  - Error handling and device reconnection
- **Reference:** Legacy `App.ForceFeedback.cs`, `FFBMultimediaTimerEventCallback()`
- **Tests:** Integration tests with real hardware (or hardware simulator)

**Implementation Steps:**
1. Create `DirectInputForceFeedbackPort` class in Infrastructure layer
2. Implement DirectInput device enumeration
3. Create constant force effect
4. Implement multimedia timer callback (360Hz)
5. Add force interpolation logic
6. Write integration tests

### 2.3 Application Use Cases

**GenerateForceFeedbackCommand**
- Main FFB loop use case
- **Orchestration:**
  1. Read telemetry from `ISimulatorTelemetryGateway`
  2. Process telemetry via `TelemetryProcessingService`
  3. Calculate force via `ForceFeedbackCalculationService`
  4. Send force to `IForceFeedbackPort`
- **Input:** None (runs continuously)
- **Output:** Success/Failure status
- **Reference:** Legacy `UpdateForceFeedback()` orchestration
- **Tests:** Integration tests with domain services and mock infrastructure

**StopForceFeedbackCommand**
- Safety stop use case
- **Orchestration:**
  1. Create zeroed `ForceFeedbackVector`
  2. Send to `IForceFeedbackPort`
  3. Optionally disconnect `ISimulatorTelemetryGateway`
- **Input:** None
- **Output:** Success/Failure status
- **Tests:** Unit tests with mocks, integration tests

**Implementation Steps:**
1. Create use case classes in Application layer
2. Implement orchestration logic
3. Wire to domain services via dependency injection
4. Write unit tests with mocks
5. Write integration tests with real adapters

### 2.4 Minimal WPF UI

**MainWindow ViewModel**
- Connection status display
- FFB strength/force visualization
- Basic scale adjustment controls (Overall, Detail)
- Start/Stop FFB button
- **Reference:** Legacy `MainWindow.xaml.cs` for UI patterns

**MainWindow View**
- Simple WPF layout
- Data binding to ViewModel
- Real-time force feedback visualization (optional)

**Dependency Injection Setup**
- Configure DI container in `App.xaml.cs`
- Register domain services
- Register infrastructure adapters
- Register use cases
- Wire ViewModels to use cases

**Implementation Steps:**
1. Create `MainViewModel` in Presentation layer
2. Create minimal `MainWindow.xaml` layout
3. Implement data binding
4. Set up DI container (e.g., Microsoft.Extensions.DependencyInjection)
5. Wire ViewModel to use cases
6. Manual testing with real simulator and hardware

### Phase 2 Deliverables

- ✅ Working iRacing telemetry reading
- ✅ Working DirectInput force feedback output
- ✅ Complete FFB loop: telemetry → calculation → output
- ✅ Minimal UI showing connection and force
- ✅ Integration tests for hardware/simulator interaction
- ✅ **Milestone: Real FFB output with iRacing!**

---

## Phase 3: Profile Management + Settings (1-2 weeks)

Add persistence and user configuration to make the application usable for real tuning.

### 3.1 Application Use Cases

**SaveForceFeedbackProfileCommand**
- Persists user-defined FFB settings
- **Input:** ForceFeedbackProfile entity
- **Orchestration:**
  1. Validate profile (domain rules)
  2. Save via `IConfigurationRepository`
- **Tests:** Unit tests with mock repository

**LoadForceFeedbackProfileQuery**
- Retrieves existing FFB profile
- **Input:** Profile ID or search criteria
- **Orchestration:**
  1. Query `IConfigurationRepository`
  2. Return profile entity or DTO
- **Tests:** Unit tests with mock repository

**GetAvailableSimulatorsQuery**
- Lists supported simulators and their status
- **Orchestration:**
  1. Query all `ISimulatorTelemetryGateway` implementations
  2. Return simulator list with status
- **Tests:** Unit tests with mock gateways

### 3.2 Infrastructure: Persistence

**XmlConfigurationRepository** or **SqliteConfigurationRepository**
- Implements `IConfigurationRepository`
- Stores FFB profiles, settings
- **Reference:** Legacy `Serializer.cs`, `Settings.cs`
- **Tests:** Integration tests with file system

**Implementation Choice:**
- Start with XML (simpler, matches legacy)
- Can migrate to SQLite later if needed

### 3.3 Presentation: Settings UI

**SettingsViewModel**
- Profile selection and management
- FFB tuning panels (scales, effects, etc.)
- Car/Track/Wheel selection
- **Reference:** Legacy settings UI patterns

**Settings Views**
- Profile list/selection
- FFB tuning controls
- Simulator selection
- Wheel selection

### Phase 3 Deliverables

- ✅ Profile save/load functionality
- ✅ Settings persistence
- ✅ Settings UI for tuning
- ✅ Profile management UI

---

## Phase 4: Expand Hardware/Simulators (Ongoing)

Add additional adapters demonstrating the abstraction works. Each new simulator/hardware is a small vertical slice.

### 4.1 Additional Simulator Gateways

**AssettoCorsaGateway** (or other simulator)
- Implements `ISimulatorTelemetryGateway`
- Uses Assetto Corsa shared memory or UDP
- **Tests:** Integration tests with simulator

**Pattern:**
1. Create new gateway class
2. Implement `ISimulatorTelemetryGateway` interface
3. Map simulator-specific data to `TelemetryDataPoint`
4. Add to DI container
5. Test with real simulator

### 4.2 Additional Wheel Protocols (if needed)

**SimucubeForceFeedbackPort** (if DirectInput insufficient)
- Implements `IForceFeedbackPort`
- Uses Simucube SDK/protocol
- **Tests:** Integration tests with hardware

**Pattern:**
1. Create new port class
2. Implement `IForceFeedbackPort` interface
3. Use manufacturer SDK/protocol
4. Add to DI container
5. Test with real hardware

### 4.3 Advanced Features

**Session Logging**
- Log telemetry data to file/database
- Replay capabilities
- **Reference:** Legacy FFB recording/playback

**Telemetry Visualization**
- Real-time graphs and charts
- **Reference:** Legacy `_ffb_writeableBitmap` visualization

**Voice Announcements**
- Text-to-speech for events
- **Reference:** Legacy `App.Voice.cs`

**LFE Integration**
- Low Frequency Effects from audio
- **Reference:** Legacy `App.LFE.cs`

### Phase 4 Deliverables

- ✅ Multiple simulator support
- ✅ Multiple wheel hardware support
- ✅ Advanced features as needed
- ✅ **Milestone: Multi-simulator, multi-hardware support!**

---

## Testing Strategy

### Phase 1: Unit Tests Only
- All domain entities: state transitions, behavior
- All domain services: business logic with mocks
- **Coverage Target:** 90%+ for domain layer

### Phase 2: Unit + Integration Tests
- Unit tests for use cases (with mocks)
- Integration tests for infrastructure adapters
- **Coverage Target:** 80%+ overall, 90%+ domain

### Phase 3: Unit + Integration + Manual
- Unit tests for new use cases
- Integration tests for persistence
- Manual testing for UI workflows

### Phase 4: Integration Tests
- Integration tests for each new adapter
- End-to-end tests for multi-simulator scenarios

---

## Key Principles

1. **TDD First** - Write tests before implementation at every phase
2. **Clean Architecture** - Dependency rule enforced: dependencies point inward
3. **No Infrastructure in Domain** - Domain layer has zero framework dependencies
4. **Interface-Driven** - Design interfaces before implementations
5. **Incremental Value** - Each phase produces working, testable software
6. **Legacy Code Reference** - Port algorithms and patterns from SimRacingFFB

---

## Suggested First Steps

1. **Create Domain Entities with TDD**
   - Start with `TelemetryFeed` (well-defined in design doc)
   - Write tests first, then implementation
   - Follow existing Value Object patterns

2. **Define Domain Interfaces**
   - `ISimulatorTelemetryGateway` based on legacy `OnTelemetryData()` method
   - `IForceFeedbackPort` based on legacy DirectInput interaction
   - Keep interfaces focused and simulator-agnostic

3. **Implement Domain Services**
   - `ForceFeedbackCalculationService` by porting from legacy `UpdateForceFeedback()`
   - Extract algorithms, keep domain logic pure
   - Write comprehensive unit tests

4. **Build iRacing Adapter**
   - `IRacingTelemetryGateway` with integration tests
   - Use IRSDK or shared memory reading
   - Map to `TelemetryDataPoint` value objects

---

## References

- [Racing Simulator Force Feedback Design](../Racing%20Simulator%20Force%20Feedback%20Design.md)
- [Legacy SimRacingFFB Analysis](../LegacySimRacingFFB.md)
- [GitHub: edswafford/SimRacingFFB](https://github.com/edswafford/SimRacingFFB)
- [IDesign C# Coding Standard](../IDesign%20C%23%20Coding%20Standard.md)
