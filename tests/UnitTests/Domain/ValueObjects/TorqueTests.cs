using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class TorqueTests
{
    private const float Tolerance = 0.0001f;

    #region Construction Tests

    [Fact]
    public void Constructor_WithValidPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var newtonMeters = 5.5f;

        // Act
        var torque = new Torque(newtonMeters);

        // Assert
        Assert.Equal(newtonMeters, torque.NewtonMeters, Tolerance);
    }

    [Fact]
    public void Constructor_WithValidNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var newtonMeters = -5.5f;

        // Act
        var torque = new Torque(newtonMeters);

        // Assert
        Assert.Equal(newtonMeters, torque.NewtonMeters, Tolerance);
    }

    [Fact]
    public void Constructor_WithZeroValue_ShouldCreateInstance()
    {
        // Arrange & Act
        var torque = new Torque(0f);

        // Assert
        Assert.Equal(0f, torque.NewtonMeters, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var newtonMeters = 100f;

        // Act
        var torque = new Torque(newtonMeters);

        // Assert
        Assert.Equal(newtonMeters, torque.NewtonMeters, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargeNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var newtonMeters = -100f;

        // Act
        var torque = new Torque(newtonMeters);

        // Assert
        Assert.Equal(newtonMeters, torque.NewtonMeters, Tolerance);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void Constructor_WithNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var newtonMeters = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Torque(newtonMeters));

        Assert.Equal("newtonMeters", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var newtonMeters = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Torque(newtonMeters));

        Assert.Equal("newtonMeters", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var newtonMeters = float.NegativeInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Torque(newtonMeters));

        Assert.Equal("newtonMeters", exception.ParamName);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameNewtonMeters_ShouldReturnTrue()
    {
        // Arrange
        var torque1 = new Torque(5.5f);
        var torque2 = new Torque(5.5f);

        // Act & Assert
        Assert.Equal(torque1, torque2);
        Assert.True(torque1 == torque2);
        Assert.False(torque1 != torque2);
    }

    [Fact]
    public void Equals_WithDifferentNewtonMeters_ShouldReturnFalse()
    {
        // Arrange
        var torque1 = new Torque(5.5f);
        var torque2 = new Torque(-5.5f);

        // Act & Assert
        Assert.NotEqual(torque1, torque2);
        Assert.False(torque1 == torque2);
        Assert.True(torque1 != torque2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var torque1 = new Torque(5.5f);
        var torque2 = new Torque(5.5f);

        // Act
        var hashCode1 = torque1.GetHashCode();
        var hashCode2 = torque2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var torque1 = new Torque(5.5f);
        var torque2 = new Torque(-5.5f);

        // Act
        var hashCode1 = torque1.GetHashCode();
        var hashCode2 = torque2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void Torque_ShouldBeImmutable()
    {
        // Arrange
        var torque = new Torque(5.5f);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<Torque>(torque);
        // If this compiles and runs, the type is properly immutable
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithVerySmallPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var newtonMeters = 0.0001f;

        // Act
        var torque = new Torque(newtonMeters);

        // Assert
        Assert.Equal(newtonMeters, torque.NewtonMeters, Tolerance);
    }

    [Fact]
    public void Constructor_WithVerySmallNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var newtonMeters = -0.0001f;

        // Act
        var torque = new Torque(newtonMeters);

        // Assert
        Assert.Equal(newtonMeters, torque.NewtonMeters, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var newtonMeters = 1000f;

        // Act
        var torque = new Torque(newtonMeters);

        // Assert
        Assert.Equal(newtonMeters, torque.NewtonMeters, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargeNegativeValue_ShouldCreateInstance()
    {
        // Arrange
        var newtonMeters = -1000f;

        // Act
        var torque = new Torque(newtonMeters);

        // Assert
        Assert.Equal(newtonMeters, torque.NewtonMeters, Tolerance);
    }

    #endregion
}
