namespace FluentEnforce.Tests.ArgumentExceptions;

public class EnumExtensionsTests
{
    public enum TestEnum
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 3
    }

    [Flags]
    public enum TestFlags
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4,
        All = Read | Write | Execute
    }

    [Fact]
    public void Defined_WhenEnumValueIsDefined_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(TestEnum.First);

        // Act
        var result = enforce.Defined();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Defined_WhenEnumValueIsNotDefined_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That((TestEnum)99);

        // Act
        var action = () => enforce.Defined();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be a defined enum value*")
            .And.ParamName.Should().Be("(TestEnum)99");
    }

    [Fact]
    public void Defined_WithZeroValue_WhenDefined_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(TestEnum.None);

        // Act
        var result = enforce.Defined();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Defined_WithFlagsEnum_SingleFlag_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(TestFlags.Read);

        // Act
        var result = enforce.Defined();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Defined_WithFlagsEnum_CombinedFlags_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That(TestFlags.Read | TestFlags.Write);

        // Act
        var action = () => enforce.Defined();

        // Assert
        // Enum.IsDefined does not support flag combinations
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be a defined enum value*");
    }

    [Fact]
    public void Defined_WithFlagsEnum_AllFlags_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That(TestFlags.All);

        // Act
        var result = enforce.Defined();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Defined_WithFlagsEnum_InvalidFlag_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var enforce = Enforce.That((TestFlags)128);

        // Act
        var action = () => enforce.Defined();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be a defined enum value*");
    }

    [Fact]
    public void Defined_WithFlagsEnum_UndefinedFlagValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        // 8 is not a defined flag
        var enforce = Enforce.That((TestFlags)8);

        // Act
        var action = () => enforce.Defined();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be a defined enum value*");
    }

    [Fact]
    public void Defined_WithNullableEnum_NonNullDefined_Works()
    {
        // Arrange
        TestEnum? value = TestEnum.Second;
        var enforce = Enforce.That(value);

        // Act & Assert
        enforce.Satisfies(x => x.HasValue && Enum.IsDefined(typeof(TestEnum), x.Value))
            .Should().BeSameAs(enforce);
    }

    [Fact]
    public void Defined_WithNullableEnum_NonNullUndefined_ThrowsException()
    {
        // Arrange
        TestEnum? value = (TestEnum)99;
        var enforce = Enforce.That(value);

        // Act & Assert
        var action = () => enforce.Satisfies(x => x.HasValue && Enum.IsDefined(typeof(TestEnum), x.Value));
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Defined_WithNullableEnum_Null_Works()
    {
        // Arrange
        TestEnum? value = null;
        var enforce = Enforce.That(value);

        // Act & Assert
        enforce.Satisfies(x => !x.HasValue).Should().BeSameAs(enforce);
    }

    [Fact]
    public void Defined_CanBeChainedWithOtherMethods()
    {
        // Arrange
        var enforce = Enforce.That(TestEnum.First);

        // Act
        var result = enforce
            .Defined()
            .Satisfies(e => e != TestEnum.None);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Defined_WithExpressionAsParameter_PreservesParameterName()
    {
        // Arrange
        var status = (TestEnum)99;

        // Act & Assert
        var action = () => Enforce.That(status).Defined();
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be a defined enum value*")
            .And.ParamName.Should().Be("status");
    }

    [Theory]
    [InlineData(TestEnum.None)]
    [InlineData(TestEnum.First)]
    [InlineData(TestEnum.Second)]
    [InlineData(TestEnum.Third)]
    public void Defined_WithAllDefinedValues_ReturnsEnforce(TestEnum value)
    {
        // Arrange
        var enforce = Enforce.That(value);

        // Act
        var result = enforce.Defined();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(10)]
    [InlineData(-1)]
    [InlineData(99)]
    public void Defined_WithUndefinedValues_ThrowsArgumentOutOfRangeException(int undefinedValue)
    {
        // Arrange
        var enforce = Enforce.That((TestEnum)undefinedValue);

        // Act
        var action = () => enforce.Defined();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be a defined enum value*");
    }

    [Theory]
    [InlineData(TestFlags.None)]
    [InlineData(TestFlags.Read)]
    [InlineData(TestFlags.Write)]
    [InlineData(TestFlags.Execute)]
    [InlineData(TestFlags.All)]  // All is explicitly defined in the enum
    public void Defined_WithSingleFlags_ReturnsEnforce(TestFlags flags)
    {
        // Arrange
        var enforce = Enforce.That(flags);

        // Act
        var result = enforce.Defined();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Theory]
    [InlineData(TestFlags.Read | TestFlags.Write)]
    [InlineData(TestFlags.Read | TestFlags.Execute)]
    [InlineData(TestFlags.Write | TestFlags.Execute)]
    public void Defined_WithFlagCombinations_ThrowsArgumentOutOfRangeException(TestFlags flags)
    {
        // Arrange
        var enforce = Enforce.That(flags);

        // Act
        var action = () => enforce.Defined();

        // Assert
        // Enum.IsDefined does not support flag combinations
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be a defined enum value*");
    }
}