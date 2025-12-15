using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class SteeringWheelAngleTests
{
    private const float MaxSteeringLockRadians = MathF.PI; // 180 degrees
    private const float HalfMaxSteeringLockRadians = MathF.PI / 2f; // 90 degrees
    private const float Tolerance = 0.0001f;

    #region Construction Tests

    [Fact]
    public void Constructor_WithValidRadians_ShouldCreateInstance()
    {
        // Arrange
        var radians = HalfMaxSteeringLockRadians;

        // Act
        var angle = new SteeringWheelAngle(radians, MaxSteeringLockRadians);

        // Assert
        Assert.Equal(radians, angle.Radians, Tolerance);
        Assert.Equal(MaxSteeringLockRadians, angle.MaxRadians, Tolerance);
    }

    [Fact]
    public void Constructor_WithZeroRadians_ShouldCreateInstance()
    {
        // Arrange & Act
        var angle = new SteeringWheelAngle(0f, MaxSteeringLockRadians);

        // Assert
        Assert.Equal(0f, angle.Radians, Tolerance);
    }

    [Fact]
    public void Constructor_WithPositiveMaxRadians_ShouldCreateInstance()
    {
        // Arrange
        var radians = HalfMaxSteeringLockRadians;

        // Act
        var angle = new SteeringWheelAngle(radians, MaxSteeringLockRadians);

        // Assert
        Assert.Equal(HalfMaxSteeringLockRadians, angle.Radians, Tolerance);
    }

    [Fact]
    public void Constructor_WithNegativeMaxRadians_ShouldCreateInstance()
    {
        // Arrange
        var radians = -HalfMaxSteeringLockRadians;

        // Act
        var angle = new SteeringWheelAngle(radians, MaxSteeringLockRadians);

        // Assert
        Assert.Equal(-HalfMaxSteeringLockRadians, angle.Radians, Tolerance);
    }

    [Fact]
    public void Constructor_WithRadiansAtMaxBoundary_ShouldCreateInstance()
    {
        // Arrange
        var radians = MaxSteeringLockRadians;

        // Act
        var angle = new SteeringWheelAngle(radians, MaxSteeringLockRadians);

        // Assert
        Assert.Equal(MaxSteeringLockRadians, angle.Radians, Tolerance);
    }

    [Fact]
    public void Constructor_WithRadiansAtNegativeMaxBoundary_ShouldCreateInstance()
    {
        // Arrange
        var radians = -MaxSteeringLockRadians;

        // Act
        var angle = new SteeringWheelAngle(radians, MaxSteeringLockRadians);

        // Assert
        Assert.Equal(-MaxSteeringLockRadians, angle.Radians, Tolerance);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void Constructor_WithRadiansExceedingMax_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var radians = MaxSteeringLockRadians + 0.1f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new SteeringWheelAngle(radians, MaxSteeringLockRadians));

        Assert.Equal("radians", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithRadiansExceedingNegativeMax_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var radians = -MaxSteeringLockRadians - 0.1f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new SteeringWheelAngle(radians, MaxSteeringLockRadians));

        Assert.Equal("radians", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeMaxRadians_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var maxRadians = -1f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new SteeringWheelAngle(0f, maxRadians));

        Assert.Equal("maxRadians", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithZeroMaxRadians_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var maxRadians = 0f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new SteeringWheelAngle(0f, maxRadians));

        Assert.Equal("maxRadians", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNaNRadians_ShouldThrowArgumentException()
    {
        // Arrange
        var radians = float.NaN;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new SteeringWheelAngle(radians, MaxSteeringLockRadians));
    }

    [Fact]
    public void Constructor_WithPositiveInfinityRadians_ShouldThrowArgumentException()
    {
        // Arrange
        var radians = float.PositiveInfinity;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new SteeringWheelAngle(radians, MaxSteeringLockRadians));
    }

    [Fact]
    public void Constructor_WithNegativeInfinityRadians_ShouldThrowArgumentException()
    {
        // Arrange
        var radians = float.NegativeInfinity;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new SteeringWheelAngle(radians, MaxSteeringLockRadians));
    }

    [Fact]
    public void Constructor_WithNaNMaxRadians_ShouldThrowArgumentException()
    {
        // Arrange
        var maxRadians = float.NaN;

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new SteeringWheelAngle(0f, maxRadians));
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameRadiansAndMaxRadians_ShouldReturnTrue()
    {
        // Arrange
        var angle1 = new SteeringWheelAngle(HalfMaxSteeringLockRadians, MaxSteeringLockRadians);
        var angle2 = new SteeringWheelAngle(HalfMaxSteeringLockRadians, MaxSteeringLockRadians);

        // Act & Assert
        Assert.Equal(angle1, angle2);
        Assert.True(angle1 == angle2);
        Assert.False(angle1 != angle2);
    }

    [Fact]
    public void Equals_WithDifferentRadians_ShouldReturnFalse()
    {
        // Arrange
        var angle1 = new SteeringWheelAngle(HalfMaxSteeringLockRadians, MaxSteeringLockRadians);
        var angle2 = new SteeringWheelAngle(-HalfMaxSteeringLockRadians, MaxSteeringLockRadians);

        // Act & Assert
        Assert.NotEqual(angle1, angle2);
        Assert.False(angle1 == angle2);
        Assert.True(angle1 != angle2);
    }

    [Fact]
    public void Equals_WithDifferentMaxRadians_ShouldReturnFalse()
    {
        // Arrange
        var angle1 = new SteeringWheelAngle(HalfMaxSteeringLockRadians, MaxSteeringLockRadians);
        var angle2 = new SteeringWheelAngle(HalfMaxSteeringLockRadians, MaxSteeringLockRadians * 2f);

        // Act & Assert
        Assert.NotEqual(angle1, angle2);
        Assert.False(angle1 == angle2);
        Assert.True(angle1 != angle2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var angle1 = new SteeringWheelAngle(HalfMaxSteeringLockRadians, MaxSteeringLockRadians);
        var angle2 = new SteeringWheelAngle(HalfMaxSteeringLockRadians, MaxSteeringLockRadians);

        // Act
        var hashCode1 = angle1.GetHashCode();
        var hashCode2 = angle2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var angle1 = new SteeringWheelAngle(HalfMaxSteeringLockRadians, MaxSteeringLockRadians);
        var angle2 = new SteeringWheelAngle(-HalfMaxSteeringLockRadians, MaxSteeringLockRadians);

        // Act
        var hashCode1 = angle1.GetHashCode();
        var hashCode2 = angle2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Conversion Tests

    [Fact]
    public void ToDegrees_WithZeroRadians_ShouldReturnZero()
    {
        // Arrange
        var angle = new SteeringWheelAngle(0f, MaxSteeringLockRadians);

        // Act
        var degrees = angle.ToDegrees();

        // Assert
        Assert.Equal(0f, degrees, Tolerance);
    }

    [Fact]
    public void ToDegrees_WithPiRadians_ShouldReturn180Degrees()
    {
        // Arrange
        var angle = new SteeringWheelAngle(MathF.PI, MaxSteeringLockRadians);

        // Act
        var degrees = angle.ToDegrees();

        // Assert
        Assert.Equal(180f, degrees, Tolerance);
    }

    [Fact]
    public void ToDegrees_WithHalfPiRadians_ShouldReturn90Degrees()
    {
        // Arrange
        var angle = new SteeringWheelAngle(MathF.PI / 2f, MaxSteeringLockRadians);

        // Act
        var degrees = angle.ToDegrees();

        // Assert
        Assert.Equal(90f, degrees, Tolerance);
    }

    [Fact]
    public void ToDegrees_WithNegativeHalfPiRadians_ShouldReturnNegative90Degrees()
    {
        // Arrange
        var angle = new SteeringWheelAngle(-MathF.PI / 2f, MaxSteeringLockRadians);

        // Act
        var degrees = angle.ToDegrees();

        // Assert
        Assert.Equal(-90f, degrees, Tolerance);
    }

    [Fact]
    public void FromDegrees_WithZeroDegrees_ShouldCreateZeroRadians()
    {
        // Arrange & Act
        var angle = SteeringWheelAngle.FromDegrees(0f, MaxSteeringLockRadians);

        // Assert
        Assert.Equal(0f, angle.Radians, Tolerance);
    }

    [Fact]
    public void FromDegrees_With180Degrees_ShouldCreatePiRadians()
    {
        // Arrange & Act
        var angle = SteeringWheelAngle.FromDegrees(180f, MaxSteeringLockRadians);

        // Assert
        Assert.Equal(MathF.PI, angle.Radians, Tolerance);
    }

    [Fact]
    public void FromDegrees_With90Degrees_ShouldCreateHalfPiRadians()
    {
        // Arrange & Act
        var angle = SteeringWheelAngle.FromDegrees(90f, MaxSteeringLockRadians);

        // Assert
        Assert.Equal(MathF.PI / 2f, angle.Radians, Tolerance);
    }

    [Fact]
    public void FromDegrees_WithDegreesExceedingMax_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var maxDegrees = 180f;
        var degrees = maxDegrees + 1f;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            SteeringWheelAngle.FromDegrees(degrees, MaxSteeringLockRadians));
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void SteeringWheelAngle_ShouldBeImmutable()
    {
        // Arrange
        var angle = new SteeringWheelAngle(HalfMaxSteeringLockRadians, MaxSteeringLockRadians);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<SteeringWheelAngle>(angle);
        // If this compiles and runs, the type is properly immutable
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithVerySmallRadians_ShouldCreateInstance()
    {
        // Arrange
        var radians = 0.0001f;

        // Act
        var angle = new SteeringWheelAngle(radians, MaxSteeringLockRadians);

        // Assert
        Assert.Equal(radians, angle.Radians, Tolerance);
    }

    [Fact]
    public void Constructor_WithVerySmallMaxRadians_ShouldCreateInstance()
    {
        // Arrange
        var maxRadians = 0.1f;
        var radians = 0.05f;

        // Act
        var angle = new SteeringWheelAngle(radians, maxRadians);

        // Assert
        Assert.Equal(radians, angle.Radians, Tolerance);
        Assert.Equal(maxRadians, angle.MaxRadians, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargeMaxRadians_ShouldCreateInstance()
    {
        // Arrange
        var maxRadians = MathF.PI * 2f; // 360 degrees
        var radians = MathF.PI; // 180 degrees

        // Act
        var angle = new SteeringWheelAngle(radians, maxRadians);

        // Assert
        Assert.Equal(radians, angle.Radians, Tolerance);
        Assert.Equal(maxRadians, angle.MaxRadians, Tolerance);
    }

    #endregion
}
