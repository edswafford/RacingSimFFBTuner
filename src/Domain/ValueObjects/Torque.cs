namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents force feedback torque in Newton-meters (Nm).
/// This is the primary input for FFB calculation.
/// </summary>
public readonly record struct Torque
{
    /// <summary>
    /// Gets the torque in Newton-meters (Nm).
    /// </summary>
    public float NewtonMeters { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Torque"/> struct.
    /// </summary>
    /// <param name="newtonMeters">The torque in Newton-meters. Cannot be NaN or Infinity.</param>
    /// <exception cref="ArgumentException">Thrown when newtonMeters is NaN or Infinity.</exception>
    public Torque(float newtonMeters)
    {
        ValidateNewtonMeters(newtonMeters);
        NewtonMeters = newtonMeters;
    }

    private static void ValidateNewtonMeters(float newtonMeters)
    {
        if (float.IsNaN(newtonMeters) || float.IsInfinity(newtonMeters))
        {
            throw new ArgumentException("NewtonMeters cannot be NaN or Infinity.", nameof(newtonMeters));
        }
    }
}
