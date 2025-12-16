namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents wheel velocity calculated from position delta over time in raw units per second.
/// </summary>
public readonly record struct WheelVelocity
{
    /// <summary>
    /// Gets the wheel velocity in raw DirectInput units per second.
    /// </summary>
    public int RawUnitsPerSecond { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WheelVelocity"/> struct.
    /// </summary>
    /// <param name="rawUnitsPerSecond">The wheel velocity in raw DirectInput units per second.</param>
    public WheelVelocity(int rawUnitsPerSecond)
    {
        RawUnitsPerSecond = rawUnitsPerSecond;
    }
}
