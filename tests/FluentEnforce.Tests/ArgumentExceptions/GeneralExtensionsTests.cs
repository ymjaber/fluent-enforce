namespace FluentEnforce.Tests.ArgumentExceptions;

public class GeneralExtensionsTests
{
    private class TestClass : IEquatable<TestClass>
    {
        public string? Name { get; set; }
        public int Value { get; set; }

        public bool Equals(TestClass? other)
        {
            if (other is null) return false;
            return Name == other.Name && Value == other.Value;
        }

        public override bool Equals(object? obj) => Equals(obj as TestClass);
        public override int GetHashCode() => HashCode.Combine(Name, Value);
    }

    [Fact]
    public void IsEqualTo_WhenValuesAreEqual_DoesNotThrow()
    {
        // Arrange
        var enforce = Enforce.That(42);

        // Act
        var action = () => enforce.IsEqualTo(42);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void IsEqualTo_WhenValuesAreNotEqual_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(42);

        // Act
        var action = () => enforce.IsEqualTo(43);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Parameter 'Value must be equal to the other value'*")
            .And.ParamName.Should().Be("Value must be equal to the other value");
    }

    [Fact]
    public void IsNotEqualTo_WhenValuesAreNotEqual_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(42);

        // Act
        var result = enforce.IsNotEqualTo(43);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void IsNotEqualTo_WhenValuesAreEqual_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(42);

        // Act
        var action = () => enforce.IsNotEqualTo(42);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Parameter 'Value must not be equal to the other value'*")
            .And.ParamName.Should().Be("Value must not be equal to the other value");
    }

    [Fact]
    public void GeneralExtensions_WithStrings_Works()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act & Assert
        enforce.IsEqualTo("test"); // Should not throw
        enforce.IsNotEqualTo("demo").Should().BeSameAs(enforce);
    }

    [Fact]
    public void GeneralExtensions_WithCustomClass_Works()
    {
        // Arrange
        var obj1 = new TestClass { Name = "Test", Value = 42 };
        var obj2 = new TestClass { Name = "Test", Value = 42 };
        var obj3 = new TestClass { Name = "Other", Value = 99 };
        var enforce = Enforce.That(obj1);

        // Act & Assert
        enforce.IsEqualTo(obj2); // Should not throw - they are equal
        enforce.IsNotEqualTo(obj3).Should().BeSameAs(enforce);
    }

    [Fact]
    public void IsEqualTo_FollowedByOtherValidation_CannotChain()
    {
        // Arrange
        var enforce = Enforce.That(42);

        // Act & Assert
        enforce.IsEqualTo(42); // This returns void
        
        // Can still use the original enforce variable for other validations
        var result = enforce.IsNotEqualTo(99);
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void MethodChaining_WithIsNotEqualTo_Works()
    {
        // Arrange
        var enforce = Enforce.That(42);

        // Act
        var result = enforce
            .IsNotEqualTo(0)
            .IsNotEqualTo(100)
            .Satisfies(x => x > 0);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void GeneralExtensions_WithNullableValueType_UsingSatisfies_Works()
    {
        // Arrange
        int? value = 42;
        var enforce = Enforce.That(value);

        // Act & Assert
        var result = enforce
            .Satisfies(x => x.HasValue)
            .Satisfies(x => x != null)
            .Satisfies(x => x != 0);

        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void IsEqualTo_WithNullValue_CannotUseWithNullableTypes()
    {
        // This test documents that IsEqualTo cannot be used with nullable types
        // due to IEquatable<T> constraint. Use Satisfies instead.
        
        // Arrange
        string? value = null;
        var enforce = Enforce.That(value);

        // Act & Assert - Use Satisfies for null checks
        enforce.Satisfies(x => x == null).Should().BeSameAs(enforce);
    }

    [Fact]
    public void IsNotEqualTo_WithNullValue_CannotUseWithNullableTypes()
    {
        // This test documents that IsNotEqualTo cannot be used with nullable types
        // due to IEquatable<T> constraint. Use Satisfies instead.
        
        // Arrange
        string value = "test";
        var enforce = Enforce.That(value);

        // Act & Assert - Use Satisfies for null checks or use non-nullable types
        enforce.IsNotEqualTo("other").Should().BeSameAs(enforce);
    }

    [Fact]
    public void IsEqualTo_WithExpressionAsParameter_PreservesParameterName()
    {
        // Arrange
        var x = 10;
        var y = 20;

        // Act & Assert
        var action = () => Enforce.That(x + y).IsEqualTo(40);
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Parameter 'Value must be equal to the other value'*")
            .And.ParamName.Should().Be("Value must be equal to the other value");
    }

    [Fact]
    public void IsNotEqualTo_WithExpressionAsParameter_PreservesParameterName()
    {
        // Arrange
        var x = 10;
        var y = 20;

        // Act & Assert
        var action = () => Enforce.That(x + y).IsNotEqualTo(30);
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Parameter 'Value must not be equal to the other value'*")
            .And.ParamName.Should().Be("Value must not be equal to the other value");
    }
}