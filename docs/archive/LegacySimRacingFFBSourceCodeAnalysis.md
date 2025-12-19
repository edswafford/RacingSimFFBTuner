# Legacy SimFFB Source Code Analysis

This document provides a comprehensive analysis of the existing SimRacingFFB application, documenting methods, key properties, and important data structures with file paths and detailed synopses. This serves as a reference for implementing the new Clean Architecture design.

## Table of Contents

1. [Application Core](#1-application-core)

   - [src/SimRacingFFB/Application/App.xaml.cs](#srcsimracingffbapplicationappxamlcs)
   - [src/SimRacingFFB/Application/App.ForceFeedback.cs](#srcsimracingffbapplicationappforcefeedbackcs)
   - [src/SimRacingFFB/Application/App.Telemetry.cs](#srcsimracingffbapplicationapptelemetrycs)
   - [src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs](#srcsimracingffbsimulatorsiracingappiracingsdkcs)
1. [Hardware Integration](#2-hardware-integration)

   - [src/SimRacingFFB/Application/App.Inputs.cs](#srcsimracingffbapplicationappinputscs)
   - [src/SimRacingFFB/Application/App.SimagicHPR.cs](#srcsimracingffbapplicationappsimagichprcs)
   - [src/SimRacingFFB/Application/App.Logitech.cs](#srcsimracingffbapplicationapplogitechcs)
   - [src/SimRacingFFB/Application/App.LFE.cs](#srcsimracingffbapplicationapplfecs)
1. [Additional Features](#3-additional-features)

   - [src/SimRacingFFB/Application/App.Voice.cs](#srcsimracingffbapplicationappvoicecs)
   - [src/SimRacingFFB/Application/App.Sounds.cs](#srcsimracingffbapplicationappsoundscs)
   - [src/SimRacingFFB/Application/App.Console.cs](#srcsimracingffbapplicationappconsolecs)
   - [src/SimRacingFFB/Application/App.Service.cs](#srcsimracingffbapplicationappservicecs)
   - [src/SimRacingFFB/Application/App.ChatQueue.cs](#srcsimracingffbapplicationappchatqueuecs)
   - [src/SimRacingFFB/Application/App.CurrentCar.cs](#srcsimracingffbapplicationappcurrentcarcs)
   - [src/SimRacingFFB/Application/App.CurrentTrack.cs](#srcsimracingffbapplicationappcurrenttrackcs)
   - [src/SimRacingFFB/Application/App.WetDryCondition.cs](#srcsimracingffbapplicationappwetdryconditioncs)
1. [Supporting Infrastructure](#4-supporting-infrastructure)

   - [src/SimRacingFFB/Common/Settings.cs](#srcsimracingffbcommonsettingscs)
   - [src/SimRacingFFB/Common/Serializer.cs](#srcsimracingffbcommonserializercs)
   - [src/SimRacingFFB/Interop/WinApi.cs](#srcsimracingffbinteropwinapics)
   - [src/SimRacingFFB/Common/SerializableDictionary.cs](#srcsimracingffbcommonserializabledictionarycs)
   - [src/SimRacingFFB/Common/ClipboardHelper.cs](#srcsimracingffbcommonclipboardhelpercs)
   - [src/SimRacingFFB/Interop/DeviceChangeNotification.cs](#srcsimracingffbinteropdevicechangenotificationcs)
   - [src/SimRacingFFB/Interop/LoopStream.cs](#srcsimracingffbinteroploopstreamcs)
   - [src/SimRacingFFB/Application/App.Settings.cs](#srcsimracingffbapplicationappsettingscs)
1. [User Interface](#5-user-interface)

   - [src/SimRacingFFB/UI/Views/MainWindow.xaml.cs](#srcsimracingffbuiviewsmainwindowxamlcs)

---

## 1. Application Core

### src/SimRacingFFB/Application/App.xaml.cs

**Class:** `App` (partial)

#### Methods

**`public App()`**

- **File:** `src/SimRacingFFB/Application/App.xaml.cs`
- **Synopsis:** Constructor that enforces single-instance application behavior using a named mutex. If another instance is already running, it brings the existing window to foreground and exits. Also disables CPU throttling to ensure consistent performance for real-time FFB processing. The mutex name "MarvinsAIRA Mutex" prevents multiple instances from running simultaneously, which is critical for exclusive hardware access.

**`public void Initialize(nint windowHandle)`**

- **File:** `src/SimRacingFFB/Application/App.xaml.cs`
- **Synopsis:** Initializes all application subsystems in a specific order. Creates the documents folder if it doesn't exist, then initializes console, settings, service, voice, sounds, inputs, force feedback, LFE (Low Frequency Effects), HPR (Simagic hardware), iRacing SDK, and telemetry. This orchestration method ensures dependencies are initialized in the correct sequence. If any initialization fails, it logs the error and rethrows the exception to prevent partial initialization states.

**`public void Stop()`**

- **File:** `src/SimRacingFFB/Application/App.xaml.cs`
- **Synopsis:** Gracefully shuts down all application subsystems in reverse order of initialization. Stops telemetry, iRacing SDK, force feedback, HPR, LFE, and console. This ensures proper cleanup of resources and hardware connections. Errors during shutdown are caught and logged but don't prevent the shutdown process from completing.

**`private static int DisableThrottling()`**

- **File:** `src/SimRacingFFB/Application/App.xaml.cs`
- **Synopsis:** Disables Windows process power throttling to prevent CPU frequency scaling that could cause FFB timing issues. Uses Windows API `SetProcessInformation` with `PROCESS_POWER_THROTTLING_IGNORE_TIMER_RESOLUTION` flag to ensure the process maintains consistent CPU performance. This is critical for maintaining the 360Hz FFB update rate. Returns 0 on success or the last Win32 error code on failure.

#### Key Properties/Fields

**`public static string DocumentsFolder`**

- **Type:** `string`
- **Synopsis:** Static property that provides the path to the application's documents folder in the user's Documents directory. Used for storing settings, logs, and other persistent data. Initialized to `Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MarvinsAIRA")`.

---

### src/SimRacingFFB/Application/App.ForceFeedback.cs

**Class:** `App` (partial)

#### Methods

**`public void InitializeForceFeedback(nint windowHandle, bool isFirstInitialization = false)`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** Initializes the force feedback system by discovering and connecting to DirectInput force feedback devices. Enumerates all attached force feedback devices, selects the device matching the saved GUID (or first available if none saved), verifies constant force effect support, and creates the DirectInput joystick interface. If this is the first initialization, it also sets up the visual feedback bitmap. The method handles device selection logic, validates force feedback capabilities, and triggers device reinitialization. If the selected device is not found, it provides user feedback about the missing device.

**`private void UninitializeForceFeedback(bool disposeOfDrivingJoystick = true)`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** Cleanly shuts down the force feedback system by stopping the multimedia timer, disposing of the constant force effect, and unacquiring the DirectInput device. The `disposeOfDrivingJoystick` parameter allows partial cleanup when reinitializing (set to false) versus complete shutdown (set to true). This ensures proper resource cleanup and prevents resource leaks or device lockups.

**`public void ReinitializeForceFeedbackDevice(nint windowHandle)`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** Reinitializes the force feedback device connection without full cleanup, used for recovery from errors or device reconnection. Sets cooperative level to exclusive background mode, acquires the device, creates a new constant force effect with infinite duration and maximum gain, downloads the effect to the device, and starts a multimedia timer for high-frequency updates. The timer period is calculated from the Frequency setting (18 - Settings.Frequency milliseconds). If initialization fails, it sets flags for retry and logs the error without crashing the application.

**`public void UpdateForceFeedback(float deltaTime, bool pauseInputProcessing, nint windowHandle)`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** Main update loop for force feedback that processes user input, manages settings loading, handles auto-centering, and coordinates FFB state. Processes button presses for scale adjustments, reinitialization, and auto-overall-scale features. Manages timers for voice announcements of scale changes. Loads force feedback and steering effects settings when wheel, car, track, or condition changes, using a ranking system to find the best matching profile. Implements auto-center wheel feature when not on track, with two algorithms: velocity-based (type 0) and position-based quadratic (type 1). This method orchestrates the high-level FFB behavior but delegates actual force calculation to `UpdateForceFeedback()`.

**`private void UpdateForceFeedback()`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** Core force feedback calculation algorithm that processes telemetry data and computes the output force. Processes yaw rate factor, G-force, and shock velocities. Calculates understeer and oversteer amounts based on yaw rate factor and lateral velocity, applying curve functions and softness for counter-steering. Implements crash protection (scales forces based on G-force threshold) and curb protection (reduces detail scale based on shock velocity). Applies speed scaling for parked/low-speed scenarios. Processes 6 samples per frame (360Hz data from 60Hz tick rate) through a sophisticated algorithm: calculates delta torque, applies delta limiter, maintains steady-state torque with exponential smoothing, applies overall and detail scales differently based on detail scale value (>=100% adds impulses, <100% blends with steady-state), mixes in LFE effects, adds soft lock forces, applies understeer/oversteer effects with multiple wave styles, applies FFB curve and min/max force limits, converts to DirectInput units, and updates visualization. Also handles recording/playback of FFB data and auto-overall-scale peak tracking.

**`private void UFF_ProcessYawRateFactor()`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** Calculates the instant yaw rate factor (steering wheel angle * speed / yaw rate) which indicates how much steering input is required for a given yaw rate, used for understeer detection. Only calculates when yaw rate is significant (>= 5 degrees/second) and vehicle has forward velocity. Maintains a circular buffer of 120 samples (2 seconds at 60Hz) to calculate average yaw rate factor for skid pad analysis. The yaw rate factor is a key metric for detecting understeer conditions.

**`private void UFF_ProcessGForce()`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** Tracks peak G-force over a 2-second rolling window using a circular buffer of 120 samples. Updates the buffer index and calculates the maximum G-force value, which is used for crash protection detection. The peak G-force is displayed in the UI and used to trigger crash protection when it exceeds the configured threshold.

**`private void UFF_ProcessShocks()`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** Calculates the maximum shock velocity across all six suspension corners (CF, CR, LF, LR, RF, RR) for the current frame by examining all 6 high-frequency samples. Maintains a 2-second rolling peak using a circular buffer. The maximum shock velocity is used for curb protection detection, as high shock velocities indicate impacts with curbs or other obstacles.

**`public void DoAutoOverallScaleNow()`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** Automatically calculates and sets the overall scale based on the peak torque measured during driving. Formula: `OverallScale = 100 * WheelMaxForce / PeakTorque`. This ensures the FFB output is scaled to prevent clipping while maximizing force range. Announces the new scale value via voice, using appropriate precision (1 decimal for values < 10, whole numbers otherwise). Resets the peak tracking after applying the scale.

**`private float UpdateScale(float scale, float direction)`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** Adjusts a scale value (overall, detail, or LFE) with variable step sizes based on the current value. For increases: 0.1 for <10, 1 for 10-50, 2 for 50-100, 5 for 100-150, 10 for >150. For decreases: reverse logic. After adjustment, rounds to appropriate precision: 1 decimal for <10, whole numbers for 10-50, even numbers for 50-100, multiples of 5 for 100-150, multiples of 10 for >150. This provides fine control at low values and coarse control at high values, matching user expectations for slider behavior.

**`public void UpdateConstantForce(int[] forceMagnitudeList)`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** Updates the DirectInput force magnitude buffer with a list of force values, cycling through the list if it's shorter than the buffer. Resets internal FFB state (previous torque, running torque, steady-state torque) and sets flags to skip updates and reset the timer. This is used for test signals and auto-center wheel feature, allowing immediate force changes without going through the normal calculation pipeline.

**`private static void FFBMultimediaTimerEventCallback(uint id, uint msg, ref uint userCtx, uint rsv1, uint rsv2)`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** High-frequency callback (typically 360Hz) that sends force feedback commands to the hardware. Uses a stopwatch to measure actual elapsed time and only processes if at least 0.25ms has passed (prevents excessive CPU usage). Calculates the current position in the force buffer using timer-based interpolation, performs cubic Hermite interpolation between buffer samples for smooth force transitions, applies clipping detection and cooldown logic (gradually reduces force when going off-track to prevent sudden stops), and updates the DirectInput constant force effect parameters. Handles exceptions by flagging for reinitialization rather than crashing. The interpolation ensures smooth force delivery between the 6 samples per frame.

**`[MethodImpl(MethodImplOptions.AggressiveInlining)] static float InterpolateHermite(float v0, float v1, float v2, float v3, float t)`**

- **File:** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
- **Synopsis:** Cubic Hermite interpolation function for smooth force transitions between discrete samples. Takes four control points (v0, v1, v2, v3) and interpolation parameter t (0-1), returns interpolated value. Uses the standard Hermite interpolation formula with coefficients calculated from the control points. Marked with AggressiveInlining for performance since it's called at 360Hz. This provides smooth force feedback between the 6 samples per frame, eliminating stepping artifacts.

#### Key Properties/Fields

**`public readonly float[] _ffb_recordedTorqueNMBuffer`**

- **Type:** `float[]`
- **Synopsis:** Circular buffer for recording FFB torque data for playback. Size: `FFB_SAMPLES_PER_FRAME * IRSDK_TICK_RATE * 60 * 10` (6 * 60 * 60 * 10 = 2,160,000 samples = 10 minutes at 360Hz). Used for recording and playback features.

**`private readonly int[] _ffb_outputDI`**

- **Type:** `int[]`
- **Synopsis:** DirectInput force magnitude buffer containing 7 values (one extra for interpolation). Updated at 60Hz with 6 samples per frame, then interpolated at 360Hz by the multimedia timer callback.

**`private float _ffb_runningTorqueNM`**

- **Type:** `float`
- **Synopsis:** Running accumulator for processed torque that includes impulse scaling and steady-state blending. This is the primary output value before final scaling and effects are applied.

**`private float _ffb_rawSteadyStateTorqueNM`**

- **Type:** `float`
- **Synopsis:** Low-pass filtered version of input torque used for steady-state force calculation. Maintained with exponential smoothing (90% previous + 10% current) to separate steady-state from transient forces.

**`private float _ffb_steadyStateTorqueNM`**

- **Type:** `float`
- **Synopsis:** Scaled steady-state torque that has overall scale applied. Used for blending when detail scale < 100% and for understeer/oversteer effect calculations.

**`private readonly float[] _ffb_yawRateFactorBuffer`**

- **Type:** `float[]`
- **Synopsis:** Circular buffer of 120 samples (2 seconds at 60Hz) for calculating average yaw rate factor. Used for skid pad analysis and understeer detection calibration.

**`private readonly float[] _ffb_gForceBuffer`**

- **Type:** `float[]`
- **Synopsis:** Circular buffer of 120 samples for tracking peak G-force over 2 seconds. Used for crash protection and UI display.

**`private readonly float[] _ffb_maxShockVelBuffer`**

- **Type:** `float[]`
- **Synopsis:** Circular buffer of 120 samples for tracking peak shock velocity over 2 seconds. Used for curb protection detection.

**`public readonly WriteableBitmap _ffb_writeableBitmap`**

- **Type:** `WriteableBitmap`
- **Synopsis:** Bitmap for visualizing FFB input and output signals in real-time. Dimensions: 978x200 pixels at 96 DPI. Used for debugging and user feedback.

**`public readonly byte[] _ffb_pixels`**

- **Type:** `byte[]`
- **Synopsis:** Pixel buffer for the FFB visualization bitmap. Size: `FFB_PIXELS_BUFFER_STRIDE * FFB_PIXELS_BUFFER_HEIGHT` (978 * 4 * 200 = 782,400 bytes). Updated during force calculation when pretty graph is enabled.

---

### src/SimRacingFFB/Application/App.Telemetry.cs

**Class:** `App` (partial)

#### Methods

**`private void InitializeTelemetry()`**

- **File:** `src/SimRacingFFB/Application/App.Telemetry.cs`
- **Synopsis:** Initializes the telemetry output system by creating or opening a memory-mapped file for inter-process communication. The memory-mapped file named "Local\\MAIRATelemetry" allows other applications (like overlays or data loggers) to read real-time telemetry data. Calculates the size of the TelemetryData struct and creates both the memory-mapped file and a view accessor for writing. Errors are silently caught to prevent initialization failures from crashing the application.

**`private void StopTelemetry()`**

- **File:** `src/SimRacingFFB/Application/App.Telemetry.cs`
- **Synopsis:** Stops the telemetry system by disposing of the memory-mapped file view accessor and the memory-mapped file itself. This releases the shared memory resource and allows other processes to clean up their connections.

**`public void UpdateTelemetry()`**

- **File:** `src/SimRacingFFB/Application/App.Telemetry.cs`
- **Synopsis:** Updates the telemetry data structure with current FFB and simulator state, then writes it to the memory-mapped file. Increments tick count, copies all relevant settings (scales, frequencies, effect parameters), current FFB state (input/output torque, clipping status, protection states), calculated values (yaw rate factor, G-force, shock velocity, understeer/oversteer amounts), and feature enable flags. Writes the entire struct atomically to the shared memory location. This provides a snapshot of the application state for external monitoring tools.

#### Important Data Structures

**`public struct TelemetryData`**

- **File:** `src/SimRacingFFB/Application/App.Telemetry.cs`
- **Synopsis:** Sequential, packed struct (4-byte alignment) containing all telemetry data exposed to external applications via memory-mapped file. Includes tick count, all FFB scale settings, understeer/oversteer effect parameters, LFE scale, input/output torque values, clipping status, calculated metrics (yaw rate factor, G-force, shock velocity, understeer/oversteer amounts), protection states, and feature enable flags. This struct serves as the contract for inter-process communication with telemetry consumers.

---

### src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs

**Class:** `App` (partial)

#### Methods

**`private void InitializeIRacingSDK()`**

- **File:** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
- **Synopsis:** Initializes the iRacing SDK connection by subscribing to all event handlers (exception, connected, disconnected, session info, telemetry data, debug log) and starting the SDK. The SDK runs in a background thread and will automatically connect when iRacing simulator is detected. This is a fire-and-forget initialization that sets up the event-driven architecture for simulator communication.

**`public void StopIRacingSDK()`**

- **File:** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
- **Synopsis:** Stops the iRacing SDK by calling Stop() and waiting in a tight loop until IsStarted becomes false. This ensures clean shutdown and prevents resource leaks. The tight loop (Thread.Sleep(0)) yields CPU time while waiting.

**`private void OnConnected()`**

- **File:** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
- **Synopsis:** Event handler called when iRacing simulator is detected and connection is established. Sets connection flag, finds the iRacing window handle for potential future use, schedules FFB reinitialization (in case device was lost), resets auto-overall-scale metrics, announces connection via voice, and updates UI on the dispatcher thread. If "PauseWhenSimulatorIsNotRunning" is enabled and FFB was disabled, it automatically enables FFB. This ensures the application is ready to process telemetry as soon as the simulator starts.

**`private void OnDisconnected()`**

- **File:** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
- **Synopsis:** Event handler called when iRacing simulator disconnects or closes. Resets all telemetry values to zero/defaults, clears session info flag, nullifies window handle, stops FFB recording/playback, triggers cooldown mode, stops ABS sound, resets HPR, updates car/track/condition (which may trigger settings loading), announces disconnection, and updates UI. This comprehensive cleanup ensures the application returns to a clean state when the simulator is not running.

**`private void OnSessionInfo()`**

- **File:** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
- **Synopsis:** Event handler called when session information is received from iRacing (typically on session start or track change). Extracts simulator mode (practice, qualify, race, etc.), shift light RPM thresholds, player car number, and number of forward gears. Updates current car, track, and wet/dry condition which may trigger settings loading. In debug builds, serializes the entire session info to JSON for inspection. This information is used for context-aware settings and feature behavior.

**`private void OnTelemetryData()`**

- **File:** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
- **Synopsis:** Main telemetry processing method called at 60Hz when iRacing sends telemetry updates. On first call, initializes datum references for all telemetry variables by looking them up in the SDK's property dictionary. On subsequent calls, reads all telemetry values using the cached datum references (much faster than dictionary lookups). Reads standard variables (brake, throttle, steering, speed, etc.) and high-frequency arrays (shock velocities, steering wheel torque at 360Hz). Calculates derived values: delta time from tick count difference, velocity magnitude from X/Y components, and G-force from velocity change over time (only when on-track and lap distance hasn't jumped). Manages session info update pausing (pauses when on-track or in replay to reduce CPU usage, resumes on session number change). Triggers ABS sound on brake ABS state changes. Updates wet/dry condition when weather changes. Resets auto-overall-scale when track state changes. Calls UpdateForceFeedback() and UpdateTelemetry() to process the new data. Updates UI visualization at reduced rates (pretty graph at 30Hz, other updates at 20Hz) to balance performance and responsiveness.

**`private void OnException(Exception exception)`**

- **File:** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
- **Synopsis:** Event handler for exceptions thrown by the iRacing SDK. Logs the exception message and rethrows it, allowing the application's exception handling to deal with it. This ensures SDK errors are visible and don't silently fail.

**`private void OnDebugLog(string message)`**

- **File:** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
- **Synopsis:** Event handler for debug log messages from the iRacing SDK. Prefixes messages with "[IRSDK]" and forwards them to the application's logging system. Used for troubleshooting SDK connection and communication issues.

#### Key Properties/Fields

**`private IRacingSdk _irsdk`**

- **Type:** `IRacingSdk`
- **Synopsis:** Instance of the iRacing SDK wrapper that handles all communication with the iRacing simulator. Manages shared memory access, event subscription, and data parsing.

**`public bool _irsdk_connected`**

- **Type:** `bool`
- **Synopsis:** Flag indicating whether the application is currently connected to iRacing simulator. Used throughout the application to determine if telemetry processing should occur.

**`public float[] _irsdk_steeringWheelTorque_ST`**

- **Type:** `float[]`
- **Synopsis:** Array of 6 samples containing steering wheel torque at 360Hz (6 samples per 60Hz frame). This is the primary input for force feedback calculation. The "_ST" suffix indicates "Sample Time" data (high-frequency samples within a frame).

**`public float[] _irsdk_cfShockVel_ST` through `_irsdk_rrShockVel_ST`**

- **Type:** `float[]`
- **Synopsis:** Arrays of 6 samples each for shock velocities at 360Hz for all six suspension corners: Center Front (CF), Center Rear (CR), Left Front (LF), Left Rear (LR), Right Front (RF), Right Rear (RR). Used for curb protection and suspension analysis.

**`public float _irsdk_steeringWheelAngle`**

- **Type:** `float`
- **Synopsis:** Current steering wheel angle in radians. Used for understeer calculation, soft lock, and auto-center wheel feature.

**`public float _irsdk_yawRate`**

- **Type:** `float`
- **Synopsis:** Vehicle yaw rate in radians per second. Combined with steering angle and speed to calculate yaw rate factor for understeer detection.

**`public float _irsdk_velocityY`**

- **Type:** `float`
- **Synopsis:** Lateral (sideways) velocity in meters per second. Used for oversteer detection - high lateral velocity indicates sliding.

**`public float _irsdk_gForce`**

- **Type:** `float`
- **Synopsis:** Calculated G-force from velocity change over time. Used for crash protection - high G-forces indicate impacts.

**`public bool _irsdk_isOnTrack`**

- **Type:** `bool`
- **Synopsis:** Flag indicating whether the vehicle is currently on the racing surface. Used to disable FFB when off-track, trigger cooldown mode, and manage session info updates.

**`public string _irsdk_simMode`**

- **Type:** `string`
- **Synopsis:** Current simulator mode: "practice", "qualify", "race", "replay", etc. Used to disable FFB during replay and manage session behavior.

---

## 2. Hardware Integration

### src/SimRacingFFB/Application/App.Inputs.cs

**Class:** `App` (partial)

#### Methods

**`public void InitializeInputs(nint windowHandle)`**

- **File:** `src/SimRacingFFB/Application/App.Inputs.cs`
- **Synopsis:** Initializes the input device system by enumerating all DirectInput devices (excluding standard devices and mice). Creates Joystick interfaces for each device, sets buffer size to 128 events, configures cooperative level to non-exclusive background (allows other applications to use devices), and acquires each device. Builds a list of force feedback-capable devices for the settings UI. Prevents duplicate device registration by checking instance GUIDs. Updates the settings with available FFB devices and wheel axis mappings (X, Y, Z, RX, RY, RZ, SX, SY). This initialization is separate from FFB device initialization to allow reading wheel position from the same device used for FFB output.

**`public void UpdateInputs(float deltaTime)`**

- **File:** `src/SimRacingFFB/Application/App.Inputs.cs`
- **Synopsis:** Processes input from all connected devices each frame. Resets button state for all mapped button configurations. Polls each joystick for buffered data to detect button presses. Tracks button holds for two-button combinations (button 1 must be held while button 2 is pressed). Reads wheel position from the selected axis of the FFB device and calculates wheel velocity from position delta over time. Handles exceptions by removing failed devices from the list. The method processes both current state (for wheel position) and buffered data (for button press detection) to support both continuous and event-based inputs.

#### Key Properties/Fields

**`public PressedButton Input_AnyPressedButton`**

- **Type:** `PressedButton`
- **Synopsis:** Contains information about the most recently pressed button from any device. Used for button mapping UI to allow users to press a button and have it automatically detected. Contains device GUID, product name, and button number.

**`public int Input_CurrentWheelPosition`**

- **Type:** `int`
- **Synopsis:** Current raw wheel position value from the selected axis (typically -32768 to 32767 for DirectInput). Used for auto-center wheel feature and wheel calibration. Updated each frame from the FFB device's selected axis.

**`public int Input_CurrentWheelVelocity`**

- **Type:** `int`
- **Synopsis:** Calculated wheel velocity in raw units per second. Computed from position delta divided by frame time. Used by auto-center wheel algorithm to determine wheel movement direction and speed.

**`private readonly List<Joystick> _input_joystickList`**

- **Type:** `List<Joystick>`
- **Synopsis:** List of all initialized DirectInput joystick devices. Used for polling button states and reading wheel position. Devices are removed from this list if they throw exceptions during polling.

#### Important Data Structures

**`public class PressedButton`**

- **File:** `src/SimRacingFFB/Application/App.Inputs.cs`
- **Synopsis:** Simple data class containing device identification and button number for the most recently pressed button. Used by the button mapping UI to detect user input for configuration.

---

### src/SimRacingFFB/Application/App.SimagicHPR.cs

**Class:** `App` (partial)

#### Methods

**`public void InitializeHPR(bool isFirstInitialization = false)`**

- **File:** `src/SimRacingFFB/Application/App.SimagicHPR.cs`
- **Synopsis:** Initializes the Simagic HPR (Haptic Pedal Response) system for pedal vibration feedback. If this is the first initialization, sets up three WriteableBitmaps for visualizing clutch, brake, and throttle haptics. Uninitializes any existing HPR connection, then initializes the HPR wrapper if pedal haptics are enabled in settings. The HPR system provides frequency and amplitude control for each pedal independently, allowing different effects to be mapped to different pedals.

**`private void UpdateHPR()`**

- **File:** `src/SimRacingFFB/Application/App.SimagicHPR.cs`
- **Synopsis:** Main update loop for pedal haptics that processes telemetry and applies effects to the three pedals. Implements 9 different effect types: (1) gear change (high frequency for neutral, low for gear changes), (2) ABS (modulated by brake pressure), (3) RPM wide range (activates in upper 50% of shift RPM range, frequency and amplitude increase with RPM and throttle), (4) RPM narrow range (activates in upper 5% of shift RPM range for precise shift timing), (5) steering effects (understeer/oversteer when effect style 4 is selected), (6) wheel lock (detected when RPM/speed ratio exceeds baseline by 5%), (7) wheel spin (detected when RPM/speed ratio is below baseline by 5%), (8) clutch slip (when clutch is partially engaged at high RPM). Each pedal can be configured with up to 3 effects in priority order (effect1, effect2, effect3) with individual strength scales. The method maintains per-gear RPM/speed ratio baselines using exponential smoothing to detect wheel lock and spin. Updates visualization graphs if enabled. Effects are applied with frequency range 15-35 Hz and amplitude range 18-60, scaled by effect strength settings.

**`private void ResetHPR()`**

- **File:** `src/SimRacingFFB/Application/App.SimagicHPR.cs`
- **Synopsis:** Resets HPR state when disconnecting from simulator. Clears gear tracking and resets all per-gear RPM/speed ratio baselines to zero. This ensures clean state when reconnecting or changing sessions.

#### Key Properties/Fields

**`private readonly HPR _hpr`**

- **Type:** `HPR`
- **Synopsis:** Instance of the Simagic HPR wrapper that communicates with Simagic pedal hardware. Provides methods to vibrate individual pedals with specified frequency and amplitude.

**`public float[] _hpr_averageRpmSpeedRatioPerGear`**

- **Type:** `float[]`
- **Synopsis:** Array of 13 elements (gears 0-12) storing the baseline RPM/speed ratio for each gear. Used to detect wheel lock (ratio too high) and wheel spin (ratio too low). Updated with exponential smoothing (95% previous + 5% current) when vehicle is in gear, moving forward, and not braking or clutching. This baseline represents normal traction conditions.

**`public float _hpr_currentRpmSpeedRatio`**

- **Type:** `float`
- **Synopsis:** Current calculated RPM/speed ratio (velocityX / RPM). Used to compare against baseline for wheel lock/spin detection. Reset to zero when not in valid conditions (gear > 0, RPM > 100, velocityX > 5).

**`public float[] _hpr_frequency` and `_hpr_amplitude`**

- **Type:** `float[]`
- **Synopsis:** Arrays of 3 elements (one per pedal: clutch, brake, throttle) containing the current frequency and amplitude values being sent to the hardware. Updated each frame based on active effects and their priority/strength settings.

**`public readonly WriteableBitmap[] _hpr_writeableBitmaps`**

- **Type:** `WriteableBitmap[]`
- **Synopsis:** Array of 3 bitmaps (317x200 pixels) for visualizing haptic signals on clutch, brake, and throttle pedals. Used for debugging and user feedback.

---

### src/SimRacingFFB/Application/App.Logitech.cs

**Class:** `App` (partial)

#### Methods

**`public void UpdateLogitech()`**

- **File:** `src/SimRacingFFB/Application/App.Logitech.cs`
- **Synopsis:** Updates Logitech wheel LED shift lights if enabled and a Logitech device is connected. Calls the Logitech G SDK function `LogiPlayLedsDInput` with current RPM and shift light thresholds. If the SDK call fails or throws an exception, disables Logitech support for the session to prevent repeated error logging. This provides visual RPM feedback on Logitech wheels that support LED shift lights.

#### Key Properties/Fields

**`private bool _logitech_disabled`**

- **Type:** `bool`
- **Synopsis:** Flag that disables Logitech SDK calls if they fail. Prevents repeated error logging when the SDK is not available or the device doesn't support the feature.

---

### src/SimRacingFFB/Application/App.LFE.cs

**Class:** `App` (partial)

#### Methods

**`public void InitializeLFE()`**

- **File:** `src/SimRacingFFB/Application/App.LFE.cs`
- **Synopsis:** Initializes the Low Frequency Effects (LFE) system that captures audio from a recording device and converts it to force feedback. Enumerates all DirectSound capture devices, builds a device list for settings, and selects the configured device. Creates a DirectSoundCapture instance and capture buffer configured for 8kHz mono 16-bit audio. Sets up notification positions in the circular buffer to trigger processing every frame (360Hz). Starts capture and launches a background thread to process audio data. The LFE system allows users to route audio (like engine sounds) through a recording device and have it converted to FFB forces, providing tactile feedback for audio cues.

**`private void LFEThread()`**

- **File:** `src/SimRacingFFB/Application/App.LFE.cs`
- **Synopsis:** Background thread that processes audio data from the capture buffer. Waits on an auto-reset event that triggers when new audio data is available (every frame at 360Hz). Reads a frame of audio data (132 samples at 8kHz = 6 samples at 360Hz), converts 16-bit PCM samples to normalized float values, averages 22 samples per FFB sample to match the 360Hz rate, and stores the magnitude in a double-buffered array. The magnitude index alternates between 0 and 1 to allow lock-free reading from the main FFB thread. This thread runs independently to avoid blocking the main FFB update loop.

**`private void UninitializeLFE()`**

- **File:** `src/SimRacingFFB/Application/App.LFE.cs`
- **Synopsis:** Stops the LFE system by signaling the thread to terminate, waiting for it to exit, stopping the capture buffer, disposing resources, and clearing magnitude data. Ensures clean shutdown without resource leaks.

#### Key Properties/Fields

**`private float[,] _lfe_magnitude`**

- **Type:** `float[,]`
- **Synopsis:** Double-buffered 2D array (2 buffers × 6 samples) containing normalized audio magnitude values at 360Hz. The first dimension alternates between 0 and 1 (controlled by `_lfe_magnitudeIndex`), allowing the FFB thread to read from one buffer while the LFE thread writes to the other. Values range from -1 to 1, representing audio amplitude.

**`private int _lfe_magnitudeIndex`**

- **Type:** `int`
- **Synopsis:** Index (0 or 1) indicating which buffer in `_lfe_magnitude` the FFB thread should read from. The LFE thread writes to the opposite buffer and then flips this index atomically.

**`private AutoResetEvent _lfe_autoResetEvent`**

- **Type:** `AutoResetEvent`
- **Synopsis:** Event used to signal the LFE thread when new audio data is available. Set by DirectSound capture buffer notifications at 360Hz. Also used to wake the thread during shutdown.

---

## 3. Additional Features

### src/SimRacingFFB/Application/App.Voice.cs

**Class:** `App` (partial)

#### Methods

**`public void InitializeVoice()`**

- **File:** `src/SimRacingFFB/Application/App.Voice.cs`
- **Synopsis:** Initializes the speech synthesizer system for text-to-speech announcements. Disposes any existing synthesizer, creates a new instance, attempts to inject OneCore voices (Windows 10+ modern voices), enumerates all installed voices, builds a voice list for settings, selects the configured voice (or first available if configured not found), sets output to default audio device, and configures speech rate. Updates volume from settings. This enables voice announcements for scale changes, car/track names, and other events.

**`public void Say(string message, string? value = null, bool interrupt = false, bool alsoAddToChatQueue = true)`**

- **File:** `src/SimRacingFFB/Application/App.Voice.cs`
- **Synopsis:** Speaks a message using the speech synthesizer. Replaces ":value:" placeholder in the message template with the provided value. If the value is empty, returns early. If interrupt is true, pauses current speech, cancels all queued speech, and resumes before speaking the new message. Speaks asynchronously to avoid blocking. Optionally adds the message to the chat queue for iRacing chat output. This method provides a unified interface for voice announcements throughout the application.

**`public void UpdateVolume()`**

- **File:** `src/SimRacingFFB/Application/App.Voice.cs`
- **Synopsis:** Updates the speech synthesizer volume from settings. Called when volume setting changes or during initialization.

#### Key Properties/Fields

**`private SpeechSynthesizer? _voice_speechSynthesizer`**

- **Type:** `SpeechSynthesizer?`
- **Synopsis:** Instance of the .NET speech synthesizer used for text-to-speech. Null when not initialized or disposed.

---

### src/SimRacingFFB/Application/App.Sounds.cs

**Class:** `App` (partial)

#### Methods

**`private void InitializeSounds()`**

- **File:** `src/SimRacingFFB/Application/App.Sounds.cs`
- **Synopsis:** Initializes the sound effect system by loading embedded WAV resources for click and ABS sounds. Creates WaveFileReader streams from embedded resources, wraps them in VolumeSampleProvider for volume control. For the click sound, creates a WaveOutEvent and initializes it. For the ABS sound, wraps the stream in a LoopStream for continuous playback, adds pitch shifting capability, wraps in volume provider, and initializes WaveOutEvent. Both sounds are ready to play but not started. Errors during initialization are logged but don't prevent application startup.

**`public void PlayClick()`**

- **File:** `src/SimRacingFFB/Application/App.Sounds.cs`
- **Synopsis:** Plays the click sound effect if enabled and initialized. Seeks the wave stream to the beginning, sets volume from settings, and starts playback. Used for button press feedback.

**`public void PlayABS()`**

- **File:** `src/SimRacingFFB/Application/App.Sounds.cs`
- **Synopsis:** Plays the ABS sound effect if enabled and initialized. Seeks the loop stream to the beginning, sets volume and pitch from settings, and starts continuous playback. The sound loops automatically via LoopStream wrapper. Used to provide audio feedback when ABS is active.

**`public void StopABS()`**

- **File:** `src/SimRacingFFB/Application/App.Sounds.cs`
- **Synopsis:** Stops the ABS sound playback. Called when ABS deactivates to silence the continuous sound.

#### Key Properties/Fields

**`private readonly WaveOutEvent _sounds_clickWaveOutEvent`**

- **Type:** `WaveOutEvent`
- **Synopsis:** NAudio wave output device for playing the click sound. Provides low-latency audio playback.

**`private LoopStream? _sounds_absLoopStream`**

- **Type:** `LoopStream?`
- **Synopsis:** Custom stream wrapper that automatically loops the ABS sound file. Allows continuous playback without manual looping logic.

**`private SmbPitchShiftingSampleProvider? _sounds_absPitchShiftingSampleProvider`**

- **Type:** `SmbPitchShiftingSampleProvider?`
- **Synopsis:** NAudio sample provider that applies pitch shifting to the ABS sound. Allows users to adjust the pitch of the ABS tone for personal preference.

---

### src/SimRacingFFB/Application/App.Console.cs

**Class:** `App` (partial)

#### Methods

**`public void InitializeConsole()`**

- **File:** `src/SimRacingFFB/Application/App.Console.cs`
- **Synopsis:** Initializes the console logging system by creating or opening a log file in the documents folder. Deletes log files older than 15 minutes to prevent disk space issues. Opens the file in append mode with read-write sharing to allow external log viewers. The log file is named "Console.log" and contains timestamped messages.

**`public void WriteLine(string message, bool addBlankLine = false)`**

- **File:** `src/SimRacingFFB/Application/App.Console.cs`
- **Synopsis:** Writes a message to the console log file and updates the UI console text box. Formats the message with a timestamp. Writes to Debug output for Visual Studio debugging. Uses a ReaderWriterLock to ensure thread-safe file access with a 250ms timeout (prevents blocking if lock is held). Writes UTF-8 encoded bytes to the file stream and flushes immediately. Updates the UI console text box on the dispatcher thread, scrolling to the end and moving the caret. This is the primary logging method used throughout the application.

**`public void StopConsole()`**

- **File:** `src/SimRacingFFB/Application/App.Console.cs`
- **Synopsis:** Closes and disposes the console log file stream during application shutdown.

#### Key Properties/Fields

**`private ReaderWriterLock _console_readerWriterLock`**

- **Type:** `ReaderWriterLock`
- **Synopsis:** Lock used to synchronize file access for console logging. Prevents concurrent writes that could corrupt the log file.

**`private FileStream? _console_fileStream`**

- **Type:** `FileStream?`
- **Synopsis:** File stream for writing console log messages. Opened in append mode to preserve existing log content.

---

### src/SimRacingFFB/Application/App.Service.cs

**Class:** `App` (partial)

#### Methods

**`private void InitializeService()`**

- **File:** `src/SimRacingFFB/Application/App.Service.cs`
- **Synopsis:** Initializes service-related functionality by obtaining the first network interface's GUID. Converts the network interface ID to a GUID, which is used as a unique identifier for the installation when checking for updates. This allows the update server to track unique installations for analytics. Throws exceptions if no network interfaces are found or if the ID cannot be converted to a GUID.

**`public async Task CheckForUpdates(bool manuallyLaunched)`**

- **File:** `src/SimRacingFFB/Application/App.Service.cs`
- **Synopsis:** Checks for application updates by querying a REST API endpoint with the network ID. Updates the UI status bar to show update check progress. Deserializes the JSON response containing current version, download URL, and changelog. Compares the current application version with the server version. If a newer version is available and not already downloaded, prompts the user (or auto-downloads if enabled) to download the update. Downloads the installer file with progress updates, then prompts the user to run the installer. If the file is already downloaded and not manually launched, skips the download process. Handles errors gracefully and hides the status bar items when done. The update check is asynchronous to avoid blocking the UI.

#### Important Data Structures

**`struct GetCurrentVersionResponse`**

- **File:** `src/SimRacingFFB/Application/App.Service.cs`
- **Synopsis:** Simple struct matching the JSON response from the update server API. Contains currentVersion (version string), downloadUrl (URL to installer), and changeLog (release notes text).

#### Key Properties/Fields

**`private Guid networkIdGuid`**

- **Type:** `Guid`
- **Synopsis:** Unique identifier derived from the first network interface ID. Used to identify this installation when checking for updates.

---

### src/SimRacingFFB/Application/App.ChatQueue.cs

**Class:** `App` (partial)

#### Methods

**`private void Chat(string message)`**

- **File:** `src/SimRacingFFB/Application/App.ChatQueue.cs`
- **Synopsis:** Adds a message to the chat queue for sending to iRacing chat. Escapes special iRacing chat characters (braces, parentheses, plus, caret, ampersand, tilde, brackets) by wrapping them in braces. Formats the message as a chat command with the player's car number prefix. Only queues messages if chat is enabled, iRacing is connected, and the player has a car number. This allows voice announcements and other events to be sent to iRacing chat for visibility.

**`public void ProcessChatMessageQueue()`**

- **File:** `src/SimRacingFFB/Application/App.ChatQueue.cs`
- **Synopsis:** Processes the chat message queue by sending messages to iRacing via Windows messages. If the chat window is not open, sends a BeginChat command to open it. Then sends each character of the queued message using PostMessage to simulate keyboard input. After sending a message, removes it from the queue. If the queue becomes empty and the chat window is open, sends a Cancel command to close it. This method should be called periodically (typically from the main update loop) to drain the queue. The queue prevents message flooding by processing one message at a time.

#### Key Properties/Fields

**`private readonly List<string> _chatQueue_messageList`**

- **Type:** `List<string>`
- **Synopsis:** Queue of chat messages waiting to be sent to iRacing. Messages are added by Chat() and processed by ProcessChatMessageQueue().

**`private bool _chatQueue_windowOpened`**

- **Type:** `bool`
- **Synopsis:** Flag tracking whether the iRacing chat window is currently open. Used to determine whether to send BeginChat or Cancel commands.

---

### src/SimRacingFFB/Application/App.CurrentCar.cs

**Class:** `App` (partial)

#### Methods

**`private void UpdateCurrentCar()`**

- **File:** `src/SimRacingFFB/Application/App.CurrentCar.cs`
- **Synopsis:** Updates the current car information from iRacing session data. Looks up the player's car in the driver list using DriverCarIdx, extracts the CarScreenName. If the car has changed, logs the change, announces it via voice, updates the save name (which may trigger settings loading), and updates the UI status bar with the car name and appropriate color (gray for no car, green for valid car). The car change flag triggers FFB settings loading in the main update loop.

**`public void UpdateCarSaveName()`**

- **File:** `src/SimRacingFFB/Application/App.CurrentCar.cs`
- **Synopsis:** Updates the car save name based on the current car and the "SaveSettingsPerCar" setting. If per-car saving is enabled, uses the car's screen name; otherwise uses "All". If the save name changes, sets the car changed flag to trigger settings loading.

#### Key Properties/Fields

**`private string _car_currentCarScreenName`**

- **Type:** `string`
- **Synopsis:** Display name of the currently driven car (e.g., "Dallara F3", "Ferrari 488 GT3"). Used for UI display and settings matching.

**`private string _car_carSaveName`**

- **Type:** `string`
- **Synopsis:** Name used for saving/loading car-specific settings. Either the car screen name or "All" depending on SaveSettingsPerCar setting.

**`private bool _car_carChanged`**

- **Type:** `bool`
- **Synopsis:** Flag indicating the car has changed since last check. Triggers FFB and steering effects settings loading in the main update loop.

---

### src/SimRacingFFB/Application/App.CurrentTrack.cs

**Class:** `App` (partial)

#### Methods

**`private void UpdateCurrentTrack()`**

- **File:** `src/SimRacingFFB/Application/App.CurrentTrack.cs`
- **Synopsis:** Updates the current track and track configuration information from iRacing session data. Extracts TrackDisplayName and TrackConfigName from WeekendInfo. If either has changed, logs the change, announces both via voice, updates save names (which may trigger settings loading), and updates the UI status bar with track name and configuration. The track change flags trigger FFB settings loading in the main update loop.

**`public void UpdateTrackSaveName()`**

- **File:** `src/SimRacingFFB/Application/App.CurrentTrack.cs`
- **Synopsis:** Updates the track save name based on the current track and the "SaveSettingsPerTrack" setting. If per-track saving is enabled, uses the track display name; otherwise uses "All". If the save name changes, sets the track changed flag.

**`public void UpdateTrackConfigSaveName()`**

- **File:** `src/SimRacingFFB/Application/App.CurrentTrack.cs`
- **Synopsis:** Updates the track configuration save name based on the current track config and the "SaveSettingsPerTrackConfig" setting. If per-config saving is enabled, uses the track config name; otherwise uses "All". If the save name changes, sets the track config changed flag.

#### Key Properties/Fields

**`private string _track_currentTrackDisplayName`**

- **Type:** `string`
- **Synopsis:** Display name of the current track (e.g., "Silverstone", "Nürburgring"). Used for UI display and settings matching.

**`private string _track_trackSaveName`**

- **Type:** `string`
- **Synopsis:** Name used for saving/loading track-specific settings. Either the track display name or "All" depending on SaveSettingsPerTrack setting.

**`private string _track_currentTrackConfigName`**

- **Type:** `string`
- **Synopsis:** Display name of the current track configuration/variant (e.g., "Grand Prix", "International"). Some tracks have multiple configurations.

**`private string _track_trackConfigSaveName`**

- **Type:** `string`
- **Synopsis:** Name used for saving/loading track configuration-specific settings. Either the track config name or "All" depending on SaveSettingsPerTrackConfig setting.

**`private bool _track_trackChanged` and `_track_trackConfigChanged`**

- **Type:** `bool`
- **Synopsis:** Flags indicating the track or track configuration has changed. Trigger FFB settings loading in the main update loop.

---

### src/SimRacingFFB/Application/App.WetDryCondition.cs

**Class:** `App` (partial)

#### Methods

**`private void InitializeWetDryCondition()`**

- **File:** `src/SimRacingFFB/Application/App.WetDryCondition.cs`
- **Synopsis:** Initializes the wet/dry condition tracking by calling UpdateCurrentWetDryCondition(). This ensures the condition is set on startup if iRacing is already connected.

**`private void UpdateCurrentWetDryCondition()`**

- **File:** `src/SimRacingFFB/Application/App.WetDryCondition.cs`
- **Synopsis:** Updates the current track condition (Wet or Dry) based on iRacing's WeatherDeclaredWet flag. If the condition has changed, logs the change, announces it via voice (different messages for wet vs dry), updates the save name (which may trigger settings loading), and updates the UI status bar. The condition change flag triggers FFB settings loading in the main update loop.

**`public void UpdateWetDryConditionSaveName()`**

- **File:** `src/SimRacingFFB/Application/App.WetDryCondition.cs`
- **Synopsis:** Updates the condition save name based on the current condition and the "SaveSettingsPerWetDryCondition" setting. If per-condition saving is enabled, uses the condition display name ("Wet" or "Dry"); otherwise uses "All". If the save name changes, sets the condition changed flag.

#### Key Properties/Fields

**`private string _wetdry_currentConditionDisplayName`**

- **Type:** `string`
- **Synopsis:** Current track condition display name: "Wet" or "Dry". Determined from iRacing's WeatherDeclaredWet flag.

**`private string _wetdry_conditionSaveName`**

- **Type:** `string`
- **Synopsis:** Name used for saving/loading condition-specific settings. Either "Wet", "Dry", or "All" depending on SaveSettingsPerWetDryCondition setting.

**`private bool _wetdry_conditionChanged`**

- **Type:** `bool`
- **Synopsis:** Flag indicating the track condition has changed. Triggers FFB settings loading in the main update loop.

---

## 4. Supporting Infrastructure

### src/SimRacingFFB/Common/Settings.cs

**Class:** `Settings`

#### Methods

**`public static void UpdateFFBDeviceList(SerializableDictionary<Guid, string> ffbDeviceList)`**

- **File:** `src/SimRacingFFB/Common/Settings.cs`
- **Synopsis:** Updates the list of available force feedback devices in settings. Called during input initialization when devices are enumerated. This list is used by the UI to populate device selection dropdowns.

**`public static void UpdateLFEDeviceList(SerializableDictionary<Guid, string> lfeDeviceList)`**

- **File:** `src/SimRacingFFB/Common/Settings.cs`
- **Synopsis:** Updates the list of available LFE (audio capture) devices in settings. Called during LFE initialization when recording devices are enumerated.

**`public static void UpdateVoiceList(SerializableDictionary<string, string> voiceList)`**

- **File:** `src/SimRacingFFB/Common/Settings.cs`
- **Synopsis:** Updates the list of available speech synthesizer voices in settings. Called during voice initialization when voices are enumerated.

**`public static void UpdateWheelAxisList(SerializableDictionary<JoystickOffset, string> wheelAxisList)`**

- **File:** `src/SimRacingFFB/Common/Settings.cs`
- **Synopsis:** Updates the list of available wheel axes (X, Y, Z, RX, RY, RZ, SX, SY) in settings. Used by the UI for axis selection.

#### Important Data Structures

**`public class MappedButton`**

- **File:** `src/SimRacingFFB/Common/Settings.cs`
- **Synopsis:** Data class representing a button mapping with device identification (GUID and product name) and button number. Used throughout the application for button configuration.

**`public class MappedButtons`**

- **File:** `src/SimRacingFFB/Common/Settings.cs`
- **Synopsis:** Container for a two-button combination mapping. Contains Button1 and Button2 (Button2 is optional for single-button mappings). Includes runtime-only properties Button1Held and ClickCount for tracking button state during input processing. Used for all button mappings in the application (scale adjustments, reinitialization, etc.).

#### Key Properties/Fields

**`public class ForceFeedbackSettings`**

- **Type:** `class` (nested in Settings)
- **Synopsis:** Contains FFB scale settings (OverallScale, DetailScale) for a specific combination of wheel, car, track, track config, and wet/dry condition. Used for per-context settings storage and loading.

**`public class SteeringEffectsSettings`**

- **Type:** `class` (nested in Settings)
- **Synopsis:** Contains steering effects configuration (enabled state, understeer/oversteer parameters) for a specific car. Used for per-car steering effects settings.

**`public List<ForceFeedbackSettings> ForceFeedbackSettingsList`**

- **Type:** `List<ForceFeedbackSettings>`
- **Synopsis:** Collection of all saved FFB settings profiles. Each profile is matched by wheel, car, track, track config, and condition using a ranking system (20 points per match) to find the best matching profile.

**`public List<SteeringEffectsSettings> SteeringEffectsSettingsList`**

- **Type:** `List<SteeringEffectsSettings>`
- **Synopsis:** Collection of all saved steering effects settings profiles, indexed by car name.

---

### src/SimRacingFFB/Common/Serializer.cs

**Class:** `Serializer` (static)

#### Methods

**`public static object? Load(string filePath, Type type)`**

- **File:** `src/SimRacingFFB/Common/Serializer.cs`
- **Synopsis:** Deserializes an object from an XML file using XmlSerializer. Opens the file, attempts deserialization, catches and ignores exceptions (returns null on failure), and closes the file. Used for loading settings and other persisted data. The null return allows the application to continue with default values if the file is corrupted or missing.

**`public static void Save(string filePath, object data)`**

- **File:** `src/SimRacingFFB/Common/Serializer.cs`
- **Synopsis:** Serializes an object to an XML file using XmlSerializer. Creates the directory if it doesn't exist, creates an XmlSerializer for the object's type, writes to a StreamWriter, and closes the file. Used for saving settings and other persisted data. This is a simple wrapper around .NET's XML serialization.

---

### src/SimRacingFFB/Interop/WinApi.cs

**Class:** `WinApi` (static)

#### Methods

**`[DllImport] public static extern bool SetProcessInformation(IntPtr hProcess, int ProcessInformationClass, IntPtr ProcessInformation, UInt32 ProcessInformationSize)`**

- **File:** `src/SimRacingFFB/Interop/WinApi.cs`
- **Synopsis:** Windows API function for setting process information. Used to disable CPU power throttling for consistent FFB timing.

**`[DllImport] public static extern UInt32 TimeSetEvent(UInt32 msDelay, UInt32 msResolution, TimerEventHandler handler, ref UInt32 userCtx, UInt32 eventType)`**

- **File:** `src/SimRacingFFB/Interop/WinApi.cs`
- **Synopsis:** Windows multimedia timer API for creating high-resolution periodic callbacks. Used for the 360Hz FFB update loop. Returns a timer ID that must be used to kill the timer.

**`[DllImport] public static extern void TimeKillEvent(UInt32 uTimerId)`**

- **File:** `src/SimRacingFFB/Interop/WinApi.cs`
- **Synopsis:** Windows multimedia timer API for stopping a timer created with TimeSetEvent. Used during FFB shutdown.

**`[DllImport] public static extern bool SetForegroundWindow(IntPtr hWnd)`**

- **File:** `src/SimRacingFFB/Interop/WinApi.cs`
- **Synopsis:** Windows API for bringing a window to the foreground. Used when detecting a second instance to focus the existing window.

**`[DllImport] public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, uint Flags)`**

- **File:** `src/SimRacingFFB/Interop/WinApi.cs`
- **Synopsis:** Windows API for registering to receive device arrival/removal notifications. Used to detect when USB HID devices (wheels, pedals) are connected or disconnected.

**`[DllImport] public static extern uint UnregisterDeviceNotification(IntPtr Handle)`**

- **File:** `src/SimRacingFFB/Interop/WinApi.cs`
- **Synopsis:** Windows API for unregistering device change notifications. Called during window cleanup.

**`[DllImport] public static extern IntPtr FindWindow(string? lpClassName, string lpWindowName)`**

- **File:** `src/SimRacingFFB/Interop/WinApi.cs`
- **Synopsis:** Windows API for finding a window by class name or window title. Used to find the iRacing simulator window for potential future use.

**`[DllImport] public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam)`**

- **File:** `src/SimRacingFFB/Interop/WinApi.cs`
- **Synopsis:** Windows API for posting a message to a window's message queue. Used to send chat messages to iRacing by simulating keyboard input.

#### Important Data Structures

**`public struct PROCESS_POWER_THROTTLING_STATE`**

- **File:** `src/SimRacingFFB/Interop/WinApi.cs`
- **Synopsis:** Windows structure for configuring process power throttling. Contains Version, ControlMask (flags for what to control), and StateMask (desired state). Used to disable CPU throttling.

**`public struct MARGINS`**

- **File:** `src/SimRacingFFB/Interop/WinApi.cs`
- **Synopsis:** Windows structure for DWM (Desktop Window Manager) margins. Used for window transparency effects.

**`public delegate void TimerEventHandler(UInt32 id, UInt32 msg, ref UInt32 userCtx, UInt32 rsv1, UInt32 rsv2)`**

- **File:** `src/SimRacingFFB/Interop/WinApi.cs`
- **Synopsis:** Delegate signature for Windows multimedia timer callbacks. Used by TimeSetEvent for the FFB update loop.

---

## 5. User Interface

### src/SimRacingFFB/UI/Views/MainWindow.xaml.cs

**Class:** `MainWindow`

#### Methods

**`public static string GetVersion()`**

- **File:** `src/SimRacingFFB/UI/Views/MainWindow.xaml.cs`
- **Synopsis:** Returns the application version string from the assembly version. Used for update checking and window title display.

**`private async void Window_Activated(object sender, EventArgs e)`**

- **File:** `src/SimRacingFFB/UI/Views/MainWindow.xaml.cs`
- **Synopsis:** Window activation event handler that performs one-time initialization on first activation. Gets the window handle, saves original window styles, calls App.Initialize() with the handle, restores window position from settings, sets up window transparency, initializes visualization bitmaps, starts the background update loop thread, starts the UI timer, loads FFB recording if available, sets window topmost if configured, handles start-minimized behavior, and checks for updates asynchronously. This deferred initialization ensures the window is fully created before initializing subsystems that need the window handle.

**`private void Window_Closing(object sender, CancelEventArgs e)`**

- **File:** `src/SimRacingFFB/UI/Views/MainWindow.xaml.cs`
- **Synopsis:** Window closing event handler that implements "close to system tray" behavior. If CloseToSystemTray is enabled and the application is not shutting down, cancels the close event, hides the window, removes it from the taskbar, and shows a tray notification. This allows the application to continue running in the background.

**`private void Window_Closed(object sender, EventArgs e)`**

- **File:** `src/SimRacingFFB/UI/Views/MainWindow.xaml.cs`
- **Synopsis:** Window closed event handler that performs cleanup. Stops and disposes the UI timer, terminates the update loop thread if running, unregisters device change notifications, calls App.Stop() to shut down all subsystems, and launches the installer if an update was downloaded. This ensures proper cleanup of all resources.

**`protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)`**

- **File:** `src/SimRacingFFB/UI/Views/MainWindow.xaml.cs`
- **Synopsis:** Windows message procedure for handling custom window messages and device change notifications. Handles WM_MAIRA custom messages for external control of FFB scales (used by overlays or other applications). The message contains a type (Overall_Scale, Detail_Scale, etc.) and a value (-10 to +10) that is added to the current setting. Also handles WM_DEVICECHANGE for USB device connect/disconnect events, which triggers FFB device reinitialization. Returns IntPtr.Zero to allow default message processing.

**`private void RegisterDeviceChangeNotification()`**

- **File:** `src/SimRacingFFB/UI/Views/MainWindow.xaml.cs`
- **Synopsis:** Registers the window to receive Windows device change notifications for USB HID devices. Creates a DEV_BROADCAST_DEVICEINTERFACE structure, allocates unmanaged memory, marshals the structure, calls RegisterDeviceNotification, and frees the memory. This allows the application to detect when wheels or other input devices are connected or disconnected.

**`private void UpdateLoop()`**

- **File:** `src/SimRacingFFB/UI/Views/MainWindow.xaml.cs`
- **Synopsis:** Background thread that runs the main application update loop. Sets a flag when started, then enters a loop that waits on an auto-reset event. When signaled, processes inputs, updates force feedback, processes chat queue, and signals the UI thread. The loop continues until _win_keepThreadsAlive becomes 0. This separates the high-frequency update logic from the UI thread to prevent blocking.

**`private void OnTimer(object? sender, System.Timers.ElapsedEventArgs e)`**

- **File:** `src/SimRacingFFB/UI/Views/MainWindow.xaml.cs`
- **Synopsis:** Timer event handler called at 100ms intervals (10Hz). Calculates delta time, calls App.UpdateInputs() and App.UpdateForceFeedback(), processes chat queue, signals the update loop thread, and updates UI elements. This provides the main application heartbeat that coordinates all subsystems.

#### Key Properties/Fields

**`public static MainWindow? Instance`**

- **Type:** `MainWindow?`
- **Synopsis:** Singleton instance reference for accessing the main window from other parts of the application. Set in constructor and cleared in Window_Closed.

**`private readonly System.Timers.Timer _win_timer`**

- **Type:** `System.Timers.Timer`
- **Synopsis:** Timer that fires every 100ms to drive the main update loop. Provides the application's heartbeat.

**`public readonly AutoResetEvent _win_autoResetEvent`**

- **Type:** `AutoResetEvent`
- **Synopsis:** Event used to signal the background update loop thread. Set by the timer to trigger updates.

**`private bool _win_updateLoopRunning`**

- **Type:** `bool`
- **Synopsis:** Flag indicating the background update loop thread is running. Used for thread synchronization during shutdown.

**`private nint _win_windowHandle`**

- **Type:** `nint`
- **Synopsis:** Windows window handle (HWND) for the main window. Required for DirectInput device initialization and Windows API calls.

**`private void UpdateWindowTransparency(bool forceOpaque)`**

- **File:** `src/SimRacingFFB/UI/Views/MainWindow.xaml.cs`
- **Synopsis:** Updates window transparency based on the WindowOpacity setting. If opacity is 100% or forceOpaque is true, restores original window styles and sets opacity to 1.0. Otherwise, modifies window styles to enable layered window and transparency, removes dialog frame, and sets opacity from settings. This allows the window to be semi-transparent for overlay-like behavior while maintaining functionality.

**`public void ReinitializeForceFeedback()`**

- **File:** `src/SimRacingFFB/UI/Views/MainWindow.xaml.cs`
- **Synopsis:** Public method to trigger force feedback device reinitialization. Called from UI buttons and when devices change. Verifies the window is initialized before calling App.ReinitializeForceFeedbackDevice(). Used for manual reset and automatic recovery scenarios.

**`private static string GetRecordingIndexAsTime()`**

- **File:** `src/SimRacingFFB/UI/Views/MainWindow.xaml.cs`
- **Synopsis:** Converts the current FFB recording buffer index to a time string (minutes:seconds.decimal). Calculates minutes by dividing index by samples per minute (360Hz * 60 seconds), and seconds from the remainder. Used to display recording/playback progress in the UI.

**`public static DependencyObject? GetNextTab(DependencyObject element, DependencyObject containerElement, bool goDownOnly)`**

- **File:** `src/SimRacingFFB/UI/Views/MainWindow.xaml.cs`
- **Synopsis:** Uses reflection to access WPF's internal KeyboardNavigation.GetNextTab method to find the next tab stop element. Used when a text box loses focus to find the associated slider control for synchronization. This allows text boxes and sliders to stay in sync when users type values directly.

---

### src/SimRacingFFB/Common/SerializableDictionary.cs

**Class:** `SerializableDictionary<TKey, TValue>`

#### Methods

**`public void ReadXml(XmlReader reader)`**

- **File:** `src/SimRacingFFB/Common/SerializableDictionary.cs`
- **Synopsis:** Custom XML deserialization implementation for dictionaries. Reads XML elements in the format `<item><key>...</key><value>...</value></item>`, deserializes each key and value using XmlSerializer, and adds them to the dictionary. Handles empty elements and properly navigates the XML structure. This allows dictionaries to be serialized to/from XML for settings persistence.

**`public void WriteXml(XmlWriter writer)`**

- **File:** `src/SimRacingFFB/Common/SerializableDictionary.cs`
- **Synopsis:** Custom XML serialization implementation for dictionaries. Writes each key-value pair as an XML element with nested key and value elements. Uses XmlSerializer to serialize the key and value types. This allows dictionaries to be persisted in XML format for settings storage.

#### Synopsis

**Class:** `SerializableDictionary<TKey, TValue>`

- **File:** `src/SimRacingFFB/Common/SerializableDictionary.cs`
- **Synopsis:** Generic dictionary class that extends SortedDictionary and implements IXmlSerializable to support XML serialization. Used throughout the application for settings that need dictionary-like storage (device lists, voice lists, etc.) that can be persisted to XML. The sorted nature ensures consistent ordering in the XML output.

---

### src/SimRacingFFB/Common/ClipboardHelper.cs

**Class:** `ClipboardHelper` (static)

#### Methods

**`public static DataObject CreateDataObject(string html, string plainText)`**

- **File:** `src/SimRacingFFB/Common/ClipboardHelper.cs`
- **Synopsis:** Creates a DataObject containing both HTML and plain text formats for clipboard operations. Formats the HTML with proper clipboard headers including byte offsets for StartHTML, EndHTML, StartFragment, and EndFragment. Handles encoding issues for .NET Framework versions before 4.0. Returns a DataObject that can be placed on the clipboard with multiple formats for maximum compatibility.

**`public static void CopyToClipboard(string html, string plainText)`**

- **File:** `src/SimRacingFFB/Common/ClipboardHelper.cs`
- **Synopsis:** Copies HTML and plain text to the clipboard using CreateDataObject and Clipboard.SetDataObject. The HTML format allows rich text pasting in applications that support it, while plain text provides fallback compatibility.

**`private static string GetHtmlDataString(string html)`**

- **File:** `src/SimRacingFFB/Common/ClipboardHelper.cs`
- **Synopsis:** Formats HTML content for clipboard operations by adding required clipboard headers and calculating byte offsets. Handles various HTML input scenarios: HTML with existing fragments, HTML without fragments, HTML without html/body tags. Calculates fragment start and end positions in bytes (not characters) for proper clipboard format. Back-patches placeholder offsets in the header with actual byte counts. This ensures the HTML can be pasted correctly into applications like Word, Outlook, etc.

---

### src/SimRacingFFB/Interop/DeviceChangeNotification.cs

**Class:** `DeviceChangeNotification`

#### Important Data Structures

**`public class DEV_BROADCAST_DEVICEINTERFACE`**

- **File:** `src/SimRacingFFB/Interop/DeviceChangeNotification.cs`
- **Synopsis:** Windows structure for device change notifications. Contains size, device type (DBT_DEVTYP_DEVICEINTERFACE), reserved field, class GUID (USB_HID_GUID for human interface devices), and device name. Used when registering for device arrival/removal notifications to filter for USB HID devices like wheels and pedals.

#### Key Properties/Fields

**`public static readonly Guid USB_HID_GUID`**

- **Type:** `Guid`
- **Synopsis:** Windows GUID for USB Human Interface Device class. Used to filter device change notifications to only USB HID devices, ignoring other device types.

**`public const ushort DBT_DEVICEARRIVAL` and `DBT_DEVICEREMOVECOMPLETE`**

- **Type:** `ushort`
- **Synopsis:** Windows message constants for device arrival and removal events. Used in the window message procedure to detect when devices are connected or disconnected.

---

### src/SimRacingFFB/Interop/LoopStream.cs

**Class:** `LoopStream`

#### Methods

**`public override int Read(byte[] buffer, int offset, int count)`**

- **File:** `src/SimRacingFFB/Interop/LoopStream.cs`
- **Synopsis:** Reads audio data from the underlying wave stream, automatically looping when the end is reached. Continues reading until the requested count is filled, seeking back to the beginning when the stream ends (unless already at position 0, which indicates the stream is empty). This allows continuous playback of short audio files like the ABS sound effect without manual looping logic.

#### Synopsis

**Class:** `LoopStream`

- **File:** `src/SimRacingFFB/Interop/LoopStream.cs`
- **Synopsis:** NAudio WaveStream wrapper that automatically loops audio playback. Used for the ABS sound effect which needs to play continuously while ABS is active. Delegates WaveFormat, Length, and Position to the underlying stream, but overrides Read() to implement looping behavior.

---

### src/SimRacingFFB/Application/App.Settings.cs

**Class:** `App` (partial)

#### Methods

**`private void InitializeSettings()`**

- **File:** `src/SimRacingFFB/Application/App.Settings.cs`
- **Synopsis:** Initializes the settings system by loading Settings.xml from the documents folder if it exists. Pauses serialization during loading to prevent saving while loading. Deserializes the XML file, applies temporary migration code for effect style settings (legacy code to be removed), and replaces the default settings instance. Sets the main window's DataContext to the settings for data binding. Calls FixRangeSliders() to initialize UI controls. If the file doesn't exist or fails to load, uses default settings.

**`public void UpdateSettings(float deltaTime)`**

- **File:** `src/SimRacingFFB/Application/App.Settings.cs`
- **Synopsis:** Updates and saves settings with a 1-second debounce timer to prevent excessive file I/O. When the timer expires, updates or creates ForceFeedbackSettings entries for the current wheel/car/track/condition combination, updates or creates SteeringEffectsSettings entries for the current car, and saves the entire settings object to XML. This ensures settings are persisted automatically after changes, but not on every single property change. The debounce prevents file locking issues and performance problems from rapid settings changes.

**`public void QueueForSerialization()`**

- **File:** `src/SimRacingFFB/Application/App.Settings.cs`
- **Synopsis:** Queues settings for serialization by starting the 1-second debounce timer. Called by settings properties when they change (via PropertyChanged handlers). If serialization is paused (during loading), the queue is ignored.

#### Key Properties/Fields

**`public Settings Settings`**

- **Type:** `Settings`
- **Synopsis:** Main settings instance that contains all application configuration. Accessed throughout the application for reading and writing settings. Implements INotifyPropertyChanged for data binding.

**`private bool _settings_pauseSerialization`**

- **Type:** `bool`
- **Synopsis:** Flag that prevents settings serialization during loading or bulk updates. Prevents saving while loading and avoids unnecessary saves during initialization.

**`private float _settings_serializationTimer`**

- **Type:** `float`
- **Synopsis:** Debounce timer that counts down from 1 second. When it reaches zero, settings are saved to disk. This prevents saving on every property change, instead batching changes.

---