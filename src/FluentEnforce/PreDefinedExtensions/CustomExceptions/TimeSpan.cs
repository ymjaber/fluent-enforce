namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing TimeSpan constraints with custom exceptions.
/// </summary>
public static partial class TimeSpanExtensions
{
    /// <summary>
    /// Enforces that the TimeSpan value is positive (greater than zero).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeSpan value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the TimeSpan is not positive.</exception>
    public static Enforce<TimeSpan> Positive(
        this Enforce<TimeSpan> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value <= TimeSpan.Zero)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the TimeSpan value is negative (less than zero).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeSpan value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the TimeSpan is not negative.</exception>
    public static Enforce<TimeSpan> Negative(
        this Enforce<TimeSpan> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value >= TimeSpan.Zero)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the TimeSpan value is zero.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeSpan value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <exception cref="Exception">The exception when the TimeSpan is not zero.</exception>
    public static void Zero(
        this Enforce<TimeSpan> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value != TimeSpan.Zero)
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the TimeSpan value is not zero.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeSpan value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the TimeSpan is zero.</exception>
    public static Enforce<TimeSpan> NonZero(
        this Enforce<TimeSpan> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value == TimeSpan.Zero)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the TimeSpan value is non-positive (less than or equal to zero).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeSpan value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the TimeSpan is positive.</exception>
    public static Enforce<TimeSpan> NonPositive(
        this Enforce<TimeSpan> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value > TimeSpan.Zero)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the TimeSpan value is non-negative (greater than or equal to zero).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeSpan value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the TimeSpan is negative.</exception>
    public static Enforce<TimeSpan> NonNegative(
        this Enforce<TimeSpan> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value < TimeSpan.Zero)
        {
            throw exception();
        }

        return enforce;
    }
}
