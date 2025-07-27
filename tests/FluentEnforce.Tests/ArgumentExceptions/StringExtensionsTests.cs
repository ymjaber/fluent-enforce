namespace FluentEnforce.Tests.ArgumentExceptions;

public class StringExtensionsTests
{
    [Fact]
    public void Empty_WhenStringIsEmpty_DoesNotThrow()
    {
        // Arrange
        var enforce = Enforce.That("");

        // Act
        var action = () => enforce.Empty();

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Empty_WhenStringIsNotEmpty_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("not empty");

        // Act
        var action = () => enforce.Empty();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be empty*")
            .And.ParamName.Should().Be("\"not empty\"");
    }

    [Fact]
    public void NotEmpty_WhenStringIsNotEmpty_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("not empty");

        // Act
        var result = enforce.NotEmpty();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void NotEmpty_WhenStringIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("");

        // Act
        var action = () => enforce.NotEmpty();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String cannot be empty*")
            .And.ParamName.Should().Be("\"\"");
    }

    [Fact]
    public void HasLength_WhenLengthMatches_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var result = enforce.HasLength(4);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void HasLength_WhenLengthDoesNotMatch_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var action = () => enforce.HasLength(5);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must have exactly 5 characters*")
            .And.ParamName.Should().Be("\"test\"");
    }

    [Fact]
    public void LongerThan_WhenStringIsLonger_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var result = enforce.LongerThan(3);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void LongerThan_WhenStringIsNotLonger_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var action = () => enforce.LongerThan(4);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be longer than 4 characters*")
            .And.ParamName.Should().Be("\"test\"");
    }

    [Fact]
    public void LongerThan_WhenStringLengthEquals_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var action = () => enforce.LongerThan(4);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be longer than 4 characters*");
    }

    [Fact]
    public void ShorterThan_WhenStringIsShorter_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var result = enforce.ShorterThan(5);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void ShorterThan_WhenStringIsNotShorter_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var action = () => enforce.ShorterThan(4);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be shorter than 4 characters*");
    }

    [Fact]
    public void Contains_WhenStringContainsSubstring_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("hello world");

        // Act
        var result = enforce.Contains("world");

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Contains_WhenStringDoesNotContainSubstring_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("hello world");

        // Act
        var action = () => enforce.Contains("goodbye");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must contain the specified substring*");
    }

    [Fact]
    public void StartsWith_WhenStringStartsWithPrefix_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("hello world");

        // Act
        var result = enforce.StartsWith("hello");

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void StartsWith_WhenStringDoesNotStartWithPrefix_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("hello world");

        // Act
        var action = () => enforce.StartsWith("world");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must start with the specified substring*");
    }

    [Fact]
    public void EndsWith_WhenStringEndsWithSuffix_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("hello world");

        // Act
        var result = enforce.EndsWith("world");

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void EndsWith_WhenStringDoesNotEndWithSuffix_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("hello world");

        // Act
        var action = () => enforce.EndsWith("hello");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must end with the specified substring*");
    }

    [Fact]
    public void Matches_WhenStringMatchesPattern_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("test123");
        var pattern = new System.Text.RegularExpressions.Regex(@"^test\d+$");

        // Act
        var result = enforce.Matches(pattern);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Matches_WhenStringDoesNotMatchPattern_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("test");
        var pattern = new System.Text.RegularExpressions.Regex(@"^\d+$");

        // Act
        var action = () => enforce.Matches(pattern);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String does not match the pattern*");
    }

    [Fact]
    public void MethodChaining_ThrowsOnFirstFailedCondition()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var action = () => enforce
            .NotEmpty()
            .LongerThan(10)  // This should fail
            .Contains("test");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be longer than 10 characters*");
    }

    [Fact]
    public void Empty_FollowedByOtherValidation_CannotChain()
    {
        // Arrange
        var enforce = Enforce.That("");

        // Act & Assert
        enforce.Empty();  // This returns void
        
        // Can still use the original enforce variable for other validations
        var result = enforce.HasLength(0);
        result.Should().BeSameAs(enforce);
    }
}