# Value Objects

## Overview

Value objects describe characteristics and have no conceptual identity; they are defined entirely by their attributes and are typically immutable. If two value objects have the same attributes, they are considered equal.

**Important:** The Domain layer must never contain anything specific to a particular racing simulator. All value objects in this document are generic and can be used with any racing simulator (iRacing, Assetto Corsa, Automobilista, etc.). References to "iRacing" appear only when describing the legacy codebase.

## Completed Value Objects

All value objects have been implemented:

- ✅ SteeringWheelAngle
- ✅ YawRate
- ✅ Velocity
- ✅ Speed
- ✅ GForce
- ✅ Torque
- ✅ EngineRPM
- ✅ ShockVelocity
- ✅ TireLoad
- ✅ WheelPosition
- ✅ WheelVelocity
- ✅ ForceFeedbackVector
- ✅ Scale
- ✅ PidConfig
- ✅ YawRateFactor
- ✅ TelemetryDataPoint

## Validation Rules

All value objects follow these high-level validation principles:

- **Physical limits:** Validate against physical limits where applicable (e.g., speed cannot be negative, G-force within safety limits)
- **Invalid values:** Reject NaN and Infinity values
- **Clear errors:** Provide clear error messages when validation fails
- **Value equality:** Two instances with the same values are considered equal

## Testing Philosophy

Value objects are tested using Test-Driven Development (TDD). Tests focus on:

- Valid construction with normal cases
- Boundary conditions (minimum, maximum, zero values)
- Invalid inputs (NaN, Infinity, out-of-range values)
- Value equality semantics
- Conversion methods (if applicable)

---

## Value Objects by Category

### SteeringWheelAngle

**Status:** ✅ COMPLETED

**Capability:** Represents the current steering input angle in a simulator-agnostic manner.

**Context:** Used throughout the domain to represent steering input from telemetry and for force feedback calculations.

**Behaviors:**
1. Accepts angles within physical steering wheel rotation limits
2. Rejects invalid values (NaN, Infinity)
3. Provides conversion between radians and degrees
4. Maintains value equality semantics

**Invariants:**
- Must remain within configured physical limits
- Cannot contain invalid floating-point values

**Legacy Reference:**
- [Legacy iRacing Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs) - Steering wheel angle field

---

### YawRate

**Status:** ✅ COMPLETED

**Capability:** Represents the vehicle's yaw rate (angular velocity around the vertical axis) in radians per second.

**Context:** Used in force feedback calculations and understeer detection algorithms. Combined with steering angle and speed to calculate yaw rate factor.

**Behaviors:**
1. Accepts any real number value (positive indicates rotation right, negative indicates rotation left)
2. Rejects invalid values (NaN, Infinity)
3. Maintains value equality semantics

**Invariants:**
- Cannot contain invalid floating-point values
- Units are always radians per second

**Legacy Reference:**
- [Legacy iRacing Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs) - `_irsdk_yawRate` field
- Used in `UFF_ProcessYawRateFactor()` for understeer detection

---

### Velocity

**Status:** ✅ COMPLETED

**Capability:** Represents a 3D velocity vector with X (forward/backward), Y (left/right lateral), and Z (up/down) components in meters per second.

**Context:** Used throughout the domain for velocity calculations, G-force calculations, and oversteer detection (high lateral velocity indicates sliding).

**Behaviors:**
1. Accepts any real number value for each component
2. Rejects invalid values (NaN, Infinity) for any component
3. Provides magnitude calculation
4. Maintains value equality semantics

**Invariants:**
- All components must be valid floating-point values
- Units are always meters per second for each component

**Legacy Reference:**
- [Legacy iRacing Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs) - `_irsdk_velocityX`, `_irsdk_velocityY`, `_irsdk_velocityZ` fields
- Used for oversteer detection and velocity magnitude calculations

---

### Speed

**Status:** ✅ COMPLETED

**Capability:** Represents the vehicle's speed (velocity magnitude) in meters per second.

**Context:** Used in yaw rate factor calculations, speed scaling for parked/low-speed scenarios in FFB, and auto-center wheel feature when speed is zero.

**Behaviors:**
1. Accepts non-negative values only (speed cannot be negative)
2. Rejects invalid values (NaN, Infinity)
3. Provides conversion to other speed units (km/h, mph)
4. Maintains value equality semantics

**Invariants:**
- Must be greater than or equal to zero
- Cannot contain invalid floating-point values
- Units are always meters per second

**Legacy Reference:**
- [Legacy iRacing Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs) - Calculated from velocityX and velocityY in `OnTelemetryData()` method

---

### GForce

**Status:** ✅ COMPLETED

