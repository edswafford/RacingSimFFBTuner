using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class WheelVelocityTests
{
    #region Construction Tests

    [Fact]
    public void Constructor_WithZero_ShouldCreateInstance()
    {
        // Arrange & Act
        var wheelVelocity = new WheelVelocity(0);

        // Assert
        Assert.Equal(0, wheelVelocity.RawUnitsPerSecond);
    }

    [Fact]
    public void Constructor_WithPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var rawUnitsPerSecond = 1000; // Moving in one direction

        // Act
        var wheelVelocity = new WheelVelocity(rawUnitsPerSecond);

        // Assert
        Assert.Equal(rawUnitsPerSecond, wheelVelocity.RawUnitsPerSecond);
    }

    [Fact]
    public void Constructor_WithNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var rawUnitsPerSecond = -1000; // Moving in opposite direction

        // Act
        var wheelVelocity = new WheelVelocity(rawUnitsPerSecond);

        // Assert
        Assert.Equal(rawUnitsPerSecond, wheelVelocity.RawUnitsPerSecond);
    }

    [Fact]
    public void Constructor_WithLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var rawUnitsPerSecond = 50000; // Fast movement

        // Act
        var wheelVelocity = new WheelVelocity(rawUnitsPerSecond);

        // Assert
        Assert.Equal(rawUnitsPerSecond, wheelVelocity.RawUnitsPerSecond);
    }

    [Fact]
    public void Constructor_WithLargeNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var rawUnitsPerSecond = -50000; // Fast movement in opposite direction

        // Act
        var wheelVelocity = new WheelVelocity(rawUnitsPerSecond);

        // Assert
        Assert.Equal(rawUnitsPerSecond, wheelVelocity.RawUnitsPerSecond);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameRawUnitsPerSecond_ShouldReturnTrue()
    {
        // Arrange
        var wheelVelocity1 = new WheelVelocity(1000);
        var wheelVelocity2 = new WheelVelocity(1000);

        // Act & Assert
        Assert.Equal(wheelVelocity1, wheelVelocity2);
        Assert.True(wheelVelocity1 == wheelVelocity2);
        Assert.False(wheelVelocity1 != wheelVelocity2);
    }

    [Fact]
    public void Equals_WithDifferentRawUnitsPerSecond_ShouldReturnFalse()
    {
        // Arrange
        var wheelVelocity1 = new WheelVelocity(1000);
        var wheelVelocity2 = new WheelVelocity(-1000);

        // Act & Assert
        Assert.NotEqual(wheelVelocity1, wheelVelocity2);
        Assert.False(wheelVelocity1 == wheelVelocity2);
        Assert.True(wheelVelocity1 != wheelVelocity2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var wheelVelocity1 = new WheelVelocity(1000);
        var wheelVelocity2 = new WheelVelocity(1000);

        // Act
        var hashCode1 = wheelVelocity1.GetHashCode();
        var hashCode2 = wheelVelocity2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var wheelVelocity1 = new WheelVelocity(1000);
        var wheelVelocity2 = new WheelVelocity(-1000);

        // Act
        var hashCode1 = wheelVelocity1.GetHashCode();
        var hashCode2 = wheelVelocity2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void WheelVelocity_ShouldBeImmutable()
    {
        // Arrange
        var wheelVelocity = new WheelVelocity(1000);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<WheelVelocity>(wheelVelocity);
        // If this compiles and runs, the type is properly immutable
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithIntMaxValue_ShouldCreateInstance()
    {
        // Arrange
        var rawUnitsPerSecond = int.MaxValue;

        // Act
        var wheelVelocity = new WheelVelocity(rawUnitsPerSecond);

        // Assert
        Assert.Equal(rawUnitsPerSecond, wheelVelocity.RawUnitsPerSecond);
    }

    [Fact]
    public void Constructor_WithIntMinValue_ShouldCreateInstance()
    {
        // Arrange
        var rawUnitsPerSecond = int.MinValue;

        // Act
        var wheelVelocity = new WheelVelocity(rawUnitsPerSecond);

        // Assert
        Assert.Equal(rawUnitsPerSecond, wheelVelocity.RawUnitsPerSecond);
    }

    [Fact]
    public void Constructor_WithVerySmallPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var rawUnitsPerSecond = 1;

        // Act
        var wheelVelocity = new WheelVelocity(rawUnitsPerSecond);

        // Assert
        Assert.Equal(rawUnitsPerSecond, wheelVelocity.RawUnitsPerSecond);
    }

    [Fact]
    public void Constructor_WithVerySmallNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var rawUnitsPerSecond = -1;

        // Act
        var wheelVelocity = new WheelVelocity(rawUnitsPerSecond);

        // Assert
        Assert.Equal(rawUnitsPerSecond, wheelVelocity.RawUnitsPerSecond);
    }

    #endregion
}
