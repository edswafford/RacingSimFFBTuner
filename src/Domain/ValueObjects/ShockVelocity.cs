namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents suspension shock velocity in meters per second for a single corner of the vehicle.
/// Positive values indicate compression, negative values indicate extension.
/// </summary>
public readonly record struct ShockVelocity
{
    /// <summary>
    /// Gets the shock velocity in meters per second.
    /// </summary>
    public float MetersPerSecond { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShockVelocity"/> struct.
    /// </summary>
    /// <param name="metersPerSecond">The shock velocity in meters per second. Cannot be NaN or Infinity.</param>
    /// <exception cref="ArgumentException">Thrown when metersPerSecond is NaN or Infinity.</exception>
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
