using System.Runtime.CompilerServices;

namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing TimeSpan constraints.
/// </summary>
public static partial class TimeSpanExtensions
{
    /// <summary>
    /// Enforces that the TimeSpan value is positive (greater than zero).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeSpan value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the TimeSpan is not positive.</exception>
    public static Enforce<TimeSpan> Positive(
        this Enforce<TimeSpan> enforce,
        string? message = null)
    {
        if (enforce.Value <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "TimeSpan must be positive");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the TimeSpan value is negative (less than zero).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeSpan value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the TimeSpan is not negative.</exception>
    public static Enforce<TimeSpan> Negative(
        this Enforce<TimeSpan> enforce,
        string? message = null)
    {
        if (enforce.Value >= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "TimeSpan must be negative");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the TimeSpan value is zero.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeSpan value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the TimeSpan is not zero.</exception>
    public static void Zero(
        this Enforce<TimeSpan> enforce,
        string? message = null)
    {
        if (enforce.Value != TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "TimeSpan must be zero");
        }
    }

    /// <summary>
    /// Enforces that the TimeSpan value is not zero.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeSpan value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the TimeSpan is zero.</exception>
    public static Enforce<TimeSpan> NonZero(
        this Enforce<TimeSpan> enforce,
        string? message = null)
    {
        if (enforce.Value == TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "TimeSpan must be non-zero");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the TimeSpan value is non-positive (less than or equal to zero).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeSpan value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the TimeSpan is positive.</exception>
    public static Enforce<TimeSpan> NonPositive(
        this Enforce<TimeSpan> enforce,
        string? message = null)
    {
        if (enforce.Value > TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "TimeSpan must be non-positive");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the TimeSpan value is non-negative (greater than or equal to zero).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the TimeSpan value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the TimeSpan is negative.</exception>
    public static Enforce<TimeSpan> NonNegative(
        this Enforce<TimeSpan> enforce,
        string? message = null)
    {
        if (enforce.Value < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "TimeSpan must be non-negative");
        }

        return enforce;
    }
}
