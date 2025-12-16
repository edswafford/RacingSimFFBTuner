namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents the raw wheel position from hardware device in DirectInput units (typically -32768 to 32767).
/// </summary>
public readonly record struct WheelPosition
{
    /// <summary>
    /// Gets the raw wheel position value in DirectInput units.
    /// </summary>
    public int RawValue { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WheelPosition"/> struct.
    /// </summary>
    /// <param name="rawValue">The raw wheel position value from the hardware device (typically -32768 to 32767 for DirectInput).</param>
    public WheelPosition(int rawValue)
    {
        RawValue = rawValue;
    }

    /// <summary>
    /// Normalizes the wheel position to a -1.0 to 1.0 range.
    /// </summary>
    /// <param name="maxRawValue">The maximum raw value for normalization. Defaults to 32767 (typical DirectInput maximum).</param>
    /// <returns>The normalized wheel position value between -1.0 and 1.0.</returns>
    public float Normalized(float maxRawValue = 32767f)
    {
        return RawValue / maxRawValue;
    }
}
