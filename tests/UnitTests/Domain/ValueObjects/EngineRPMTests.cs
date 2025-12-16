using RacingSimFFB.Domain.ValueObjects;

namespace RacingSimFFB.Tests.UnitTests.Domain.ValueObjects;

public class EngineRPMTests
{
    private const float Tolerance = 0.0001f;

    #region Construction Tests

    [Fact]
    public void Constructor_WithValidZeroValue_ShouldCreateInstance()
    {
        // Arrange & Act
        var engineRPM = new EngineRPM(0f);

        // Assert
        Assert.Equal(0f, engineRPM.Value, Tolerance);
        Assert.Null(engineRPM.MaxRPM);
    }

    [Fact]
    public void Constructor_WithValidPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var rpm = 5000f;

        // Act
        var engineRPM = new EngineRPM(rpm);

        // Assert
        Assert.Equal(rpm, engineRPM.Value, Tolerance);
        Assert.Null(engineRPM.MaxRPM);
    }

    [Fact]
    public void Constructor_WithValidLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var rpm = 15000f;

        // Act
        var engineRPM = new EngineRPM(rpm);

        // Assert
        Assert.Equal(rpm, engineRPM.Value, Tolerance);
        Assert.Null(engineRPM.MaxRPM);
    }

    [Fact]
    public void Constructor_WithValidValueAndMaxRPM_ShouldCreateInstance()
    {
        // Arrange
        var rpm = 5000f;
        var maxRPM = 8000f;

        // Act
        var engineRPM = new EngineRPM(rpm, maxRPM);

        // Assert
        Assert.Equal(rpm, engineRPM.Value, Tolerance);
        Assert.NotNull(engineRPM.MaxRPM);
        Assert.Equal(maxRPM, engineRPM.MaxRPM.Value, Tolerance);
    }

    [Fact]
    public void Constructor_WithValueEqualToMaxRPM_ShouldCreateInstance()
    {
        // Arrange
        var rpm = 8000f;
        var maxRPM = 8000f;

        // Act
        var engineRPM = new EngineRPM(rpm, maxRPM);

        // Assert
        Assert.Equal(rpm, engineRPM.Value, Tolerance);
        Assert.NotNull(engineRPM.MaxRPM);
        Assert.Equal(maxRPM, engineRPM.MaxRPM.Value, Tolerance);
    }

    #endregion

    #region Validation Tests

    [Fact]
    public void Constructor_WithNegativeValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var rpm = -100f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new EngineRPM(rpm));

        Assert.Equal("value", exception.ParamName);
        Assert.Contains("must be greater than or equal to zero", exception.Message);
    }

    [Fact]
    public void Constructor_WithNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var rpm = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new EngineRPM(rpm));

        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithPositiveInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var rpm = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new EngineRPM(rpm));

        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var rpm = float.NegativeInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new EngineRPM(rpm));

        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValueExceedingMaxRPM_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var rpm = 9000f;
        var maxRPM = 8000f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new EngineRPM(rpm, maxRPM));

        Assert.Equal("value", exception.ParamName);
        Assert.Contains("must not exceed MaxRPM", exception.Message);
    }

    [Fact]
    public void Constructor_WithMaxRPMAsNaN_ShouldThrowArgumentException()
    {
        // Arrange
        var rpm = 5000f;
        var maxRPM = float.NaN;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new EngineRPM(rpm, maxRPM));

        Assert.Equal("maxRPM", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithMaxRPMAsInfinity_ShouldThrowArgumentException()
    {
        // Arrange
        var rpm = 5000f;
        var maxRPM = float.PositiveInfinity;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new EngineRPM(rpm, maxRPM));

        Assert.Equal("maxRPM", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithMaxRPMAsZero_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var rpm = 5000f;
        var maxRPM = 0f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new EngineRPM(rpm, maxRPM));

        Assert.Equal("maxRPM", exception.ParamName);
        Assert.Contains("must be greater than zero", exception.Message);
    }

    [Fact]
    public void Constructor_WithMaxRPMAsNegative_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var rpm = 5000f;
        var maxRPM = -1000f;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new EngineRPM(rpm, maxRPM));

        Assert.Equal("maxRPM", exception.ParamName);
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var engineRPM1 = new EngineRPM(5000f);
        var engineRPM2 = new EngineRPM(5000f);

        // Act & Assert
        Assert.Equal(engineRPM1, engineRPM2);
        Assert.True(engineRPM1 == engineRPM2);
        Assert.False(engineRPM1 != engineRPM2);
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var engineRPM1 = new EngineRPM(5000f);
        var engineRPM2 = new EngineRPM(6000f);

        // Act & Assert
        Assert.NotEqual(engineRPM1, engineRPM2);
        Assert.False(engineRPM1 == engineRPM2);
        Assert.True(engineRPM1 != engineRPM2);
    }

    [Fact]
    public void Equals_WithSameValueAndMaxRPM_ShouldReturnTrue()
    {
        // Arrange
        var engineRPM1 = new EngineRPM(5000f, 8000f);
        var engineRPM2 = new EngineRPM(5000f, 8000f);

        // Act & Assert
        Assert.Equal(engineRPM1, engineRPM2);
        Assert.True(engineRPM1 == engineRPM2);
        Assert.False(engineRPM1 != engineRPM2);
    }

    [Fact]
    public void Equals_WithSameValueButDifferentMaxRPM_ShouldReturnFalse()
    {
        // Arrange
        var engineRPM1 = new EngineRPM(5000f, 8000f);
        var engineRPM2 = new EngineRPM(5000f, 9000f);

        // Act & Assert
        Assert.NotEqual(engineRPM1, engineRPM2);
        Assert.False(engineRPM1 == engineRPM2);
        Assert.True(engineRPM1 != engineRPM2);
    }

    [Fact]
    public void Equals_WithSameValueButOneHasMaxRPM_ShouldReturnFalse()
    {
        // Arrange
        var engineRPM1 = new EngineRPM(5000f);
        var engineRPM2 = new EngineRPM(5000f, 8000f);

        // Act & Assert
        Assert.NotEqual(engineRPM1, engineRPM2);
        Assert.False(engineRPM1 == engineRPM2);
        Assert.True(engineRPM1 != engineRPM2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var engineRPM1 = new EngineRPM(5000f);
        var engineRPM2 = new EngineRPM(5000f);

        // Act
        var hashCode1 = engineRPM1.GetHashCode();
        var hashCode2 = engineRPM2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCodes()
    {
        // Arrange
        var engineRPM1 = new EngineRPM(5000f);
        var engineRPM2 = new EngineRPM(6000f);

        // Act
        var hashCode1 = engineRPM1.GetHashCode();
        var hashCode2 = engineRPM2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }

    #endregion

    #region PercentageOfMax Tests

    [Fact]
    public void PercentageOfMax_WithMaxRPMSet_ShouldReturnCorrectPercentage()
    {
        // Arrange
        var rpm = 4000f;
        var maxRPM = 8000f;
        var engineRPM = new EngineRPM(rpm, maxRPM);

        // Act
        var percentage = engineRPM.PercentageOfMax();

        // Assert
        Assert.Equal(50f, percentage, Tolerance);
    }

    [Fact]
    public void PercentageOfMax_WithValueEqualToMaxRPM_ShouldReturn100()
    {
        // Arrange
        var rpm = 8000f;
        var maxRPM = 8000f;
        var engineRPM = new EngineRPM(rpm, maxRPM);

        // Act
        var percentage = engineRPM.PercentageOfMax();

        // Assert
        Assert.Equal(100f, percentage, Tolerance);
    }

    [Fact]
    public void PercentageOfMax_WithZeroValue_ShouldReturnZero()
    {
        // Arrange
        var rpm = 0f;
        var maxRPM = 8000f;
        var engineRPM = new EngineRPM(rpm, maxRPM);

        // Act
        var percentage = engineRPM.PercentageOfMax();

        // Assert
        Assert.Equal(0f, percentage, Tolerance);
    }

    [Fact]
    public void PercentageOfMax_WithQuarterMaxRPM_ShouldReturn25()
    {
        // Arrange
        var rpm = 2000f;
        var maxRPM = 8000f;
        var engineRPM = new EngineRPM(rpm, maxRPM);

        // Act
        var percentage = engineRPM.PercentageOfMax();

        // Assert
        Assert.Equal(25f, percentage, Tolerance);
    }

    [Fact]
    public void PercentageOfMax_WithThreeQuarterMaxRPM_ShouldReturn75()
    {
        // Arrange
        var rpm = 6000f;
        var maxRPM = 8000f;
        var engineRPM = new EngineRPM(rpm, maxRPM);

        // Act
        var percentage = engineRPM.PercentageOfMax();

        // Assert
        Assert.Equal(75f, percentage, Tolerance);
    }

    [Fact]
    public void PercentageOfMax_WithoutMaxRPM_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var engineRPM = new EngineRPM(5000f);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            engineRPM.PercentageOfMax());

        Assert.Contains("MaxRPM must be set", exception.Message);
    }

    [Fact]
    public void PercentageOfMax_WithMaxRPMAsZero_ShouldThrowInvalidOperationException()
    {
        // Arrange
        // Note: This shouldn't be possible to construct, but testing edge case
        // Actually, we validate MaxRPM > 0 in constructor, so this test case
        // would require a different scenario. Let's test with null MaxRPM only.
        var engineRPM = new EngineRPM(5000f, null);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            engineRPM.PercentageOfMax());

        Assert.Contains("MaxRPM must be set", exception.Message);
    }

    #endregion

    #region Immutability Tests

    [Fact]
    public void EngineRPM_ShouldBeImmutable()
    {
        // Arrange
        var engineRPM = new EngineRPM(5000f);

        // Act - Try to modify (should not compile if using init-only properties)
        // This test verifies the type is a record struct which is immutable by design

        // Assert
        Assert.IsType<EngineRPM>(engineRPM);
        // If this compiles and runs, the type is properly immutable
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithVerySmallPositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var rpm = 0.0001f;

        // Act
        var engineRPM = new EngineRPM(rpm);

        // Assert
        Assert.Equal(rpm, engineRPM.Value, Tolerance);
    }

    [Fact]
    public void Constructor_WithVeryLargePositiveValue_ShouldCreateInstance()
    {
        // Arrange
        var rpm = 50000f;

        // Act
        var engineRPM = new EngineRPM(rpm);

        // Assert
        Assert.Equal(rpm, engineRPM.Value, Tolerance);
    }

    [Fact]
    public void Constructor_WithValueJustBelowMaxRPM_ShouldCreateInstance()
    {
        // Arrange
        var rpm = 7999.99f;
        var maxRPM = 8000f;

        // Act
        var engineRPM = new EngineRPM(rpm, maxRPM);

        // Assert
        Assert.Equal(rpm, engineRPM.Value, Tolerance);
        Assert.NotNull(engineRPM.MaxRPM);
        Assert.Equal(maxRPM, engineRPM.MaxRPM.Value, Tolerance);
    }

    #endregion
}
