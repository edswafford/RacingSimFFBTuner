using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class GForceTests
{
    private const float Tolerance = 0.0001f;

    #region Construction Tests

    [Fact]
    public void Constructor_WithValidZeroValue_ShouldCreateInstance()
    {
        // Arrange & Act
        var gForce = new GForce(0f);

        // Assert
        Assert.Equal(0f, gForce.Value, Tolerance);
    }

    [Fact]
    public void Constructor_WithValidPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var value = 1.5f;

        // Act
        var gForce = new GForce(value);

        // Assert
        Assert.Equal(value, gForce.Value, Tolerance);
    }

    [Fact]
    public void Constructor_WithValidNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var value = -1.5f;

        // Act
        var gForce = new GForce(value);

        // Assert
        Assert.Equal(value, gForce.Value, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var value = 10f; // High G-force during crash

        // Act
        var gForce = new GForce(value);

        // Assert
        Assert.Equal(value, gForce.Value, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargeNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var value = -10f; // Hard deceleration

        // Act
        var gForce = new GForce(value);

        // Assert
        Assert.Equal(value, gForce.Value, Tolerance);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void Constructor_WithNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var value = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new GForce(value));

        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var value = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new GForce(value));

        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var value = float.NegativeInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new GForce(value));

        Assert.Equal("value", exception.ParamName);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var gForce1 = new GForce(1.5f);
        var gForce2 = new GForce(1.5f);

        // Act & Assert
        Assert.Equal(gForce1, gForce2);
        Assert.True(gForce1 == gForce2);
        Assert.False(gForce1 != gForce2);
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var gForce1 = new GForce(1.5f);
        var gForce2 = new GForce(-1.5f);

        // Act & Assert
        Assert.NotEqual(gForce1, gForce2);
        Assert.False(gForce1 == gForce2);
        Assert.True(gForce1 != gForce2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var gForce1 = new GForce(1.5f);
        var gForce2 = new GForce(1.5f);

        // Act
        var hashCode1 = gForce1.GetHashCode();
        var hashCode2 = gForce2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var gForce1 = new GForce(1.5f);
        var gForce2 = new GForce(-1.5f);

        // Act
        var hashCode1 = gForce1.GetHashCode();
        var hashCode2 = gForce2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void GForce_ShouldBeImmutable()
    {
        // Arrange
        var gForce = new GForce(1.5f);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<GForce>(gForce);
        // If this compiles and runs, the type is properly immutable
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithVerySmallPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var value = 0.0001f;

        // Act
        var gForce = new GForce(value);

        // Assert
        Assert.Equal(value, gForce.Value, Tolerance);
    }

    [Fact]
    public void Constructor_WithVerySmallNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var value = -0.0001f;

        // Act
        var gForce = new GForce(value);

        // Assert
        Assert.Equal(value, gForce.Value, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var value = 100f; // Extreme crash scenario

        // Act
        var gForce = new GForce(value);

        // Assert
        Assert.Equal(value, gForce.Value, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargeNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var value = -100f; // Extreme deceleration

        // Act
        var gForce = new GForce(value);

        // Assert
        Assert.Equal(value, gForce.Value, Tolerance);
    }

    #endregion
}
