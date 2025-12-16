namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents the current force/load on a single tire in Newtons, along with the slip ratio (unitless).
/// </summary>
public readonly record struct TireLoad
{
    /// <summary>
    /// Gets the tire load in Newtons.
    /// </summary>
    public float Load { get; init; }

    /// <summary>
    /// Gets the slip ratio (unitless).
    /// </summary>
    public float SlipRatio { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TireLoad"/> struct.
    /// </summary>
    /// <param name="load">The tire load in Newtons. Must be greater than or equal to zero. Cannot be NaN or Infinity.</param>
    /// <param name="slipRatio">The slip ratio (unitless). Cannot be NaN or Infinity.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when load is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when load or slipRatio is NaN or Infinity.</exception>
    public TireLoad(float load, float slipRatio)
    {
        ValidateLoad(load);
        ValidateSlipRatio(slipRatio);

        Load = load;
        SlipRatio = slipRatio;
    }

    private static void ValidateLoad(float load)
    {
        if (float.IsNaN(load) || float.IsInfinity(load))
        {
            throw new ArgumentException("Load cannot be NaN or Infinity.", nameof(load));
        }

        if (load < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(load),
                load,
                "Load must be greater than or equal to zero.");
        }
    }

    private static void ValidateSlipRatio(float slipRatio)
    {
        if (float.IsNaN(slipRatio) || float.IsInfinity(slipRatio))
        {
            throw new ArgumentException("SlipRatio cannot be NaN or Infinity.", nameof(slipRatio));
        }
    }
}
