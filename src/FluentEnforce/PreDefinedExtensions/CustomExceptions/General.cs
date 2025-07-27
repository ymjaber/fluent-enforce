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
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is not equal to the specified value.</exception>
    public static void IsEqualTo<TValue>(
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
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is equal to the specified value.</exception>
    public static Enforce<TValue> IsNotEqualTo<TValue>(
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
}