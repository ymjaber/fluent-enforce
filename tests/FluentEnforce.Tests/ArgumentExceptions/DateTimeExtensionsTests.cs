namespace FluentEnforce.Tests.ArgumentExceptions;

public class DateTimeExtensionsTests
{
    [Fact]
    public void InFuture_WhenDateIsInFuture_ReturnsEnforce()
    {
        // Arrange
        var date = DateTime.Now.AddDays(1);
        var enforce = Enforce.That(date);

        // Act
        var result = enforce.InFuture();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void InFuture_WhenDateIsInPast_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var date = DateTime.Now.AddDays(-1);
        var enforce = Enforce.That(date);

        // Act
        var action = () => enforce.InFuture();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be in the future*")
            .And.ParamName.Should().Be("date");
    }

    [Fact]
    public void InFuture_WhenDateIsNow_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var date = DateTime.Now;
        var enforce = Enforce.That(date);

        // Act
        var action = () => enforce.InFuture();

        // Assert
        // Note: This might occasionally pass if the milliseconds differ
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be in the future*");
    }

    [Fact]
    public void InPast_WhenDateIsInPast_ReturnsEnforce()
    {
        // Arrange
        var date = DateTime.Now.AddDays(-1);
        var enforce = Enforce.That(date);

        // Act
        var result = enforce.InPast();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void InPast_WhenDateIsInFuture_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var date = DateTime.Now.AddDays(1);
        var enforce = Enforce.That(date);

        // Act
        var action = () => enforce.InPast();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be in the past*");
    }

    [Fact]
    public void DateOnly_InFuture_WhenDateIsInFuture_ReturnsEnforce()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var enforce = Enforce.That(date);

        // Act
        var result = enforce.InFuture();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void DateOnly_InFuture_WhenDateIsToday_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now);
        var enforce = Enforce.That(date);

        // Act
        var action = () => enforce.InFuture();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be in the future*");
    }

    [Fact]
    public void DateOnly_InPast_WhenDateIsInPast_ReturnsEnforce()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
        var enforce = Enforce.That(date);

        // Act
        var result = enforce.InPast();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void DateOnly_InPast_WhenDateIsToday_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now);
        var enforce = Enforce.That(date);

        // Act
        var action = () => enforce.InPast();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be in the past*");
    }

    [Fact]
    public void DateOnly_InFutureOrPresent_WhenDateIsToday_ReturnsEnforce()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now);
        var enforce = Enforce.That(date);

        // Act
        var result = enforce.InFutureOrPresent();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void DateOnly_InFutureOrPresent_WhenDateIsInFuture_ReturnsEnforce()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var enforce = Enforce.That(date);

        // Act
        var result = enforce.InFutureOrPresent();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void DateOnly_InFutureOrPresent_WhenDateIsInPast_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
        var enforce = Enforce.That(date);

        // Act
        var action = () => enforce.InFutureOrPresent();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be in the future or present*");
    }

    [Fact]
    public void DateOnly_InPastOrPresent_WhenDateIsToday_ReturnsEnforce()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now);
        var enforce = Enforce.That(date);

        // Act
        var result = enforce.InPastOrPresent();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void DateOnly_InPastOrPresent_WhenDateIsInPast_ReturnsEnforce()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
        var enforce = Enforce.That(date);

        // Act
        var result = enforce.InPastOrPresent();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void DateOnly_InPastOrPresent_WhenDateIsInFuture_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var enforce = Enforce.That(date);

        // Act
        var action = () => enforce.InPastOrPresent();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be in the past or present*");
    }

    [Fact]
    public void TimeOnly_InFuture_WhenTimeIsInFuture_ReturnsEnforce()
    {
        // Arrange
        var time = TimeOnly.FromDateTime(DateTime.Now.AddHours(1));
        var enforce = Enforce.That(time);

        // Act
        var result = enforce.InFuture();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void TimeOnly_InPast_WhenTimeIsInFuture_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var time = TimeOnly.FromDateTime(DateTime.Now.AddHours(1));
        var enforce = Enforce.That(time);

        // Act
        var action = () => enforce.InPast();

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be in the past*");
    }

    [Fact]
    public void MethodChaining_Works()
    {
        // Arrange
        var yesterday = DateTime.Now.AddDays(-1);
        var enforce = Enforce.That(yesterday);

        // Act
        var result = enforce
            .InPast()
            .Satisfies(d => d.DayOfWeek != DayOfWeek.Sunday || d.DayOfWeek != DayOfWeek.Saturday);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void DateTimeExtensions_WithNullableDateTime_NonNull_Works()
    {
        // Arrange
        DateTime? date = DateTime.Now.AddDays(1);
        var enforce = Enforce.That(date);

        // Act & Assert
        enforce.Satisfies(x => x.HasValue && x.Value > DateTime.Now).Should().BeSameAs(enforce);
    }

    [Fact]
    public void DateTimeExtensions_WithNullableDateTime_Null_Works()
    {
        // Arrange
        DateTime? date = null;
        var enforce = Enforce.That(date);

        // Act & Assert
        enforce.Satisfies(x => !x.HasValue).Should().BeSameAs(enforce);
    }
}