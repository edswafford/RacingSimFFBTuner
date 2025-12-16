namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents a 3D force feedback vector with X, Y, Z components, plus damper and inertia values to be applied to the wheel.
/// </summary>
public readonly record struct ForceFeedbackVector
{
    /// <summary>
    /// Gets the X component of the force vector in Newtons.
    /// </summary>
    public float X { get; init; }

    /// <summary>
    /// Gets the Y component of the force vector in Newtons.
    /// </summary>
    public float Y { get; init; }

    /// <summary>
    /// Gets the Z component of the force vector in Newtons.
    /// </summary>
    public float Z { get; init; }

    /// <summary>
    /// Gets the damper value in N·s/m.
    /// </summary>
    public float Damper { get; init; }

    /// <summary>
    /// Gets the inertia value in kg·m².
    /// </summary>
    public float Inertia { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForceFeedbackVector"/> struct.
    /// </summary>
    /// <param name="x">The X component of the force vector in Newtons. Cannot be NaN or Infinity.</param>
    /// <param name="y">The Y component of the force vector in Newtons. Cannot be NaN or Infinity.</param>
    /// <param name="z">The Z component of the force vector in Newtons. Cannot be NaN or Infinity.</param>
    /// <param name="damper">The damper value in N·s/m. Must be greater than or equal to zero. Cannot be NaN or Infinity.</param>
    /// <param name="inertia">The inertia value in kg·m². Must be greater than or equal to zero. Cannot be NaN or Infinity.</param>
    /// <exception cref="ArgumentException">Thrown when any force component, damper, or inertia is NaN or Infinity.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when damper or inertia is negative.</exception>
    public ForceFeedbackVector(float x, float y, float z, float damper, float inertia)
    {
        ValidateForceComponent(x, nameof(x));
        ValidateForceComponent(y, nameof(y));
        ValidateForceComponent(z, nameof(z));
        ValidateDamper(damper);
        ValidateInertia(inertia);

        X = x;
        Y = y;
        Z = z;
        Damper = damper;
        Inertia = inertia;
    }

    /// <summary>
    /// Creates a zero force feedback vector with all components set to zero.
    /// </summary>
    /// <returns>A <see cref="ForceFeedbackVector"/> with all components set to zero.</returns>
    public static ForceFeedbackVector Zero()
    {
        return new ForceFeedbackVector(0f, 0f, 0f, 0f, 0f);
    }

    /// <summary>
    /// Calculates the magnitude (length) of the force vector.
    /// </summary>
    /// <returns>The magnitude of the force vector in Newtons.</returns>
    public float Magnitude()
    {
        return MathF.Sqrt((X * X) + (Y * Y) + (Z * Z));
    }

    private static void ValidateForceComponent(float value, string paramName)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            throw new ArgumentException($"{paramName} cannot be NaN or Infinity.", paramName);
        }
    }

    private static void ValidateDamper(float damper)
    {
        if (float.IsNaN(damper) || float.IsInfinity(damper))
        {
            throw new ArgumentException("Damper cannot be NaN or Infinity.", nameof(damper));
        }

        if (damper < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(damper),
                damper,
                "Damper must be greater than or equal to zero.");
        }
    }

    private static void ValidateInertia(float inertia)
    {
        if (float.IsNaN(inertia) || float.IsInfinity(inertia))
        {
            throw new ArgumentException("Inertia cannot be NaN or Infinity.", nameof(inertia));
        }

        if (inertia < 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(inertia),
                inertia,
                "Inertia must be greater than or equal to zero.");
        }
    }
}
