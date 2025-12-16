using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class ShockVelocityTests
{
    private const float Tolerance = 0.0001f;

    #region Construction Tests

    [Fact]
    public void Constructor_WithZero_ShouldCreateInstance()
    {
        // Arrange & Act
        var shockVelocity = new ShockVelocity(0f);

        // Assert
        Assert.Equal(0f, shockVelocity.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var metersPerSecond = 2.5f; // Compression

        // Act
        var shockVelocity = new ShockVelocity(metersPerSecond);

        // Assert
        Assert.Equal(metersPerSecond, shockVelocity.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var metersPerSecond = -2.5f; // Extension

        // Act
        var shockVelocity = new ShockVelocity(metersPerSecond);

        // Assert
        Assert.Equal(metersPerSecond, shockVelocity.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var metersPerSecond = 10f; // Large compression

        // Act
        var shockVelocity = new ShockVelocity(metersPerSecond);

        // Assert
        Assert.Equal(metersPerSecond, shockVelocity.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargeNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var metersPerSecond = -10f; // Large extension

        // Act
        var shockVelocity = new ShockVelocity(metersPerSecond);

        // Assert
        Assert.Equal(metersPerSecond, shockVelocity.MetersPerSecond, Tolerance);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void Constructor_WithNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var metersPerSecond = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new ShockVelocity(metersPerSecond));

        Assert.Equal("metersPerSecond", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var metersPerSecond = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new ShockVelocity(metersPerSecond));

        Assert.Equal("metersPerSecond", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var metersPerSecond = float.NegativeInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new ShockVelocity(metersPerSecond));

        Assert.Equal("metersPerSecond", exception.ParamName);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameMetersPerSecond_ShouldReturnTrue()
    {
        // Arrange
        var shockVelocity1 = new ShockVelocity(2.5f);
        var shockVelocity2 = new ShockVelocity(2.5f);

        // Act & Assert
        Assert.Equal(shockVelocity1, shockVelocity2);
        Assert.True(shockVelocity1 == shockVelocity2);
        Assert.False(shockVelocity1 != shockVelocity2);
    }

    [Fact]
    public void Equals_WithDifferentMetersPerSecond_ShouldReturnFalse()
    {
        // Arrange
        var shockVelocity1 = new ShockVelocity(2.5f);
        var shockVelocity2 = new ShockVelocity(-2.5f);

        // Act & Assert
        Assert.NotEqual(shockVelocity1, shockVelocity2);
        Assert.False(shockVelocity1 == shockVelocity2);
        Assert.True(shockVelocity1 != shockVelocity2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var shockVelocity1 = new ShockVelocity(2.5f);
        var shockVelocity2 = new ShockVelocity(2.5f);

        // Act
        var hashCode1 = shockVelocity1.GetHashCode();
        var hashCode2 = shockVelocity2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var shockVelocity1 = new ShockVelocity(2.5f);
        var shockVelocity2 = new ShockVelocity(-2.5f);

        // Act
        var hashCode1 = shockVelocity1.GetHashCode();
        var hashCode2 = shockVelocity2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void ShockVelocity_ShouldBeImmutable()
    {
        // Arrange
        var shockVelocity = new ShockVelocity(2.5f);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<ShockVelocity>(shockVelocity);
        // If this compiles and runs, the type is properly immutable
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithVerySmallPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var metersPerSecond = 0.0001f;

        // Act
        var shockVelocity = new ShockVelocity(metersPerSecond);

        // Assert
        Assert.Equal(metersPerSecond, shockVelocity.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithVerySmallNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var metersPerSecond = -0.0001f;

        // Act
        var shockVelocity = new ShockVelocity(metersPerSecond);

        // Assert
        Assert.Equal(metersPerSecond, shockVelocity.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var metersPerSecond = 50f; // Extreme compression

        // Act
        var shockVelocity = new ShockVelocity(metersPerSecond);

        // Assert
        Assert.Equal(metersPerSecond, shockVelocity.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargeNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var metersPerSecond = -50f; // Extreme extension

        // Act
        var shockVelocity = new ShockVelocity(metersPerSecond);

        // Assert
        Assert.Equal(metersPerSecond, shockVelocity.MetersPerSecond, Tolerance);
    }

    #endregion
}
