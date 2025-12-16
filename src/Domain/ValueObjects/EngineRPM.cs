namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents engine rotational speed in revolutions per minute (RPM).
/// </summary>
public readonly record struct EngineRPM
{
    /// <summary>
    /// Gets the engine RPM value.
    /// </summary>
    public float Value { get; init; }

    /// <summary>
    /// Gets the optional maximum RPM for validation.
    /// </summary>
    public float? MaxRPM { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EngineRPM"/> struct.
    /// </summary>
    /// <param name="value">The engine RPM value. Must be greater than or equal to zero.</param>
    /// <param name="maxRPM">Optional maximum RPM for validation. If provided, must be greater than zero and value must not exceed it.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when value is negative, exceeds maxRPM, or maxRPM is invalid.</exception>
    /// <exception cref="ArgumentException">Thrown when value or maxRPM is NaN or Infinity.</exception>
    public EngineRPM(float value, float? maxRPM = null)
    {
        ValidateValue(value, maxRPM);
        Value = value;
        MaxRPM = maxRPM;
    }

    /// <summary>
    /// Calculates the percentage of maximum RPM.
    /// </summary>
    /// <returns>The percentage of maximum RPM (0-100).</returns>
    /// <exception cref="InvalidOperationException">Thrown when MaxRPM is not set or is less than or equal to zero.</exception>
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

        if (maxRPM.HasValue)
        {
            ValidateMaxRPM(maxRPM.Value);

            if (value > maxRPM.Value)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    value,
                    $"Value must not exceed MaxRPM of {maxRPM.Value}.");
            }
        }
    }

    private static void ValidateMaxRPM(float maxRPM)
    {
        if (float.IsNaN(maxRPM) || float.IsInfinity(maxRPM))
        {
            throw new ArgumentException("MaxRPM cannot be NaN or Infinity.", nameof(maxRPM));
        }

        if (maxRPM <= 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxRPM),
                maxRPM,
                "MaxRPM must be greater than zero.");
        }
    }
}
