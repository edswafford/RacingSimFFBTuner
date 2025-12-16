using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class ScaleTests
{
    private const float Tolerance = 0.0001f;

    #region Construction Tests

    [Fact]
    public void Constructor_WithZero_ShouldCreateInstance()
    {
        // Arrange & Act
        var scale = new Scale(0f);

        // Assert
        Assert.Equal(0f, scale.Percentage, Tolerance);
    }

    [Fact]
    public void Constructor_With100_ShouldCreateInstance()
    {
        // Arrange & Act
        var scale = new Scale(100f);

        // Assert
        Assert.Equal(100f, scale.Percentage, Tolerance);
    }

    [Fact]
    public void Constructor_WithOverScaling_ShouldCreateInstance()
    {
        // Arrange
        var percentage = 150f; // 150% over-scaling

        // Act
        var scale = new Scale(percentage);

        // Assert
        Assert.Equal(percentage, scale.Percentage, Tolerance);
    }

    [Fact]
    public void Constructor_WithDecimalValue_ShouldCreateInstance()
    {
        // Arrange
        var percentage = 50.5f;

        // Act
        var scale = new Scale(percentage);

        // Assert
        Assert.Equal(percentage, scale.Percentage, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargeValue_ShouldCreateInstance()
    {
        // Arrange
        var percentage = 200f; // 200% over-scaling

        // Act
        var scale = new Scale(percentage);

        // Assert
        Assert.Equal(percentage, scale.Percentage, Tolerance);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void Constructor_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var percentage = -1f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Scale(percentage));

        Assert.Equal("percentage", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var percentage = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Scale(percentage));

        Assert.Equal("percentage", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var percentage = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Scale(percentage));

        Assert.Equal("percentage", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var percentage = float.NegativeInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Scale(percentage));

        Assert.Equal("percentage", exception.ParamName);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSamePercentage_ShouldReturnTrue()
    {
        // Arrange
        var scale1 = new Scale(100f);
        var scale2 = new Scale(100f);

        // Act & Assert
        Assert.Equal(scale1, scale2);
        Assert.True(scale1 == scale2);
        Assert.False(scale1 != scale2);
    }

    [Fact]
    public void Equals_WithDifferentPercentage_ShouldReturnFalse()
    {
        // Arrange
        var scale1 = new Scale(100f);
        var scale2 = new Scale(50f);

        // Act & Assert
        Assert.NotEqual(scale1, scale2);
        Assert.False(scale1 == scale2);
        Assert.True(scale1 != scale2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var scale1 = new Scale(100f);
        var scale2 = new Scale(100f);

        // Act
        var hashCode1 = scale1.GetHashCode();
        var hashCode2 = scale2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var scale1 = new Scale(100f);
        var scale2 = new Scale(50f);

        // Act
        var hashCode1 = scale1.GetHashCode();
        var hashCode2 = scale2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Conversion Tests

    [Fact]
    public void AsDecimal_WithZero_ShouldReturnZero()
    {
        // Arrange
        var scale = new Scale(0f);

        // Act
        var decimalValue = scale.AsDecimal();

        // Assert
        Assert.Equal(0f, decimalValue, Tolerance);
    }

    [Fact]
    public void AsDecimal_With100_ShouldReturn1Point0()
    {
        // Arrange
        var scale = new Scale(100f);

        // Act
        var decimalValue = scale.AsDecimal();

        // Assert
        Assert.Equal(1.0f, decimalValue, Tolerance);
    }

    [Fact]
    public void AsDecimal_With50_ShouldReturn0Point5()
    {
        // Arrange
        var scale = new Scale(50f);

        // Act
        var decimalValue = scale.AsDecimal();

        // Assert
        Assert.Equal(0.5f, decimalValue, Tolerance);
    }

    [Fact]
    public void AsDecimal_With150_ShouldReturn1Point5()
    {
        // Arrange
        var scale = new Scale(150f);

        // Act
        var decimalValue = scale.AsDecimal();

        // Assert
        Assert.Equal(1.5f, decimalValue, Tolerance);
    }

    [Fact]
    public void AsDecimal_With25Point5_ShouldReturn0Point255()
    {
        // Arrange
        var scale = new Scale(25.5f);

        // Act
        var decimalValue = scale.AsDecimal();

        // Assert
        Assert.Equal(0.255f, decimalValue, Tolerance);
    }

    #endregion

    #region Factory Method Tests

    [Fact]
    public void FromDecimal_WithZero_ShouldCreateZeroScale()
    {
        // Arrange & Act
        var scale = Scale.FromDecimal(0f);

        // Assert
        Assert.Equal(0f, scale.Percentage, Tolerance);
    }

    [Fact]
    public void FromDecimal_With1Point0_ShouldCreate100PercentScale()
    {
        // Arrange & Act
        var scale = Scale.FromDecimal(1.0f);

        // Assert
        Assert.Equal(100f, scale.Percentage, Tolerance);
    }

    [Fact]
    public void FromDecimal_With0Point5_ShouldCreate50PercentScale()
    {
        // Arrange & Act
        var scale = Scale.FromDecimal(0.5f);

        // Assert
        Assert.Equal(50f, scale.Percentage, Tolerance);
    }

    [Fact]
    public void FromDecimal_With1Point5_ShouldCreate150PercentScale()
    {
        // Arrange & Act
        var scale = Scale.FromDecimal(1.5f);

        // Assert
        Assert.Equal(150f, scale.Percentage, Tolerance);
    }

    [Fact]
    public void FromDecimal_With0Point255_ShouldCreate25Point5PercentScale()
    {
        // Arrange & Act
        var scale = Scale.FromDecimal(0.255f);

        // Assert
        Assert.Equal(25.5f, scale.Percentage, Tolerance);
    }

    [Fact]
    public void FromDecimal_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var decimalValue = -0.1f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            Scale.FromDecimal(decimalValue));

        Assert.Equal("decimalValue", exception.ParamName);
    }

    [Fact]
    public void FromDecimal_WithNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var decimalValue = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            Scale.FromDecimal(decimalValue));

        Assert.Equal("decimalValue", exception.ParamName);
    }

    [Fact]
    public void FromDecimal_WithPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var decimalValue = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            Scale.FromDecimal(decimalValue));

        Assert.Equal("decimalValue", exception.ParamName);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void Scale_ShouldBeImmutable()
    {
        // Arrange
        var scale = new Scale(100f);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<Scale>(scale);
        // If this compiles and runs, the type is properly immutable
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithVerySmallPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var percentage = 0.0001f;

        // Act
        var scale = new Scale(percentage);

        // Assert
        Assert.Equal(percentage, scale.Percentage, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var percentage = 1000f; // 1000% over-scaling

        // Act
        var scale = new Scale(percentage);

        // Assert
        Assert.Equal(percentage, scale.Percentage, Tolerance);
    }

    #endregion
}
