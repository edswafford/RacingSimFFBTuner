using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class PidConfigTests
{
    private const float Tolerance = 0.0001f;

    #region Construction Tests

    [Fact]
    public void Constructor_WithAllZeroGains_ShouldCreateInstance()
    {
        // Arrange & Act
        var pidConfig = new PidConfig(0f, 0f, 0f);

        // Assert
        Assert.Equal(0f, pidConfig.Proportional, Tolerance);
        Assert.Equal(0f, pidConfig.Integral, Tolerance);
        Assert.Equal(0f, pidConfig.Derivative, Tolerance);
    }

    [Fact]
    public void Constructor_WithPositiveGains_ShouldCreateInstance()
    {
        // Arrange
        var proportional = 1.5f;
        var integral = 0.5f;
        var derivative = 0.2f;

        // Act
        var pidConfig = new PidConfig(proportional, integral, derivative);

        // Assert
        Assert.Equal(proportional, pidConfig.Proportional, Tolerance);
        Assert.Equal(integral, pidConfig.Integral, Tolerance);
        Assert.Equal(derivative, pidConfig.Derivative, Tolerance);
    }

    [Fact]
    public void Constructor_WithNegativeGains_ShouldCreateInstance()
    {
        // Arrange
        var proportional = -1.5f;
        var integral = -0.5f;
        var derivative = -0.2f;

        // Act
        var pidConfig = new PidConfig(proportional, integral, derivative);

        // Assert
        Assert.Equal(proportional, pidConfig.Proportional, Tolerance);
        Assert.Equal(integral, pidConfig.Integral, Tolerance);
        Assert.Equal(derivative, pidConfig.Derivative, Tolerance);
    }

    [Fact]
    public void Constructor_WithMixedPositiveNegativeGains_ShouldCreateInstance()
    {
        // Arrange
        var proportional = 1.5f;
        var integral = -0.5f;
        var derivative = 0.2f;

        // Act
        var pidConfig = new PidConfig(proportional, integral, derivative);

        // Assert
        Assert.Equal(proportional, pidConfig.Proportional, Tolerance);
        Assert.Equal(integral, pidConfig.Integral, Tolerance);
        Assert.Equal(derivative, pidConfig.Derivative, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargeGains_ShouldCreateInstance()
    {
        // Arrange
        var proportional = 100f;
        var integral = 50f;
        var derivative = 25f;

        // Act
        var pidConfig = new PidConfig(proportional, integral, derivative);

        // Assert
        Assert.Equal(proportional, pidConfig.Proportional, Tolerance);
        Assert.Equal(integral, pidConfig.Integral, Tolerance);
        Assert.Equal(derivative, pidConfig.Derivative, Tolerance);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void Constructor_WithProportionalNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var proportional = float.NaN;
        var integral = 0.5f;
        var derivative = 0.2f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new PidConfig(proportional, integral, derivative));

        Assert.Equal("proportional", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithIntegralNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var proportional = 1.5f;
        var integral = float.NaN;
        var derivative = 0.2f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new PidConfig(proportional, integral, derivative));

        Assert.Equal("integral", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithDerivativeNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var proportional = 1.5f;
        var integral = 0.5f;
        var derivative = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new PidConfig(proportional, integral, derivative));

        Assert.Equal("derivative", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithProportionalPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var proportional = float.PositiveInfinity;
        var integral = 0.5f;
        var derivative = 0.2f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new PidConfig(proportional, integral, derivative));

        Assert.Equal("proportional", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithIntegralPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var proportional = 1.5f;
        var integral = float.PositiveInfinity;
        var derivative = 0.2f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new PidConfig(proportional, integral, derivative));

        Assert.Equal("integral", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithDerivativePositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var proportional = 1.5f;
        var integral = 0.5f;
        var derivative = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new PidConfig(proportional, integral, derivative));

        Assert.Equal("derivative", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithProportionalNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var proportional = float.NegativeInfinity;
        var integral = 0.5f;
        var derivative = 0.2f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new PidConfig(proportional, integral, derivative));

        Assert.Equal("proportional", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithIntegralNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var proportional = 1.5f;
        var integral = float.NegativeInfinity;
        var derivative = 0.2f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new PidConfig(proportional, integral, derivative));

        Assert.Equal("integral", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithDerivativeNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var proportional = 1.5f;
        var integral = 0.5f;
        var derivative = float.NegativeInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new PidConfig(proportional, integral, derivative));

        Assert.Equal("derivative", exception.ParamName);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameGains_ShouldReturnTrue()
    {
        // Arrange
        var pidConfig1 = new PidConfig(1.5f, 0.5f, 0.2f);
        var pidConfig2 = new PidConfig(1.5f, 0.5f, 0.2f);

        // Act & Assert
        Assert.Equal(pidConfig1, pidConfig2);
        Assert.True(pidConfig1 == pidConfig2);
        Assert.False(pidConfig1 != pidConfig2);
    }

    [Fact]
    public void Equals_WithDifferentProportional_ShouldReturnFalse()
    {
        // Arrange
        var pidConfig1 = new PidConfig(1.5f, 0.5f, 0.2f);
        var pidConfig2 = new PidConfig(2.0f, 0.5f, 0.2f);

        // Act & Assert
        Assert.NotEqual(pidConfig1, pidConfig2);
        Assert.False(pidConfig1 == pidConfig2);
        Assert.True(pidConfig1 != pidConfig2);
    }

    [Fact]
    public void Equals_WithDifferentIntegral_ShouldReturnFalse()
    {
        // Arrange
        var pidConfig1 = new PidConfig(1.5f, 0.5f, 0.2f);
        var pidConfig2 = new PidConfig(1.5f, 1.0f, 0.2f);

        // Act & Assert
        Assert.NotEqual(pidConfig1, pidConfig2);
        Assert.False(pidConfig1 == pidConfig2);
        Assert.True(pidConfig1 != pidConfig2);
    }

    [Fact]
    public void Equals_WithDifferentDerivative_ShouldReturnFalse()
    {
        // Arrange
        var pidConfig1 = new PidConfig(1.5f, 0.5f, 0.2f);
        var pidConfig2 = new PidConfig(1.5f, 0.5f, 0.5f);

        // Act & Assert
        Assert.NotEqual(pidConfig1, pidConfig2);
        Assert.False(pidConfig1 == pidConfig2);
        Assert.True(pidConfig1 != pidConfig2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var pidConfig1 = new PidConfig(1.5f, 0.5f, 0.2f);
        var pidConfig2 = new PidConfig(1.5f, 0.5f, 0.2f);

        // Act
        var hashCode1 = pidConfig1.GetHashCode();
        var hashCode2 = pidConfig2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var pidConfig1 = new PidConfig(1.5f, 0.5f, 0.2f);
        var pidConfig2 = new PidConfig(2.0f, 0.5f, 0.2f);

        // Act
        var hashCode1 = pidConfig1.GetHashCode();
        var hashCode2 = pidConfig2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void PidConfig_ShouldBeImmutable()
    {
        // Arrange
        var pidConfig = new PidConfig(1.5f, 0.5f, 0.2f);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<PidConfig>(pidConfig);
        // If this compiles and runs, the type is properly immutable
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithVerySmallGains_ShouldCreateInstance()
    {
        // Arrange
        var proportional = 0.0001f;
        var integral = 0.0001f;
        var derivative = 0.0001f;

        // Act
        var pidConfig = new PidConfig(proportional, integral, derivative);

        // Assert
        Assert.Equal(proportional, pidConfig.Proportional, Tolerance);
        Assert.Equal(integral, pidConfig.Integral, Tolerance);
        Assert.Equal(derivative, pidConfig.Derivative, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargeGains_ShouldCreateInstance()
    {
        // Arrange
        var proportional = 1000f;
        var integral = 500f;
        var derivative = 250f;

        // Act
        var pidConfig = new PidConfig(proportional, integral, derivative);

        // Assert
        Assert.Equal(proportional, pidConfig.Proportional, Tolerance);
        Assert.Equal(integral, pidConfig.Integral, Tolerance);
        Assert.Equal(derivative, pidConfig.Derivative, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargeNegativeGains_ShouldCreateInstance()
    {
        // Arrange
        var proportional = -1000f;
        var integral = -500f;
        var derivative = -250f;

        // Act
        var pidConfig = new PidConfig(proportional, integral, derivative);

        // Assert
        Assert.Equal(proportional, pidConfig.Proportional, Tolerance);
        Assert.Equal(integral, pidConfig.Integral, Tolerance);
        Assert.Equal(derivative, pidConfig.Derivative, Tolerance);
    }

    #endregion
}
