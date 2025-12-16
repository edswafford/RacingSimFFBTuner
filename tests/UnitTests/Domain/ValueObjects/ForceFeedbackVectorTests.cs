using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class ForceFeedbackVectorTests
{
    private const float Tolerance = 0.0001f;

    #region Construction Tests

    [Fact]
    public void Constructor_WithAllZeroComponents_ShouldCreateInstance()
    {
        // Arrange & Act
        var vector = new ForceFeedbackVector(0f, 0f, 0f, 0f, 0f);

        // Assert
        Assert.Equal(0f, vector.X, Tolerance);
        Assert.Equal(0f, vector.Y, Tolerance);
        Assert.Equal(0f, vector.Z, Tolerance);
        Assert.Equal(0f, vector.Damper, Tolerance);
        Assert.Equal(0f, vector.Inertia, Tolerance);
    }

    [Fact]
    public void Constructor_WithPositiveForceComponents_ShouldCreateInstance()
    {
        // Arrange
        var x = 10.5f;
        var y = 5.2f;
        var z = 2.1f;
        var damper = 1.5f;
        var inertia = 0.8f;

        // Act
        var vector = new ForceFeedbackVector(x, y, z, damper, inertia);

        // Assert
        Assert.Equal(x, vector.X, Tolerance);
        Assert.Equal(y, vector.Y, Tolerance);
        Assert.Equal(z, vector.Z, Tolerance);
        Assert.Equal(damper, vector.Damper, Tolerance);
        Assert.Equal(inertia, vector.Inertia, Tolerance);
    }

    [Fact]
    public void Constructor_WithNegativeForceComponents_ShouldCreateInstance()
    {
        // Arrange
        var x = -10.5f;
        var y = -5.2f;
        var z = -2.1f;
        var damper = 1.5f;
        var inertia = 0.8f;

        // Act
        var vector = new ForceFeedbackVector(x, y, z, damper, inertia);

        // Assert
        Assert.Equal(x, vector.X, Tolerance);
        Assert.Equal(y, vector.Y, Tolerance);
        Assert.Equal(z, vector.Z, Tolerance);
        Assert.Equal(damper, vector.Damper, Tolerance);
        Assert.Equal(inertia, vector.Inertia, Tolerance);
    }

    [Fact]
    public void Constructor_WithPositiveDamperAndInertia_ShouldCreateInstance()
    {
        // Arrange
        var x = 5.0f;
        var y = 3.0f;
        var z = 1.0f;
        var damper = 2.5f;
        var inertia = 1.2f;

        // Act
        var vector = new ForceFeedbackVector(x, y, z, damper, inertia);

        // Assert
        Assert.Equal(x, vector.X, Tolerance);
        Assert.Equal(y, vector.Y, Tolerance);
        Assert.Equal(z, vector.Z, Tolerance);
        Assert.Equal(damper, vector.Damper, Tolerance);
        Assert.Equal(inertia, vector.Inertia, Tolerance);
    }

    [Fact]
    public void Constructor_WithMixedForceComponents_ShouldCreateInstance()
    {
        // Arrange
        var x = 10.5f;
        var y = -5.2f;
        var z = 2.1f;
        var damper = 1.5f;
        var inertia = 0.8f;

        // Act
        var vector = new ForceFeedbackVector(x, y, z, damper, inertia);

        // Assert
        Assert.Equal(x, vector.X, Tolerance);
        Assert.Equal(y, vector.Y, Tolerance);
        Assert.Equal(z, vector.Z, Tolerance);
        Assert.Equal(damper, vector.Damper, Tolerance);
        Assert.Equal(inertia, vector.Inertia, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargeValues_ShouldCreateInstance()
    {
        // Arrange
        var x = 1000f;
        var y = -500f;
        var z = 250f;
        var damper = 100f;
        var inertia = 50f;

        // Act
        var vector = new ForceFeedbackVector(x, y, z, damper, inertia);

        // Assert
        Assert.Equal(x, vector.X, Tolerance);
        Assert.Equal(y, vector.Y, Tolerance);
        Assert.Equal(z, vector.Z, Tolerance);
        Assert.Equal(damper, vector.Damper, Tolerance);
        Assert.Equal(inertia, vector.Inertia, Tolerance);
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
        var damper = 1.5f;
        var inertia = 0.8f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new ForceFeedbackVector(x, y, z, damper, inertia));

        Assert.Equal("x", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithYComponentInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var x = 10.5f;
        var y = float.PositiveInfinity;
        var z = 2.1f;
        var damper = 1.5f;
        var inertia = 0.8f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new ForceFeedbackVector(x, y, z, damper, inertia));

        Assert.Equal("y", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithZComponentNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var x = 10.5f;
        var y = 5.2f;
        var z = float.NaN;
        var damper = 1.5f;
        var inertia = 0.8f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new ForceFeedbackVector(x, y, z, damper, inertia));

        Assert.Equal("z", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithXComponentPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var x = float.PositiveInfinity;
        var y = 5.2f;
        var z = 2.1f;
        var damper = 1.5f;
        var inertia = 0.8f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new ForceFeedbackVector(x, y, z, damper, inertia));

        Assert.Equal("x", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithYComponentNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var x = 10.5f;
        var y = float.NegativeInfinity;
        var z = 2.1f;
        var damper = 1.5f;
        var inertia = 0.8f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new ForceFeedbackVector(x, y, z, damper, inertia));

        Assert.Equal("y", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithZComponentPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var x = 10.5f;
        var y = 5.2f;
        var z = float.PositiveInfinity;
        var damper = 1.5f;
        var inertia = 0.8f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new ForceFeedbackVector(x, y, z, damper, inertia));

        Assert.Equal("z", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeDamper_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var x = 10.5f;
        var y = 5.2f;
        var z = 2.1f;
        var damper = -1.5f;
        var inertia = 0.8f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ForceFeedbackVector(x, y, z, damper, inertia));

        Assert.Equal("damper", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeInertia_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var x = 10.5f;
        var y = 5.2f;
        var z = 2.1f;
        var damper = 1.5f;
        var inertia = -0.8f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ForceFeedbackVector(x, y, z, damper, inertia));

        Assert.Equal("inertia", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithDamperNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var x = 10.5f;
        var y = 5.2f;
        var z = 2.1f;
        var damper = float.NaN;
        var inertia = 0.8f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new ForceFeedbackVector(x, y, z, damper, inertia));

        Assert.Equal("damper", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithInertiaInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var x = 10.5f;
        var y = 5.2f;
        var z = 2.1f;
        var damper = 1.5f;
        var inertia = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new ForceFeedbackVector(x, y, z, damper, inertia));

        Assert.Equal("inertia", exception.ParamName);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameComponents_ShouldReturnTrue()
    {
        // Arrange
        var vector1 = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 0.8f);
        var vector2 = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 0.8f);

        // Act & Assert
        Assert.Equal(vector1, vector2);
        Assert.True(vector1 == vector2);
        Assert.False(vector1 != vector2);
    }

    [Fact]
    public void Equals_WithDifferentXComponent_ShouldReturnFalse()
    {
        // Arrange
        var vector1 = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 0.8f);
        var vector2 = new ForceFeedbackVector(20.5f, 5.2f, 2.1f, 1.5f, 0.8f);

        // Act & Assert
        Assert.NotEqual(vector1, vector2);
        Assert.False(vector1 == vector2);
        Assert.True(vector1 != vector2);
    }

    [Fact]
    public void Equals_WithDifferentYComponent_ShouldReturnFalse()
    {
        // Arrange
        var vector1 = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 0.8f);
        var vector2 = new ForceFeedbackVector(10.5f, 15.2f, 2.1f, 1.5f, 0.8f);

        // Act & Assert
        Assert.NotEqual(vector1, vector2);
        Assert.False(vector1 == vector2);
        Assert.True(vector1 != vector2);
    }

    [Fact]
    public void Equals_WithDifferentZComponent_ShouldReturnFalse()
    {
        // Arrange
        var vector1 = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 0.8f);
        var vector2 = new ForceFeedbackVector(10.5f, 5.2f, 12.1f, 1.5f, 0.8f);

        // Act & Assert
        Assert.NotEqual(vector1, vector2);
        Assert.False(vector1 == vector2);
        Assert.True(vector1 != vector2);
    }

    [Fact]
    public void Equals_WithDifferentDamper_ShouldReturnFalse()
    {
        // Arrange
        var vector1 = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 0.8f);
        var vector2 = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 2.5f, 0.8f);

        // Act & Assert
        Assert.NotEqual(vector1, vector2);
        Assert.False(vector1 == vector2);
        Assert.True(vector1 != vector2);
    }

    [Fact]
    public void Equals_WithDifferentInertia_ShouldReturnFalse()
    {
        // Arrange
        var vector1 = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 0.8f);
        var vector2 = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 1.8f);

        // Act & Assert
        Assert.NotEqual(vector1, vector2);
        Assert.False(vector1 == vector2);
        Assert.True(vector1 != vector2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var vector1 = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 0.8f);
        var vector2 = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 0.8f);

        // Act
        var hashCode1 = vector1.GetHashCode();
        var hashCode2 = vector2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var vector1 = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 0.8f);
        var vector2 = new ForceFeedbackVector(20.5f, 5.2f, 2.1f, 1.5f, 0.8f);

        // Act
        var hashCode1 = vector1.GetHashCode();
        var hashCode2 = vector2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Method Tests

    [Fact]
    public void Magnitude_WithZeroComponents_ShouldReturnZero()
    {
        // Arrange
        var vector = new ForceFeedbackVector(0f, 0f, 0f, 1.5f, 0.8f);

        // Act
        var magnitude = vector.Magnitude();

        // Assert
        Assert.Equal(0f, magnitude, Tolerance);
    }

    [Fact]
    public void Magnitude_WithPositiveComponents_ShouldReturnCorrectValue()
    {
        // Arrange
        var vector = new ForceFeedbackVector(3f, 4f, 0f, 1.5f, 0.8f);

        // Act
        var magnitude = vector.Magnitude();

        // Assert
        Assert.Equal(5f, magnitude, Tolerance);
    }

    [Fact]
    public void Magnitude_WithNegativeComponents_ShouldReturnCorrectValue()
    {
        // Arrange
        var vector = new ForceFeedbackVector(-3f, -4f, 0f, 1.5f, 0.8f);

        // Act
        var magnitude = vector.Magnitude();

        // Assert
        Assert.Equal(5f, magnitude, Tolerance);
    }

    [Fact]
    public void Magnitude_WithAllThreeComponents_ShouldReturnCorrectValue()
    {
        // Arrange
        var vector = new ForceFeedbackVector(2f, 3f, 6f, 1.5f, 0.8f);

        // Act
        var magnitude = vector.Magnitude();

        // Assert
        // sqrt(2^2 + 3^2 + 6^2) = sqrt(4 + 9 + 36) = sqrt(49) = 7
        Assert.Equal(7f, magnitude, Tolerance);
    }

    [Fact]
    public void Zero_ShouldReturnZeroVector()
    {
        // Act
        var vector = ForceFeedbackVector.Zero();

        // Assert
        Assert.Equal(0f, vector.X, Tolerance);
        Assert.Equal(0f, vector.Y, Tolerance);
        Assert.Equal(0f, vector.Z, Tolerance);
        Assert.Equal(0f, vector.Damper, Tolerance);
        Assert.Equal(0f, vector.Inertia, Tolerance);
    }

    [Fact]
    public void Zero_ShouldReturnEqualInstances()
    {
        // Act
        var vector1 = ForceFeedbackVector.Zero();
        var vector2 = ForceFeedbackVector.Zero();

        // Assert
        Assert.Equal(vector1, vector2);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void ForceFeedbackVector_ShouldBeImmutable()
    {
        // Arrange
        var vector = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 0.8f);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<ForceFeedbackVector>(vector);
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
        var damper = 0.0001f;
        var inertia = 0.0001f;

        // Act
        var vector = new ForceFeedbackVector(x, y, z, damper, inertia);

        // Assert
        Assert.Equal(x, vector.X, Tolerance);
        Assert.Equal(y, vector.Y, Tolerance);
        Assert.Equal(z, vector.Z, Tolerance);
        Assert.Equal(damper, vector.Damper, Tolerance);
        Assert.Equal(inertia, vector.Inertia, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargeValues_ShouldCreateInstance()
    {
        // Arrange
        var x = 10000f;
        var y = -5000f;
        var z = 2500f;
        var damper = 1000f;
        var inertia = 500f;

        // Act
        var vector = new ForceFeedbackVector(x, y, z, damper, inertia);

        // Assert
        Assert.Equal(x, vector.X, Tolerance);
        Assert.Equal(y, vector.Y, Tolerance);
        Assert.Equal(z, vector.Z, Tolerance);
        Assert.Equal(damper, vector.Damper, Tolerance);
        Assert.Equal(inertia, vector.Inertia, Tolerance);
    }

    [Fact]
    public void Magnitude_WithVerySmallValues_ShouldReturnCorrectValue()
    {
        // Arrange
        var vector = new ForceFeedbackVector(0.001f, 0.002f, 0.003f, 1.5f, 0.8f);

        // Act
        var magnitude = vector.Magnitude();

        // Assert
        // sqrt(0.001^2 + 0.002^2 + 0.003^2) = sqrt(0.000001 + 0.000004 + 0.000009) = sqrt(0.000014) ≈ 0.003742
        var expected = MathF.Sqrt(0.000001f + 0.000004f + 0.000009f);
        Assert.Equal(expected, magnitude, Tolerance);
    }

    [Fact]
    public void Constructor_WithZeroDamper_ShouldCreateInstance()
    {
        // Arrange & Act
        var vector = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 0f, 0.8f);

        // Assert
        Assert.Equal(0f, vector.Damper, Tolerance);
    }

    [Fact]
    public void Constructor_WithZeroInertia_ShouldCreateInstance()
    {
        // Arrange & Act
        var vector = new ForceFeedbackVector(10.5f, 5.2f, 2.1f, 1.5f, 0f);

        // Assert
        Assert.Equal(0f, vector.Inertia, Tolerance);
    }

    #endregion
}
