namespace FluentEnforce.Tests.ArgumentExceptions;

public class GuidExtensionsTests
{
    [Fact]
    public void Empty_WhenGuidIsEmpty_DoesNotThrow()
    {
        // Arrange
        var guid = Guid.Empty;
        var enforce = Enforce.That(guid);

        // Act
        var action = () => enforce.Empty();

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Empty_WhenGuidIsNotEmpty_ThrowsArgumentException()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var enforce = Enforce.That(guid);

        // Act
        var action = () => enforce.Empty();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value must be empty*")
            .And.ParamName.Should().Be("guid");
    }

    [Fact]
    public void NotEmpty_WhenGuidIsNotEmpty_ReturnsEnforce()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var enforce = Enforce.That(guid);

        // Act
        var result = enforce.NotEmpty();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void NotEmpty_WhenGuidIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var guid = Guid.Empty;
        var enforce = Enforce.That(guid);

        // Act
        var action = () => enforce.NotEmpty();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be empty*")
            .And.ParamName.Should().Be("guid");
    }

    [Fact]
    public void NotEmpty_WhenGuidIsDefault_ThrowsArgumentException()
    {
        // Arrange
        var guid = default(Guid);
        var enforce = Enforce.That(guid);

        // Act
        var action = () => enforce.NotEmpty();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be empty*");
    }

    [Fact]
    public void NotEmpty_WhenGuidIsAllZeros_ThrowsArgumentException()
    {
        // Arrange
        var guid = new Guid("00000000-0000-0000-0000-000000000000");
        var enforce = Enforce.That(guid);

        // Act
        var action = () => enforce.NotEmpty();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be empty*");
    }

    [Fact]
    public void GuidExtensions_WithNullableGuid_NonEmptyValue_Works()
    {
        // Arrange
        Guid? guid = Guid.NewGuid();
        var enforce = Enforce.That(guid);

        // Act & Assert
        enforce.Satisfies(x => x.HasValue && x.Value != Guid.Empty).Should().BeSameAs(enforce);
    }

    [Fact]
    public void GuidExtensions_WithNullableGuid_EmptyValue_Works()
    {
        // Arrange
        Guid? guid = Guid.Empty;
        var enforce = Enforce.That(guid);

        // Act & Assert
        enforce.Satisfies(x => x.HasValue).Should().BeSameAs(enforce);
    }

    [Fact]
    public void GuidExtensions_WithNullableGuid_Null_Works()
    {
        // Arrange
        Guid? guid = null;
        var enforce = Enforce.That(guid);

        // Act & Assert
        enforce.Satisfies(x => !x.HasValue).Should().BeSameAs(enforce);
    }

    [Fact]
    public void Empty_FollowedByOtherValidation_CannotChain()
    {
        // Arrange
        var guid = Guid.Empty;
        var enforce = Enforce.That(guid);

        // Act & Assert
        enforce.Empty();  // This returns void
        
        // Can still use the original enforce variable for other validations
        var result = enforce.Satisfies(g => g == Guid.Empty);
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void NotEmpty_CanBeChainedWithOtherMethods()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var enforce = Enforce.That(guid);

        // Act
        var result = enforce
            .NotEmpty()
            .Satisfies(g => g != new Guid("12345678-1234-1234-1234-123456789012"));

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void NotEmpty_WithExpressionAsParameter_PreservesParameterName()
    {
        // Arrange
        var id = Guid.Empty;

        // Act & Assert
        var action = () => Enforce.That(id).NotEmpty();
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be empty*")
            .And.ParamName.Should().Be("id");
    }

    [Fact]
    public void NotEmpty_MultipleGuids_AllValid_Works()
    {
        // Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();

        // Act & Assert - All should pass
        Enforce.That(guid1).NotEmpty();
        Enforce.That(guid2).NotEmpty();
        Enforce.That(guid3).NotEmpty();
    }

    [Fact]
    public void NotEmpty_DifferentGuidFormats_Works()
    {
        // Arrange
        var guidN = Guid.Parse("12345678123412341234123456789012");
        var guidD = Guid.Parse("12345678-1234-1234-1234-123456789012");
        var guidB = Guid.Parse("{12345678-1234-1234-1234-123456789012}");
        var guidP = Guid.Parse("(12345678-1234-1234-1234-123456789012)");

        // Act & Assert - All should pass
        Enforce.That(guidN).NotEmpty();
        Enforce.That(guidD).NotEmpty();
        Enforce.That(guidB).NotEmpty();
        Enforce.That(guidP).NotEmpty();
    }
}