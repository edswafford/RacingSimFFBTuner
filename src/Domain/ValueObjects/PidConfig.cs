namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents PID (Proportional, Integral, Derivative) controller configuration parameters
/// for FFB algorithm tuning.
/// </summary>
public readonly record struct PidConfig
{
    /// <summary>
    /// Gets the proportional gain (P gain).
    /// </summary>
    public float Proportional { get; init; }

    /// <summary>
    /// Gets the integral gain (I gain).
    /// </summary>
    public float Integral { get; init; }

    /// <summary>
    /// Gets the derivative gain (D gain).
    /// </summary>
    public float Derivative { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PidConfig"/> struct.
    /// </summary>
    /// <param name="proportional">The proportional gain (P gain). Cannot be NaN or Infinity.</param>
    /// <param name="integral">The integral gain (I gain). Cannot be NaN or Infinity.</param>
    /// <param name="derivative">The derivative gain (D gain). Cannot be NaN or Infinity.</param>
    /// <exception cref="ArgumentException">Thrown when any gain is NaN or Infinity.</exception>
    public PidConfig(float proportional, float integral, float derivative)
    {
        ValidateGain(proportional, nameof(proportional));
        ValidateGain(integral, nameof(integral));
        ValidateGain(derivative, nameof(derivative));

        Proportional = proportional;
        Integral = integral;
        Derivative = derivative;
    }

    private static void ValidateGain(float gain, string paramName)
    {
        if (float.IsNaN(gain) || float.IsInfinity(gain))
        {
            throw new ArgumentException($"{paramName} cannot be NaN or Infinity.", paramName);
        }
    }
}
