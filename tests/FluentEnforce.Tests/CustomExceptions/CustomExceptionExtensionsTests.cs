namespace FluentEnforce.Tests.CustomExceptions;

public class CustomExceptionExtensionsTests
{
    private class TestException : Exception
    {
        public TestException(string message) : base(message) { }
    }

    [Fact]
    public void NotEmpty_WithCustomException_WhenStringIsNotEmpty_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("not empty");

        // Act
        var result = enforce.NotEmpty(() => new TestException("Custom error"));

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void NotEmpty_WithCustomException_WhenStringIsEmpty_ThrowsCustomException()
    {
        // Arrange
        var enforce = Enforce.That("");

        // Act
        var action = () => enforce.NotEmpty(() => new TestException("String cannot be empty!"));

        // Assert
        action.Should().Throw<TestException>()
            .WithMessage("String cannot be empty!");
    }

    [Fact]
    public void Empty_WithCustomException_WhenStringIsEmpty_DoesNotThrow()
    {
        // Arrange
        var enforce = Enforce.That("");

        // Act
        var action = () => enforce.Empty(() => new TestException("String must be empty!"));

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Empty_WithCustomException_WhenStringIsNotEmpty_ThrowsCustomException()
    {
        // Arrange
        var enforce = Enforce.That("not empty");

        // Act
        var action = () => enforce.Empty(() => new TestException("String must be empty!"));

        // Assert
        action.Should().Throw<TestException>()
            .WithMessage("String must be empty!");
    }

    [Fact]
    public void Positive_WithCustomException_WhenValueIsPositive_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(42);

        // Act
        var result = enforce.Positive(() => new InvalidOperationException("Value must be positive"));

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Positive_WithCustomException_WhenValueIsNotPositive_ThrowsCustomException()
    {
        // Arrange
        var enforce = Enforce.That(-5);

        // Act
        var action = () => enforce.Positive(() => new InvalidOperationException("Value must be positive"));

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Value must be positive");
    }

    [Fact]
    public void NotEmpty_Guid_WithCustomException_WhenGuidIsNotEmpty_ReturnsEnforce()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var enforce = Enforce.That(guid);

        // Act
        var result = enforce.NotEmpty(() => new TestException("Guid is required"));

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void NotEmpty_Guid_WithCustomException_WhenGuidIsEmpty_ThrowsCustomException()
    {
        // Arrange
        var guid = Guid.Empty;
        var enforce = Enforce.That(guid);

        // Act
        var action = () => enforce.NotEmpty(() => new TestException("Guid is required"));

        // Assert
        action.Should().Throw<TestException>()
            .WithMessage("Guid is required");
    }

    [Fact]
    public void CustomException_WithNullExceptionFunc_ThrowsNullReferenceException()
    {
        // Arrange
        var enforce = Enforce.That("");

        // Act
        Func<Exception>? nullFunc = null;
        var action = () => enforce.NotEmpty(nullFunc!);

        // Assert
        action.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void MethodChaining_ThrowsCorrectExceptionType()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var action = () => enforce
            .NotEmpty()  // Passes
            .LongerThan(10, () => new InvalidOperationException("String too short"));  // Should throw custom exception

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("String too short");
    }

    [Fact]
    public void AllExtensionMethods_HaveCustomExceptionOverloads()
    {
        // This test demonstrates that extension methods have custom exception overloads
        // Arrange
        var stringEnforce = Enforce.That("test");
        var intEnforce = Enforce.That(42);
        var boolEnforce = Enforce.That(true);
        var dateEnforce = Enforce.That(DateTime.Now.AddDays(1)); // Future date
        var guidEnforce = Enforce.That(Guid.NewGuid());
        
        // Act & Assert - These should all compile and work
        stringEnforce.NotEmpty(() => new TestException("Custom"));
        stringEnforce.HasLength(4, () => new TestException("Custom"));
        stringEnforce.Contains("es", () => new TestException("Custom"));
        
        intEnforce.Positive(() => new TestException("Custom"));
        intEnforce.GreaterThan(0, () => new TestException("Custom"));
        intEnforce.GreaterThanOrEqualTo(0, () => new TestException("Custom"))
                  .LessThanOrEqualTo(100, () => new TestException("Custom"));
        
        boolEnforce.True(() => new TestException("Custom"));
        
        dateEnforce.InFuture(() => new TestException("Custom"));
        
        guidEnforce.NotEmpty(() => new TestException("Custom"));
    }

    [Fact]
    public void CustomException_PreservesStackTrace()
    {
        // Arrange
        var enforce = Enforce.That("");
        Exception? capturedEx = null;

        // Act
        try
        {
            enforce.NotEmpty(() =>
            {
                capturedEx = new TestException("Custom error with stack trace");
                return capturedEx;
            });
        }
        catch (TestException ex)
        {
            // Assert
            ex.Should().BeSameAs(capturedEx);
            ex.Message.Should().Be("Custom error with stack trace");
        }
    }

    [Fact]
    public void CustomException_LazyEvaluation_OnlyCreatedWhenNeeded()
    {
        // Arrange
        var enforce = Enforce.That("not empty");
        var exceptionCreated = false;

        // Act
        var result = enforce.NotEmpty(() =>
        {
            exceptionCreated = true;
            return new TestException("Should not be created");
        });

        // Assert
        exceptionCreated.Should().BeFalse();
        result.Should().BeSameAs(enforce);
    }
}