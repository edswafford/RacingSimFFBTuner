Implementation Plan

**Step-by-step Plan (Detailed)**

**Phase 0 — Setup & Conventions**

1. Initialize git repository; add .gitignore (VisualStudio, NuGet, bin/ obj/).
2. Create solution and projects following skeleton above.
3. Add README, CONTRIBUTING, and link to the IDesign C# guide. Declare target .NET version net10.0.
4. Configure solution-level Directory.Build.props to enforce analyzer rules and LangVersion.
5. Add CI pipeline stub (GitHub Actions) to run dotnet build and dotnet test on push.

*TDD tasks:* create a test project for RacingSimFFB.Core.Tests and add a first trivial test verifying the solution loads.

**Phase 1 — Prototype Shell (Mockup) — MVP**

**Goal:** small working WPF shell + DI + simple test demonstrating telemetry adapter interface mocked.

**Tasks:**

1. **Create Host & Bootstrap
   -** Implement RacingSimFFB.Host using GenericHost to wire logging, configuration, and DI.
   * Register required services and ViewModels.
2. **Create WPF Shell App (RacingSimFFB.UI)
   -** Minimal MainWindow with placeholder areas: Top toolbar, left telemetry pane, center visualization, right FFB settings.
   * Use MVVM: MainWindowViewModel with injected IAppOrchestrator.
3. **Define Core Interfaces (first TDD tests)
   -** ITelemetrySource with events or channels that publish TelemetryFrame (struct) at arbitrary rates.
   -\*\* IFFBOutput with SendFrame(FFBCommandSpan) or use Channel<FFBCommand>.
   -\*\* ISettingsRepository to store user preferences.
4. **Mock Telemetry Adapter for Prototype
   -** Implement SimulatedTelemetrySource that emits synthetic frames at 60Hz (or configurable). Provide tests asserting frames are received.
5. **UI Binding
   -** Show a live label for last telemetry timestamp and a start/stop button. Bind to IAppOrchestrator.StartAsync() and StopAsync().
6. **Unit Tests (TDD)
   -** Tests for SimulatedTelemetrySource and MainWindowViewModel behaviors.

**Deliverable:** Running WPF app with Start/Stop and a simulated telemetry stream displayed in the UI; tests passing.

**Phase 2 — Telemetry Layer**

**Goal:** Plug in real telemetry adapters (iRacing SDK, UDP parser, shared memory readers)

**Tasks:**

1. Define a compact and allocation-minimizing TelemetryFrame struct.
2. Implement adapters behind ITelemetrySource:
   * IRacing adapter (IRSDK wrapper) — provide a test shim that returns captured sample frames.
   * UDP telemetry parser(s) for other sims.
   * Shared memory reader for AC/AMS.
3. Provide robust error handling and health checks (watchdog + reconnection strategies).
4. Unit tests: parser correctness, boundary conditions, malformed packets.

**Performance notes:** read binary directly into buffers, avoid per-frame heap allocations.

**Phase 3 — FFB Pipeline & DSP**

**Goal:** Implement an extensible FFB processing pipeline.

**Design:**

* **FFB Pipeline** = sequence of processors:
  + Input normalization (map telemetry to forces)
  + Filters (low-pass, high-pass, deadzone)
  + Effect composers (rumble, slip, road texture)
  + Output limiter / scaler

**Tasks:**

1. Define IFFBProcessor interface and FFBPipeline orchestrator.
2. Implement RateLimiter and Interpolator utilities.
3. Implement a reference BasicForceCalculator that converts telemetry (steering torque, lateral accel) to force values.
4. Unit tests for processors and numeric stability.

**Hot path tips:**

* Use arrays and small structs in pipeline; avoid virtual dispatch in the 360 Hz path if possible (or use aggressive inlining).
* Consider precomputing coefficients for filters.

**Phase 4 — Device Adapters & Output**

**Goal:** Device-agnostic output layer with concrete Simucube adapter.

**Design:**

* IDeviceAdapter interface with lifecycle: Open(), Close(), Send(Span<FFBCommand>).
* Implementation: SimucubeAdapter (wrap Simucube SDK or HID commands), FanatecAdapter (if APIs available), MockDeviceAdapter for tests.

**Safety:** Add a KillSwitch mechanism to immediately disable outputs.

**Tests:** Integration tests with MockDeviceAdapter verifying message sequences.

**Phase 5 — UI: Profiles, Tuning, Visualization**

**Goal:** Expose controls for player tuning; persistent profiles; live telemetry visualization.

**Tasks:**

1. Create a Settings page to configure profile, global gain, filters, and device selection.
2. Implement ProfileManager to save/load JSON profiles (backed by ISettingsRepository).
3. Add live graph (limited history buffer) showing steering torque, lateral accel, and commanded force. Use a lightweight open-source chart control or render with DrawingVisual for performance.
4. Ensure UI updates are throttled (do not update graphs at 360Hz — sample at 60Hz or less) to avoid UI jitter.

**Phase 6 — Testing Strategy**

**Unit Tests**

* All domain logic, parsers, and pipeline processors.

**Integration Tests**

* Adapters using recorded/canned telemetry inputs.
* Device adapter stubs validating output frames.

**Performance Tests**

* Microbenchmarks for FFB loop: latency from telemetry ingestion to device send (use BenchmarkDotNet or custom high-resolution timers).

**Hardware-in-the-loop (HIL)**

* Optional tests running against real wheel in a controlled environment.

**Phase 7 — Performance Tuning & Real-time Concerns**

* **Threading model**: Use a dedicated high-priority thread for the FFB loop. Use Thread with Priority = ThreadPriority.Highest and Process priority hints. Prefer SpinWait / busy-wait with short sleeps only if required on Windows — measure carefully.
* **Buffers & Queues**: Use System.Threading.Channels or lock-free ring buffer for telemetry -> FFB pipeline handoff.
* **Avoid GC pauses**: pre-allocate buffers, use ArrayPool<T>, minimize allocations per frame.
* **Timer**: Avoid System.Timers.Timer for 360Hz exactness; run a tight loop with Stopwatch-driven sleep/yield strategy.
* **Precision**: Use Stopwatch.GetTimestamp() and convert to microseconds for scheduling.

**Safety Net:** allow users to lower FFB update rate in settings and detect when system can't sustain desired frequency.

**Phase 8 — Packaging, Installer, and Deployment**

* Use MSIX or WiX to build an installer. Include runtime checks for drivers and required permissions.
* Provide auto-update mechanism (optional).
* Ship debug logging and diagnostic export for support.