**Capability:** Represents G-force (acceleration relative to Earth's gravity) calculated from velocity change over time.

**Context:** Used for crash protection detection - high G-forces indicate impacts. Tracks peak G-force over rolling windows.

**Behaviors:**
1. Accepts values between -32 and +32 G-forces (safety limit)
2. Rejects invalid values (NaN, Infinity)
3. Rejects values outside the safety range
4. Maintains value equality semantics

**Invariants:**
- Must be within safety limits (-32 to +32 G-forces)
- Cannot contain invalid floating-point values
- Units are multiples of Earth's gravity (9.81 m/s²)

**Legacy Reference:**
- [Legacy iRacing Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs) - `_irsdk_gForce` field
- Used in `UFF_ProcessGForce()` for crash protection

---

### Torque

**Status:** ✅ COMPLETED

**Capability:** Represents force feedback torque in Newton-meters (Nm).

**Context:** This is the primary input for FFB calculation. Used in core FFB calculation algorithms, delta torque calculations, and steady-state torque calculations.

**Behaviors:**
1. Accepts any real number value (can be positive or negative)
2. Rejects invalid values (NaN, Infinity)
3. Maintains value equality semantics

**Invariants:**
- Cannot contain invalid floating-point values
- Units are always Newton-meters

**Legacy Reference:**
- [Legacy iRacing Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs) - `_irsdk_steeringWheelTorque_ST` array (6 samples at 360Hz)
- Note: Legacy code uses arrays; value object represents a single torque sample

---

### EngineRPM

**Status:** ✅ COMPLETED

**Capability:** Represents engine rotational speed in revolutions per minute (RPM).

**Context:** Used for RPM-based pedal haptics, shift light calculations, and wheel lock/spin detection via RPM/speed ratio.

**Behaviors:**
1. Accepts non-negative values only (RPM cannot be negative)
2. Rejects invalid values (NaN, Infinity)
3. Optionally validates against maximum RPM limits
4. Maintains value equality semantics

**Invariants:**
- Must be greater than or equal to zero
- Cannot contain invalid floating-point values
- Units are always revolutions per minute

**Legacy Reference:**
- [Legacy iRacing Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs) - `_irsdk_rpm` field
- Used in HPR (pedal haptics) and wheel lock/spin detection

---

### ShockVelocity

**Status:** ✅ COMPLETED

**Capability:** Represents suspension shock velocity in meters per second for a single corner of the vehicle.

**Context:** Used for curb protection and suspension analysis. Calculates maximum shock velocity across all corners for curb protection algorithms.

**Behaviors:**
1. Accepts any real number value (positive indicates compression, negative indicates extension)
2. Rejects invalid values (NaN, Infinity)
3. Maintains value equality semantics

**Invariants:**
- Cannot contain invalid floating-point values
- Units are always meters per second

**Legacy Reference:**
- [Legacy iRacing Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs) - Arrays of 6 samples each: `_irsdk_cfShockVel_ST`, `_irsdk_crShockVel_ST`, `_irsdk_lfShockVel_ST`, `_irsdk_lrShockVel_ST`, `_irsdk_rfShockVel_ST`, `_irsdk_rrShockVel_ST`
- Used in `UFF_ProcessShocks()` for curb protection
- Note: Legacy code uses arrays; value object represents a single shock velocity sample

---

### TireLoad

**Status:** ✅ COMPLETED

**Capability:** Represents the current force/load on a single tire in Newtons, along with the slip ratio (unitless).

**Context:** Used in advanced FFB calculations to represent tire force and slip characteristics.

**Behaviors:**
1. Accepts non-negative load values (force cannot be negative)
2. Accepts any real number for slip ratio (typically -1 to 1, but can exceed in extreme cases)
3. Rejects invalid values (NaN, Infinity) for both properties
4. Maintains value equality semantics

**Invariants:**
- Load must be greater than or equal to zero
- Both load and slip ratio cannot contain invalid floating-point values
- Units: Load in Newtons, Slip Ratio is unitless

**Legacy Reference:**
- Design document reference - fundamental racing concept for advanced FFB calculations

---

### WheelPosition

**Status:** ✅ COMPLETED

**Capability:** Represents the raw wheel position from hardware device in DirectInput units.

**Context:** Used for auto-center wheel feature when not on track, wheel calibration, and wheel velocity calculation (from position delta).

**Behaviors:**
1. Accepts integer values (typically -32768 to 32767 for DirectInput, but flexible)
2. Maintains value equality semantics
3. Optionally provides normalization methods

**Invariants:**
- Value represents raw hardware position
- Units are DirectInput raw units

**Legacy Reference:**
- [Legacy Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Application/App.Inputs.cs) - `Input_CurrentWheelPosition` field

---

### WheelVelocity

**Status:** ✅ COMPLETED

**Capability:** Represents wheel velocity calculated from position delta over time in raw units per second.

**Context:** Used by auto-center wheel algorithm to determine wheel movement direction and speed.

**Behaviors:**
1. Accepts any integer value (can be positive or negative)
2. Maintains value equality semantics

**Invariants:**
- Value represents calculated velocity from position delta
- Units are raw DirectInput units per second

**Legacy Reference:**
- [Legacy Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Application/App.Inputs.cs) - `Input_CurrentWheelVelocity` field
- Calculated from position delta divided by frame time

---

### ForceFeedbackVector

**Status:** ✅ COMPLETED

**Capability:** Represents a 3D force feedback vector with X, Y, Z components, plus damper and inertia values to be applied to the wheel.

**Context:** This is the final output of the FFB calculation pipeline. Represents the actual high-frequency force feedback signal to be sent to hardware.

**Behaviors:**
1. Accepts any real number value for force components (X, Y, Z)
2. Accepts non-negative values for damper and inertia (damping and inertia cannot be negative)
3. Rejects invalid values (NaN, Infinity) for all components
4. Provides magnitude calculation
5. Maintains value equality semantics

**Invariants:**
- All force components must be valid floating-point values
- Damper must be greater than or equal to zero
- Inertia must be greater than or equal to zero
- Units: Force in Newtons (X, Y, Z), Damper in N·s/m, Inertia in kg·m²

**Legacy Reference:**
- Design document reference - represents the actual high-frequency force feedback signal
- Note: Legacy code primarily uses DirectInput (single force magnitude), but design specifies 3D vector for future hardware support (e.g., Simucube)

---

### Scale

**Status:** ✅ COMPLETED

**Capability:** Represents a scale factor as a percentage (0-100% or higher for over-scaling).

**Context:** Used for FFB overall scale, detail scale, and LFE (Low Frequency Effects) scale. Applied throughout the FFB calculation pipeline.

**Behaviors:**
1. Accepts non-negative values (typically 0-200% for over-scaling support)
2. Rejects invalid values (NaN, Infinity)
3. Provides conversion between percentage and decimal representations
4. Maintains value equality semantics

**Invariants:**
- Must be greater than or equal to zero
- Cannot contain invalid floating-point values
- Units are percentage (0.0 = 0%, 100.0 = 100%)

**Legacy Reference:**
- [Legacy Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Application/App.ForceFeedback.cs) - `Settings.OverallScale`, `Settings.DetailScale`, `Settings.LFEScale` fields

---

### PidConfig

**Status:** ✅ COMPLETED

**Capability:** Represents PID (Proportional, Integral, Derivative) controller configuration parameters for FFB algorithm tuning.

**Context:** Used for future FFB algorithm enhancements requiring PID control.

**Behaviors:**
1. Accepts any real number value for each gain (gains can be positive or negative depending on control strategy)
2. Rejects invalid values (NaN, Infinity) for all gains
3. Maintains value equality semantics

**Invariants:**
- All gains must be valid floating-point values
- Units are unitless gains (proportional, integral, derivative)

**Legacy Reference:**
- Design document reference - identified for future FFB algorithm enhancements

---

### YawRateFactor

**Status:** ✅ COMPLETED

**Capability:** Represents the yaw rate factor, a calculated metric indicating how much steering input is required for a given yaw rate, used for understeer detection.

**Context:** Used in understeer detection algorithms and skid pad analysis. Formula: steering wheel angle * speed / yaw rate.

**Behaviors:**
1. Accepts any real number value (unitless coefficient)
2. Rejects invalid values (NaN, Infinity)
3. Can be calculated from components (steering angle, speed, yaw rate) when conditions are met
4. Maintains value equality semantics

**Invariants:**
- Cannot contain invalid floating-point values
- Units are unitless (ratio/coefficient)
- Calculation is only valid when yaw rate >= 5 degrees/second and speed > 0

**Legacy Reference:**
- [Legacy Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Application/App.ForceFeedback.cs) - `UFF_ProcessYawRateFactor()` method
- Used for understeer detection and skid pad analysis

---

### TelemetryDataPoint

**Status:** ✅ COMPLETED

**Capability:** Represents an immutable snapshot of all telemetry data at a specific moment, aggregating multiple value objects into a single cohesive unit.

**Context:** Used throughout the system as the primary data structure for telemetry processing. This is the composite value object that contains all telemetry measurements.

**Behaviors:**
1. Contains all core telemetry value objects
2. Optionally contains additional telemetry values (gear, throttle, brake, etc.)
3. Includes timestamp for ordering/sequencing
4. Maintains immutability once created
5. Maintains value equality semantics

**Invariants:**
- All required value objects must be present
- Arrays must have correct length (e.g., 6 samples for torque, 6 corners for shock velocities)
- Timestamp must be valid
- Once created, cannot be modified

**Legacy Reference:**
- [Legacy iRacing Implementation](https://github.com/edswafford/SimRacingFFB/blob/main/src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs) - `OnTelemetryData()` method processes all telemetry values
- Note: This is a composite value object that aggregates all other value objects

---

## References

**Legacy Codebase:** https://github.com/edswafford/SimRacingFFB

**Legacy Documentation:** LegacySimRacingFFB.md

**Design Document:** Racing Simulator Force Feedback Design.md
