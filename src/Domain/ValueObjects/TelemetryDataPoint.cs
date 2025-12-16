namespace RacingSimFFB.Domain.ValueObjects;

/// <summary>
/// A composite value object representing an immutable snapshot of all telemetry data at a specific moment.
/// This aggregates multiple value objects into a single cohesive unit.
/// </summary>
public readonly record struct TelemetryDataPoint
{
    /// <summary>
    /// Gets the steering wheel angle.
    /// </summary>
    public SteeringWheelAngle SteeringWheelAngle { get; init; }

    /// <summary>
    /// Gets the yaw rate.
    /// </summary>
    public YawRate YawRate { get; init; }

    /// <summary>
    /// Gets the velocity vector.
    /// </summary>
    public Velocity Velocity { get; init; }

    /// <summary>
    /// Gets the speed.
    /// </summary>
    public Speed Speed { get; init; }

    /// <summary>
    /// Gets the G-force.
    /// </summary>
    public GForce GForce { get; init; }

    /// <summary>
    /// Gets the engine RPM.
    /// </summary>
    public EngineRPM EngineRPM { get; init; }

    /// <summary>
    /// Gets the steering wheel torque samples (6 samples at 360Hz).
    /// </summary>
    public Torque[] SteeringWheelTorqueSamples { get; init; }

    /// <summary>
    /// Gets the shock velocities (6 corners).
    /// </summary>
    public ShockVelocity[] ShockVelocities { get; init; }

    /// <summary>
    /// Gets the timestamp for ordering/sequencing.
    /// </summary>
    public DateTime Timestamp { get; init; }

    /// <summary>
    /// Gets a value indicating whether the vehicle is on track.
    /// </summary>
    public bool IsOnTrack { get; init; }

    /// <summary>
    /// Gets the current gear.
    /// </summary>
    public int Gear { get; init; }

    /// <summary>
    /// Gets the throttle input (0.0 to 1.0).
    /// </summary>
    public float Throttle { get; init; }

    /// <summary>
    /// Gets the brake input (0.0 to 1.0).
    /// </summary>
    public float Brake { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TelemetryDataPoint"/> struct.
    /// </summary>
    /// <param name="steeringWheelAngle">The steering wheel angle.</param>
    /// <param name="yawRate">The yaw rate.</param>
    /// <param name="velocity">The velocity vector.</param>
    /// <param name="speed">The speed.</param>
    /// <param name="gForce">The G-force.</param>
    /// <param name="engineRPM">The engine RPM.</param>
    /// <param name="steeringWheelTorqueSamples">The steering wheel torque samples. Must contain exactly 6 samples.</param>
    /// <param name="shockVelocities">The shock velocities. Must contain exactly 6 samples (one per corner).</param>
    /// <param name="timestamp">The timestamp. Must be a valid DateTime value.</param>
    /// <param name="isOnTrack">Whether the vehicle is on track. Defaults to true.</param>
    /// <param name="gear">The current gear. Defaults to 0.</param>
    /// <param name="throttle">The throttle input (0.0 to 1.0). Defaults to 0.0.</param>
    /// <param name="brake">The brake input (0.0 to 1.0). Defaults to 0.0.</param>
    /// <exception cref="ArgumentNullException">Thrown when steeringWheelTorqueSamples or shockVelocities is null.</exception>
    /// <exception cref="ArgumentException">Thrown when arrays have incorrect length or timestamp is invalid.</exception>
    public TelemetryDataPoint(
        SteeringWheelAngle steeringWheelAngle,
        YawRate yawRate,
        Velocity velocity,
        Speed speed,
        GForce gForce,
        EngineRPM engineRPM,
        Torque[] steeringWheelTorqueSamples,
        ShockVelocity[] shockVelocities,
        DateTime timestamp,
        bool isOnTrack = true,
        int gear = 0,
        float throttle = 0f,
        float brake = 0f)
    {
        ValidateRequiredComponents(
            steeringWheelTorqueSamples,
            shockVelocities,
            timestamp);

        SteeringWheelAngle = steeringWheelAngle;
        YawRate = yawRate;
        Velocity = velocity;
        Speed = speed;
        GForce = gForce;
        EngineRPM = engineRPM;
        SteeringWheelTorqueSamples = steeringWheelTorqueSamples;
        ShockVelocities = shockVelocities;
        Timestamp = timestamp;
        IsOnTrack = isOnTrack;
        Gear = gear;
        Throttle = throttle;
        Brake = brake;
    }

    private static void ValidateRequiredComponents(
        Torque[] steeringWheelTorqueSamples,
        ShockVelocity[] shockVelocities,
        DateTime timestamp)
    {
        if (steeringWheelTorqueSamples == null)
        {
            throw new ArgumentNullException(nameof(steeringWheelTorqueSamples));
        }

        if (steeringWheelTorqueSamples.Length != 6)
        {
            throw new ArgumentException(
                "SteeringWheelTorqueSamples must contain exactly 6 samples.",
                nameof(steeringWheelTorqueSamples));
        }

        if (shockVelocities == null)
        {
            throw new ArgumentNullException(nameof(shockVelocities));
        }

        if (shockVelocities.Length != 6)
        {
            throw new ArgumentException(
                "ShockVelocities must contain exactly 6 samples (one per corner).",
                nameof(shockVelocities));
        }

        if (timestamp == default)
        {
            throw new ArgumentException(
                "Timestamp must be a valid DateTime value.",
                nameof(timestamp));
        }
    }
}
