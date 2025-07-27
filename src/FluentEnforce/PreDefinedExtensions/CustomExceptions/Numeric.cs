using System.Numerics;

namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing numeric constraints on values with custom exceptions.
/// </summary>
public static partial class NumericExtensions
{
    /// <summary>
    /// Enforces that the value is greater than the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IComparable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is not greater than the specified value.</exception>
    public static Enforce<TValue> GreaterThan<TValue>(
        this Enforce<TValue> enforce,
        TValue other,
        Func<Exception> exception)
        where TValue : IComparable<TValue>
    {
        if (enforce.Value.CompareTo(other) <= 0)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is greater than or equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IComparable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is not greater than or equal to the specified value.</exception>
    public static Enforce<TValue> GreaterThanOrEqualTo<TValue>(
        this Enforce<TValue> enforce,
        TValue other,
        Func<Exception> exception)
        where TValue : IComparable<TValue>
    {
        if (enforce.Value.CompareTo(other) < 0)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is less than the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IComparable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is not less than the specified value.</exception>
    public static Enforce<TValue> LessThan<TValue>(
        this Enforce<TValue> enforce,
        TValue other,
        Func<Exception> exception)
        where TValue : IComparable<TValue>
    {
        if (enforce.Value.CompareTo(other) >= 0)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is less than or equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IComparable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is not less than or equal to the specified value.</exception>
    public static Enforce<TValue> LessThanOrEqualTo<TValue>(
        this Enforce<TValue> enforce,
        TValue other,
        Func<Exception> exception)
        where TValue : IComparable<TValue>
    {
        if (enforce.Value.CompareTo(other) > 0)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is within the specified range.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IComparable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="minInclusive">Whether the minimum value is inclusive.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <param name="maxInclusive">Whether the maximum value is inclusive.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is outside the specified range.</exception>
    public static Enforce<TValue> InRange<TValue>(
        this Enforce<TValue> enforce,
        TValue min,
        bool minInclusive,
        TValue max,
        bool maxInclusive,
        Func<Exception> exception)
        where TValue : IComparable<TValue>
    {
        var minComparison = enforce.Value.CompareTo(min);
        var maxComparison = enforce.Value.CompareTo(max);

        if ((minInclusive && minComparison < 0) || (!minInclusive && minComparison <= 0))
        {
            throw exception();
        }

        if ((maxInclusive && maxComparison > 0) || (!maxInclusive && maxComparison >= 0))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is positive (greater than zero).
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is not positive.</exception>
    public static Enforce<TValue> Positive<TValue>(
        this Enforce<TValue> enforce,
        Func<Exception> exception)
        where TValue : INumberBase<TValue>
    {
        if (!TValue.IsPositive(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is negative (less than zero).
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is not negative.</exception>
    public static Enforce<TValue> Negative<TValue>(
        this Enforce<TValue> enforce,
        Func<Exception> exception)
        where TValue : INumberBase<TValue>
    {
        if (!TValue.IsNegative(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is zero.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is not zero.</exception>
    public static void Zero<TValue>(
        this Enforce<TValue> enforce,
        Func<Exception> exception)
        where TValue : INumberBase<TValue>
    {
        if (!TValue.IsZero(enforce.Value))
        {
            throw exception();
        }
    }
    
    /// <summary>
    /// Enforces that the value is not zero.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is zero.</exception>
    public static Enforce<TValue> NonZero<TValue>(
        this Enforce<TValue> enforce,
        Func<Exception> exception)
        where TValue : INumberBase<TValue>
    {
        if (TValue.IsZero(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }
    
    /// <summary>
    /// Enforces that the value is non-positive (less than or equal to zero).
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is positive.</exception>
    public static Enforce<TValue> NonPositive<TValue>(
        this Enforce<TValue> enforce,
        Func<Exception> exception)
        where TValue : INumberBase<TValue>
    {
        if (TValue.IsPositive(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }
    
    /// <summary>
    /// Enforces that the value is non-negative (greater than or equal to zero).
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement INumberBase.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is negative.</exception>
    public static Enforce<TValue> NonNegative<TValue>(
        this Enforce<TValue> enforce,
        Func<Exception> exception)
        where TValue : INumberBase<TValue>
    {
        if (TValue.IsNegative(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }
}