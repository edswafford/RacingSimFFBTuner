namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents the yaw rate factor, which is a calculated metric indicating how much steering input is required for a given yaw rate.
/// Used for understeer detection. Formula: steering wheel angle * speed / yaw rate.
/// </summary>
public readonly record struct YawRateFactor
{
    /// <summary>
    /// Gets the yaw rate factor value (unitless coefficient).
    /// </summary>
    public float Value { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="YawRateFactor"/> struct.
    /// </summary>
    /// <param name="value">The yaw rate factor value. Cannot be NaN or Infinity.</param>
    /// <exception cref="ArgumentException">Thrown when value is NaN or Infinity.</exception>
    private YawRateFactor(float value)
    {
        ValidateValue(value);
        Value = value;
    }

    /// <summary>
    /// Creates a new <see cref="YawRateFactor"/> from steering wheel angle, speed, and yaw rate components.
    /// </summary>
    /// <param name="angle">The steering wheel angle.</param>
    /// <param name="speed">The vehicle speed. Must be greater than zero.</param>
    /// <param name="yawRate">The yaw rate. Must be at least 5 degrees/second (≈0.0873 rad/s) and non-zero.</param>
    /// <returns>A new <see cref="YawRateFactor"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when yaw rate is below minimum, speed is zero, or yaw rate is zero.</exception>
    public static YawRateFactor FromComponents(
        SteeringWheelAngle angle,
        Speed speed,
        YawRate yawRate)
    {
        // Convert 5 degrees/second to radians
        const float MinYawRateRadiansPerSecond = 5f * MathF.PI / 180f;

        if (MathF.Abs(yawRate.RadiansPerSecond) < float.Epsilon)
        {
            throw new InvalidOperationException(
                "Yaw rate cannot be zero to calculate yaw rate factor.");
        }

        if (MathF.Abs(yawRate.RadiansPerSecond) < MinYawRateRadiansPerSecond)
        {
            throw new InvalidOperationException(
                $"Yaw rate must be at least 5 degrees/second ({MinYawRateRadiansPerSecond} rad/s) to calculate yaw rate factor.");
        }

        if (speed.MetersPerSecond <= 0f)
        {
            throw new InvalidOperationException(
                "Speed must be greater than zero to calculate yaw rate factor.");
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
