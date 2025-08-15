namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing date and time constraints with custom exceptions.
/// </summary>
public static partial class DateTimeExtensions
{
    /// <summary>
    /// Enforces that the DateTime value is in the future, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the DateTime value to validate.</param>
    /// <param name="exception">The exception to throw if the value is not in the future.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the future.</exception>
    public static Enforce<DateTime> InFuture(
        this Enforce<DateTime> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value <= DateTime.Now)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the DateTime value is in the future, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the DateTime value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current UTC time. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="exception">The exception to throw if the value is not in the future.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the future.</exception>
    public static Enforce<DateTime> InFuture(
        this Enforce<DateTime> enforce,
        TimeProvider timeProvider,
        Func<Exception> exception)
    {
        var clock = timeProvider ?? TimeProvider.System;
        var nowUtc = clock.GetUtcNow().UtcDateTime;
        var valueUtc = enforce.Value.Kind == DateTimeKind.Utc
            ? enforce.Value
            : enforce.Value.ToUniversalTime();

        if (valueUtc <= nowUtc)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the DateOnly value is in the future, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the DateOnly value to validate.</param>
    /// <param name="exception">The exception to throw if the value is not in the future.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the future.</exception>
    public static Enforce<DateOnly> InFuture(
        this Enforce<DateOnly> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value <= DateOnly.FromDateTime(DateTime.Now))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the DateOnly value is in the future, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the DateOnly value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current local date. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="exception">The exception to throw if the value is not in the future.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the future.</exception>
    public static Enforce<DateOnly> InFuture(
        this Enforce<DateOnly> enforce,
        TimeProvider timeProvider,
        Func<Exception> exception)
    {
        var today = DateOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetLocalNow().DateTime);
        if (enforce.Value <= today)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the TimeOnly value is in the future, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeOnly value to validate.</param>
    /// <param name="exception">The exception to throw if the value is not in the future.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the future.</exception>
    public static Enforce<TimeOnly> InFuture(
        this Enforce<TimeOnly> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value <= TimeOnly.FromDateTime(DateTime.Now))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the TimeOnly value is in the future, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeOnly value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current local time. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="exception">The exception to throw if the value is not in the future.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the future.</exception>
    public static Enforce<TimeOnly> InFuture(
        this Enforce<TimeOnly> enforce,
        TimeProvider timeProvider,
        Func<Exception> exception)
    {
        var nowTime = TimeOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetLocalNow().DateTime);
        if (enforce.Value <= nowTime)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the DateTime value is in the past, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the DateTime value to validate.</param>
    /// <param name="exception">The exception to throw if the value is not in the past.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the past.</exception>
    public static Enforce<DateTime> InPast(
        this Enforce<DateTime> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value >= DateTime.Now)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the DateTime value is in the past, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the DateTime value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current UTC time. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="exception">The exception to throw if the value is not in the past.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the past.</exception>
    public static Enforce<DateTime> InPast(
        this Enforce<DateTime> enforce,
        TimeProvider timeProvider,
        Func<Exception> exception)
    {
        var clock = timeProvider ?? TimeProvider.System;
        var nowUtc = clock.GetUtcNow().UtcDateTime;
        var valueUtc = enforce.Value.Kind == DateTimeKind.Utc
            ? enforce.Value
            : enforce.Value.ToUniversalTime();

        if (valueUtc >= nowUtc)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the DateOnly value is in the past, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the DateOnly value to validate.</param>
    /// <param name="exception">The exception to throw if the value is not in the past.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the past.</exception>
    public static Enforce<DateOnly> InPast(
        this Enforce<DateOnly> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value >= DateOnly.FromDateTime(DateTime.Now))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the DateOnly value is in the past, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the DateOnly value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current local date. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="exception">The exception to throw if the value is not in the past.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the past.</exception>
    public static Enforce<DateOnly> InPast(
        this Enforce<DateOnly> enforce,
        TimeProvider timeProvider,
        Func<Exception> exception)
    {
        var today = DateOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetLocalNow().DateTime);
        if (enforce.Value >= today)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the TimeOnly value is in the past, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeOnly value to validate.</param>
    /// <param name="exception">The exception to throw if the value is not in the past.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the past.</exception>
    public static Enforce<TimeOnly> InPast(
        this Enforce<TimeOnly> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value >= TimeOnly.FromDateTime(DateTime.Now))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the TimeOnly value is in the past, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeOnly value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current local time. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="exception">The exception to throw if the value is not in the past.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the past.</exception>
    public static Enforce<TimeOnly> InPast(
        this Enforce<TimeOnly> enforce,
        TimeProvider timeProvider,
        Func<Exception> exception)
    {
        var nowTime = TimeOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetLocalNow().DateTime);
        if (enforce.Value >= nowTime)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the DateOnly value is today or in the future, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the DateOnly value to validate.</param>
    /// <param name="exception">The exception to throw if the value is in the past.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is in the past.</exception>
    public static Enforce<DateOnly> InFutureOrPresent(
        this Enforce<DateOnly> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value < DateOnly.FromDateTime(DateTime.Now))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the DateOnly value is today or in the future, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the DateOnly value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current local date. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="exception">The exception to throw if the value is in the past.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is in the past.</exception>
    public static Enforce<DateOnly> InFutureOrPresent(
        this Enforce<DateOnly> enforce,
        TimeProvider timeProvider,
        Func<Exception> exception)
    {
        var today = DateOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetLocalNow().DateTime);
        if (enforce.Value < today)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the DateOnly value is today or in the past, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the DateOnly value to validate.</param>
    /// <param name="exception">The exception to throw if the value is in the future.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is in the future.</exception>
    public static Enforce<DateOnly> InPastOrPresent(
        this Enforce<DateOnly> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value > DateOnly.FromDateTime(DateTime.Now))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the DateOnly value is today or in the past, throwing a custom exception if it is not.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the DateOnly value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current local date. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="exception">The exception to throw if the value is in the future.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is in the future.</exception>
    public static Enforce<DateOnly> InPastOrPresent(
        this Enforce<DateOnly> enforce,
        TimeProvider timeProvider,
        Func<Exception> exception)
    {
        var today = DateOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetLocalNow().DateTime);
        if (enforce.Value > today)
        {
            throw exception();
        }

        return enforce;
    }
}
