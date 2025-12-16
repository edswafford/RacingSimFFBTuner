using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class YawRateFactorTests
{
    private const float Tolerance = 0.0001f;
    private const float MinYawRateRadiansPerSecond = 5f * MathF.PI / 180f; // 5 degrees/second

    #region Construction Tests (via FromComponents)

    [Fact]
    public void FromComponents_WithValidInputs_ShouldCreateInstance()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed = new Speed(27.78f); // ~100 km/h
        var yawRate = new YawRate(0.2f); // > 5 deg/s

        // Act
        var factor = YawRateFactor.FromComponents(angle, speed, yawRate);

        // Assert
        var expected = angle.Radians * speed.MetersPerSecond / yawRate.RadiansPerSecond;
        Assert.Equal(expected, factor.Value, Tolerance);
    }

    [Fact]
    public void FromComponents_WithPositiveValues_ShouldCreatePositiveFactor()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.3f, MathF.PI);
        var speed = new Speed(20f);
        var yawRate = new YawRate(0.15f);

        // Act
        var factor = YawRateFactor.FromComponents(angle, speed, yawRate);

        // Assert
        Assert.True(factor.Value > 0f);
    }

    [Fact]
    public void FromComponents_WithNegativeAngle_ShouldCreateNegativeFactor()
    {
        // Arrange
        var angle = new SteeringWheelAngle(-0.3f, MathF.PI);
        var speed = new Speed(20f);
        var yawRate = new YawRate(0.15f);

        // Act
        var factor = YawRateFactor.FromComponents(angle, speed, yawRate);

        // Assert
        Assert.True(factor.Value < 0f);
    }

    [Fact]
    public void FromComponents_WithMinimumYawRate_ShouldCreateInstance()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.1f, MathF.PI);
        var speed = new Speed(10f);
        var yawRate = new YawRate(MinYawRateRadiansPerSecond);

        // Act
        var factor = YawRateFactor.FromComponents(angle, speed, yawRate);

        // Assert
        var expected = angle.Radians * speed.MetersPerSecond / yawRate.RadiansPerSecond;
        Assert.Equal(expected, factor.Value, Tolerance);
    }

    [Fact]
    public void FromComponents_WithLargeValues_ShouldCreateInstance()
    {
        // Arrange
        var angle = new SteeringWheelAngle(1.0f, MathF.PI);
        var speed = new Speed(50f);
        var yawRate = new YawRate(1.0f);

        // Act
        var factor = YawRateFactor.FromComponents(angle, speed, yawRate);

        // Assert
        var expected = angle.Radians * speed.MetersPerSecond / yawRate.RadiansPerSecond;
        Assert.Equal(expected, factor.Value, Tolerance);
    }

    #endregion

    #region Validation Tests (FromComponents)

    [Fact]
    public void FromComponents_WithYawRateBelowMinimum_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed = new Speed(27.78f);
        var yawRate = new YawRate(MinYawRateRadiansPerSecond - 0.01f); // Just below minimum

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            YawRateFactor.FromComponents(angle, speed, yawRate));

        Assert.Contains("Yaw rate must be at least 5 degrees/second", exception.Message);
    }

    [Fact]
    public void FromComponents_WithZeroSpeed_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed = new Speed(0f);
        var yawRate = new YawRate(0.2f);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            YawRateFactor.FromComponents(angle, speed, yawRate));

        Assert.Contains("Speed must be greater than zero", exception.Message);
    }

    [Fact]
    public void FromComponents_WithZeroYawRate_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed = new Speed(27.78f);
        var yawRate = new YawRate(0f);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            YawRateFactor.FromComponents(angle, speed, yawRate));

        Assert.Contains("Yaw rate cannot be zero", exception.Message);
    }

    [Fact]
    public void FromComponents_WithVerySmallYawRate_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed = new Speed(27.78f);
        var yawRate = new YawRate(float.Epsilon);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            YawRateFactor.FromComponents(angle, speed, yawRate));

        // float.Epsilon is very small but non-zero, so it should fail the minimum check
        Assert.Contains("Yaw rate must be at least 5 degrees/second", exception.Message);
    }

    [Fact]
    public void FromComponents_WithNegativeYawRateAboveMinimum_ShouldCreateInstance()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.3f, MathF.PI);
        var speed = new Speed(20f);
        var yawRate = new YawRate(-0.15f); // Negative but above minimum in absolute value

        // Act
        var factor = YawRateFactor.FromComponents(angle, speed, yawRate);

        // Assert
        var expected = angle.Radians * speed.MetersPerSecond / yawRate.RadiansPerSecond;
        Assert.Equal(expected, factor.Value, Tolerance);
    }

    [Fact]
    public void FromComponents_WithNegativeYawRateBelowMinimum_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed = new Speed(27.78f);
        var yawRate = new YawRate(-(MinYawRateRadiansPerSecond - 0.01f)); // Just below minimum in absolute value

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            YawRateFactor.FromComponents(angle, speed, yawRate));

        Assert.Contains("Yaw rate must be at least 5 degrees/second", exception.Message);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var angle1 = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed1 = new Speed(27.78f);
        var yawRate1 = new YawRate(0.2f);

        var angle2 = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed2 = new Speed(27.78f);
        var yawRate2 = new YawRate(0.2f);

        // Act
        var factor1 = YawRateFactor.FromComponents(angle1, speed1, yawRate1);
        var factor2 = YawRateFactor.FromComponents(angle2, speed2, yawRate2);

        // Assert
        Assert.Equal(factor1, factor2);
        Assert.True(factor1 == factor2);
        Assert.False(factor1 != factor2);
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var angle1 = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed1 = new Speed(27.78f);
        var yawRate1 = new YawRate(0.2f);

        var angle2 = new SteeringWheelAngle(0.6f, MathF.PI);
        var speed2 = new Speed(27.78f);
        var yawRate2 = new YawRate(0.2f);

        // Act
        var factor1 = YawRateFactor.FromComponents(angle1, speed1, yawRate1);
        var factor2 = YawRateFactor.FromComponents(angle2, speed2, yawRate2);

        // Assert
        Assert.NotEqual(factor1, factor2);
        Assert.False(factor1 == factor2);
        Assert.True(factor1 != factor2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var angle1 = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed1 = new Speed(27.78f);
        var yawRate1 = new YawRate(0.2f);

        var angle2 = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed2 = new Speed(27.78f);
        var yawRate2 = new YawRate(0.2f);

        // Act
        var factor1 = YawRateFactor.FromComponents(angle1, speed1, yawRate1);
        var factor2 = YawRateFactor.FromComponents(angle2, speed2, yawRate2);

        // Assert
        Assert.Equal(factor1.GetHashCode(), factor2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var angle1 = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed1 = new Speed(27.78f);
        var yawRate1 = new YawRate(0.2f);

        var angle2 = new SteeringWheelAngle(0.6f, MathF.PI);
        var speed2 = new Speed(27.78f);
        var yawRate2 = new YawRate(0.2f);

        // Act
        var factor1 = YawRateFactor.FromComponents(angle1, speed1, yawRate1);
        var factor2 = YawRateFactor.FromComponents(angle2, speed2, yawRate2);

        // Assert
        Assert.NotEqual(factor1.GetHashCode(), factor2.GetHashCode());
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void FromComponents_WithVerySmallSpeed_ShouldCreateInstance()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.1f, MathF.PI);
        var speed = new Speed(0.1f); // Very small but > 0
        var yawRate = new YawRate(0.2f);

        // Act
        var factor = YawRateFactor.FromComponents(angle, speed, yawRate);

        // Assert
        var expected = angle.Radians * speed.MetersPerSecond / yawRate.RadiansPerSecond;
        Assert.Equal(expected, factor.Value, Tolerance);
    }

    [Fact]
    public void FromComponents_WithVeryLargeSpeed_ShouldCreateInstance()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.5f, MathF.PI);
        var speed = new Speed(100f); // Very large speed
        var yawRate = new YawRate(0.2f);

        // Act
        var factor = YawRateFactor.FromComponents(angle, speed, yawRate);

        // Assert
        var expected = angle.Radians * speed.MetersPerSecond / yawRate.RadiansPerSecond;
        Assert.Equal(expected, factor.Value, Tolerance);
    }

    [Fact]
    public void FromComponents_WithZeroAngle_ShouldCreateZeroFactor()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0f, MathF.PI);
        var speed = new Speed(27.78f);
        var yawRate = new YawRate(0.2f);

        // Act
        var factor = YawRateFactor.FromComponents(angle, speed, yawRate);

        // Assert
        Assert.Equal(0f, factor.Value, Tolerance);
    }

    [Fact]
    public void FromComponents_WithExactMinimumYawRate_ShouldCreateInstance()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.1f, MathF.PI);
        var speed = new Speed(10f);
        var yawRate = new YawRate(MinYawRateRadiansPerSecond);

        // Act
        var factor = YawRateFactor.FromComponents(angle, speed, yawRate);

        // Assert
        var expected = angle.Radians * speed.MetersPerSecond / yawRate.RadiansPerSecond;
        Assert.Equal(expected, factor.Value, Tolerance);
    }

    #endregion

    #region Calculation Accuracy Tests

    [Fact]
    public void FromComponents_ShouldCalculateCorrectFactor()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.5f, MathF.PI); // 0.5 radians
        var speed = new Speed(20f); // 20 m/s
        var yawRate = new YawRate(0.1f); // 0.1 rad/s

        // Act
        var factor = YawRateFactor.FromComponents(angle, speed, yawRate);

        // Assert
        // Expected: 0.5 * 20 / 0.1 = 100
        Assert.Equal(100f, factor.Value, Tolerance);
    }

    [Fact]
    public void FromComponents_WithNegativeYawRate_ShouldCalculateCorrectFactor()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0.3f, MathF.PI);
        var speed = new Speed(15f);
        var yawRate = new YawRate(-0.15f);

        // Act
        var factor = YawRateFactor.FromComponents(angle, speed, yawRate);

        // Assert
        // Expected: 0.3 * 15 / (-0.15) = -30
        Assert.Equal(-30f, factor.Value, Tolerance);
    }

    #endregion
}
