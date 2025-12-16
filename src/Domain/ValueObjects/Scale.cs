namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents a scale factor as a percentage (0-100% or higher for over-scaling).
/// Used for FFB overall scale, detail scale, and LFE scale.
/// </summary>
public readonly record struct Scale
{
    /// <summary>
    /// Gets the scale percentage. 0.0 = 0%, 100.0 = 100%.
    /// </summary>
    public float Percentage { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Scale"/> struct.
    /// </summary>
    /// <param name="percentage">The scale percentage. Must be greater than or equal to zero. 0.0 = 0%, 100.0 = 100%.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when percentage is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when percentage is NaN or Infinity.</exception>
    public Scale(float percentage)
    {
        ValidatePercentage(percentage);
        Percentage = percentage;
    }

    /// <summary>
    /// Converts the percentage to decimal form (100% = 1.0).
    /// </summary>
    /// <returns>The scale as a decimal value where 100% = 1.0.</returns>
    public float AsDecimal()
    {
        return Percentage / 100f;
    }

    /// <summary>
    /// Creates a new <see cref="Scale"/> from a decimal value.
    /// </summary>
    /// <param name="decimalValue">The scale as a decimal value (1.0 = 100%). Must be greater than or equal to zero.</param>
    /// <returns>A new <see cref="Scale"/> instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when decimalValue is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when decimalValue is NaN or Infinity.</exception>
    public static Scale FromDecimal(float decimalValue)
    {
        if (float.IsNaN(decimalValue) || float.IsInfinity(decimalValue))
        {
            throw new ArgumentException("decimalValue cannot be NaN or Infinity.", nameof(decimalValue));
        }

        if (decimalValue < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(decimalValue),
                decimalValue,
                "decimalValue must be greater than or equal to zero.");
        }

        var percentage = decimalValue * 100f;
        return new Scale(percentage);
    }

    private static void ValidatePercentage(float percentage)
    {
        if (float.IsNaN(percentage) || float.IsInfinity(percentage))
        {
            throw new ArgumentException("Percentage cannot be NaN or Infinity.", nameof(percentage));
        }

        if (percentage < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(percentage),
                percentage,
                "Percentage must be greater than or equal to zero.");
        }
    }
}
