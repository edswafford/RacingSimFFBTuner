# Value Objects Analysis and Design

## Purpose

This document provides a comprehensive analysis of all value objects identified from the legacy MarvinsAIRA codebase at `C:\dev\FFB\SimRacing\SimRacingFFB`. The analysis serves as the foundation for implementing value objects in the refactored RacingSimFFBTuner application using Test-Driven Development (TDD).

**Important:** The Domain layer must never contain anything specific to a particular racing simulator. All value objects in this document are generic and can be used with any racing simulator (iRacing, Assetto Corsa, Automobilista, etc.). References to "iRacing" appear only when describing the legacy codebase.

## Table of Contents

1. [Overview and Implementation Strategy](#overview-and-implementation-strategy)
2. [Value Objects by Category](#value-objects-by-category)
   - [SteeringWheelAngle](#steeringwheelangle) ✅ COMPLETED
   - [YawRate](#yawrate)
   - [Velocity](#velocity)
   - [Speed](#speed)
   - [GForce](#gforce)
   - [Torque](#torque)
   - [EngineRPM](#enginerpm)
   - [ShockVelocity](#shockvelocity)
   - [TireLoad](#tireload)
   - [WheelPosition](#wheelposition)
   - [WheelVelocity](#wheelvelocity)
   - [ForceFeedbackVector](#forcefeedbackvector)
   - [Scale](#scale)
   - [PidConfig](#pidconfig)
   - [YawRateFactor](#yawratefactor)
   - [TelemetryDataPoint](#telemetrydatapoint)
3. [Implementation Recommendations](#implementation-recommendations)
4. [Challenges and Solutions](#challenges-and-solutions)

---

## Overview and Implementation Strategy

### Implementation Order

Following the TDD approach and dependency analysis, value objects should be implemented in this order:

#### Phase 1: Foundation (Generic, No Dependencies)
1. ✅ **SteeringWheelAngle** - COMPLETED
2. **YawRate** - Angular velocity (radians/second)
3. **Speed** - Velocity magnitude
4. **Torque** - Force feedback torque in Newton-meters
5. **Velocity** - 3D velocity vector (X, Y, Z components)
6. **GForce** - Calculated from velocity change
7. **EngineRPM** - Engine rotational speed

#### Phase 2: Telemetry Components (Depend on Phase 1)
8. **ShockVelocity** - Suspension shock velocity per corner
9. **TireLoad** - Tire force and slip ratio

#### Phase 3: Hardware Input (Independent)
10. **WheelPosition** - Raw hardware wheel position
11. **WheelVelocity** - Calculated from wheel position

#### Phase 4: FFB Output (Depend on Torque)
12. **ForceFeedbackVector** - 3D force vector (X, Y, Z, Damper, Inertia)

#### Phase 5: Configuration (Independent)
13. **Scale** - FFB scale values (overall, detail, LFE)
14. **PidConfig** - PID controller parameters

#### Phase 6: Calculated Metrics (Depend on Generic)
15. **YawRateFactor** - Calculated from steering angle, speed, yaw rate (for understeer detection)
16. **TelemetryDataPoint** - Composite value object containing all telemetry

### Pattern to Follow

All value objects should follow the pattern established in `SteeringWheelAngle.cs`:

1. **Properties first** (IDesign ordering)
2. **Constructor after properties** with validation
3. **Public methods before private validation methods**
4. **XML documentation comments** (even though SA0001 is disabled, existing code has them)
5. **File ends with single newline** (SA1518)
6. **Immutable** - Use `readonly record struct`
7. **Validation** - Reject NaN, Infinity, and out-of-range values
8. **Value equality** - Two instances with same values are equal

### Validation Strategy

Each value object should:
- Validate against physical limits where applicable
- Reject NaN and Infinity values
- Provide clear error messages
- Use appropriate exception types (ArgumentOutOfRangeException, ArgumentException)

### Testing Requirements

For each value object, TDD tests should cover:

1. **Valid construction** - Normal cases with valid inputs
2. **Boundary conditions** - Minimum, maximum, zero values
3. **Invalid inputs** - NaN, Infinity, out-of-range values
4. **Equality** - Value equality, not reference equality
5. **Conversion methods** - If applicable (e.g., radians to degrees)
6. **Factory methods** - If applicable (e.g., FromDegrees)

---

## Value Objects by Category

### SteeringWheelAngle

✅ **COMPLETED** - See `src/Domain/ValueObjects/SteeringWheelAngle.cs`

**Priority:** Phase 1, #1

---

### YawRate

#### Description
Represents the vehicle's yaw rate (angular velocity around the vertical axis) in radians per second. Positive values indicate rotation to the right, negative to the left.

#### Usage in Legacy Code

**Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`

**Field (Legacy):** `public float _irsdk_yawRate`
- **Type:** `float`
- **Synopsis:** Vehicle yaw rate in radians per second. Combined with steering angle and speed to calculate yaw rate factor for understeer detection.

**Used in (Legacy):**
- `UFF_ProcessYawRateFactor()` - Calculates yaw rate factor: `steering wheel angle * speed / yaw rate`
- Understeer detection algorithm
- FFB calculation pipeline

**Reference:** [LegacySimRacingFFB.md](../../LegacySimRacingFFB.md) (line 260-262)

#### Validation Rules

1. **Range:** No specific physical limits (can be any real number)
2. **Invalid values:** Reject NaN and Infinity
3. **Units:** Radians per second

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with positive value
- ✅ Create with negative value
- ✅ Create with zero value
- ✅ Create with large positive value
- ✅ Create with large negative value

##### Invalid Inputs
- ❌ NaN should throw ArgumentException
- ❌ PositiveInfinity should throw ArgumentException
- ❌ NegativeInfinity should throw ArgumentException

##### Equality
- ✅ Two instances with same value are equal
- ✅ Two instances with different values are not equal

##### Conversion Methods (if needed)
- Consider `ToDegreesPerSecond()` if needed for display

#### Implementation Recommendations

```csharp
public readonly record struct YawRate
{
    public float RadiansPerSecond { get; init; }
    
    public YawRate(float radiansPerSecond)
    {
        ValidateRadiansPerSecond(radiansPerSecond);
        RadiansPerSecond = radiansPerSecond;
    }
    
    private static void ValidateRadiansPerSecond(float radiansPerSecond)
    {
        if (float.IsNaN(radiansPerSecond) || float.IsInfinity(radiansPerSecond))
        {
            throw new ArgumentException("RadiansPerSecond cannot be NaN or Infinity.", nameof(radiansPerSecond));
        }
    }
}
```

**Priority:** High (Phase 1, #2)

---

### Velocity

#### Description
Represents a 3D velocity vector with X (forward/backward), Y (left/right lateral), and Z (up/down) components in meters per second.

#### Usage in Legacy Code

**Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`

**Fields (Legacy):**
- `_irsdk_velocityX` - Forward/backward velocity (not explicitly documented but used)
- `_irsdk_velocityY` - Lateral (sideways) velocity in meters per second
- `_irsdk_velocityZ` - Vertical velocity (not explicitly documented but used)

**Field (Legacy):** `public float _irsdk_velocityY`
- **Type:** `float`
- **Synopsis:** Lateral (sideways) velocity in meters per second. Used for oversteer detection - high lateral velocity indicates sliding.

**Used in (Legacy):**
- Oversteer detection (high lateral velocity)
- Velocity magnitude calculation: `MathF.Sqrt(velocityX * velocityX + velocityY * velocityY)`
- G-force calculation from velocity change over time

**Reference:** [LegacySimRacingFFB.md](../../LegacySimRacingFFB.md) (line 264-266)

#### Validation Rules

1. **Range:** No specific physical limits (can be any real number for each component)
2. **Invalid values:** Reject NaN and Infinity for any component
3. **Units:** Meters per second for each component

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with all positive components
- ✅ Create with all negative components
- ✅ Create with mixed positive/negative components
- ✅ Create with zero components
- ✅ Create with large values

##### Invalid Inputs
- ❌ X component NaN should throw ArgumentException
- ❌ Y component Infinity should throw ArgumentException
- ❌ Z component NaN should throw ArgumentException
- ❌ Any component with invalid value should throw

##### Equality
- ✅ Two instances with same X, Y, Z are equal
- ✅ Two instances with different values are not equal

##### Methods
- `Magnitude()` - Calculate velocity magnitude: `MathF.Sqrt(X * X + Y * Y + Z * Z)`
- `Lateral()` - Get lateral velocity (Y component)
- `Forward()` - Get forward velocity (X component)

#### Implementation Recommendations

```csharp
public readonly record struct Velocity
{
    public float X { get; init; }  // Forward/backward (m/s)
    public float Y { get; init; }  // Lateral/left-right (m/s)
    public float Z { get; init; }  // Vertical/up-down (m/s)
    
    public Velocity(float x, float y, float z)
    {
        ValidateComponent(x, nameof(x));
        ValidateComponent(y, nameof(y));
        ValidateComponent(z, nameof(z));
        
        X = x;
        Y = y;
        Z = z;
    }
    
    public float Magnitude() => MathF.Sqrt(X * X + Y * Y + Z * Z);
    public float Lateral() => Y;
    public float Forward() => X;
    
    private static void ValidateComponent(float value, string paramName)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            throw new ArgumentException($"{paramName} cannot be NaN or Infinity.", paramName);
        }
    }
}
```

**Priority:** High (Phase 1, #5)

---

### Speed

#### Description
Represents the vehicle's speed (velocity magnitude) in meters per second. This is a scalar value derived from the velocity vector.

#### Usage in Legacy Code

**Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`

**Calculation (Legacy):** Velocity magnitude calculated from `velocityX` and `velocityY`:
```csharp
var velocityMagnitude = MathF.Sqrt(velocityX * velocityX + velocityY * velocityY);
```

**Used in (Legacy):**
- Yaw rate factor calculation: `steering wheel angle * speed / yaw rate`
- Speed scaling for parked/low-speed scenarios in FFB
- Auto-center wheel feature (when speed is zero)

**Reference:** Calculated in `OnTelemetryData()` method

#### Validation Rules

1. **Range:** Must be >= 0 (speed cannot be negative)
2. **Invalid values:** Reject NaN and Infinity
3. **Units:** Meters per second

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with zero
- ✅ Create with positive value
- ✅ Create with large positive value

##### Invalid Inputs
- ❌ Negative value should throw ArgumentOutOfRangeException
- ❌ NaN should throw ArgumentException
- ❌ Infinity should throw ArgumentException

##### Equality
- ✅ Two instances with same value are equal

##### Conversion Methods
- `ToKilometersPerHour()` - Convert m/s to km/h
- `ToMilesPerHour()` - Convert m/s to mph
- `FromKilometersPerHour(float kmh)` - Factory method
- `FromMilesPerHour(float mph)` - Factory method

#### Implementation Recommendations

```csharp
public readonly record struct Speed
{
    public float MetersPerSecond { get; init; }
    
    public Speed(float metersPerSecond)
    {
        ValidateMetersPerSecond(metersPerSecond);
        MetersPerSecond = metersPerSecond;
    }
    
    public float ToKilometersPerHour() => MetersPerSecond * 3.6f;
    public float ToMilesPerHour() => MetersPerSecond * 2.237f;
    
    public static Speed FromKilometersPerHour(float kmh)
    {
        var mps = kmh / 3.6f;
        return new Speed(mps);
    }
    
    public static Speed FromMilesPerHour(float mph)
    {
        var mps = mph / 2.237f;
        return new Speed(mps);
    }
    
    private static void ValidateMetersPerSecond(float metersPerSecond)
    {
        if (float.IsNaN(metersPerSecond) || float.IsInfinity(metersPerSecond))
        {
            throw new ArgumentException("MetersPerSecond cannot be NaN or Infinity.", nameof(metersPerSecond));
        }
        
        if (metersPerSecond < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(metersPerSecond),
                metersPerSecond,
                "MetersPerSecond must be greater than or equal to zero.");
        }
    }
}
```

**Priority:** High (Phase 1, #3)

---

### GForce

#### Description
Represents G-force (acceleration relative to Earth's gravity) calculated from velocity change over time. Used for crash protection detection.

#### Usage in Legacy Code

**Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`

**Field (Legacy):** `public float _irsdk_gForce`
- **Type:** `float`
- **Synopsis:** Calculated G-force from velocity change over time. Used for crash protection - high G-forces indicate impacts.

**Calculation (Legacy):** Calculated in `OnTelemetryData()` from velocity change:
- Only calculated when on-track and lap distance hasn't jumped
- Derived from velocity change over time

**Used in (Legacy):**
- `UFF_ProcessGForce()` - Tracks peak G-force over 2-second rolling window
- Crash protection - scales forces based on G-force threshold
- UI display of peak G-force

**Reference:** [LegacySimRacingFFB.md](../../LegacySimRacingFFB.md) (line 268-270, 104-106)

#### Validation Rules

1. **Range:** No specific upper limit (can be very high during crashes)
2. **Invalid values:** Reject NaN and Infinity
3. **Units:** G-forces (multiples of Earth's gravity, 9.81 m/s²)

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with zero (no acceleration)
- ✅ Create with positive value (acceleration in one direction)
- ✅ Create with negative value (deceleration)
- ✅ Create with large positive value (crash scenario)
- ✅ Create with large negative value (hard braking)

##### Invalid Inputs
- ❌ NaN should throw ArgumentException
- ❌ Infinity should throw ArgumentException

##### Equality
- ✅ Two instances with same value are equal

#### Implementation Recommendations

```csharp
public readonly record struct GForce
{
    public float Value { get; init; }  // In G-forces (multiples of 9.81 m/s²)
    
    public GForce(float value)
    {
        ValidateValue(value);
        Value = value;
    }
    
    private static void ValidateValue(float value)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            throw new ArgumentException("Value cannot be NaN or Infinity.", nameof(value));
        }
    }
}
```

**Priority:** Medium (Phase 1, #6)

---

### Torque

#### Description
Represents force feedback torque in Newton-meters (Nm). This is the primary input for FFB calculation.

#### Usage in Legacy Code

**Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`

**Field (Legacy):** `public float[] _irsdk_steeringWheelTorque_ST`
- **Type:** `float[]`
- **Synopsis:** Array of 6 samples containing steering wheel torque at 360Hz (6 samples per 60Hz frame). This is the primary input for force feedback calculation. The "_ST" suffix indicates "Sample Time" data (high-frequency samples within a frame).

**Used in (Legacy):**
- Core FFB calculation algorithm
- Delta torque calculation
- Steady-state torque calculation with exponential smoothing
- FFB recording/playback
- Auto-overall-scale peak tracking

**Reference:** [LegacySimRacingFFB.md](../../LegacySimRacingFFB.md) (line 248-250)

**Note:** The legacy code uses an array of 6 samples. For the value object, we'll represent a single torque sample. Arrays will be handled at a higher level (Domain Services or Use Cases).

#### Validation Rules

1. **Range:** No specific physical limits (can be positive or negative)
2. **Invalid values:** Reject NaN and Infinity
3. **Units:** Newton-meters (Nm)

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with positive value
- ✅ Create with negative value
- ✅ Create with zero
- ✅ Create with large positive value
- ✅ Create with large negative value

##### Invalid Inputs
- ❌ NaN should throw ArgumentException
- ❌ Infinity should throw ArgumentException

##### Equality
- ✅ Two instances with same value are equal

#### Implementation Recommendations

```csharp
public readonly record struct Torque
{
    public float NewtonMeters { get; init; }
    
    public Torque(float newtonMeters)
    {
        ValidateNewtonMeters(newtonMeters);
        NewtonMeters = newtonMeters;
    }
    
    private static void ValidateNewtonMeters(float newtonMeters)
    {
        if (float.IsNaN(newtonMeters) || float.IsInfinity(newtonMeters))
        {
            throw new ArgumentException("NewtonMeters cannot be NaN or Infinity.", nameof(newtonMeters));
        }
    }
}
```

**Priority:** High (Phase 1, #4)

---

### EngineRPM

#### Description
Represents engine rotational speed in revolutions per minute (RPM).

#### Usage in Legacy Code

**Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`

**Field (Legacy):** `_irsdk_rpm` (referenced in HPR code)
- Used for RPM-based pedal haptics
- Used for shift light calculations
- Used for wheel lock/spin detection (RPM/speed ratio)

**Used in (Legacy):**
- HPR (pedal haptics) - RPM-based effects
- Logitech LED shift lights
- Wheel lock/spin detection via RPM/speed ratio

**Reference:** Referenced in `App.SimagicHPR.cs` for RPM/speed ratio calculations

#### Validation Rules

1. **Range:** Must be >= 0 (RPM cannot be negative)
2. **Invalid values:** Reject NaN and Infinity
3. **Units:** Revolutions per minute (RPM)
4. **Optional:** MaxRPM limit for validation (car-specific)

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with zero (engine off)
- ✅ Create with positive value
- ✅ Create with large positive value
- ✅ Create with MaxRPM limit (if implemented)

##### Invalid Inputs
- ❌ Negative value should throw ArgumentOutOfRangeException
- ❌ NaN should throw ArgumentException
- ❌ Infinity should throw ArgumentException
- ❌ Value exceeding MaxRPM (if MaxRPM provided) should throw

##### Equality
- ✅ Two instances with same value are equal

##### Optional Features
- Consider `MaxRPM` property for validation
- Consider `PercentageOfMax()` method if MaxRPM is provided

#### Implementation Recommendations

```csharp
public readonly record struct EngineRPM
{
    public float Value { get; init; }  // In RPM
    public float? MaxRPM { get; init; }  // Optional maximum RPM for validation
    
    public EngineRPM(float value, float? maxRPM = null)
    {
        ValidateValue(value, maxRPM);
        Value = value;
        MaxRPM = maxRPM;
    }
    
    public float PercentageOfMax()
    {
        if (MaxRPM == null || MaxRPM.Value <= 0f)
        {
            throw new InvalidOperationException("MaxRPM must be set to calculate percentage.");
        }
        
        return Value / MaxRPM.Value * 100f;
    }
    
    private static void ValidateValue(float value, float? maxRPM)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            throw new ArgumentException("Value cannot be NaN or Infinity.", nameof(value));
        }
        
        if (value < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                "Value must be greater than or equal to zero.");
        }
        
        if (maxRPM.HasValue && value > maxRPM.Value)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"Value must not exceed MaxRPM of {maxRPM.Value}.");
        }
    }
}
```

**Priority:** Medium (Phase 1, #7)

---

### ShockVelocity

#### Description
Represents suspension shock velocity in meters per second for a single corner of the vehicle.

#### Usage in Legacy Code

**Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`

**Fields (Legacy):** Arrays of 6 samples each at 360Hz:
- `_irsdk_cfShockVel_ST` - Center Front
- `_irsdk_crShockVel_ST` - Center Rear
- `_irsdk_lfShockVel_ST` - Left Front
- `_irsdk_lrShockVel_ST` - Left Rear
- `_irsdk_rfShockVel_ST` - Right Front
- `_irsdk_rrShockVel_ST` - Right Rear

**Synopsis (Legacy):** Arrays of 6 samples each for shock velocities at 360Hz for all six suspension corners. Used for curb protection and suspension analysis.

**Used in (Legacy):**
- `UFF_ProcessShocks()` - Calculates maximum shock velocity across all corners
- Curb protection - reduces detail scale based on shock velocity
- Peak shock velocity tracking over 2-second window

**Reference:** [LegacySimRacingFFB.md](../../LegacySimRacingFFB.md) (line 252-254, 108-110)

**Note:** The legacy code uses arrays. For the value object, we'll represent a single shock velocity sample. Arrays and corner identification will be handled at a higher level.

#### Validation Rules

1. **Range:** No specific physical limits (can be positive or negative)
2. **Invalid values:** Reject NaN and Infinity
3. **Units:** Meters per second (m/s)

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with positive value (compression)
- ✅ Create with negative value (extension)
- ✅ Create with zero
- ✅ Create with large positive value
- ✅ Create with large negative value

##### Invalid Inputs
- ❌ NaN should throw ArgumentException
- ❌ Infinity should throw ArgumentException

##### Equality
- ✅ Two instances with same value are equal

#### Implementation Recommendations

```csharp
public readonly record struct ShockVelocity
{
    public float MetersPerSecond { get; init; }
    
    public ShockVelocity(float metersPerSecond)
    {
        ValidateMetersPerSecond(metersPerSecond);
        MetersPerSecond = metersPerSecond;
    }
    
    private static void ValidateMetersPerSecond(float metersPerSecond)
    {
        if (float.IsNaN(metersPerSecond) || float.IsInfinity(metersPerSecond))
        {
            throw new ArgumentException("MetersPerSecond cannot be NaN or Infinity.", nameof(metersPerSecond));
        }
    }
}
```

**Priority:** Medium (Phase 2, #8)

---

### TireLoad

#### Description
Represents the current force/load on a single tire in Newtons, along with the slip ratio (unitless).

#### Usage in Legacy Code

**Source:** Design document reference

**From Design:** "Represents the current force/load on a single tire. Defined by the Load (Newtons); Slip Ratio (unitless)."

**Note:** While not explicitly found in the legacy code documentation, this is a fundamental racing concept that would be used in advanced FFB calculations. The design document identifies it as a value object.

#### Validation Rules

1. **Load Range:** Must be >= 0 (force cannot be negative)
2. **Slip Ratio Range:** Typically -1 to 1, but can exceed in extreme cases
3. **Invalid values:** Reject NaN and Infinity for both properties
4. **Units:** Load in Newtons, Slip Ratio is unitless

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with positive load and zero slip ratio
- ✅ Create with positive load and positive slip ratio
- ✅ Create with positive load and negative slip ratio
- ✅ Create with zero load
- ✅ Create with large load values

##### Invalid Inputs
- ❌ Negative load should throw ArgumentOutOfRangeException
- ❌ Load NaN should throw ArgumentException
- ❌ Slip ratio NaN should throw ArgumentException
- ❌ Any component Infinity should throw ArgumentException

##### Equality
- ✅ Two instances with same load and slip ratio are equal
- ✅ Two instances with different values are not equal

#### Implementation Recommendations

```csharp
public readonly record struct TireLoad
{
    public float Load { get; init; }  // In Newtons
    public float SlipRatio { get; init; }  // Unitless
    
    public TireLoad(float load, float slipRatio)
    {
        ValidateLoad(load);
        ValidateSlipRatio(slipRatio);
        
        Load = load;
        SlipRatio = slipRatio;
    }
    
    private static void ValidateLoad(float load)
    {
        if (float.IsNaN(load) || float.IsInfinity(load))
        {
            throw new ArgumentException("Load cannot be NaN or Infinity.", nameof(load));
        }
        
        if (load < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(load),
                load,
                "Load must be greater than or equal to zero.");
        }
    }
    
    private static void ValidateSlipRatio(float slipRatio)
    {
        if (float.IsNaN(slipRatio) || float.IsInfinity(slipRatio))
        {
            throw new ArgumentException("SlipRatio cannot be NaN or Infinity.", nameof(slipRatio));
        }
    }
}
```

**Priority:** Low (Phase 2, #10) - May not be immediately needed

---

### WheelPosition

#### Description
Represents the raw wheel position from hardware device in DirectInput units (typically -32768 to 32767).

#### Usage in Legacy Code

**Source (Legacy):** `src/SimRacingFFB/Application/App.Inputs.cs`

**Field (Legacy):** `public int Input_CurrentWheelPosition`
- **Type:** `int`
- **Synopsis:** Current raw wheel position value from the selected axis (typically -32768 to 32767 for DirectInput). Used for auto-center wheel feature and wheel calibration. Updated each frame from the FFB device's selected axis.

**Used in (Legacy):**
- Auto-center wheel feature when not on track
- Wheel calibration
- Wheel velocity calculation (from position delta)

**Reference:** [LegacySimRacingFFB.md](../../LegacySimRacingFFB.md) (line 303-305)

#### Validation Rules

1. **Range:** Typically -32768 to 32767 for DirectInput, but should be flexible
2. **Invalid values:** No NaN/Infinity (int type)
3. **Units:** Raw DirectInput units

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with zero (centered)
- ✅ Create with positive value
- ✅ Create with negative value
- ✅ Create with maximum positive value (32767)
- ✅ Create with maximum negative value (-32768)

##### Equality
- ✅ Two instances with same value are equal

##### Conversion Methods (if needed)
- Consider normalization methods if needed for calculations

#### Implementation Recommendations

```csharp
public readonly record struct WheelPosition
{
    public int RawValue { get; init; }  // DirectInput raw units
    
    public WheelPosition(int rawValue)
    {
        RawValue = rawValue;
    }
    
    // Optional: Normalize to -1.0 to 1.0 range
    public float Normalized(float maxRawValue = 32767f)
    {
        return RawValue / maxRawValue;
    }
}
```

**Priority:** Medium (Phase 3, #11)

---

### WheelVelocity

#### Description
Represents wheel velocity calculated from position delta over time in raw units per second.

#### Usage in Legacy Code

**Source (Legacy):** `src/SimRacingFFB/Application/App.Inputs.cs`

**Field (Legacy):** `public int Input_CurrentWheelVelocity`
- **Type:** `int`
- **Synopsis:** Calculated wheel velocity in raw units per second. Computed from position delta divided by frame time. Used by auto-center wheel algorithm to determine wheel movement direction and speed.

**Used in (Legacy):**
- Auto-center wheel algorithm (velocity-based type 0)
- Determining wheel movement direction and speed

**Reference:** [LegacySimRacingFFB.md](../../LegacySimRacingFFB.md) (line 307-309)

#### Validation Rules

1. **Range:** No specific limits (can be positive or negative)
2. **Invalid values:** No NaN/Infinity (int type)
3. **Units:** Raw DirectInput units per second

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with zero (stationary)
- ✅ Create with positive value (moving in one direction)
- ✅ Create with negative value (moving in opposite direction)
- ✅ Create with large positive value
- ✅ Create with large negative value

##### Equality
- ✅ Two instances with same value are equal

#### Implementation Recommendations

```csharp
public readonly record struct WheelVelocity
{
    public int RawUnitsPerSecond { get; init; }
    
    public WheelVelocity(int rawUnitsPerSecond)
    {
        RawUnitsPerSecond = rawUnitsPerSecond;
    }
}
```

**Priority:** Medium (Phase 3, #12)

---

### ForceFeedbackVector

#### Description
Represents a 3D force feedback vector with X, Y, Z components, plus damper and inertia values to be applied to the wheel.

#### Usage in Legacy Code

**Source:** Design document and FFB calculation

**From Design:** "The actual high-frequency force feedback signal. Represents a 3D force vector (X, Y, Z components) to be applied to the wheel. Immutable force data. Data: Force (Newtons), Damper (N·s/m), Inertia (kg·m²)."

**Note:** The legacy code primarily uses DirectInput which uses a single force magnitude. However, the design document specifies a 3D vector with damper and inertia for future hardware support (e.g., Simucube).

#### Validation Rules

1. **Range:** No specific limits for force components (can be positive or negative)
2. **Damper Range:** Must be >= 0 (damping cannot be negative)
3. **Inertia Range:** Must be >= 0 (inertia cannot be negative)
4. **Invalid values:** Reject NaN and Infinity for all components
5. **Units:** 
   - Force: Newtons (X, Y, Z)
   - Damper: N·s/m
   - Inertia: kg·m²

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with all zero components
- ✅ Create with positive force components
- ✅ Create with negative force components
- ✅ Create with positive damper and inertia
- ✅ Create with mixed force components

##### Invalid Inputs
- ❌ X component NaN should throw ArgumentException
- ❌ Y component Infinity should throw ArgumentException
- ❌ Negative damper should throw ArgumentOutOfRangeException
- ❌ Negative inertia should throw ArgumentOutOfRangeException
- ❌ Any component with invalid value should throw

##### Equality
- ✅ Two instances with same X, Y, Z, Damper, Inertia are equal
- ✅ Two instances with different values are not equal

##### Methods
- `Magnitude()` - Calculate force magnitude: `MathF.Sqrt(X * X + Y * Y + Z * Z)`
- `Zero()` - Static factory for zero vector

#### Implementation Recommendations

```csharp
public readonly record struct ForceFeedbackVector
{
    public float X { get; init; }  // Force in Newtons
    public float Y { get; init; }  // Force in Newtons
    public float Z { get; init; }  // Force in Newtons
    public float Damper { get; init; }  // N·s/m
    public float Inertia { get; init; }  // kg·m²
    
    public ForceFeedbackVector(float x, float y, float z, float damper, float inertia)
    {
        ValidateForceComponent(x, nameof(x));
        ValidateForceComponent(y, nameof(y));
        ValidateForceComponent(z, nameof(z));
        ValidateDamper(damper);
        ValidateInertia(inertia);
        
        X = x;
        Y = y;
        Z = z;
        Damper = damper;
        Inertia = inertia;
    }
    
    public static ForceFeedbackVector Zero() => new(0f, 0f, 0f, 0f, 0f);
    
    public float Magnitude() => MathF.Sqrt(X * X + Y * Y + Z * Z);
    
    private static void ValidateForceComponent(float value, string paramName)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            throw new ArgumentException($"{paramName} cannot be NaN or Infinity.", paramName);
        }
    }
    
    private static void ValidateDamper(float damper)
    {
        if (float.IsNaN(damper) || float.IsInfinity(damper))
        {
            throw new ArgumentException("Damper cannot be NaN or Infinity.", nameof(damper));
        }
        
        if (damper < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(damper),
                damper,
                "Damper must be greater than or equal to zero.");
        }
    }
    
    private static void ValidateInertia(float inertia)
    {
        if (float.IsNaN(inertia) || float.IsInfinity(inertia))
        {
            throw new ArgumentException("Inertia cannot be NaN or Infinity.", nameof(inertia));
        }
        
        if (inertia < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(inertia),
                inertia,
                "Inertia must be greater than or equal to zero.");
        }
    }
}
```

**Priority:** High (Phase 4, #13)

---

### Scale

#### Description
Represents a scale factor as a percentage (0-100% or higher for over-scaling). Used for FFB overall scale, detail scale, and LFE scale.

#### Usage in Legacy Code

**Source (Legacy):** `src/SimRacingFFB/Application/App.ForceFeedback.cs`

**Fields (Legacy):**
- `Settings.OverallScale` - Overall FFB strength scale
- `Settings.DetailScale` - Detail/impulse scale
- `Settings.LFEScale` - Low Frequency Effects scale

**Used in (Legacy):**
- FFB calculation - applies scales to torque values
- Auto-overall-scale feature
- Scale adjustment via buttons

**Reference:** Used throughout FFB calculation pipeline

#### Validation Rules

1. **Range:** Typically 0-200% (can exceed 100% for over-scaling)
2. **Invalid values:** Reject NaN and Infinity
3. **Units:** Percentage (0.0 = 0%, 100.0 = 100%)

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with zero
- ✅ Create with 100 (100%)
- ✅ Create with values > 100 (over-scaling)
- ✅ Create with decimal values (e.g., 50.5)

##### Invalid Inputs
- ❌ Negative value should throw ArgumentOutOfRangeException
- ❌ NaN should throw ArgumentException
- ❌ Infinity should throw ArgumentException

##### Equality
- ✅ Two instances with same value are equal

##### Methods
- `AsDecimal()` - Convert percentage to decimal (100% = 1.0)
- `FromDecimal(float decimal)` - Factory method from decimal

#### Implementation Recommendations

```csharp
public readonly record struct Scale
{
    public float Percentage { get; init; }  // 0.0 = 0%, 100.0 = 100%
    
    public Scale(float percentage)
    {
        ValidatePercentage(percentage);
        Percentage = percentage;
    }
    
    public float AsDecimal() => Percentage / 100f;
    
    public static Scale FromDecimal(float decimalValue)
    {
        var percentage = decimalValue * 100f;
        return new Scale(percentage);
    }
    
    private static void ValidatePercentage(float percentage)
    {
        if (float.IsNaN(percentage) || float.IsInfinity(percentage))
        {
            throw new ArgumentException("Percentage cannot be NaN or Infinity.", nameof(percentage));
        }
        
        if (percentage < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(percentage),
                percentage,
                "Percentage must be greater than or equal to zero.");
        }
    }
}
```

**Priority:** Low (Phase 5, #14)

---

### PidConfig

#### Description
Represents PID (Proportional, Integral, Derivative) controller configuration parameters for FFB algorithm tuning.

#### Usage in Legacy Code

**Source:** Design document reference

**From Design:** "Configuration parameters for the FFB algorithm (e.g., Proportional, Integral, Derivative gains"

**Note:** While not explicitly found in the legacy code, this is identified in the design document as a value object for future FFB algorithm enhancements.

#### Validation Rules

1. **Range:** No specific limits (gains can be positive or negative depending on control strategy)
2. **Invalid values:** Reject NaN and Infinity for all gains
3. **Units:** Unitless gains (proportional, integral, derivative)

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with all zero gains
- ✅ Create with positive gains
- ✅ Create with negative gains
- ✅ Create with mixed positive/negative gains

##### Invalid Inputs
- ❌ Proportional gain NaN should throw ArgumentException
- ❌ Integral gain Infinity should throw ArgumentException
- ❌ Derivative gain NaN should throw ArgumentException
- ❌ Any gain with invalid value should throw

##### Equality
- ✅ Two instances with same P, I, D values are equal
- ✅ Two instances with different values are not equal

#### Implementation Recommendations

```csharp
public readonly record struct PidConfig
{
    public float Proportional { get; init; }  // P gain
    public float Integral { get; init; }  // I gain
    public float Derivative { get; init; }  // D gain
    
    public PidConfig(float proportional, float integral, float derivative)
    {
        ValidateGain(proportional, nameof(proportional));
        ValidateGain(integral, nameof(integral));
        ValidateGain(derivative, nameof(derivative));
        
        Proportional = proportional;
        Integral = integral;
        Derivative = derivative;
    }
    
    private static void ValidateGain(float gain, string paramName)
    {
        if (float.IsNaN(gain) || float.IsInfinity(gain))
        {
            throw new ArgumentException($"{paramName} cannot be NaN or Infinity.", paramName);
        }
    }
}
```

**Priority:** Low (Phase 5, #15) - May not be immediately needed

---

### YawRateFactor

#### Description
Represents the yaw rate factor, which is a calculated metric indicating how much steering input is required for a given yaw rate. Used for understeer detection. Formula: `steering wheel angle * speed / yaw rate`.

**Note:** This is a generic concept (understeer detection metric) that can be used with any racing simulator. The specific formula was derived from the legacy codebase, but the concept is universal.

#### Usage in Legacy Code

**Source (Legacy):** `src/SimRacingFFB/Application/App.ForceFeedback.cs`

**Method (Legacy):** `UFF_ProcessYawRateFactor()`
- **Synopsis:** Calculates the instant yaw rate factor (steering wheel angle * speed / yaw rate) which indicates how much steering input is required for a given yaw rate, used for understeer detection. Only calculates when yaw rate is significant (>= 5 degrees/second) and vehicle has forward velocity. Maintains a circular buffer of 120 samples (2 seconds at 60Hz) to calculate average yaw rate factor for skid pad analysis. The yaw rate factor is a key metric for detecting understeer conditions.

**Calculation:**
```csharp
var yawRateFactor = steeringWheelAngle * speed / yawRate;
```

**Used in (Legacy):**
- Understeer detection algorithm
- Skid pad analysis (average yaw rate factor over 2 seconds)
- FFB understeer effect calculations

**Reference:** [LegacySimRacingFFB.md](../../LegacySimRacingFFB.md) (line 100-102)

#### Validation Rules

1. **Range:** No specific physical limits (can be any real number)
2. **Invalid values:** Reject NaN and Infinity
3. **Units:** Unitless (ratio/coefficient)
4. **Calculation constraints:** Only valid when yaw rate >= 5 degrees/second and speed > 0

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with positive value
- ✅ Create with negative value
- ✅ Create with zero
- ✅ Create with large positive value
- ✅ Create with large negative value

##### Invalid Inputs
- ❌ NaN should throw ArgumentException
- ❌ Infinity should throw ArgumentException

##### Equality
- ✅ Two instances with same value are equal

##### Factory Methods
- `FromComponents(SteeringWheelAngle angle, Speed speed, YawRate yawRate)` - Calculate from components
  - Should only calculate when yaw rate >= 5 degrees/second (≈0.0873 rad/s) and speed > 0
  - Should throw InvalidOperationException if conditions not met

#### Implementation Recommendations

```csharp
public readonly record struct YawRateFactor
{
    public float Value { get; init; }  // Unitless coefficient
    
    private YawRateFactor(float value)
    {
        ValidateValue(value);
        Value = value;
    }
    
    public static YawRateFactor FromComponents(
        SteeringWheelAngle angle,
        Speed speed,
        YawRate yawRate)
    {
        // Convert 5 degrees/second to radians
        const float MinYawRateRadiansPerSecond = 5f * MathF.PI / 180f;
        
        if (yawRate.RadiansPerSecond < MinYawRateRadiansPerSecond)
        {
            throw new InvalidOperationException(
                $"Yaw rate must be at least 5 degrees/second ({MinYawRateRadiansPerSecond} rad/s) to calculate yaw rate factor.");
        }
        
        if (speed.MetersPerSecond <= 0f)
        {
            throw new InvalidOperationException(
                "Speed must be greater than zero to calculate yaw rate factor.");
        }
        
        if (MathF.Abs(yawRate.RadiansPerSecond) < float.Epsilon)
        {
            throw new InvalidOperationException(
                "Yaw rate cannot be zero to calculate yaw rate factor.");
        }
        
        var factor = angle.Radians * speed.MetersPerSecond / yawRate.RadiansPerSecond;
        return new YawRateFactor(factor);
    }
    
    private static void ValidateValue(float value)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            throw new ArgumentException("Value cannot be NaN or Infinity.", nameof(value));
        }
    }
}
```

**Priority:** Medium (Phase 6, #15) - Depends on SteeringWheelAngle, Speed, YawRate

---

### TelemetryDataPoint

#### Description
A composite value object representing an immutable snapshot of all telemetry data at a specific moment. This aggregates multiple value objects into a single cohesive unit.

#### Usage in Legacy Code

**Source (Legacy):** `src/SimRacingFFB/Simulators/IRacing/App.IRacingSDK.cs`

**Method (Legacy):** `OnTelemetryData()`
- **Synopsis:** Main telemetry processing method called at 60Hz when the simulator sends telemetry updates. Reads all telemetry values including:
  - Steering wheel angle
  - Yaw rate
  - Velocity (X, Y, Z)
  - G-force
  - Steering wheel torque (array of 6 samples)
  - Shock velocities (arrays for 6 corners)
  - Engine RPM
  - Speed (calculated)
  - And many other values

**Reference:** [LegacySimRacingFFB.md](../../LegacySimRacingFFB.md) (line 226-228)

**Note:** This is a composite value object that will contain references to other value objects. It should be implemented last, after all individual value objects are complete.

#### Validation Rules

1. **Required components:** All core telemetry value objects must be present
2. **Optional components:** Some values may be optional (e.g., when not on track)
3. **Timestamp:** Should include a timestamp for ordering/sequencing
4. **Immutable:** Once created, cannot be modified

#### TDD Test Requirements

##### Valid Construction
- ✅ Create with all required value objects
- ✅ Create with optional values
- ✅ Create with minimal required values

##### Invalid Inputs
- ❌ Null required value object should throw ArgumentNullException
- ❌ Invalid timestamp should throw ArgumentException
- ❌ Arrays with incorrect length should throw ArgumentException

##### Equality
- ✅ Two instances with same values are equal
- ✅ Two instances with different timestamps are not equal (if timestamp included)

##### Properties
- All contained value objects should be accessible as properties
- Timestamp property for sequencing

#### Implementation Recommendations

```csharp
public readonly record struct TelemetryDataPoint
{
    public SteeringWheelAngle SteeringWheelAngle { get; init; }
    public YawRate YawRate { get; init; }
    public Velocity Velocity { get; init; }
    public Speed Speed { get; init; }
    public GForce GForce { get; init; }
    public EngineRPM EngineRPM { get; init; }
    public Torque[] SteeringWheelTorqueSamples { get; init; }  // 6 samples at 360Hz
    public ShockVelocity[] ShockVelocities { get; init; }  // 6 corners
    public DateTime Timestamp { get; init; }
    
    // Optional values
    public bool IsOnTrack { get; init; }
    public int Gear { get; init; }
    public float Throttle { get; init; }  // 0.0 to 1.0
    public float Brake { get; init; }  // 0.0 to 1.0
    
    public TelemetryDataPoint(
        SteeringWheelAngle steeringWheelAngle,
        YawRate yawRate,
        Velocity velocity,
        Speed speed,
        GForce gForce,
        EngineRPM engineRPM,
        Torque[] steeringWheelTorqueSamples,
        ShockVelocity[] shockVelocities,
        DateTime timestamp,
        bool isOnTrack = true,
        int gear = 0,
        float throttle = 0f,
        float brake = 0f)
    {
        ValidateRequiredComponents(
            steeringWheelTorqueSamples,
            shockVelocities,
            timestamp);
        
        SteeringWheelAngle = steeringWheelAngle;
        YawRate = yawRate;
        Velocity = velocity;
        Speed = speed;
        GForce = gForce;
        EngineRPM = engineRPM;
        SteeringWheelTorqueSamples = steeringWheelTorqueSamples;
        ShockVelocities = shockVelocities;
        Timestamp = timestamp;
        IsOnTrack = isOnTrack;
        Gear = gear;
        Throttle = throttle;
        Brake = brake;
    }
    
    private static void ValidateRequiredComponents(
        Torque[] steeringWheelTorqueSamples,
        ShockVelocity[] shockVelocities,
        DateTime timestamp)
    {
        if (steeringWheelTorqueSamples == null)
        {
            throw new ArgumentNullException(nameof(steeringWheelTorqueSamples));
        }
        
        if (steeringWheelTorqueSamples.Length != 6)
        {
            throw new ArgumentException(
                "SteeringWheelTorqueSamples must contain exactly 6 samples.",
                nameof(steeringWheelTorqueSamples));
        }
        
        if (shockVelocities == null)
        {
            throw new ArgumentNullException(nameof(shockVelocities));
        }
        
        if (shockVelocities.Length != 6)
        {
            throw new ArgumentException(
                "ShockVelocities must contain exactly 6 samples (one per corner).",
                nameof(shockVelocities));
        }
        
        if (timestamp == default)
        {
            throw new ArgumentException(
                "Timestamp must be a valid DateTime value.",
                nameof(timestamp));
        }
    }
}
```

**Priority:** Very Low (Phase 6, #16) - Depends on ALL other value objects

---

## Implementation Recommendations

### Pattern to Follow
All value objects should follow the pattern established in `SteeringWheelAngle.cs`:

1. **Properties first** (IDesign ordering)
2. **Constructor after properties** with validation
3. **Public methods before private validation methods**
4. **XML documentation comments** (even though SA0001 is disabled, existing code has them)
5. **File ends with single newline** (SA1518)
6. **Immutable** - Use `readonly record struct`
7. **Validation** - Reject NaN, Infinity, and out-of-range values
8. **Value equality** - Two instances with same values are equal

### Validation Strategy

Each value object should:
- Validate against physical limits where applicable
- Reject NaN and Infinity values
- Provide clear error messages
- Use appropriate exception types (ArgumentOutOfRangeException, ArgumentException)

### Testing Requirements

For each value object, TDD tests should cover:

1. **Valid construction** - Normal cases with valid inputs
2. **Boundary conditions** - Minimum, maximum, zero values
3. **Invalid inputs** - NaN, Infinity, out-of-range values
4. **Equality** - Value equality, not reference equality
5. **Conversion methods** - If applicable (e.g., radians to degrees)
6. **Factory methods** - If applicable (e.g., FromDegrees)

---

## Challenges and Solutions

### Challenge 1: High-Frequency Arrays
**Issue:** Some telemetry comes as arrays (e.g., steering wheel torque with 6 samples at 360Hz)

**Solution:** 
- For now, treat individual samples as value objects (e.g., `Torque`)
- Arrays can be handled at a higher level (Domain Services or Use Cases)
- Consider `TorqueSample` or `TorqueArray` value objects later if needed

### Challenge 2: Simulator-Specific vs Generic
**Issue:** Some data comes from simulator SDKs but represents generic concepts

**Solution:**
- Extract generic concepts first (Velocity, Speed, Torque)
- Create simulator-specific adapters in Infrastructure layer
- Use value objects in Domain layer, adapters convert simulator format to value objects
- Domain layer never references specific simulators

### Challenge 3: Calculated Values
**Issue:** Some values are calculated from others (e.g., GForce from velocity change, YawRateFactor from multiple inputs)

**Solution:**
- Create value objects for calculated values
- Calculation logic goes in Domain Services
- Value objects represent the result, not the calculation

### Challenge 4: Composite Value Objects
**Issue:** `TelemetryDataPoint` contains many value objects

**Solution:**
- Implement individual value objects first
- Create composite value object last
- Composite can reference other value objects

---

## References

- Legacy Codebase: `C:\dev\FFB\SimRacing\SimRacingFFB`
- Legacy Documentation: [LegacySimRacingFFB.md](../../LegacySimRacingFFB.md)
- Design Document: [Racing Simulator Force Feedback Design.md](../../Racing%20Simulator%20Force%20Feedback%20Design.md)
- Existing Implementation: `src/Domain/ValueObjects/SteeringWheelAngle.cs`
- iRacing-Specific Considerations: [iRacing-Specific Value Objects.md](iRacing-Specific%20Value%20Objects.md)

---

## Summary

This document covers all value objects for the Domain layer. All value objects are generic and can be used with any racing simulator. The Domain layer follows Clean Architecture principles and contains no simulator-specific code.

**Key Principles:**
- Domain layer is simulator-agnostic
- All value objects represent generic racing concepts
- Simulator-specific adapters live in Infrastructure layer
- References to "iRacing" appear only when describing legacy code

**Next Steps:**
1. Review this document
2. Begin implementing Phase 1 value objects using TDD
3. Reference [iRacing-Specific Value Objects.md](iRacing-Specific%20Value%20Objects.md) for simulator-specific implementation details that cannot be generalized

