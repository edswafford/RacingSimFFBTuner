namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents the vehicle's yaw rate (angular velocity around the vertical axis) in radians per second.
/// Positive values indicate rotation to the right, negative to the left.
/// </summary>
public readonly record struct YawRate
{
    /// <summary>
    /// Gets the yaw rate in radians per second.
    /// </summary>
    public float RadiansPerSecond { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="YawRate"/> struct.
    /// </summary>
    /// <param name="radiansPerSecond">The yaw rate in radians per second. Cannot be NaN or Infinity.</param>
    /// <exception cref="ArgumentException">Thrown when radiansPerSecond is NaN or Infinity.</exception>
    public YawRate(float radiansPerSecond)
    {
        ValidateRadiansPerSecond(radiansPerSecond);
        RadiansPerSecond = radiansPerSecond;
    }

    /// <summary>
    /// Converts the yaw rate from radians per second to degrees per second.
    /// </summary>
    /// <returns>The yaw rate in degrees per second.</returns>
    public float ToDegreesPerSecond()
    {
        return RadiansPerSecond * 180f / MathF.PI;
    }

    private static void ValidateRadiansPerSecond(float radiansPerSecond)
    {
        if (float.IsNaN(radiansPerSecond) || float.IsInfinity(radiansPerSecond))
        {
            throw new ArgumentException("RadiansPerSecond cannot be NaN or Infinity.", nameof(radiansPerSecond));
        }
    }
}
