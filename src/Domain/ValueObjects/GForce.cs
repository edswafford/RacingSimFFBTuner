namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents G-force (acceleration relative to Earth's gravity) calculated from velocity change over time.
/// Used for crash protection detection.
/// </summary>
public readonly record struct GForce
{
    /// <summary>
    /// The maximum allowed G-force value for safety reasons.
    /// </summary>
    public const float MaxGForce = 32f;

    /// <summary>
    /// Gets the G-force value in multiples of Earth's gravity (9.81 m/s²).
    /// </summary>
    public float Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GForce"/> struct.
    /// </summary>
    /// <param name="value">The G-force value in multiples of Earth's gravity. Must be between -32 and +32 G-forces. Cannot be NaN or Infinity.</param>
    /// <exception cref="ArgumentException">Thrown when value is NaN or Infinity.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when value exceeds the maximum allowed range of ±32 G-forces.</exception>
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

        if (value > MaxGForce)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"Value must be less than or equal to {MaxGForce} G-forces.");
        }

        if (value < -MaxGForce)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                $"Value must be greater than or equal to {-MaxGForce} G-forces.");
        }
    }
}
