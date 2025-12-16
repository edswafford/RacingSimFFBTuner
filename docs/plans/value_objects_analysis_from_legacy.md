# Value Objects Analysis from Legacy Code

This document provides a quick reference table of contents with links to all value objects identified from the legacy MarvinsAIRA codebase.

## Main Document

For complete analysis, implementation recommendations, and TDD test requirements, see:
**[Value Objects Analysis and Design.md](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md)**

## Table of Contents - Value Objects

### Phase 1: Foundation (Generic, No Dependencies)

1. ✅ **[SteeringWheelAngle](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#steeringwheelangle)** - COMPLETED
   - **Description:** Steering wheel angle in radians with maximum steering lock validation
   - **Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
   - **Field (Legacy):** `public float _irsdk_steeringWheelAngle` - Current steering wheel angle in radians
   - **Used in (Legacy):** Understeer calculation (yaw rate factor), soft lock forces, auto-center wheel feature
   - **Reference:** [LegacySimRacingFFB.md](../LegacySimRacingFFB.md) (line 256-258)
   - **Validation:** Must be within ±maxRadians range, rejects NaN/Infinity
   - **Units:** Radians
   - **Priority:** Phase 1, #1
   - **Implementation:** See `src/Domain/ValueObjects/SteeringWheelAngle.cs`

2. ☐ **[YawRate](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#yawrate)**
   - **Description:** Angular velocity (radians/second) - Vehicle yaw rate around vertical axis
   - **Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
   - **Field (Legacy):** `public float _irsdk_yawRate` - Vehicle yaw rate in radians per second
   - **Used in (Legacy):** Yaw rate factor calculation for understeer detection, FFB calculation pipeline
   - **Reference:** [LegacySimRacingFFB.md](../LegacySimRacingFFB.md) (line 260-262)
   - **Validation:** Rejects NaN/Infinity, no specific range limits
   - **Units:** Radians per second
   - **Priority:** Phase 1, #2

3. ☐ **[Speed](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#speed)**
   - **Description:** Velocity magnitude - Vehicle speed in meters per second
   - **Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
   - **Calculation (Legacy):** Calculated from velocityX and velocityY: `MathF.Sqrt(velocityX * velocityX + velocityY * velocityY)`
   - **Used in (Legacy):** Yaw rate factor calculation, speed scaling for FFB, auto-center wheel feature
   - **Validation:** Must be >= 0, rejects NaN/Infinity
   - **Units:** Meters per second
   - **Priority:** Phase 1, #3

4. ☐ **[Torque](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#torque)**
   - **Description:** Force feedback torque in Newton-meters (Nm), primary input for FFB calculation
   - **Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
   - **Field (Legacy):** `public float[] _irsdk_steeringWheelTorque_ST` - Array of 6 samples at 360Hz
   - **Used in (Legacy):** Core FFB calculation algorithm, delta torque calculation, steady-state torque calculation
   - **Reference:** [LegacySimRacingFFB.md](../LegacySimRacingFFB.md) (line 248-250)
   - **Validation:** Rejects NaN/Infinity, no specific range limits
   - **Units:** Newton-meters (Nm)
   - **Priority:** Phase 1, #4

5. ☐ **[Velocity](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#velocity)**
   - **Description:** 3D velocity vector with X (forward/backward), Y (lateral), Z (vertical) components in m/s
   - **Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
   - **Fields (Legacy):** `_irsdk_velocityX`, `_irsdk_velocityY`, `_irsdk_velocityZ`
   - **Used in (Legacy):** Oversteer detection (lateral velocity), velocity magnitude calculation, G-force calculation
   - **Reference:** [LegacySimRacingFFB.md](../LegacySimRacingFFB.md) (line 264-266)
   - **Validation:** Rejects NaN/Infinity for any component, no specific range limits
   - **Units:** Meters per second for each component
   - **Priority:** Phase 1, #5

6. ☐ **[GForce](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#gforce)**
   - **Description:** Acceleration relative to gravity, calculated from velocity change, used for crash protection
   - **Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
   - **Field (Legacy):** `public float _irsdk_gForce` - Calculated from velocity change over time
   - **Used in (Legacy):** Crash protection (scales forces based on G-force threshold), peak G-force tracking
   - **Reference:** [LegacySimRacingFFB.md](../LegacySimRacingFFB.md) (line 268-270, 104-106)
   - **Validation:** Rejects NaN/Infinity, no specific upper limit
   - **Units:** G-forces (multiples of Earth's gravity, 9.81 m/s²)
   - **Priority:** Phase 1, #6

7. ☐ **[EngineRPM](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#enginerpm)**
   - **Description:** Engine rotational speed in revolutions per minute (RPM), optional MaxRPM validation
   - **Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
   - **Field (Legacy):** `_irsdk_rpm` - Referenced in HPR code
   - **Used in (Legacy):** RPM-based pedal haptics, shift light calculations, wheel lock/spin detection
   - **Validation:** Must be >= 0, optional MaxRPM limit, rejects NaN/Infinity
   - **Units:** Revolutions per minute (RPM)
   - **Priority:** Phase 1, #7

### Phase 2: Telemetry Components (Depend on Phase 1)

8. ☐ **[ShockVelocity](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#shockvelocity)**
   - **Description:** Suspension shock velocity per corner in meters per second
   - **Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
   - **Fields (Legacy):** Arrays of 6 samples each at 360Hz for 6 corners (CF, CR, LF, LR, RF, RR)
   - **Used in (Legacy):** Curb protection (reduces detail scale), maximum shock velocity tracking
   - **Reference:** [LegacySimRacingFFB.md](../LegacySimRacingFFB.md) (line 252-254, 108-110)
   - **Validation:** Rejects NaN/Infinity, no specific range limits
   - **Units:** Meters per second (m/s)
   - **Priority:** Phase 2, #8

9. ☐ **[TireLoad](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#tireload)**
   - **Description:** Tire force and slip ratio - Load in Newtons and slip ratio (unitless)
   - **Source:** Design document reference
   - **Note:** Fundamental racing concept for advanced FFB calculations
   - **Validation:** Load must be >= 0, slip ratio typically -1 to 1, rejects NaN/Infinity
   - **Units:** Load in Newtons, Slip Ratio is unitless
   - **Priority:** Phase 2, #10

### Phase 3: Hardware Input (Independent)

10. ☐ **[WheelPosition](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#wheelposition)**
    - **Description:** Raw hardware wheel position in DirectInput units (typically -32768 to 32767)
    - **Source (Legacy):** `src/SimRacingFFB/Application/App.Inputs.cs`
    - **Field (Legacy):** `public int Input_CurrentWheelPosition` - Raw wheel position from selected axis
    - **Used in (Legacy):** Auto-center wheel feature when not on track, wheel calibration, wheel velocity calculation
    - **Reference:** [LegacySimRacingFFB.md](../LegacySimRacingFFB.md) (line 303-305)
    - **Validation:** No validation (int type), typically -32768 to 32767
    - **Units:** Raw DirectInput units
    - **Priority:** Phase 3, #11

11. ☐ **[WheelVelocity](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#wheelvelocity)**
    - **Description:** Calculated wheel velocity in raw units per second, calculated from position delta
    - **Source (Legacy):** `src/SimRacingFFB/Application/App.Inputs.cs`
    - **Field (Legacy):** `public int Input_CurrentWheelVelocity` - Calculated from position delta over time
    - **Used in (Legacy):** Auto-center wheel algorithm (velocity-based type 0), determining wheel movement direction
    - **Reference:** [LegacySimRacingFFB.md](../LegacySimRacingFFB.md) (line 307-309)
    - **Validation:** No validation (int type), no specific limits
    - **Units:** Raw DirectInput units per second
    - **Priority:** Phase 3, #12

### Phase 4: FFB Output (Depend on Torque)

12. ☐ **[ForceFeedbackVector](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#forcefeedbackvector)**
    - **Description:** 3D force vector with X, Y, Z force components (Newtons), plus Damper (N·s/m) and Inertia (kg·m²)
    - **Source:** Design document and FFB calculation
    - **Note:** Legacy code uses single force magnitude (DirectInput), but design specifies 3D vector for future hardware support
    - **Validation:** Force components reject NaN/Infinity, Damper and Inertia must be >= 0
    - **Units:** Force in Newtons, Damper in N·s/m, Inertia in kg·m²
    - **Priority:** Phase 4, #13

### Phase 5: Configuration (Independent)

13. ☐ **[Scale](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#scale)**
    - **Description:** FFB scale factor as percentage (0-100% or higher for over-scaling)
    - **Source (Legacy):** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
    - **Fields (Legacy):** `Settings.OverallScale`, `Settings.DetailScale`, `Settings.LFEScale`
    - **Used in (Legacy):** FFB calculation (applies scales to torque), auto-overall-scale feature, scale adjustment via buttons
    - **Validation:** Must be >= 0, rejects NaN/Infinity
    - **Units:** Percentage (0.0 = 0%, 100.0 = 100%)
    - **Priority:** Phase 5, #14

14. ☐ **[PidConfig](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#pidconfig)**
    - **Description:** PID controller parameters (Proportional, Integral, Derivative gains) for FFB algorithm tuning
    - **Source:** Design document reference
    - **Note:** Not explicitly found in legacy code, identified in design document for future FFB algorithm enhancements
    - **Validation:** Rejects NaN/Infinity for all gains, no specific range limits
    - **Units:** Unitless gains
    - **Priority:** Phase 5, #15

### Phase 6: Calculated Metrics (Depend on Generic)

15. ☐ **[YawRateFactor](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#yawratefactor)**
    - **Description:** Understeer detection metric calculated from steering angle, speed, and yaw rate
    - **Source (Legacy):** `src/SimRacingFFB/Application/App.ForceFeedback.cs`
    - **Method (Legacy):** `UFF_ProcessYawRateFactor()` - Formula: `steering wheel angle * speed / yaw rate`
    - **Used in (Legacy):** Understeer detection algorithm, skid pad analysis, FFB understeer effect calculations
    - **Reference:** [LegacySimRacingFFB.md](../LegacySimRacingFFB.md) (line 100-102)
    - **Validation:** Rejects NaN/Infinity, calculation requires yaw rate >= 5 degrees/second and speed > 0
    - **Units:** Unitless (ratio/coefficient)
    - **Priority:** Phase 6, #15

16. ☐ **[TelemetryDataPoint](../Detailed%20Design/Value%20Objects/Value%20Objects%20Analysis%20and%20Design.md#telemetrydatapoint)**
    - **Description:** Composite telemetry snapshot - Immutable snapshot containing all telemetry value objects at a specific moment
    - **Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`
    - **Method (Legacy):** `OnTelemetryData()` - Main telemetry processing method called at 60Hz
    - **Used in (Legacy):** Aggregates all telemetry values (steering angle, yaw rate, velocity, G-force, torque, shock velocities, RPM, speed, etc.)
    - **Reference:** [LegacySimRacingFFB.md](../LegacySimRacingFFB.md) (line 226-228)
    - **Validation:** All required value objects must be present, arrays must have correct length (6 samples), timestamp must be valid
    - **Priority:** Phase 6, #16

## Additional Resources

- **[iRacing-Specific Value Objects.md](../Detailed%20Design/Value%20Objects/iRacing-Specific%20Value%20Objects.md)** - Notes on simulator-specific considerations
- **[LegacySimRacingFFB.md](../LegacySimRacingFFB.md)** - Complete legacy codebase analysis
- **[Racing Simulator Force Feedback Design.md](../Racing%20Simulator%20Force%20Feedback%20Design.md)** - High-level architecture and design

## Implementation Status

- ✅ **SteeringWheelAngle** - Completed (Phase 1, #1)
- ☐ **YawRate** - To be implemented (Phase 1, #2)
- ☐ **Speed** - To be implemented (Phase 1, #3)
- ☐ **Torque** - To be implemented (Phase 1, #4)
- ☐ **Velocity** - To be implemented (Phase 1, #5)
- ☐ **GForce** - To be implemented (Phase 1, #6)
- ☐ **EngineRPM** - To be implemented (Phase 1, #7)
- ☐ **ShockVelocity** - To be implemented (Phase 2, #8)
- ☐ **TireLoad** - To be implemented (Phase 2, #10)
- ☐ **WheelPosition** - To be implemented (Phase 3, #11)
- ☐ **WheelVelocity** - To be implemented (Phase 3, #12)
- ☐ **ForceFeedbackVector** - To be implemented (Phase 4, #13)
- ☐ **Scale** - To be implemented (Phase 5, #14)
- ☐ **PidConfig** - To be implemented (Phase 5, #15)
- ☐ **YawRateFactor** - To be implemented (Phase 6, #15)
- ☐ **TelemetryDataPoint** - To be implemented (Phase 6, #16)

## Quick Reference

All value objects are:
- **Generic** - Work with any racing simulator (iRacing, Assetto Corsa, Automobilista, etc.)
- **Immutable** - Use `readonly record struct`
- **Validated** - Reject NaN, Infinity, and out-of-range values
- **Tested** - Full TDD test requirements documented for each

See the main document for complete details on validation rules, TDD test requirements, and implementation recommendations for each value object.

