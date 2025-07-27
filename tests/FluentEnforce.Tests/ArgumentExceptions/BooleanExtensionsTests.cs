namespace FluentEnforce.Tests.ArgumentExceptions;

public class BooleanExtensionsTests
{
    [Fact]
    public void True_WhenValueIsTrue_DoesNotThrow()
    {
        // Arrange
        var enforce = Enforce.That(true);

        // Act
        var action = () => enforce.True();

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void True_WhenValueIsFalse_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That(false);

        // Act
        var action = () => enforce.True();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value must be true*")
            .And.ParamName.Should().Be("false");
    }

    [Fact]
    public void False_WhenValueIsFalse_DoesNotThrow()
    {
        // Arrange
        var enforce = Enforce.That(false);

        // Act
        var action = () => enforce.False();

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void False_WhenValueIsTrue_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That(true);

        // Act
        var action = () => enforce.False();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value must be false*")
            .And.ParamName.Should().Be("true");
    }

    [Fact]
    public void BooleanExtensions_CannotBeChainedWithTrue()
    {
        // Arrange
        var enforce = Enforce.That(true);

        // Act & Assert
        enforce.True(); // Returns void
        
        // Can still use satisfies for additional validation
        var result = enforce.Satisfies(x => x == true);
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void BooleanExtensions_CannotBeChainedWithFalse()
    {
        // Arrange
        var enforce = Enforce.That(false);

        // Act & Assert
        enforce.False(); // Returns void
        
        // Can still use satisfies for additional validation
        var result = enforce.Satisfies(x => x == false);
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void BooleanExtensions_WithNullableBoolean_True_Works()
    {
        // Arrange
        bool? value = true;
        var enforce = Enforce.That(value);

        // Act & Assert
        enforce.Satisfies(x => x.HasValue && x.Value).Should().BeSameAs(enforce);
    }

    [Fact]
    public void BooleanExtensions_WithNullableBoolean_False_Works()
    {
        // Arrange
        bool? value = false;
        var enforce = Enforce.That(value);

        // Act & Assert
        enforce.Satisfies(x => x.HasValue && !x.Value).Should().BeSameAs(enforce);
    }

    [Fact]
    public void BooleanExtensions_WithNullableBoolean_Null_Works()
    {
        // Arrange
        bool? value = null;
        var enforce = Enforce.That(value);

        // Act & Assert
        enforce.Satisfies(x => !x.HasValue).Should().BeSameAs(enforce);
    }

    [Fact]
    public void True_WithExpressionAsParameter_PreservesParameterName()
    {
        // Arrange
        bool condition = false;
        var result = false;

        // Act & Assert
        var action = () => Enforce.That(condition && result).True();
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value must be true*")
            .And.ParamName.Should().Be("condition && result");
    }

    [Fact]
    public void False_WithExpressionAsParameter_PreservesParameterName()
    {
        // Arrange
        bool condition = true;
        var result = true;

        // Act & Assert
        var action = () => Enforce.That(condition || result).False();
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value must be false*")
            .And.ParamName.Should().Be("condition || result");
    }
}