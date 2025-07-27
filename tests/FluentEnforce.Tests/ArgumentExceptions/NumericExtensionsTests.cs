namespace FluentEnforce.Tests.ArgumentExceptions;

public class NumericExtensionsTests
{
    [Fact]
    public void Zero_WhenValueIsZero_DoesNotThrow()
    {
        // Arrange
        var enforce = Enforce.That(0);

        // Act
        var action = () => enforce.Zero();

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Zero_WhenValueIsNotZero_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(5);

        // Act
        var action = () => enforce.Zero();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Parameter 'Value must be zero'*")
            .And.ParamName.Should().Be("Value must be zero");
    }

    [Fact]
    public void NonZero_WhenValueIsNotZero_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(5);

        // Act
        var result = enforce.NonZero();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void NonZero_WhenValueIsZero_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(0);

        // Act
        var action = () => enforce.NonZero();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Parameter 'Value must be non-zero'*")
            .And.ParamName.Should().Be("Value must be non-zero");
    }

    [Fact]
    public void Positive_WhenValueIsPositive_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(5);

        // Act
        var result = enforce.Positive();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Positive_WhenValueIsZero_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(0);

        // Act
        var result = enforce.Positive();

        // Assert
        // Note: In .NET, INumberBase.IsPositive(0) returns true, so 0 is considered positive
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Positive_WhenValueIsNegative_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(-5);

        // Act
        var action = () => enforce.Positive();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be positive*");
    }

    [Fact]
    public void Negative_WhenValueIsNegative_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(-5);

        // Act
        var result = enforce.Negative();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Negative_WhenValueIsZero_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(0);

        // Act
        var action = () => enforce.Negative();

        // Assert
        // Note: In .NET, INumberBase.IsNegative(0) returns false, so 0 is not considered negative
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be negative*");
    }

    [Fact]
    public void Negative_WhenValueIsPositive_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(5);

        // Act
        var action = () => enforce.Negative();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be negative*");
    }

    [Fact]
    public void GreaterThan_WhenValueIsGreater_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(10);

        // Act
        var result = enforce.GreaterThan(5);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void GreaterThan_WhenValueIsEqual_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(5);

        // Act
        var action = () => enforce.GreaterThan(5);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be greater than 5*");
    }

    [Fact]
    public void GreaterThan_WhenValueIsLess_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(3);

        // Act
        var action = () => enforce.GreaterThan(5);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be greater than 5*");
    }

    [Fact]
    public void GreaterThanOrEqualTo_WhenValueIsGreater_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(10);

        // Act
        var result = enforce.GreaterThanOrEqualTo(5);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void GreaterThanOrEqualTo_WhenValueIsEqual_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(5);

        // Act
        var result = enforce.GreaterThanOrEqualTo(5);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void GreaterThanOrEqualTo_WhenValueIsLess_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(3);

        // Act
        var action = () => enforce.GreaterThanOrEqualTo(5);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be greater than or equal to 5*");
    }

    [Fact]
    public void LessThan_WhenValueIsLess_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(3);

        // Act
        var result = enforce.LessThan(5);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void LessThan_WhenValueIsEqual_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(5);

        // Act
        var action = () => enforce.LessThan(5);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be less than 5*");
    }

    [Fact]
    public void LessThan_WhenValueIsGreater_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(10);

        // Act
        var action = () => enforce.LessThan(5);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be less than 5*");
    }

    [Fact]
    public void LessThanOrEqualTo_WhenValueIsLess_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(3);

        // Act
        var result = enforce.LessThanOrEqualTo(5);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void LessThanOrEqualTo_WhenValueIsEqual_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(5);

        // Act
        var result = enforce.LessThanOrEqualTo(5);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void LessThanOrEqualTo_WhenValueIsGreater_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(10);

        // Act
        var action = () => enforce.LessThanOrEqualTo(5);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be less than or equal to 5*");
    }

    [Fact]
    public void InRange_WhenValueIsInRange_Works()
    {
        // Arrange
        var enforce = Enforce.That(5);

        // Act & Assert - Use combination of GreaterThanOrEqualTo and LessThanOrEqualTo
        enforce.GreaterThanOrEqualTo(1).LessThanOrEqualTo(10).Should().BeSameAs(enforce);
    }

    [Fact]
    public void InRange_WhenValueIsAtMinBoundary_Works()
    {
        // Arrange
        var enforce = Enforce.That(1);

        // Act & Assert
        enforce.GreaterThanOrEqualTo(1).LessThanOrEqualTo(10).Should().BeSameAs(enforce);
    }

    [Fact]
    public void InRange_WhenValueIsAtMaxBoundary_Works()
    {
        // Arrange
        var enforce = Enforce.That(10);

        // Act & Assert
        enforce.GreaterThanOrEqualTo(1).LessThanOrEqualTo(10).Should().BeSameAs(enforce);
    }

    [Fact]
    public void InRange_WhenValueIsBelowRange_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(0);

        // Act
        var action = () => enforce.GreaterThanOrEqualTo(1).LessThanOrEqualTo(10);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be greater than or equal to 1*");
    }

    [Fact]
    public void InRange_WhenValueIsAboveRange_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(11);

        // Act
        var action = () => enforce.GreaterThanOrEqualTo(1).LessThanOrEqualTo(10);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be less than or equal to 10*");
    }

    [Fact]
    public void NumericExtensions_WithDouble_Works()
    {
        // Arrange
        var enforce = Enforce.That(3.14);

        // Act & Assert
        enforce.Positive().GreaterThan(3.0).LessThan(4.0).Should().BeSameAs(enforce);
    }

    [Fact]
    public void NumericExtensions_WithDecimal_Works()
    {
        // Arrange
        var enforce = Enforce.That(123.45m);

        // Act & Assert
        enforce.Positive().GreaterThan(100m).GreaterThanOrEqualTo(100m).LessThanOrEqualTo(200m).Should().BeSameAs(enforce);
    }

    [Fact]
    public void NumericExtensions_WithLong_Works()
    {
        // Arrange
        var enforce = Enforce.That(1000000000L);

        // Act & Assert
        enforce.Positive().GreaterThan(999999999L).Should().BeSameAs(enforce);
    }

    [Fact]
    public void MethodChaining_Works()
    {
        // Arrange
        var enforce = Enforce.That(42);

        // Act
        var result = enforce
            .NonZero()
            .Positive()
            .GreaterThan(10)
            .LessThan(100)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void MethodChaining_ThrowsOnFirstFailedCondition()
    {
        // Arrange
        var enforce = Enforce.That(-5);

        // Act
        var action = () => enforce
            .NonZero()
            .Positive()  // This should fail
            .GreaterThan(-10);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be positive*");
    }

    [Fact]
    public void Zero_FollowedByOtherValidation_CannotChain()
    {
        // Arrange
        var enforce = Enforce.That(0);

        // Act & Assert
        enforce.Zero();  // This returns void
        
        // Can still use the original enforce variable for other validations
        var result = enforce.NonNegative();
        result.Should().BeSameAs(enforce);
    }
}