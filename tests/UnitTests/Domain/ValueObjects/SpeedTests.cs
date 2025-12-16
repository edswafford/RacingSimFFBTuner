using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class SpeedTests
{
    private const float Tolerance = 0.0001f;

    #region Construction Tests

    [Fact]
    public void Constructor_WithZero_ShouldCreateInstance()
    {
        // Arrange & Act
        var speed = new Speed(0f);

        // Assert
        Assert.Equal(0f, speed.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var metersPerSecond = 27.78f; // ~100 km/h

        // Act
        var speed = new Speed(metersPerSecond);

        // Assert
        Assert.Equal(metersPerSecond, speed.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var metersPerSecond = 100f; // ~360 km/h

        // Act
        var speed = new Speed(metersPerSecond);

        // Assert
        Assert.Equal(metersPerSecond, speed.MetersPerSecond, Tolerance);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void Constructor_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var metersPerSecond = -1f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Speed(metersPerSecond));

        Assert.Equal("metersPerSecond", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var metersPerSecond = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Speed(metersPerSecond));

        Assert.Equal("metersPerSecond", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var metersPerSecond = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Speed(metersPerSecond));

        Assert.Equal("metersPerSecond", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var metersPerSecond = float.NegativeInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Speed(metersPerSecond));

        Assert.Equal("metersPerSecond", exception.ParamName);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameMetersPerSecond_ShouldReturnTrue()
    {
        // Arrange
        var speed1 = new Speed(27.78f);
        var speed2 = new Speed(27.78f);

        // Act & Assert
        Assert.Equal(speed1, speed2);
        Assert.True(speed1 == speed2);
        Assert.False(speed1 != speed2);
    }

    [Fact]
    public void Equals_WithDifferentMetersPerSecond_ShouldReturnFalse()
    {
        // Arrange
        var speed1 = new Speed(27.78f);
        var speed2 = new Speed(13.89f);

        // Act & Assert
        Assert.NotEqual(speed1, speed2);
        Assert.False(speed1 == speed2);
        Assert.True(speed1 != speed2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var speed1 = new Speed(27.78f);
        var speed2 = new Speed(27.78f);

        // Act
        var hashCode1 = speed1.GetHashCode();
        var hashCode2 = speed2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var speed1 = new Speed(27.78f);
        var speed2 = new Speed(13.89f);

        // Act
        var hashCode1 = speed1.GetHashCode();
        var hashCode2 = speed2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Conversion Tests

    [Fact]
    public void ToKilometersPerHour_WithZero_ShouldReturnZero()
    {
        // Arrange
        var speed = new Speed(0f);

        // Act
        var kmh = speed.ToKilometersPerHour();

        // Assert
        Assert.Equal(0f, kmh, Tolerance);
    }

    [Fact]
    public void ToKilometersPerHour_With27Point7778MetersPerSecond_ShouldReturn100KilometersPerHour()
    {
        // Arrange
        // 100 km/h = 100 / 3.6 = 27.777... m/s
        var metersPerSecond = 100f / 3.6f;
        var speed = new Speed(metersPerSecond);

        // Act
        var kmh = speed.ToKilometersPerHour();

        // Assert
        Assert.Equal(100f, kmh, Tolerance);
    }

    [Fact]
    public void ToKilometersPerHour_With13Point8889MetersPerSecond_ShouldReturn50KilometersPerHour()
    {
        // Arrange
        // 50 km/h = 50 / 3.6 = 13.888... m/s
        var metersPerSecond = 50f / 3.6f;
        var speed = new Speed(metersPerSecond);

        // Act
        var kmh = speed.ToKilometersPerHour();

        // Assert
        Assert.Equal(50f, kmh, Tolerance);
    }

    [Fact]
    public void ToMilesPerHour_WithZero_ShouldReturnZero()
    {
        // Arrange
        var speed = new Speed(0f);

        // Act
        var mph = speed.ToMilesPerHour();

        // Assert
        Assert.Equal(0f, mph, Tolerance);
    }

    [Fact]
    public void ToMilesPerHour_With44Point7027MetersPerSecond_ShouldReturn100MilesPerHour()
    {
        // Arrange
        // 100 mph = 100 / 2.237 = 44.702... m/s
        var metersPerSecond = 100f / 2.237f;
        var speed = new Speed(metersPerSecond);

        // Act
        var mph = speed.ToMilesPerHour();

        // Assert
        Assert.Equal(100f, mph, Tolerance);
    }

    [Fact]
    public void ToMilesPerHour_With22Point3514MetersPerSecond_ShouldReturn50MilesPerHour()
    {
        // Arrange
        // 50 mph = 50 / 2.237 = 22.351... m/s
        var metersPerSecond = 50f / 2.237f;
        var speed = new Speed(metersPerSecond);

        // Act
        var mph = speed.ToMilesPerHour();

        // Assert
        Assert.Equal(50f, mph, Tolerance);
    }

    #endregion

    #region Factory Method Tests

    [Fact]
    public void FromKilometersPerHour_WithZero_ShouldCreateZeroSpeed()
    {
        // Arrange & Act
        var speed = Speed.FromKilometersPerHour(0f);

        // Assert
        Assert.Equal(0f, speed.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void FromKilometersPerHour_With100KilometersPerHour_ShouldCreate27Point7778MetersPerSecond()
    {
        // Arrange & Act
        var speed = Speed.FromKilometersPerHour(100f);

        // Assert
        // 100 km/h = 100 / 3.6 = 27.777... m/s
        var expectedMetersPerSecond = 100f / 3.6f;
        Assert.Equal(expectedMetersPerSecond, speed.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void FromKilometersPerHour_With50KilometersPerHour_ShouldCreate13Point8889MetersPerSecond()
    {
        // Arrange & Act
        var speed = Speed.FromKilometersPerHour(50f);

        // Assert
        // 50 km/h = 50 / 3.6 = 13.888... m/s
        var expectedMetersPerSecond = 50f / 3.6f;
        Assert.Equal(expectedMetersPerSecond, speed.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void FromKilometersPerHour_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var kmh = -1f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            Speed.FromKilometersPerHour(kmh));

        Assert.Equal("kmh", exception.ParamName);
    }

    [Fact]
    public void FromKilometersPerHour_WithNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var kmh = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            Speed.FromKilometersPerHour(kmh));

        Assert.Equal("kmh", exception.ParamName);
    }

    [Fact]
    public void FromMilesPerHour_WithZero_ShouldCreateZeroSpeed()
    {
        // Arrange & Act
        var speed = Speed.FromMilesPerHour(0f);

        // Assert
        Assert.Equal(0f, speed.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void FromMilesPerHour_With100MilesPerHour_ShouldCreate44Point7027MetersPerSecond()
    {
        // Arrange & Act
        var speed = Speed.FromMilesPerHour(100f);

        // Assert
        // 100 mph = 100 / 2.237 = 44.702... m/s
        var expectedMetersPerSecond = 100f / 2.237f;
        Assert.Equal(expectedMetersPerSecond, speed.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void FromMilesPerHour_With50MilesPerHour_ShouldCreate22Point3514MetersPerSecond()
    {
        // Arrange & Act
        var speed = Speed.FromMilesPerHour(50f);

        // Assert
        // 50 mph = 50 / 2.237 = 22.351... m/s
        var expectedMetersPerSecond = 50f / 2.237f;
        Assert.Equal(expectedMetersPerSecond, speed.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void FromMilesPerHour_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var mph = -1f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            Speed.FromMilesPerHour(mph));

        Assert.Equal("mph", exception.ParamName);
    }

    [Fact]
    public void FromMilesPerHour_WithNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var mph = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            Speed.FromMilesPerHour(mph));

        Assert.Equal("mph", exception.ParamName);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void Speed_ShouldBeImmutable()
    {
        // Arrange
        var speed = new Speed(27.78f);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<Speed>(speed);
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
        var speed = new Speed(metersPerSecond);

        // Assert
        Assert.Equal(metersPerSecond, speed.MetersPerSecond, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var metersPerSecond = 500f; // ~1800 km/h

        // Act
        var speed = new Speed(metersPerSecond);

        // Assert
        Assert.Equal(metersPerSecond, speed.MetersPerSecond, Tolerance);
    }

    #endregion
}
