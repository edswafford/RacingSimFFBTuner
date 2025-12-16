namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents the vehicle's speed (velocity magnitude) in meters per second.
/// This is a scalar value derived from the velocity vector.
/// </summary>
public readonly record struct Speed
{
    /// <summary>
    /// Gets the speed in meters per second.
    /// </summary>
    public float MetersPerSecond { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Speed"/> struct.
    /// </summary>
    /// <param name="metersPerSecond">The speed in meters per second. Must be greater than or equal to zero.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when metersPerSecond is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when metersPerSecond is NaN or Infinity.</exception>
    public Speed(float metersPerSecond)
    {
        ValidateMetersPerSecond(metersPerSecond);
        MetersPerSecond = metersPerSecond;
    }

    /// <summary>
    /// Converts the speed from meters per second to kilometers per hour.
    /// </summary>
    /// <returns>The speed in kilometers per hour.</returns>
    public float ToKilometersPerHour()
    {
        return MetersPerSecond * 3.6f;
    }

    /// <summary>
    /// Converts the speed from meters per second to miles per hour.
    /// </summary>
    /// <returns>The speed in miles per hour.</returns>
    public float ToMilesPerHour()
    {
        return MetersPerSecond * 2.237f;
    }

    /// <summary>
    /// Creates a new <see cref="Speed"/> from kilometers per hour.
    /// </summary>
    /// <param name="kmh">The speed in kilometers per hour. Must be greater than or equal to zero.</param>
    /// <returns>A new <see cref="Speed"/> instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when kmh is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when kmh is NaN or Infinity.</exception>
    public static Speed FromKilometersPerHour(float kmh)
    {
        if (float.IsNaN(kmh) || float.IsInfinity(kmh))
        {
            throw new ArgumentException("kmh cannot be NaN or Infinity.", nameof(kmh));
        }

        if (kmh < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(kmh),
                kmh,
                "kmh must be greater than or equal to zero.");
        }

        var mps = kmh / 3.6f;
        return new Speed(mps);
    }

    /// <summary>
    /// Creates a new <see cref="Speed"/> from miles per hour.
    /// </summary>
    /// <param name="mph">The speed in miles per hour. Must be greater than or equal to zero.</param>
    /// <returns>A new <see cref="Speed"/> instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when mph is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when mph is NaN or Infinity.</exception>
    public static Speed FromMilesPerHour(float mph)
    {
        if (float.IsNaN(mph) || float.IsInfinity(mph))
        {
            throw new ArgumentException("mph cannot be NaN or Infinity.", nameof(mph));
        }

        if (mph < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(mph),
                mph,
                "mph must be greater than or equal to zero.");
        }

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
