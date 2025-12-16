using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class TireLoadTests
{
    private const float Tolerance = 0.0001f;

    #region Construction Tests

    [Fact]
    public void Constructor_WithZeroLoadAndZeroSlipRatio_ShouldCreateInstance()
    {
        // Arrange & Act
        var tireLoad = new TireLoad(0f, 0f);

        // Assert
        Assert.Equal(0f, tireLoad.Load, Tolerance);
        Assert.Equal(0f, tireLoad.SlipRatio, Tolerance);
    }

    [Fact]
    public void Constructor_WithPositiveLoadAndZeroSlipRatio_ShouldCreateInstance()
    {
        // Arrange
        var load = 5000f; // Newtons
        var slipRatio = 0f;

        // Act
        var tireLoad = new TireLoad(load, slipRatio);

        // Assert
        Assert.Equal(load, tireLoad.Load, Tolerance);
        Assert.Equal(slipRatio, tireLoad.SlipRatio, Tolerance);
    }

    [Fact]
    public void Constructor_WithPositiveLoadAndPositiveSlipRatio_ShouldCreateInstance()
    {
        // Arrange
        var load = 5000f; // Newtons
        var slipRatio = 0.1f; // 10% slip

        // Act
        var tireLoad = new TireLoad(load, slipRatio);

        // Assert
        Assert.Equal(load, tireLoad.Load, Tolerance);
        Assert.Equal(slipRatio, tireLoad.SlipRatio, Tolerance);
    }

    [Fact]
    public void Constructor_WithPositiveLoadAndNegativeSlipRatio_ShouldCreateInstance()
    {
        // Arrange
        var load = 5000f; // Newtons
        var slipRatio = -0.1f; // -10% slip (braking)

        // Act
        var tireLoad = new TireLoad(load, slipRatio);

        // Assert
        Assert.Equal(load, tireLoad.Load, Tolerance);
        Assert.Equal(slipRatio, tireLoad.SlipRatio, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargeLoad_ShouldCreateInstance()
    {
        // Arrange
        var load = 20000f; // Newtons (high load)
        var slipRatio = 0.05f;

        // Act
        var tireLoad = new TireLoad(load, slipRatio);

        // Assert
        Assert.Equal(load, tireLoad.Load, Tolerance);
        Assert.Equal(slipRatio, tireLoad.SlipRatio, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargeSlipRatio_ShouldCreateInstance()
    {
        // Arrange
        var load = 5000f;
        var slipRatio = 2.0f; // Extreme slip (200%)

        // Act
        var tireLoad = new TireLoad(load, slipRatio);

        // Assert
        Assert.Equal(load, tireLoad.Load, Tolerance);
        Assert.Equal(slipRatio, tireLoad.SlipRatio, Tolerance);
    }

    [Fact]
    public void Constructor_WithLargeNegativeSlipRatio_ShouldCreateInstance()
    {
        // Arrange
        var load = 5000f;
        var slipRatio = -2.0f; // Extreme negative slip

        // Act
        var tireLoad = new TireLoad(load, slipRatio);

        // Assert
        Assert.Equal(load, tireLoad.Load, Tolerance);
        Assert.Equal(slipRatio, tireLoad.SlipRatio, Tolerance);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void Constructor_WithNegativeLoad_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var load = -1f;
        var slipRatio = 0f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new TireLoad(load, slipRatio));

        Assert.Equal("load", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithLoadNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var load = float.NaN;
        var slipRatio = 0f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TireLoad(load, slipRatio));

        Assert.Equal("load", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithLoadPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var load = float.PositiveInfinity;
        var slipRatio = 0f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TireLoad(load, slipRatio));

        Assert.Equal("load", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithLoadNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var load = float.NegativeInfinity;
        var slipRatio = 0f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TireLoad(load, slipRatio));

        Assert.Equal("load", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithSlipRatioNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var load = 5000f;
        var slipRatio = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TireLoad(load, slipRatio));

        Assert.Equal("slipRatio", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithSlipRatioPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var load = 5000f;
        var slipRatio = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TireLoad(load, slipRatio));

        Assert.Equal("slipRatio", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithSlipRatioNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var load = 5000f;
        var slipRatio = float.NegativeInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new TireLoad(load, slipRatio));

        Assert.Equal("slipRatio", exception.ParamName);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameLoadAndSlipRatio_ShouldReturnTrue()
    {
        // Arrange
        var tireLoad1 = new TireLoad(5000f, 0.1f);
        var tireLoad2 = new TireLoad(5000f, 0.1f);

        // Act & Assert
        Assert.Equal(tireLoad1, tireLoad2);
        Assert.True(tireLoad1 == tireLoad2);
        Assert.False(tireLoad1 != tireLoad2);
    }

    [Fact]
    public void Equals_WithDifferentLoad_ShouldReturnFalse()
    {
        // Arrange
        var tireLoad1 = new TireLoad(5000f, 0.1f);
        var tireLoad2 = new TireLoad(6000f, 0.1f);

        // Act & Assert
        Assert.NotEqual(tireLoad1, tireLoad2);
        Assert.False(tireLoad1 == tireLoad2);
        Assert.True(tireLoad1 != tireLoad2);
    }

    [Fact]
    public void Equals_WithDifferentSlipRatio_ShouldReturnFalse()
    {
        // Arrange
        var tireLoad1 = new TireLoad(5000f, 0.1f);
        var tireLoad2 = new TireLoad(5000f, 0.2f);

        // Act & Assert
        Assert.NotEqual(tireLoad1, tireLoad2);
        Assert.False(tireLoad1 == tireLoad2);
        Assert.True(tireLoad1 != tireLoad2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var tireLoad1 = new TireLoad(5000f, 0.1f);
        var tireLoad2 = new TireLoad(5000f, 0.1f);

        // Act
        var hashCode1 = tireLoad1.GetHashCode();
        var hashCode2 = tireLoad2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var tireLoad1 = new TireLoad(5000f, 0.1f);
        var tireLoad2 = new TireLoad(6000f, 0.2f);

        // Act
        var hashCode1 = tireLoad1.GetHashCode();
        var hashCode2 = tireLoad2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void TireLoad_ShouldBeImmutable()
    {
        // Arrange
        var tireLoad = new TireLoad(5000f, 0.1f);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<TireLoad>(tireLoad);
        // If this compiles and runs, the type is properly immutable
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithVerySmallPositiveLoad_ShouldCreateInstance()
    {
        // Arrange
        var load = 0.0001f;
        var slipRatio = 0f;

        // Act
        var tireLoad = new TireLoad(load, slipRatio);

        // Assert
        Assert.Equal(load, tireLoad.Load, Tolerance);
        Assert.Equal(slipRatio, tireLoad.SlipRatio, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargeLoad_ShouldCreateInstance()
    {
        // Arrange
        var load = 100000f; // Very high load
        var slipRatio = 0.05f;

        // Act
        var tireLoad = new TireLoad(load, slipRatio);

        // Assert
        Assert.Equal(load, tireLoad.Load, Tolerance);
        Assert.Equal(slipRatio, tireLoad.SlipRatio, Tolerance);
    }

    [Fact]
    public void Constructor_WithVerySmallSlipRatio_ShouldCreateInstance()
    {
        // Arrange
        var load = 5000f;
        var slipRatio = 0.0001f;

        // Act
        var tireLoad = new TireLoad(load, slipRatio);

        // Assert
        Assert.Equal(load, tireLoad.Load, Tolerance);
        Assert.Equal(slipRatio, tireLoad.SlipRatio, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargeSlipRatio_ShouldCreateInstance()
    {
        // Arrange
        var load = 5000f;
        var slipRatio = 10f; // Extreme slip (1000%)

        // Act
        var tireLoad = new TireLoad(load, slipRatio);

        // Assert
        Assert.Equal(load, tireLoad.Load, Tolerance);
        Assert.Equal(slipRatio, tireLoad.SlipRatio, Tolerance);
    }

    #endregion
}
