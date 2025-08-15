namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing general constraints on values with custom exceptions.
/// </summary>
public static partial class GeneralExtensions
{
    /// <summary>
    /// Enforces that the value is equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IEquatable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="other">The value to compare for equality.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <exception cref="Exception">The exception when the value is not equal to the specified value.</exception>
    public static void EqualsTo<TValue>(
        this Enforce<TValue> enforce,
        TValue other,
        Func<Exception> exception)
        where TValue : IEquatable<TValue>
    {
        if (!enforce.Value.Equals(other))
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the value is not equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IEquatable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="other">The value to compare for inequality.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is equal to the specified value.</exception>
    public static Enforce<TValue> NotEqualsTo<TValue>(
        this Enforce<TValue> enforce,
        TValue other,
        Func<Exception> exception)
        where TValue : IEquatable<TValue>
    {
        if (enforce.Value.Equals(other))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is null.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <exception cref="Exception">The exception when the value is not null.</exception>
    public static void Null<TValue>(
        this Enforce<TValue> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value is not null)
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the value is contained in the specified set of allowed values.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IEquatable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <param name="allowedValues">The set of allowed values.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the allowed set.</exception>
    public static Enforce<TValue> In<TValue>(
        this Enforce<TValue> enforce,
        Func<Exception> exception,
        params ReadOnlySpan<TValue> allowedValues)
        where TValue : IEquatable<TValue>
    {
        foreach (var allowed in allowedValues)
        {
            if (allowed.Equals(enforce.Value))
            {
                return enforce;
            }
        }

        throw exception();
    }

    /// <summary>
    /// Enforces that the value is contained in the specified collection of allowed values.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IEquatable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="allowedValues">The collection of allowed values.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is not in the allowed collection.</exception>
    public static Enforce<TValue> In<TValue>(
        this Enforce<TValue> enforce,
        IEnumerable<TValue> allowedValues,
        Func<Exception> exception)
        where TValue : IEquatable<TValue>
    {
        if (!allowedValues.Any(v => v.Equals(enforce.Value)))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is not contained in the specified set of forbidden values.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IEquatable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <param name="forbiddenValues">The set of forbidden values.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is in the forbidden set.</exception>
    public static Enforce<TValue> NotIn<TValue>(
        this Enforce<TValue> enforce,
        Func<Exception> exception,
        params ReadOnlySpan<TValue> forbiddenValues)
        where TValue : IEquatable<TValue>
    {
        foreach (var forbidden in forbiddenValues)
        {
            if (forbidden.Equals(enforce.Value))
            {
                throw exception();
            }
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the value is not contained in the specified collection of forbidden values.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IEquatable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="forbiddenValues">The collection of forbidden values.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the value is in the forbidden collection.</exception>
    public static Enforce<TValue> NotIn<TValue>(
        this Enforce<TValue> enforce,
        IEnumerable<TValue> forbiddenValues,
        Func<Exception> exception)
        where TValue : IEquatable<TValue>
    {
        if (forbiddenValues.Any(v => v.Equals(enforce.Value)))
        {
            throw exception();
        }

        return enforce;
    }
}
