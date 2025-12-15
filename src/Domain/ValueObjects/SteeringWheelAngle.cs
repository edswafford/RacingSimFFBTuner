namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents a steering wheel angle value object.
/// Immutable value object that validates steering angle is within maximum steering lock.
/// </summary>
public readonly record struct SteeringWheelAngle
{
    /// <summary>
    /// Gets the steering wheel angle in radians.
    /// </summary>
    public float Radians { get; init; }

    /// <summary>
    /// Gets the maximum steering lock in radians.
    /// </summary>
    public float MaxRadians { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SteeringWheelAngle"/> struct.
    /// </summary>
    /// <param name="radians">The steering wheel angle in radians. Must be between -maxRadians and +maxRadians.</param>
    /// <param name="maxRadians">The maximum steering lock in radians. Must be greater than zero.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when radians exceeds maxRadians or maxRadians is invalid.</exception>
    /// <exception cref="ArgumentException">Thrown when radians or maxRadians is NaN or Infinity.</exception>
    public SteeringWheelAngle(float radians, float maxRadians)
    {
        ValidateMaxRadians(maxRadians);
        ValidateRadians(radians, maxRadians);

        Radians = radians;
        MaxRadians = maxRadians;
    }

    /// <summary>
    /// Converts the steering wheel angle from radians to degrees.
    /// </summary>
    /// <returns>The steering wheel angle in degrees.</returns>
    public float ToDegrees()
    {
        return Radians * 180f / MathF.PI;
    }

    /// <summary>
    /// Creates a new <see cref="SteeringWheelAngle"/> from degrees.
    /// </summary>
    /// <param name="degrees">The steering wheel angle in degrees. Must be between -maxDegrees and +maxDegrees.</param>
    /// <param name="maxRadians">The maximum steering lock in radians. Must be greater than zero.</param>
    /// <returns>A new <see cref="SteeringWheelAngle"/> instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when degrees exceeds maxDegrees or maxRadians is invalid.</exception>
    /// <exception cref="ArgumentException">Thrown when degrees or maxRadians is NaN or Infinity.</exception>
    public static SteeringWheelAngle FromDegrees(float degrees, float maxRadians)
    {
        var maxDegrees = maxRadians * 180f / MathF.PI;

        if (float.IsNaN(degrees) || float.IsInfinity(degrees))
        {
            throw new ArgumentException("Degrees cannot be NaN or Infinity.", nameof(degrees));
        }

        if (MathF.Abs(degrees) > maxDegrees)
        {
            throw new ArgumentOutOfRangeException(
                nameof(degrees),
                degrees,
                $"Degrees must be between -{maxDegrees} and +{maxDegrees}.");
        }

        var radians = degrees * MathF.PI / 180f;
        return new SteeringWheelAngle(radians, maxRadians);
    }

    private static void ValidateMaxRadians(float maxRadians)
    {
        if (float.IsNaN(maxRadians) || float.IsInfinity(maxRadians))
        {
            throw new ArgumentException("MaxRadians cannot be NaN or Infinity.", nameof(maxRadians));
        }

        if (maxRadians <= 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxRadians),
                maxRadians,
                "MaxRadians must be greater than zero.");
        }
    }

    private static void ValidateRadians(float radians, float maxRadians)
    {
        if (float.IsNaN(radians) || float.IsInfinity(radians))
        {
            throw new ArgumentException("Radians cannot be NaN or Infinity.", nameof(radians));
        }

        if (MathF.Abs(radians) > maxRadians)
        {
            throw new ArgumentOutOfRangeException(
                nameof(radians),
                radians,
                $"Radians must be between -{maxRadians} and +{maxRadians}.");
        }
    }
}
