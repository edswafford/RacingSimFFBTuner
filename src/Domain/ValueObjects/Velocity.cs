namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// Represents a 3D velocity vector with X (forward/backward), Y (left/right lateral), and Z (up/down) components in meters per second.
/// </summary>
public readonly record struct Velocity
{
    /// <summary>
    /// Gets the forward/backward velocity component in meters per second.
    /// </summary>
    public float X { get; init; }

    /// <summary>
    /// Gets the lateral (left/right) velocity component in meters per second.
    /// </summary>
    public float Y { get; init; }

    /// <summary>
    /// Gets the vertical (up/down) velocity component in meters per second.
    /// </summary>
    public float Z { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Velocity"/> struct.
    /// </summary>
    /// <param name="x">The forward/backward velocity component in meters per second. Cannot be NaN or Infinity.</param>
    /// <param name="y">The lateral (left/right) velocity component in meters per second. Cannot be NaN or Infinity.</param>
    /// <param name="z">The vertical (up/down) velocity component in meters per second. Cannot be NaN or Infinity.</param>
    /// <exception cref="ArgumentException">Thrown when any component is NaN or Infinity.</exception>
    public Velocity(float x, float y, float z)
    {
        ValidateComponent(x, nameof(x));
        ValidateComponent(y, nameof(y));
        ValidateComponent(z, nameof(z));

        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Calculates the magnitude (speed) of the velocity vector.
    /// </summary>
    /// <returns>The magnitude of the velocity vector in meters per second.</returns>
    public float Magnitude()
    {
        return MathF.Sqrt((X * X) + (Y * Y) + (Z * Z));
    }

    /// <summary>
    /// Gets the lateral (sideways) velocity component.
    /// </summary>
    /// <returns>The lateral velocity (Y component) in meters per second.</returns>
    public float Lateral()
    {
        return Y;
    }

    /// <summary>
    /// Gets the forward velocity component.
    /// </summary>
    /// <returns>The forward velocity (X component) in meters per second.</returns>
    public float Forward()
    {
        return X;
    }

    private static void ValidateComponent(float value, string paramName)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            throw new ArgumentException($"{paramName} cannot be NaN or Infinity.", paramName);
        }
    }
}
