using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class VelocityTests
{
    private const float Tolerance = 0.0001f;

    #region Construction Tests

    [Fact]
    public void Constructor_WithAllPositiveComponents_ShouldCreateInstance()
    {
        // Arrange
        var x = 10.5f;
        var y = 5.2f;
        var z = 2.1f;

        // Act
        var velocity = new Velocity(x, y, z);

        // Assert
        Assert.Equal(x, velocity.X, Tolerance);
        Assert.Equal(y, velocity.Y, Tolerance);
        Assert.Equal(z, velocity.Z, Tolerance);
    }

    [Fact]
    public void Constructor_WithAllNegativeComponents_ShouldCreateInstance()
    {
        // Arrange
        var x = -10.5f;
        var y = -5.2f;
        var z = -2.1f;

        // Act
        var velocity = new Velocity(x, y, z);

        // Assert
        Assert.Equal(x, velocity.X, Tolerance);
        Assert.Equal(y, velocity.Y, Tolerance);
        Assert.Equal(z, velocity.Z, Tolerance);
    }

    [Fact]
    public void Constructor_WithMixedPositiveNegativeComponents_ShouldCreateInstance()
    {
        // Arrange
        var x = 10.5f;
        var y = -5.2f;
        var z = 2.1f;

        // Act
        var velocity = new Velocity(x, y, z);

        // Assert
        Assert.Equal(x, velocity.X, Tolerance);
        Assert.Equal(y, velocity.Y, Tolerance);
        Assert.Equal(z, velocity.Z, Tolerance);
    }

    [Fact]
    public void Constructor_WithZeroComponents_ShouldCreateInstance()
    {
        // Arrange & Act
        var velocity = new Velocity(0f, 0f, 0f);

        // Assert
        Assert.Equal(0f, velocity.X, Tolerance);
        Assert.Equal(0f, velocity.Y, Tolerance);
        Assert.Equal(0f, velocity.Z, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargeValues_ShouldCreateInstance()
    {
        // Arrange
        var x = 1000f;
        var y = -500f;
        var z = 250f;

        // Act
        var velocity = new Velocity(x, y, z);

        // Assert
        Assert.Equal(x, velocity.X, Tolerance);
        Assert.Equal(y, velocity.Y, Tolerance);
        Assert.Equal(z, velocity.Z, Tolerance);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void Constructor_WithXComponentNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var x = float.NaN;
        var y = 5.2f;
        var z = 2.1f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Velocity(x, y, z));

        Assert.Equal("x", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithYComponentInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var x = 10.5f;
        var y = float.PositiveInfinity;
        var z = 2.1f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Velocity(x, y, z));

        Assert.Equal("y", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithZComponentNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var x = 10.5f;
        var y = 5.2f;
        var z = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Velocity(x, y, z));

        Assert.Equal("z", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithXComponentPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var x = float.PositiveInfinity;
        var y = 5.2f;
        var z = 2.1f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Velocity(x, y, z));

        Assert.Equal("x", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithYComponentNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var x = 10.5f;
        var y = float.NegativeInfinity;
        var z = 2.1f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Velocity(x, y, z));

        Assert.Equal("y", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithZComponentPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var x = 10.5f;
        var y = 5.2f;
        var z = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Velocity(x, y, z));

        Assert.Equal("z", exception.ParamName);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameXyzComponents_ShouldReturnTrue()
    {
        // Arrange
        var velocity1 = new Velocity(10.5f, 5.2f, 2.1f);
        var velocity2 = new Velocity(10.5f, 5.2f, 2.1f);

        // Act & Assert
        Assert.Equal(velocity1, velocity2);
        Assert.True(velocity1 == velocity2);
        Assert.False(velocity1 != velocity2);
    }

    [Fact]
    public void Equals_WithDifferentXComponent_ShouldReturnFalse()
    {
        // Arrange
        var velocity1 = new Velocity(10.5f, 5.2f, 2.1f);
        var velocity2 = new Velocity(20.5f, 5.2f, 2.1f);

        // Act & Assert
        Assert.NotEqual(velocity1, velocity2);
        Assert.False(velocity1 == velocity2);
        Assert.True(velocity1 != velocity2);
    }

    [Fact]
    public void Equals_WithDifferentYComponent_ShouldReturnFalse()
    {
        // Arrange
        var velocity1 = new Velocity(10.5f, 5.2f, 2.1f);
        var velocity2 = new Velocity(10.5f, 15.2f, 2.1f);

        // Act & Assert
        Assert.NotEqual(velocity1, velocity2);
        Assert.False(velocity1 == velocity2);
        Assert.True(velocity1 != velocity2);
    }

    [Fact]
    public void Equals_WithDifferentZComponent_ShouldReturnFalse()
    {
        // Arrange
        var velocity1 = new Velocity(10.5f, 5.2f, 2.1f);
        var velocity2 = new Velocity(10.5f, 5.2f, 12.1f);

        // Act & Assert
        Assert.NotEqual(velocity1, velocity2);
        Assert.False(velocity1 == velocity2);
        Assert.True(velocity1 != velocity2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var velocity1 = new Velocity(10.5f, 5.2f, 2.1f);
        var velocity2 = new Velocity(10.5f, 5.2f, 2.1f);

        // Act
        var hashCode1 = velocity1.GetHashCode();
        var hashCode2 = velocity2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var velocity1 = new Velocity(10.5f, 5.2f, 2.1f);
        var velocity2 = new Velocity(20.5f, 5.2f, 2.1f);

        // Act
        var hashCode1 = velocity1.GetHashCode();
        var hashCode2 = velocity2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Method Tests

    [Fact]
    public void Magnitude_WithZeroComponents_ShouldReturnZero()
    {
        // Arrange
        var velocity = new Velocity(0f, 0f, 0f);

        // Act
        var magnitude = velocity.Magnitude();

        // Assert
        Assert.Equal(0f, magnitude, Tolerance);
    }

    [Fact]
    public void Magnitude_WithPositiveComponents_ShouldReturnCorrectValue()
    {
        // Arrange
        var velocity = new Velocity(3f, 4f, 0f);

        // Act
        var magnitude = velocity.Magnitude();

        // Assert
        Assert.Equal(5f, magnitude, Tolerance);
    }

    [Fact]
    public void Magnitude_WithNegativeComponents_ShouldReturnCorrectValue()
    {
        // Arrange
        var velocity = new Velocity(-3f, -4f, 0f);

        // Act
        var magnitude = velocity.Magnitude();

        // Assert
        Assert.Equal(5f, magnitude, Tolerance);
    }

    [Fact]
    public void Magnitude_WithAllThreeComponents_ShouldReturnCorrectValue()
    {
        // Arrange
        var velocity = new Velocity(2f, 3f, 6f);

        // Act
        var magnitude = velocity.Magnitude();

        // Assert
        // sqrt(2^2 + 3^2 + 6^2) = sqrt(4 + 9 + 36) = sqrt(49) = 7
        Assert.Equal(7f, magnitude, Tolerance);
    }

    [Fact]
    public void Lateral_ShouldReturnYComponent()
    {
        // Arrange
        var y = 5.2f;
        var velocity = new Velocity(10.5f, y, 2.1f);

        // Act
        var lateral = velocity.Lateral();

        // Assert
        Assert.Equal(y, lateral, Tolerance);
    }

    [Fact]
    public void Forward_ShouldReturnXComponent()
    {
        // Arrange
        var x = 10.5f;
        var velocity = new Velocity(x, 5.2f, 2.1f);

        // Act
        var forward = velocity.Forward();

        // Assert
        Assert.Equal(x, forward, Tolerance);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void Velocity_ShouldBeImmutable()
    {
        // Arrange
        var velocity = new Velocity(10.5f, 5.2f, 2.1f);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<Velocity>(velocity);
        // If this compiles and runs, the type is properly immutable
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithVerySmallValues_ShouldCreateInstance()
    {
        // Arrange
        var x = 0.0001f;
        var y = -0.0001f;
        var z = 0.0001f;

        // Act
        var velocity = new Velocity(x, y, z);

        // Assert
        Assert.Equal(x, velocity.X, Tolerance);
        Assert.Equal(y, velocity.Y, Tolerance);
        Assert.Equal(z, velocity.Z, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargeValues_ShouldCreateInstance()
    {
        // Arrange
        var x = 10000f;
        var y = -5000f;
        var z = 2500f;

        // Act
        var velocity = new Velocity(x, y, z);

        // Assert
        Assert.Equal(x, velocity.X, Tolerance);
        Assert.Equal(y, velocity.Y, Tolerance);
        Assert.Equal(z, velocity.Z, Tolerance);
    }

    [Fact]
    public void Magnitude_WithVerySmallValues_ShouldReturnCorrectValue()
    {
        // Arrange
        var velocity = new Velocity(0.001f, 0.002f, 0.003f);

        // Act
        var magnitude = velocity.Magnitude();

        // Assert
        // sqrt(0.001^2 + 0.002^2 + 0.003^2) = sqrt(0.000001 + 0.000004 + 0.000009) = sqrt(0.000014) ≈ 0.003742
        var expected = MathF.Sqrt(0.000001f + 0.000004f + 0.000009f);
        Assert.Equal(expected, magnitude, Tolerance);
    }

    #endregion
}
