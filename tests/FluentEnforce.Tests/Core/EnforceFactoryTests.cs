namespace FluentEnforce.Tests.Core;

public class EnforceFactoryTests
{
    [Fact]
    public void That_WithValue_ReturnsEnforceInstance()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = Enforce.That(value);

        // Assert
        result.Should().BeOfType<Enforce<int>>();
        result.Value.Should().Be(value);
        result.ParamName.Should().Be("value");
    }

    [Fact]
    public void That_WithDifferentParameterName_CapturesCorrectName()
    {
        // Arrange
        const string myCustomParameter = "test value";

        // Act
        var result = Enforce.That(myCustomParameter);

        // Assert
        result.Should().BeOfType<Enforce<string>>();
        result.Value.Should().Be(myCustomParameter);
        result.ParamName.Should().Be("myCustomParameter");
    }

    [Fact]
    public void That_WithExpressionResult_CapturesExpression()
    {
        // Arrange
        const int x = 10;
        const int y = 20;

        // Act
        var result = Enforce.That(x + y);

        // Assert
        result.Value.Should().Be(30);
        result.ParamName.Should().Be("x + y");
    }

    [Fact]
    public void That_WithMethodCallResult_CapturesMethodCall()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        var result = Enforce.That(list.Count);

        // Assert
        result.Value.Should().Be(3);
        result.ParamName.Should().Be("list.Count");
    }

    [Fact]
    public void Null_WithNullValue_DoesNotThrow()
    {
        // Arrange
        string? value = null;

        // Act
        var action = () => Enforce.Null(value);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Null_WithNonNullValue_ThrowsArgumentException()
    {
        // Arrange
        string value = "not null";

        // Act
        var action = () => Enforce.Null(value);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value must be null*")
            .And.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNull_WithNonNullValue_ReturnsEnforceInstance()
    {
        // Arrange
        const string value = "test";

        // Act
        var result = Enforce.NotNull(value);

        // Assert
        result.Should().BeOfType<Enforce<string>>();
        result.Value.Should().Be(value);
        result.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNull_WithNullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var action = () => Enforce.NotNull(value);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("value");
    }

    [Fact]
    public void NotNull_WithNullableValueType_NonNull_ReturnsEnforceInstance()
    {
        // Arrange
        int? value = 42;

        // Act
        var result = Enforce.NotNull(value);

        // Assert
        result.Should().BeOfType<Enforce<int?>>();
        result.Value.Should().Be(42);
        result.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNull_WithNullableValueType_Null_ThrowsArgumentNullException()
    {
        // Arrange
        int? value = null;

        // Act
        var action = () => Enforce.NotNull(value);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("value");
    }

    [Fact]
    public void NotNullOrEmpty_WithNonEmptyString_ReturnsEnforceInstance()
    {
        // Arrange
        const string value = "test";

        // Act
        var result = Enforce.NotNullOrEmpty(value);

        // Assert
        result.Should().BeOfType<Enforce<string>>();
        result.Value.Should().Be(value);
        result.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNullOrEmpty_WithEmptyString_ThrowsArgumentException()
    {
        // Arrange
        const string value = "";

        // Act
        var action = () => Enforce.NotNullOrEmpty(value);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.*")
            .And.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNullOrEmpty_WithNullString_ThrowsArgumentException()
    {
        // Arrange
        string? value = null;

        // Act
        var action = () => Enforce.NotNullOrEmpty(value);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithParameterName("value")
            .WithMessage("*Value cannot be null or empty*");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WithNonWhiteSpaceString_ReturnsEnforceInstance()
    {
        // Arrange
        const string value = "test";

        // Act
        var result = Enforce.NotNullOrWhiteSpace(value);

        // Assert
        result.Should().BeOfType<Enforce<string>>();
        result.Value.Should().Be(value);
        result.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WithWhiteSpaceString_ThrowsArgumentException()
    {
        // Arrange
        const string value = "   ";

        // Act
        var action = () => Enforce.NotNullOrWhiteSpace(value);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or whitespace.*")
            .And.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WithEmptyString_ThrowsArgumentException()
    {
        // Arrange
        const string value = "";

        // Act
        var action = () => Enforce.NotNullOrWhiteSpace(value);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or whitespace.*")
            .And.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WithNullString_ThrowsArgumentException()
    {
        // Arrange
        string? value = null;

        // Act
        var action = () => Enforce.NotNullOrWhiteSpace(value);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithParameterName("value")
            .WithMessage("*Value cannot be null or whitespace*");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WithTabsAndNewlines_ThrowsArgumentException()
    {
        // Arrange
        const string value = "\t\n\r";

        // Act
        var action = () => Enforce.NotNullOrWhiteSpace(value);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or whitespace.*")
            .And.ParamName.Should().Be("value");
    }

    [Fact]
    public void That_CanChainWithExtensionMethods()
    {
        // Arrange
        const int value = 42;

        // Act & Assert
        var result = Enforce.That(value)
            .Satisfies(x => x > 0)
            .Satisfies(x => x < 100);

        result.Value.Should().Be(value);
    }

    [Fact]
    public void NotNull_CanChainWithExtensionMethods()
    {
        // Arrange
        const string value = "test";

        // Act & Assert
        var result = Enforce.NotNull(value)
            .Satisfies(x => x.Length > 0);

        result.Value.Should().Be(value);
    }
}