using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class TelemetryDataPointTests
{
    #region Construction Tests

    [Fact]
    public void Constructor_WithAllRequiredValues_ShouldCreateInstance()
    {
        // Arrange
        var steeringWheelAngle = new SteeringWheelAngle(0.1f, MathF.PI);
        var yawRate = new YawRate(0.2f);
        var velocity = new Velocity(20f, 1f, 0f);
        var speed = new Speed(20.025f);
        var gForce = new GForce(1.5f);
        var engineRPM = new EngineRPM(5000f);
        var torqueSamples = new Torque[]
        {
            new Torque(1.0f),
            new Torque(1.1f),
            new Torque(1.2f),
            new Torque(1.3f),
            new Torque(1.4f),
            new Torque(1.5f),
        };
        var shockVelocities = new ShockVelocity[]
        {
            new ShockVelocity(0.1f),
            new ShockVelocity(0.2f),
            new ShockVelocity(0.3f),
            new ShockVelocity(0.4f),
            new ShockVelocity(0.5f),
            new ShockVelocity(0.6f),
        };
        var timestamp = DateTime.UtcNow;

        // Act
        var dataPoint = new TelemetryDataPoint(
            steeringWheelAngle,
            yawRate,
            velocity,
            speed,
            gForce,
            engineRPM,
            torqueSamples,
            shockVelocities,
            timestamp);

        // Assert
        Assert.Equal(steeringWheelAngle, dataPoint.SteeringWheelAngle);
        Assert.Equal(yawRate, dataPoint.YawRate);
        Assert.Equal(velocity, dataPoint.Velocity);
        Assert.Equal(speed, dataPoint.Speed);
        Assert.Equal(gForce, dataPoint.GForce);
        Assert.Equal(engineRPM, dataPoint.EngineRPM);
        Assert.Equal(torqueSamples, dataPoint.SteeringWheelTorqueSamples);
        Assert.Equal(shockVelocities, dataPoint.ShockVelocities);
        Assert.Equal(timestamp, dataPoint.Timestamp);
        Assert.True(dataPoint.IsOnTrack); // Default value
        Assert.Equal(0, dataPoint.Gear); // Default value
        Assert.Equal(0f, dataPoint.Throttle); // Default value
        Assert.Equal(0f, dataPoint.Brake); // Default value
    }

    [Fact]
    public void Constructor_WithOptionalValues_ShouldCreateInstance()
    {
        // Arrange
        var timestamp = DateTime.UtcNow;
        var isOnTrack = false;
        var gear = 3;
        var throttle = 0.8f;
        var brake = 0.2f;

        // Act
        var dataPoint = CreateValidTelemetryDataPoint(
            timestamp,
            isOnTrack,
            gear,
            throttle,
            brake);

        // Assert
        Assert.Equal(timestamp, dataPoint.Timestamp);
        Assert.Equal(isOnTrack, dataPoint.IsOnTrack);
        Assert.Equal(gear, dataPoint.Gear);
        Assert.Equal(throttle, dataPoint.Throttle);
        Assert.Equal(brake, dataPoint.Brake);
    }

    [Fact]
    public void Constructor_WithMinimalRequiredValues_ShouldCreateInstance()
    {
        // Arrange & Act
        var dataPoint = CreateValidTelemetryDataPoint();

        // Assert
        Assert.NotNull(dataPoint.SteeringWheelTorqueSamples);
        Assert.Equal(6, dataPoint.SteeringWheelTorqueSamples.Length);
        Assert.NotNull(dataPoint.ShockVelocities);
        Assert.Equal(6, dataPoint.ShockVelocities.Length);
        Assert.NotEqual(default(DateTime), dataPoint.Timestamp);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void Constructor_WithNullTorqueSamples_ShouldThrowArgumentNullException()
    {
        // Arrange
        var steeringWheelAngle = new SteeringWheelAngle(0.1f, MathF.PI);
        var yawRate = new YawRate(0.2f);
        var velocity = new Velocity(20f, 1f, 0f);
        var speed = new Speed(20.025f);
        var gForce = new GForce(1.5f);
        var engineRPM = new EngineRPM(5000f);
        Torque[]? torqueSamples = null;
        var shockVelocities = new ShockVelocity[]
        {
            new ShockVelocity(0.1f),
            new ShockVelocity(0.2f),
            new ShockVelocity(0.3f),
            new ShockVelocity(0.4f),
            new ShockVelocity(0.5f),
            new ShockVelocity(0.6f),
        };
        var timestamp = DateTime.UtcNow;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new TelemetryDataPoint(
                steeringWheelAngle,
                yawRate,
                velocity,
                speed,
                gForce,
                engineRPM,
                torqueSamples!,
                shockVelocities,
                timestamp));
    }

    [Fact]
    public void Constructor_WithNullShockVelocities_ShouldThrowArgumentNullException()
    {
        // Arrange
        var steeringWheelAngle = new SteeringWheelAngle(0.1f, MathF.PI);
        var yawRate = new YawRate(0.2f);
        var velocity = new Velocity(20f, 1f, 0f);
        var speed = new Speed(20.025f);
        var gForce = new GForce(1.5f);
        var engineRPM = new EngineRPM(5000f);
        var torqueSamples = new Torque[]
        {
            new Torque(1.0f),
            new Torque(1.1f),
            new Torque(1.2f),
            new Torque(1.3f),
            new Torque(1.4f),
            new Torque(1.5f),
        };
        ShockVelocity[]? shockVelocities = null;
        var timestamp = DateTime.UtcNow;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new TelemetryDataPoint(
                steeringWheelAngle,
                yawRate,
                velocity,
                speed,
                gForce,
                engineRPM,
                torqueSamples,
                shockVelocities!,
                timestamp));
    }

    [Fact]
    public void Constructor_WithTorqueSamplesWrongLength_ShouldThrowArgumentException()
    {
        // Arrange
        var steeringWheelAngle = new SteeringWheelAngle(0.1f, MathF.PI);
        var yawRate = new YawRate(0.2f);
        var velocity = new Velocity(20f, 1f, 0f);
        var speed = new Speed(20.025f);
        var gForce = new GForce(1.5f);
        var engineRPM = new EngineRPM(5000f);
        var torqueSamples = new Torque[] { new Torque(1.0f), new Torque(1.1f) }; // Wrong length
        var shockVelocities = new ShockVelocity[]
        {
            new ShockVelocity(0.1f),
            new ShockVelocity(0.2f),
            new ShockVelocity(0.3f),
            new ShockVelocity(0.4f),
            new ShockVelocity(0.5f),
            new ShockVelocity(0.6f),
        };
        var timestamp = DateTime.UtcNow;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TelemetryDataPoint(
                steeringWheelAngle,
                yawRate,
                velocity,
                speed,
                gForce,
                engineRPM,
                torqueSamples,
                shockVelocities,
                timestamp));

        Assert.Contains("SteeringWheelTorqueSamples must contain exactly 6 samples", exception.Message);
        Assert.Equal("steeringWheelTorqueSamples", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithShockVelocitiesWrongLength_ShouldThrowArgumentException()
    {
        // Arrange
        var steeringWheelAngle = new SteeringWheelAngle(0.1f, MathF.PI);
        var yawRate = new YawRate(0.2f);
        var velocity = new Velocity(20f, 1f, 0f);
        var speed = new Speed(20.025f);
        var gForce = new GForce(1.5f);
        var engineRPM = new EngineRPM(5000f);
        var torqueSamples = new Torque[]
        {
            new Torque(1.0f),
            new Torque(1.1f),
            new Torque(1.2f),
            new Torque(1.3f),
            new Torque(1.4f),
            new Torque(1.5f),
        };
        var shockVelocities = new ShockVelocity[] { new ShockVelocity(0.1f) }; // Wrong length
        var timestamp = DateTime.UtcNow;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TelemetryDataPoint(
                steeringWheelAngle,
                yawRate,
                velocity,
                speed,
                gForce,
                engineRPM,
                torqueSamples,
                shockVelocities,
                timestamp));

        Assert.Contains("ShockVelocities must contain exactly 6 samples", exception.Message);
        Assert.Equal("shockVelocities", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithDefaultTimestamp_ShouldThrowArgumentException()
    {
        // Arrange
        var steeringWheelAngle = new SteeringWheelAngle(0.1f, MathF.PI);
        var yawRate = new YawRate(0.2f);
        var velocity = new Velocity(20f, 1f, 0f);
        var speed = new Speed(20.025f);
        var gForce = new GForce(1.5f);
        var engineRPM = new EngineRPM(5000f);
        var torqueSamples = new Torque[]
        {
            new Torque(1.0f),
            new Torque(1.1f),
            new Torque(1.2f),
            new Torque(1.3f),
            new Torque(1.4f),
            new Torque(1.5f),
        };
        var shockVelocities = new ShockVelocity[]
        {
            new ShockVelocity(0.1f),
            new ShockVelocity(0.2f),
            new ShockVelocity(0.3f),
            new ShockVelocity(0.4f),
            new ShockVelocity(0.5f),
            new ShockVelocity(0.6f),
        };
        var timestamp = default(DateTime);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TelemetryDataPoint(
                steeringWheelAngle,
                yawRate,
                velocity,
                speed,
                gForce,
                engineRPM,
                torqueSamples,
                shockVelocities,
                timestamp));

        Assert.Contains("Timestamp must be a valid DateTime value", exception.Message);
        Assert.Equal("timestamp", exception.ParamName);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var timestamp = DateTime.UtcNow;
        var steeringWheelAngle = new SteeringWheelAngle(0.1f, MathF.PI);
        var yawRate = new YawRate(0.2f);
        var velocity = new Velocity(20f, 1f, 0f);
        var speed = new Speed(20.025f);
        var gForce = new GForce(1.5f);
        var engineRPM = new EngineRPM(5000f);
        var torqueSamples = new Torque[]
        {
            new Torque(1.0f),
            new Torque(1.1f),
            new Torque(1.2f),
            new Torque(1.3f),
            new Torque(1.4f),
            new Torque(1.5f),
        };
        var shockVelocities = new ShockVelocity[]
        {
            new ShockVelocity(0.1f),
            new ShockVelocity(0.2f),
            new ShockVelocity(0.3f),
            new ShockVelocity(0.4f),
            new ShockVelocity(0.5f),
            new ShockVelocity(0.6f),
        };
        var dataPoint1 = new TelemetryDataPoint(
            steeringWheelAngle,
            yawRate,
            velocity,
            speed,
            gForce,
            engineRPM,
            torqueSamples,
            shockVelocities,
            timestamp);
        var dataPoint2 = new TelemetryDataPoint(
            steeringWheelAngle,
            yawRate,
            velocity,
            speed,
            gForce,
            engineRPM,
            torqueSamples,
            shockVelocities,
            timestamp);

        // Act & Assert
        Assert.Equal(dataPoint1, dataPoint2);
        Assert.True(dataPoint1 == dataPoint2);
        Assert.False(dataPoint1 != dataPoint2);
    }

    [Fact]
    public void Equals_WithDifferentTimestamps_ShouldReturnFalse()
    {
        // Arrange
        var timestamp1 = DateTime.UtcNow;
        var timestamp2 = timestamp1.AddMilliseconds(1);
        var dataPoint1 = CreateValidTelemetryDataPoint(timestamp1);
        var dataPoint2 = CreateValidTelemetryDataPoint(timestamp2);

        // Act & Assert
        Assert.NotEqual(dataPoint1, dataPoint2);
        Assert.False(dataPoint1 == dataPoint2);
        Assert.True(dataPoint1 != dataPoint2);
    }

    [Fact]
    public void Equals_WithDifferentSteeringWheelAngle_ShouldReturnFalse()
    {
        // Arrange
        var timestamp = DateTime.UtcNow;
        var dataPoint1 = CreateValidTelemetryDataPoint(timestamp);

        var steeringWheelAngle2 = new SteeringWheelAngle(0.2f, MathF.PI); // Different angle
        var yawRate = new YawRate(0.2f);
        var velocity = new Velocity(20f, 1f, 0f);
        var speed = new Speed(20.025f);
        var gForce = new GForce(1.5f);
        var engineRPM = new EngineRPM(5000f);
        var torqueSamples = new Torque[]
        {
            new Torque(1.0f),
            new Torque(1.1f),
            new Torque(1.2f),
            new Torque(1.3f),
            new Torque(1.4f),
            new Torque(1.5f),
        };
        var shockVelocities = new ShockVelocity[]
        {
            new ShockVelocity(0.1f),
            new ShockVelocity(0.2f),
            new ShockVelocity(0.3f),
            new ShockVelocity(0.4f),
            new ShockVelocity(0.5f),
            new ShockVelocity(0.6f),
        };
        var dataPoint2 = new TelemetryDataPoint(
            steeringWheelAngle2,
            yawRate,
            velocity,
            speed,
            gForce,
            engineRPM,
            torqueSamples,
            shockVelocities,
            timestamp);

        // Act & Assert
        Assert.NotEqual(dataPoint1, dataPoint2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var timestamp = DateTime.UtcNow;
        var steeringWheelAngle = new SteeringWheelAngle(0.1f, MathF.PI);
        var yawRate = new YawRate(0.2f);
        var velocity = new Velocity(20f, 1f, 0f);
        var speed = new Speed(20.025f);
        var gForce = new GForce(1.5f);
        var engineRPM = new EngineRPM(5000f);
        var torqueSamples = new Torque[]
        {
            new Torque(1.0f),
            new Torque(1.1f),
            new Torque(1.2f),
            new Torque(1.3f),
            new Torque(1.4f),
            new Torque(1.5f),
        };
        var shockVelocities = new ShockVelocity[]
        {
            new ShockVelocity(0.1f),
            new ShockVelocity(0.2f),
            new ShockVelocity(0.3f),
            new ShockVelocity(0.4f),
            new ShockVelocity(0.5f),
            new ShockVelocity(0.6f),
        };
        var dataPoint1 = new TelemetryDataPoint(
            steeringWheelAngle,
            yawRate,
            velocity,
            speed,
            gForce,
            engineRPM,
            torqueSamples,
            shockVelocities,
            timestamp);
        var dataPoint2 = new TelemetryDataPoint(
            steeringWheelAngle,
            yawRate,
            velocity,
            speed,
            gForce,
            engineRPM,
            torqueSamples,
            shockVelocities,
            timestamp);

        // Act
        var hashCode1 = dataPoint1.GetHashCode();
        var hashCode2 = dataPoint2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var timestamp1 = DateTime.UtcNow;
        var timestamp2 = timestamp1.AddMilliseconds(1);
        var dataPoint1 = CreateValidTelemetryDataPoint(timestamp1);
        var dataPoint2 = CreateValidTelemetryDataPoint(timestamp2);

        // Act
        var hashCode1 = dataPoint1.GetHashCode();
        var hashCode2 = dataPoint2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Property Access Tests

    [Fact]
    public void Properties_ShouldBeAccessible()
    {
        // Arrange & Act
        var dataPoint = CreateValidTelemetryDataPoint();

        // Assert
        Assert.NotNull(dataPoint.SteeringWheelTorqueSamples);
        Assert.NotNull(dataPoint.ShockVelocities);
        Assert.NotEqual(default(DateTime), dataPoint.Timestamp);
        Assert.IsType<SteeringWheelAngle>(dataPoint.SteeringWheelAngle);
        Assert.IsType<YawRate>(dataPoint.YawRate);
        Assert.IsType<Velocity>(dataPoint.Velocity);
        Assert.IsType<Speed>(dataPoint.Speed);
        Assert.IsType<GForce>(dataPoint.GForce);
        Assert.IsType<EngineRPM>(dataPoint.EngineRPM);
    }

    [Fact]
    public void Properties_WithOptionalValues_ShouldBeAccessible()
    {
        // Arrange & Act
        var dataPoint = CreateValidTelemetryDataPoint(
            DateTime.UtcNow,
            isOnTrack: false,
            gear: 3,
            throttle: 0.8f,
            brake: 0.2f);

        // Assert
        Assert.False(dataPoint.IsOnTrack);
        Assert.Equal(3, dataPoint.Gear);
        Assert.Equal(0.8f, dataPoint.Throttle);
        Assert.Equal(0.2f, dataPoint.Brake);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithEmptyTorqueSamplesArray_ShouldThrowArgumentException()
    {
        // Arrange
        var steeringWheelAngle = new SteeringWheelAngle(0.1f, MathF.PI);
        var yawRate = new YawRate(0.2f);
        var velocity = new Velocity(20f, 1f, 0f);
        var speed = new Speed(20.025f);
        var gForce = new GForce(1.5f);
        var engineRPM = new EngineRPM(5000f);
        var torqueSamples = Array.Empty<Torque>(); // Empty array
        var shockVelocities = new ShockVelocity[]
        {
            new ShockVelocity(0.1f),
            new ShockVelocity(0.2f),
            new ShockVelocity(0.3f),
            new ShockVelocity(0.4f),
            new ShockVelocity(0.5f),
            new ShockVelocity(0.6f),
        };
        var timestamp = DateTime.UtcNow;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TelemetryDataPoint(
                steeringWheelAngle,
                yawRate,
                velocity,
                speed,
                gForce,
                engineRPM,
                torqueSamples,
                shockVelocities,
                timestamp));

        Assert.Contains("SteeringWheelTorqueSamples must contain exactly 6 samples", exception.Message);
    }

    [Fact]
    public void Constructor_WithTooManyTorqueSamples_ShouldThrowArgumentException()
    {
        // Arrange
        var steeringWheelAngle = new SteeringWheelAngle(0.1f, MathF.PI);
        var yawRate = new YawRate(0.2f);
        var velocity = new Velocity(20f, 1f, 0f);
        var speed = new Speed(20.025f);
        var gForce = new GForce(1.5f);
        var engineRPM = new EngineRPM(5000f);
        var torqueSamples = new Torque[]
        {
            new Torque(1.0f),
            new Torque(1.1f),
            new Torque(1.2f),
            new Torque(1.3f),
            new Torque(1.4f),
            new Torque(1.5f),
            new Torque(1.6f), // Too many
        };
        var shockVelocities = new ShockVelocity[]
        {
            new ShockVelocity(0.1f),
            new ShockVelocity(0.2f),
            new ShockVelocity(0.3f),
            new ShockVelocity(0.4f),
            new ShockVelocity(0.5f),
            new ShockVelocity(0.6f),
        };
        var timestamp = DateTime.UtcNow;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TelemetryDataPoint(
                steeringWheelAngle,
                yawRate,
                velocity,
                speed,
                gForce,
                engineRPM,
                torqueSamples,
                shockVelocities,
                timestamp));

        Assert.Contains("SteeringWheelTorqueSamples must contain exactly 6 samples", exception.Message);
    }

    #endregion

    #region Private Helper Methods

    private static TelemetryDataPoint CreateValidTelemetryDataPoint(
        DateTime? timestamp = null,
        bool isOnTrack = true,
        int gear = 0,
        float throttle = 0f,
        float brake = 0f)
    {
        var steeringWheelAngle = new SteeringWheelAngle(0.1f, MathF.PI);
        var yawRate = new YawRate(0.2f);
        var velocity = new Velocity(20f, 1f, 0f);
        var speed = new Speed(20.025f);
        var gForce = new GForce(1.5f);
        var engineRPM = new EngineRPM(5000f);
        var torqueSamples = new Torque[]
        {
            new Torque(1.0f),
            new Torque(1.1f),
            new Torque(1.2f),
            new Torque(1.3f),
            new Torque(1.4f),
            new Torque(1.5f),
        };
        var shockVelocities = new ShockVelocity[]
        {
            new ShockVelocity(0.1f),
            new ShockVelocity(0.2f),
            new ShockVelocity(0.3f),
            new ShockVelocity(0.4f),
            new ShockVelocity(0.5f),
            new ShockVelocity(0.6f),
        };
        var actualTimestamp = timestamp ?? DateTime.UtcNow;

        return new TelemetryDataPoint(
            steeringWheelAngle,
            yawRate,
            velocity,
            speed,
            gForce,
            engineRPM,
            torqueSamples,
            shockVelocities,
            actualTimestamp,
            isOnTrack,
            gear,
            throttle,
            brake);
    }

    #endregion
}
