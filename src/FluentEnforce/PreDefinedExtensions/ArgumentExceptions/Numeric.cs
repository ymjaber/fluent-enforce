using System.Numerics;
using System.Runtime.CompilerServices;

namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing numeric constraints on values.
/// </summary>
public static partial class NumericExtensions
{
    /// <summary>
    /// Enforces that the value is greater than the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IComparable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not greater than the specified value.</exception>
    public static Enforce<TValue> GreaterThan<TValue>(
        this Enforce<TValue> enforce,
        TValue other,
        string? message = null)
        where TValue : IComparable<TValue>
    {
        if (enforce.Value.CompareTo(other) <= 0)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? $"Value must be greater than {other}");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is greater than or equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IComparable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not greater than or equal to the specified value.</exception>
    public static Enforce<TValue> GreaterThanOrEqualsTo<TValue>(
        this Enforce<TValue> enforce,
        TValue other,
        string? message = null)
        where TValue : IComparable<TValue>
    {
        if (enforce.Value.CompareTo(other) < 0)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? $"Value must be greater than or equal to {other}");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is less than the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IComparable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not less than the specified value.</exception>
    public static Enforce<TValue> LessThan<TValue>(
        this Enforce<TValue> enforce,
        TValue other,
        string? message = null)
        where TValue : IComparable<TValue>
    {
        if (enforce.Value.CompareTo(other) >= 0)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? $"Value must be less than {other}");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is less than or equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IComparable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not less than or equal to the specified value.</exception>
    public static Enforce<TValue> LessThanOrEqualsTo<TValue>(
        this Enforce<TValue> enforce,
        TValue other,
        string? message = null)
        where TValue : IComparable<TValue>
    {
        if (enforce.Value.CompareTo(other) > 0)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? $"Value must be less than or equal to {other}");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is within the specified range.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IComparable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <param name="bounds">The inclusivity of the range boundaries. Defaults to Inclusive.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is outside the specified range.</exception>
    public static Enforce<TValue> InRange<TValue>(
        this Enforce<TValue> enforce,
        TValue min,
        TValue max,
        RangeBounds bounds = RangeBounds.Inclusive,
        string? message = null)
        where TValue : IComparable<TValue>
    {
        var minComparison = enforce.Value.CompareTo(min);
        var maxComparison = enforce.Value.CompareTo(max);

        bool minInclusive = bounds == RangeBounds.Inclusive || bounds == RangeBounds.LeftInclusive;
        bool maxInclusive = bounds == RangeBounds.Inclusive || bounds == RangeBounds.RightInclusive;

        if ((minInclusive && minComparison < 0) || (!minInclusive && minComparison <= 0))
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? $"Value must be greater than {(minInclusive ? "or equal to" : "")} {min}");
        }

        if ((maxInclusive && maxComparison > 0) || (!maxInclusive && maxComparison >= 0))
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? $"Value must be less than {(maxInclusive ? "or equal to" : "")} {max}");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is positive (greater than zero).
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not positive.</exception>
    public static Enforce<TValue> Positive<TValue>(
        this Enforce<TValue> enforce,
        string? message = null)
        where TValue : INumberBase<TValue>
    {
        if (!TValue.IsPositive(enforce.Value))
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be positive");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is negative (less than zero).
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not negative.</exception>
    public static Enforce<TValue> Negative<TValue>(
        this Enforce<TValue> enforce,
        string? message = null)
        where TValue : INumberBase<TValue>
    {
        if (!TValue.IsNegative(enforce.Value))
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be negative");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is zero.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not zero.</exception>
    public static void Zero<TValue>(
        this Enforce<TValue> enforce,
        string? message = null)
        where TValue : INumberBase<TValue>
    {
        if (!TValue.IsZero(enforce.Value))
        {
            throw new ArgumentOutOfRangeException(message ?? "Value must be zero", enforce.ParamName);
        }
    }

    /// <summary>
    /// Enforces that the value is not zero.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is zero.</exception>
    public static Enforce<TValue> NonZero<TValue>(
        this Enforce<TValue> enforce,
        string? message = null)
        where TValue : INumberBase<TValue>
    {
        if (TValue.IsZero(enforce.Value))
        {
            throw new ArgumentOutOfRangeException(message ?? "Value must be non-zero", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is non-positive (less than or equal to zero).
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is positive.</exception>
    public static Enforce<TValue> NonPositive<TValue>(
        this Enforce<TValue> enforce,
        string? message = null)
        where TValue : INumberBase<TValue>
    {
        if (TValue.IsPositive(enforce.Value))
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be non-positive");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is non-negative (greater than or equal to zero).
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is negative.</exception>
    public static Enforce<TValue> NonNegative<TValue>(
        this Enforce<TValue> enforce,
        string? message = null)
        where TValue : INumberBase<TValue>
    {
        if (TValue.IsNegative(enforce.Value))
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be non-negative");
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the numeric value is even.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is not even.</exception>
    public static Enforce<TValue> Even<TValue>(
        this Enforce<TValue> enforce,
        string? message = null)
        where TValue : INumberBase<TValue>
    {
        if (!TValue.IsEvenInteger(enforce.Value))
        {
            throw new ArgumentException(message ?? "Value must be even", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the numeric value is odd.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is not odd.</exception>
    public static Enforce<TValue> Odd<TValue>(
        this Enforce<TValue> enforce,
        string? message = null)
        where TValue : INumberBase<TValue>
    {
        if (!TValue.IsOddInteger(enforce.Value))
        {
            throw new ArgumentException(message ?? "Value must be odd", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the integer value is divisible by the specified divisor.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="divisor">The divisor to check divisibility against.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is not divisible by the divisor.</exception>
    public static Enforce<int> DivisibleBy(
        this Enforce<int> enforce,
        int divisor,
        string? message = null)
    {
        if (enforce.Value % divisor != 0)
        {
            throw new ArgumentException(
                message ?? $"Value must be divisible by {divisor}",
                enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the long value is divisible by the specified divisor.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="divisor">The divisor to check divisibility against.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is not divisible by the divisor.</exception>
    public static Enforce<long> DivisibleBy(
        this Enforce<long> enforce,
        long divisor,
        string? message = null)
    {
        if (enforce.Value % divisor != 0)
        {
            throw new ArgumentException(
                message ?? $"Value must be divisible by {divisor}",
                enforce.ParamName);
        }

        return enforce;
    }

}
