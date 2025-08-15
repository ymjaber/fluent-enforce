namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing date and time constraints.
/// </summary>
public static partial class DateTimeExtensions
{
    /// <summary>
    /// Ensures that the DateTime value is in the future.
    /// </summary>
    /// <param name="enforce">The enforce instance containing the DateTime value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current UTC time. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="message">The error message to use if the value is not in the future (optional).</param>
    /// <returns>The current instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not in the future.</exception>
    public static Enforce<DateTime> InFuture(
        this Enforce<DateTime> enforce,
        TimeProvider timeProvider = null!,
        string? message = null)
    {
        var clock = timeProvider ?? TimeProvider.System;
        var nowUtc = clock.GetUtcNow().UtcDateTime;
        var valueUtc = enforce.Value.Kind == DateTimeKind.Utc
            ? enforce.Value
            : enforce.Value.ToUniversalTime();
        if (valueUtc <= nowUtc)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be in the future");
        }

        return enforce;
    }
    
    /// <summary>
    /// Ensures that the DateOnly value is in the future.
    /// </summary>
    /// <param name="enforce">The enforce instance containing the DateOnly value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current local date. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="message">The error message to use if the value is not in the future (optional).</param>
    /// <returns>The current instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not in the future.</exception>
    public static Enforce<DateOnly> InFuture(
        this Enforce<DateOnly> enforce,
        TimeProvider timeProvider = null!,
        string? message = null)
    {
        var today = DateOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetLocalNow().DateTime);
        if (enforce.Value <= today)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be in the future");
        }

        return enforce;
    }
    
    /// <summary>
    /// Ensures that the TimeOnly value is in the future compared to the current time.
    /// </summary>
    /// <param name="enforce">The enforce instance containing the TimeOnly value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current local time. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="message">The error message to use if the value is not in the future (optional).</param>
    /// <returns>The current instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not in the future.</exception>
    public static Enforce<TimeOnly> InFuture(
        this Enforce<TimeOnly> enforce,
        TimeProvider timeProvider = null!,
        string? message = null)
    {
        var nowTime = TimeOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetLocalNow().DateTime);
        if (enforce.Value <= nowTime)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be in the future");
        }

        return enforce;
    }
    
    /// <summary>
    /// Ensures that the DateTime value is in the past.
    /// </summary>
    /// <param name="enforce">The enforce instance containing the DateTime value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current UTC time. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="message">The error message to use if the value is not in the past (optional).</param>
    /// <returns>The current instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not in the past.</exception>
    public static Enforce<DateTime> InPast(
        this Enforce<DateTime> enforce,
        TimeProvider timeProvider = null!,
        string? message = null)
    {
        var clock = timeProvider ?? TimeProvider.System;
        var nowUtc = clock.GetUtcNow().UtcDateTime;
        var valueUtc = enforce.Value.Kind == DateTimeKind.Utc
            ? enforce.Value
            : enforce.Value.ToUniversalTime();
        if (valueUtc >= nowUtc)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be in the past");
        }

        return enforce;
    }
    
    /// <summary>
    /// Ensures that the DateOnly value is in the past.
    /// </summary>
    /// <param name="enforce">The enforce instance containing the DateOnly value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current local date. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="message">The error message to use if the value is not in the past (optional).</param>
    /// <returns>The current instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not in the past.</exception>
    public static Enforce<DateOnly> InPast(
        this Enforce<DateOnly> enforce,
        TimeProvider timeProvider = null!,
        string? message = null)
    {
        var today = DateOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetLocalNow().DateTime);
        if (enforce.Value >= today)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be in the past");
        }

        return enforce;
    }
    
    /// <summary>
    /// Ensures that the TimeOnly value is in the past compared to the current time.
    /// </summary>
    /// <param name="enforce">The enforce instance containing the TimeOnly value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current local time. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="message">The error message to use if the value is not in the past (optional).</param>
    /// <returns>The current instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not in the past.</exception>
    public static Enforce<TimeOnly> InPast(
        this Enforce<TimeOnly> enforce,
        TimeProvider timeProvider = null!,
        string? message = null)
    {
        var nowTime = TimeOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetLocalNow().DateTime);
        if (enforce.Value >= nowTime)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be in the past");
        }

        return enforce;
    }
    
    /// <summary>
    /// Ensures that the DateOnly value is in the future or present (today).
    /// </summary>
    /// <param name="enforce">The enforce instance containing the DateOnly value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current local date. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="message">The error message to use if the value is in the past (optional).</param>
    /// <returns>The current instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is in the past.</exception>
    public static Enforce<DateOnly> InFutureOrPresent(
        this Enforce<DateOnly> enforce,
        TimeProvider timeProvider = null!,
        string? message = null)
    {
        var today = DateOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetLocalNow().DateTime);
        if (enforce.Value < today)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be in the future or present");
        }

        return enforce;
    }
    
    /// <summary>
    /// Ensures that the DateOnly value is in the past or present (today).
    /// </summary>
    /// <param name="enforce">The enforce instance containing the DateOnly value to validate.</param>
    /// <param name="timeProvider">The time provider used to obtain the current local date. If null, uses <see cref="TimeProvider.System"/>.</param>
    /// <param name="message">The error message to use if the value is in the future (optional).</param>
    /// <returns>The current instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is in the future.</exception>
    public static Enforce<DateOnly> InPastOrPresent(
        this Enforce<DateOnly> enforce,
        TimeProvider timeProvider = null!,
        string? message = null)
    {
        var today = DateOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetLocalNow().DateTime);
        if (enforce.Value > today)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be in the past or present");
        }

        return enforce;
    }
}