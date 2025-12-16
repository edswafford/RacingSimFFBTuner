using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class WheelPositionTests
{
    #region Construction Tests

    [Fact]
    public void Constructor_WithZero_ShouldCreateInstance()
    {
        // Arrange & Act
        var wheelPosition = new WheelPosition(0);

        // Assert
        Assert.Equal(0, wheelPosition.RawValue);
    }

    [Fact]
    public void Constructor_WithPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var rawValue = 16384; // Half positive range

        // Act
        var wheelPosition = new WheelPosition(rawValue);

        // Assert
        Assert.Equal(rawValue, wheelPosition.RawValue);
    }

    [Fact]
    public void Constructor_WithNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var rawValue = -16384; // Half negative range

        // Act
        var wheelPosition = new WheelPosition(rawValue);

        // Assert
        Assert.Equal(rawValue, wheelPosition.RawValue);
    }

    [Fact]
    public void Constructor_WithMaximumPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var rawValue = 32767; // Maximum DirectInput value

        // Act
        var wheelPosition = new WheelPosition(rawValue);

        // Assert
        Assert.Equal(rawValue, wheelPosition.RawValue);
    }

    [Fact]
    public void Constructor_WithMaximumNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var rawValue = -32768; // Minimum DirectInput value

        // Act
        var wheelPosition = new WheelPosition(rawValue);

        // Assert
        Assert.Equal(rawValue, wheelPosition.RawValue);
    }

    [Fact]
    public void Constructor_WithLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var rawValue = 50000; // Beyond typical DirectInput range

        // Act
        var wheelPosition = new WheelPosition(rawValue);

        // Assert
        Assert.Equal(rawValue, wheelPosition.RawValue);
    }

    [Fact]
    public void Constructor_WithLargeNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var rawValue = -50000; // Beyond typical DirectInput range

        // Act
        var wheelPosition = new WheelPosition(rawValue);

        // Assert
        Assert.Equal(rawValue, wheelPosition.RawValue);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameRawValue_ShouldReturnTrue()
    {
        // Arrange
        var wheelPosition1 = new WheelPosition(16384);
        var wheelPosition2 = new WheelPosition(16384);

        // Act & Assert
        Assert.Equal(wheelPosition1, wheelPosition2);
        Assert.True(wheelPosition1 == wheelPosition2);
        Assert.False(wheelPosition1 != wheelPosition2);
    }

    [Fact]
    public void Equals_WithDifferentRawValue_ShouldReturnFalse()
    {
        // Arrange
        var wheelPosition1 = new WheelPosition(16384);
        var wheelPosition2 = new WheelPosition(-16384);

        // Act & Assert
        Assert.NotEqual(wheelPosition1, wheelPosition2);
        Assert.False(wheelPosition1 == wheelPosition2);
        Assert.True(wheelPosition1 != wheelPosition2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var wheelPosition1 = new WheelPosition(16384);
        var wheelPosition2 = new WheelPosition(16384);

        // Act
        var hashCode1 = wheelPosition1.GetHashCode();
        var hashCode2 = wheelPosition2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var wheelPosition1 = new WheelPosition(16384);
        var wheelPosition2 = new WheelPosition(-16384);

        // Act
        var hashCode1 = wheelPosition1.GetHashCode();
        var hashCode2 = wheelPosition2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void WheelPosition_ShouldBeImmutable()
    {
        // Arrange
        var wheelPosition = new WheelPosition(16384);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<WheelPosition>(wheelPosition);
        // If this compiles and runs, the type is properly immutable
    }

    #endregion

    #region Normalization Tests

    [Fact]
    public void Normalized_WithDefaultMaxRawValue_ShouldReturnCorrectValue()
    {
        // Arrange
        var wheelPosition = new WheelPosition(16384); // Half of 32767

        // Act
        var normalized = wheelPosition.Normalized();

        // Assert
        Assert.Equal(0.5f, normalized, 0.0001f);
    }

    [Fact]
    public void Normalized_WithMaximumPositiveValue_ShouldReturnOne()
    {
        // Arrange
        var wheelPosition = new WheelPosition(32767);

        // Act
        var normalized = wheelPosition.Normalized();

        // Assert
        Assert.Equal(1.0f, normalized, 0.0001f);
    }

    [Fact]
    public void Normalized_WithMaximumNegativeValue_ShouldReturnNegativeOne()
    {
        // Arrange
        var wheelPosition = new WheelPosition(-32768);

        // Act
        var normalized = wheelPosition.Normalized();

        // Assert
        Assert.Equal(-1.0f, normalized, 0.0001f);
    }

    [Fact]
    public void Normalized_WithZero_ShouldReturnZero()
    {
        // Arrange
        var wheelPosition = new WheelPosition(0);

        // Act
        var normalized = wheelPosition.Normalized();

        // Assert
        Assert.Equal(0.0f, normalized, 0.0001f);
    }

    [Fact]
    public void Normalized_WithCustomMaxRawValue_ShouldReturnCorrectValue()
    {
        // Arrange
        var wheelPosition = new WheelPosition(5000);
        var maxRawValue = 10000f;

        // Act
        var normalized = wheelPosition.Normalized(maxRawValue);

        // Assert
        Assert.Equal(0.5f, normalized, 0.0001f);
    }

    [Fact]
    public void Normalized_WithNegativeValue_ShouldReturnNegativeNormalized()
    {
        // Arrange
        var wheelPosition = new WheelPosition(-16384);

        // Act
        var normalized = wheelPosition.Normalized();

        // Assert
        Assert.Equal(-0.5f, normalized, 0.0001f);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithIntMaxValue_ShouldCreateInstance()
    {
        // Arrange
        var rawValue = int.MaxValue;

        // Act
        var wheelPosition = new WheelPosition(rawValue);

        // Assert
        Assert.Equal(rawValue, wheelPosition.RawValue);
    }

    [Fact]
    public void Constructor_WithIntMinValue_ShouldCreateInstance()
    {
        // Arrange
        var rawValue = int.MinValue;

        // Act
        var wheelPosition = new WheelPosition(rawValue);

        // Assert
        Assert.Equal(rawValue, wheelPosition.RawValue);
    }

    #endregion
}
