namespace FluentEnforce.Tests.Core;

public class EnforceTests
{
    [Fact]
    public void That_WithValue_StoresValue()
    {
        // Arrange
        const int expectedValue = 42;

        // Act
        var enforce = Enforce.That(expectedValue);

        // Assert
        enforce.Value.Should().Be(expectedValue);
        enforce.ParamName.Should().Be("expectedValue");
    }

    [Fact]
    public void That_WithExpression_CapturesExpressionAsParamName()
    {
        // Arrange
        var x = 10;
        var y = 20;

        // Act
        var enforce = Enforce.That(x + y);

        // Assert
        enforce.Value.Should().Be(30);
        enforce.ParamName.Should().Be("x + y");
    }

    [Fact]
    public void ImplicitConversion_FromEnforce_ReturnsValue()
    {
        // Arrange
        const int expectedValue = 42;
        var enforce = Enforce.That(expectedValue);

        // Act
        int actualValue = enforce;

        // Assert
        actualValue.Should().Be(expectedValue);
    }

    [Fact]
    public void ImplicitConversion_WithNullableValue_ReturnsValue()
    {
        // Arrange
        int? expectedValue = 42;
        var enforce = Enforce.That(expectedValue);

        // Act
        int? actualValue = enforce;

        // Assert
        actualValue.Should().Be(expectedValue);
    }

    [Fact]
    public void ImplicitConversion_WithNullValue_ReturnsNull()
    {
        // Arrange
        string? expectedValue = null;
        var enforce = Enforce.That(expectedValue);

        // Act
        string? actualValue = enforce;

        // Assert
        actualValue.Should().BeNull();
    }

    [Fact]
    public void Satisfies_WithPredicate_WhenPredicateTrue_ReturnsEnforce()
    {
        // Arrange
        const int value = 42;
        var enforce = Enforce.That(value);

        // Act
        var result = enforce.Satisfies(x => x > 0);

        // Assert
        result.Should().BeSameAs(enforce);
        result.Value.Should().Be(value);
    }

    [Fact]
    public void Satisfies_WithPredicate_WhenPredicateFalse_ThrowsArgumentException()
    {
        // Arrange
        const int value = -42;
        var enforce = Enforce.That(value);

        // Act
        var action = () => enforce.Satisfies(x => x > 0);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Condition not satisfied*")
            .And.ParamName.Should().Be("value");
    }

    [Fact]
    public void Satisfies_WithCustomMessage_WhenPredicateFalse_ThrowsWithCustomMessage()
    {
        // Arrange
        const int value = -42;
        var enforce = Enforce.That(value);

        // Act
        var action = () => enforce.Satisfies(x => x > 0, "Value must be positive");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value must be positive*")
            .And.ParamName.Should().Be("value");
    }

    [Fact]
    public void Satisfies_WithPredicateAndCustomException_WhenPredicateTrue_ReturnsEnforce()
    {
        // Arrange
        const int value = 42;
        var enforce = Enforce.That(value);

        // Act
        var result = enforce.Satisfies(x => x > 0, () => new InvalidOperationException("Custom message"));

        // Assert
        result.Should().BeSameAs(enforce);
        result.Value.Should().Be(value);
    }

    [Fact]
    public void Satisfies_WithPredicateAndCustomException_WhenPredicateFalse_ThrowsCustomException()
    {
        // Arrange
        const int value = -42;
        var enforce = Enforce.That(value);

        // Act
        var action = () => enforce.Satisfies(x => x > 0, () => new InvalidOperationException("Custom error message"));

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Custom error message");
    }

    [Fact]
    public void Satisfies_WithNullPredicate_ThrowsNullReferenceException()
    {
        // Arrange
        var enforce = Enforce.That(42);

        // Act
        var action = () => enforce.Satisfies(null!);

        // Assert
        action.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void Satisfies_WithCustomException_AndNullPredicate_ThrowsNullReferenceException()
    {
        // Arrange
        var enforce = Enforce.That(42);

        // Act
        var action = () => enforce.Satisfies(null!, () => new InvalidOperationException());

        // Assert
        action.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void Satisfies_WithCustomException_AndNullExceptionFunc_ThrowsNullReferenceException()
    {
        // Arrange
        var enforce = Enforce.That(42);

        // Act
        Func<Exception>? nullFunc = null;
        var action = () => enforce.Satisfies(x => false, nullFunc!);

        // Assert
        action.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void Satisfies_MethodChaining_Works()
    {
        // Arrange
        const int value = 42;
        var enforce = Enforce.That(value);

        // Act
        var result = enforce
            .Satisfies(x => x > 0)
            .Satisfies(x => x < 100)
            .Satisfies(x => x % 2 == 0);

        // Assert
        result.Should().BeSameAs(enforce);
        result.Value.Should().Be(value);
    }

    [Fact]
    public void Satisfies_MethodChaining_ThrowsOnFirstFailedCondition()
    {
        // Arrange
        const int value = 42;
        var enforce = Enforce.That(value);

        // Act
        var action = () => enforce
            .Satisfies(x => x > 0)
            .Satisfies(x => x < 10)  // This should fail
            .Satisfies(x => x % 2 == 0);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Condition not satisfied*");
    }

    [Fact]
    public void Enforce_WithReferenceType_Works()
    {
        // Arrange
        const string value = "test string";

        // Act
        var enforce = Enforce.That(value);

        // Assert
        enforce.Value.Should().Be(value);
        enforce.ParamName.Should().Be("value");
    }

    [Fact]
    public void Enforce_WithCollectionType_Works()
    {
        // Arrange
        var value = new List<int> { 1, 2, 3 };

        // Act
        var enforce = Enforce.That(value);

        // Assert
        enforce.Value.Should().BeEquivalentTo(value);
        enforce.ParamName.Should().Be("value");
    }
}