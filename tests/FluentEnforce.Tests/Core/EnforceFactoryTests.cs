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
        result.Should().BeOfType<Enforce<int>>();
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
    public void NotNull_ThenNotEmpty_WithNonEmptyString_ReturnsEnforceInstance()
    {
        // Arrange
        const string value = "test";

        // Act
        var result = Enforce.NotNull(value).NotEmpty();

        // Assert
        result.Should().BeOfType<Enforce<string>>();
        result.Value.Should().Be(value);
        result.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNull_ThenNotEmpty_WithEmptyString_ThrowsArgumentException()
    {
        // Arrange
        const string value = "";

        // Act
        var action = () => Enforce.NotNull(value).NotEmpty();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String cannot be empty*")
            .And.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNull_WithNullString_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var action = () => Enforce.NotNull(value);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("value")
            .WithMessage("*Value cannot be null*");
    }

    [Fact]
    public void NotNull_ThenNotWhiteSpace_WithNonWhiteSpaceString_ReturnsEnforceInstance()
    {
        // Arrange
        const string value = "test";

        // Act
        var result = Enforce.NotNull(value).NotWhiteSpace();

        // Assert
        result.Should().BeOfType<Enforce<string>>();
        result.Value.Should().Be(value);
        result.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNull_ThenNotWhiteSpace_WithWhiteSpaceString_ThrowsArgumentException()
    {
        // Arrange
        const string value = "   ";

        // Act
        var action = () => Enforce.NotNull(value).NotWhiteSpace();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String cannot be empty or consist only of white-space characters*")
            .And.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNull_ThenNotWhiteSpace_WithEmptyString_ThrowsArgumentException()
    {
        // Arrange
        const string value = "";

        // Act
        var action = () => Enforce.NotNull(value).NotWhiteSpace();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String cannot be empty or consist only of white-space characters*")
            .And.ParamName.Should().Be("value");
    }

    [Fact]
    public void NotNull_ThenNotWhiteSpace_WithTabsAndNewlines_ThrowsArgumentException()
    {
        // Arrange
        const string value = "\t\n\r";

        // Act
        var action = () => Enforce.NotNull(value).NotWhiteSpace();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String cannot be empty or consist only of white-space characters*")
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