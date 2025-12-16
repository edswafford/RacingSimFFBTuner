# iRacing-Specific Value Objects

## Purpose

This document identifies value objects or concepts that are specific to iRacing's telemetry format and cannot be easily generalized for use with other racing simulators. These should **NOT** be implemented in the Domain layer, as the Domain layer must remain simulator-agnostic.

## Important Note

**All value objects in the Domain layer must be generic and simulator-agnostic.** This document serves as a reference for:
1. Understanding what iRacing-specific concepts exist in the legacy codebase
2. Identifying where simulator-specific adapters in the Infrastructure layer will need to handle format differences
3. Documenting any iRacing-specific calculations or data structures that cannot be generalized

## Current Status

**All identified value objects from the legacy codebase are generic and have been included in the main [Value Objects Analysis and Design.md](Value%20Objects%20Analysis%20and%20Design.md) document.**

The following concepts are generic and work with any racing simulator:
- SteeringWheelAngle
- YawRate
- Velocity
- Speed
- GForce
- Torque
- EngineRPM
- ShockVelocity
- TireLoad
- WheelPosition
- WheelVelocity
- ForceFeedbackVector
- Scale
- PidConfig
- YawRateFactor (understeer detection is a universal concept)
- TelemetryDataPoint (composite of generic telemetry)

## iRacing-Specific Considerations

While the value objects themselves are generic, the Infrastructure layer adapters for iRacing may need to handle:

### Telemetry Format Differences

**Note:** These are implementation details for Infrastructure adapters, not Domain value objects:

1. **High-Frequency Sample Arrays:** iRacing provides 6 samples per frame at 360Hz for some telemetry (torque, shock velocities). Other simulators may provide different sample rates or formats.

2. **Telemetry Field Names:** iRacing uses specific field names (e.g., `_irsdk_steeringWheelAngle`). The Infrastructure adapter will map these to generic value objects.

3. **Calculation Methods:** Some calculations in the legacy code (like yaw rate factor) use specific formulas. These formulas are generic concepts, but the exact implementation may vary by simulator.

### Future Considerations

If, in the future, we discover value objects that are truly iRacing-specific and cannot be generalized, they should be documented here. However, the goal is to keep the Domain layer completely simulator-agnostic.

**Examples of what would NOT be a Domain value object:**
- iRacing session info structures
- iRacing-specific telemetry field mappings
- iRacing SDK-specific data formats

These belong in the Infrastructure layer as adapter implementations.

## Summary

Currently, there are **no iRacing-specific value objects** that need to be implemented. All value objects are generic and suitable for the Domain layer.

The Infrastructure layer will contain iRacing-specific adapters that:
- Read iRacing telemetry format
- Convert iRacing data to generic Domain value objects
- Handle iRacing-specific connection and communication protocols

**See:** [Value Objects Analysis and Design.md](Value%20Objects%20Analysis%20and%20Design.md) for all value object specifications.

