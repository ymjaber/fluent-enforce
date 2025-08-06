namespace FluentEnforce.Tests.Core;

public class EnforceExtensionsTests
{
    #region Enforce Extension Tests

    [Fact]
    public void Enforce_CreatesEnforceInstance()
    {
        // Arrange
        var value = 42;

        // Act
        var result = value.Enforce();

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Enforce_WithStringValue_CreatesEnforceInstance()
    {
        // Arrange
        var value = "test";

        // Act
        var result = value.Enforce();

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be("test");
    }

    [Fact]
    public void Enforce_AllowsMethodChaining()
    {
        // Arrange
        var age = 25;

        // Act & Assert
        age.Enforce()
            .GreaterThan(18)
            .LessThan(100);
    }

    #endregion

    #region EnforceNotNull Extension Tests

    [Fact]
    public void EnforceNotNull_WithNonNullValue_ReturnsEnforceInstance()
    {
        // Arrange
        string? value = "test";

        // Act
        var result = value.EnforceNotNull();

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be("test");
    }

    [Fact]
    public void EnforceNotNull_WithNullValue_ThrowsArgumentNullException()
    {
        // Arrange
        string? value = null;

        // Act
        var action = () => value.EnforceNotNull();

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null.*");
    }

    [Fact]
    public void EnforceNotNull_WithCustomMessage_ThrowsWithCustomMessage()
    {
        // Arrange
        object? value = null;

        // Act
        var action = () => value.EnforceNotNull("Custom null message");

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithMessage("Custom null message*");
    }

    [Fact]
    public void EnforceNotNull_PreservesNullSafety()
    {
        // This test verifies that the compiler recognizes the value as non-null after EnforceNotNull
        string? nullableString = "test";
        
        // After EnforceNotNull, the compiler should know this is not null
        var result = nullableString.EnforceNotNull();
        
        // This should compile without null warnings
        var length = result.Value.Length;
        length.Should().Be(4);
    }

    #endregion

    #region Method Chaining Tests (NotEmpty and NotWhiteSpace)

    [Fact]
    public void EnforceNotNull_NotEmpty_WithValidString_ReturnsEnforceInstance()
    {
        // Arrange
        string? value = "test";

        // Act
        var result = value.EnforceNotNull().NotEmpty();

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be("test");
    }

    [Fact]
    public void EnforceNotNull_NotEmpty_WithEmptyString_ThrowsArgumentException()
    {
        // Arrange
        string? value = "";

        // Act
        var action = () => value.EnforceNotNull().NotEmpty();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String cannot be empty*");
    }

    [Fact]
    public void EnforceNotNull_NotEmpty_AllowsMethodChaining()
    {
        // Arrange
        var email = "test@example.com";

        // Act & Assert
        email.EnforceNotNull()
            .NotEmpty()
            .Contains("@")
            .ShorterThan(100);
    }

    [Fact]
    public void EnforceNotNull_NotWhiteSpace_WithValidString_ReturnsEnforceInstance()
    {
        // Arrange
        string? value = "test";

        // Act
        var result = value.EnforceNotNull().NotWhiteSpace();

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be("test");
    }

    [Fact]
    public void EnforceNotNull_NotWhiteSpace_WithWhitespace_ThrowsArgumentException()
    {
        // Arrange
        string? value = "   ";

        // Act
        var action = () => value.EnforceNotNull().NotWhiteSpace();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String cannot be empty or consist only of white-space characters*");
    }

    [Fact]
    public void EnforceNotNull_NotWhiteSpace_WithEmptyString_ThrowsArgumentException()
    {
        // Arrange
        string? value = "";

        // Act
        var action = () => value.EnforceNotNull().NotWhiteSpace();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String cannot be empty or consist only of white-space characters*");
    }

    [Fact]
    public void EnforceNotNull_NotWhiteSpace_WithRealWorldExample()
    {
        // Arrange
        var email = "user@example.com";

        // Act & Assert
        var validEmail = email.EnforceNotNull()
            .NotWhiteSpace()
            .MatchesEmail();
        
        validEmail.Value.Should().Be("user@example.com");
    }

    #endregion

    #region Null Extension Tests

    [Fact]
    public void Null_WithNullValue_DoesNotThrow()
    {
        // Arrange
        string? value = null;
        var enforce = Enforce.That(value);

        // Act
        var action = () => enforce.Null();

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Null_WithNonNullValue_ThrowsArgumentException()
    {
        // Arrange
        string? value = "not null";
        var enforce = Enforce.That(value);

        // Act
        var action = () => enforce.Null();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value must be null*");
    }

    [Fact]
    public void Null_WithCustomMessage_ThrowsWithCustomMessage()
    {
        // Arrange
        object? value = new object();
        var enforce = Enforce.That(value);

        // Act
        var action = () => enforce.Null("Expected null value");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Expected null value*");
    }

    #endregion
}