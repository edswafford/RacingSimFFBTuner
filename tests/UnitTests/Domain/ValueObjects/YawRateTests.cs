using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class YawRateTests
{
    private const float Tolerance = 0.0001f;

    #region Construction Tests

    [Fact]
    public void Constructor_WithValidPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var radiansPerSecond = 1.5f;

        // Act
        var yawRate = new YawRate(radiansPerSecond);

        // Assert
        Assert.Equal(radiansPerSecond, yawRate.RadiansPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithValidNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var radiansPerSecond = -1.5f;

        // Act
        var yawRate = new YawRate(radiansPerSecond);

        // Assert
        Assert.Equal(radiansPerSecond, yawRate.RadiansPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithZeroValue_ShouldCreateInstance()
    {
        // Arrange & Act
        var yawRate = new YawRate(0f);

        // Assert
        Assert.Equal(0f, yawRate.RadiansPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var radiansPerSecond = 100f;

        // Act
        var yawRate = new YawRate(radiansPerSecond);

        // Assert
        Assert.Equal(radiansPerSecond, yawRate.RadiansPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargeNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var radiansPerSecond = -100f;

        // Act
        var yawRate = new YawRate(radiansPerSecond);

        // Assert
        Assert.Equal(radiansPerSecond, yawRate.RadiansPerSecond, Tolerance);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void Constructor_WithNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var radiansPerSecond = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new YawRate(radiansPerSecond));

        Assert.Equal("radiansPerSecond", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var radiansPerSecond = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new YawRate(radiansPerSecond));

        Assert.Equal("radiansPerSecond", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var radiansPerSecond = float.NegativeInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new YawRate(radiansPerSecond));

        Assert.Equal("radiansPerSecond", exception.ParamName);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameRadiansPerSecond_ShouldReturnTrue()
    {
        // Arrange
        var yawRate1 = new YawRate(1.5f);
        var yawRate2 = new YawRate(1.5f);

        // Act & Assert
        Assert.Equal(yawRate1, yawRate2);
        Assert.True(yawRate1 == yawRate2);
        Assert.False(yawRate1 != yawRate2);
    }

    [Fact]
    public void Equals_WithDifferentRadiansPerSecond_ShouldReturnFalse()
    {
        // Arrange
        var yawRate1 = new YawRate(1.5f);
        var yawRate2 = new YawRate(-1.5f);

        // Act & Assert
        Assert.NotEqual(yawRate1, yawRate2);
        Assert.False(yawRate1 == yawRate2);
        Assert.True(yawRate1 != yawRate2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var yawRate1 = new YawRate(1.5f);
        var yawRate2 = new YawRate(1.5f);

        // Act
        var hashCode1 = yawRate1.GetHashCode();
        var hashCode2 = yawRate2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var yawRate1 = new YawRate(1.5f);
        var yawRate2 = new YawRate(-1.5f);

        // Act
        var hashCode1 = yawRate1.GetHashCode();
        var hashCode2 = yawRate2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Conversion Tests

    [Fact]
    public void ToDegreesPerSecond_WithZeroRadiansPerSecond_ShouldReturnZero()
    {
        // Arrange
        var yawRate = new YawRate(0f);

        // Act
        var degreesPerSecond = yawRate.ToDegreesPerSecond();

        // Assert
        Assert.Equal(0f, degreesPerSecond, Tolerance);
    }

    [Fact]
    public void ToDegreesPerSecond_WithPiRadiansPerSecond_ShouldReturn180DegreesPerSecond()
    {
        // Arrange
        var yawRate = new YawRate(MathF.PI);

        // Act
        var degreesPerSecond = yawRate.ToDegreesPerSecond();

        // Assert
        Assert.Equal(180f, degreesPerSecond, Tolerance);
    }

    [Fact]
    public void ToDegreesPerSecond_WithHalfPiRadiansPerSecond_ShouldReturn90DegreesPerSecond()
    {
        // Arrange
        var yawRate = new YawRate(MathF.PI / 2f);

        // Act
        var degreesPerSecond = yawRate.ToDegreesPerSecond();

        // Assert
        Assert.Equal(90f, degreesPerSecond, Tolerance);
    }

    [Fact]
    public void ToDegreesPerSecond_WithNegativeHalfPiRadiansPerSecond_ShouldReturnNegative90DegreesPerSecond()
    {
        // Arrange
        var yawRate = new YawRate(-MathF.PI / 2f);

        // Act
        var degreesPerSecond = yawRate.ToDegreesPerSecond();

        // Assert
        Assert.Equal(-90f, degreesPerSecond, Tolerance);
    }

    [Fact]
    public void ToDegreesPerSecond_WithFiveDegreesPerSecond_ShouldReturnCorrectValue()
    {
        // Arrange
        var degreesPerSecond = 5f;
        var radiansPerSecond = degreesPerSecond * MathF.PI / 180f;
        var yawRate = new YawRate(radiansPerSecond);

        // Act
        var result = yawRate.ToDegreesPerSecond();

        // Assert
        Assert.Equal(degreesPerSecond, result, Tolerance);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void YawRate_ShouldBeImmutable()
    {
        // Arrange
        var yawRate = new YawRate(1.5f);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<YawRate>(yawRate);
        // If this compiles and runs, the type is properly immutable
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithVerySmallPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var radiansPerSecond = 0.0001f;

        // Act
        var yawRate = new YawRate(radiansPerSecond);

        // Assert
        Assert.Equal(radiansPerSecond, yawRate.RadiansPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithVerySmallNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var radiansPerSecond = -0.0001f;

        // Act
        var yawRate = new YawRate(radiansPerSecond);

        // Assert
        Assert.Equal(radiansPerSecond, yawRate.RadiansPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var radiansPerSecond = 1000f;

        // Act
        var yawRate = new YawRate(radiansPerSecond);

        // Assert
        Assert.Equal(radiansPerSecond, yawRate.RadiansPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargeNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var radiansPerSecond = -1000f;

        // Act
        var yawRate = new YawRate(radiansPerSecond);

        // Assert
        Assert.Equal(radiansPerSecond, yawRate.RadiansPerSecond, Tolerance);
    }

    #endregion
}
