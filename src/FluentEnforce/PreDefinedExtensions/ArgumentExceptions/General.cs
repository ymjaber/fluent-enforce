namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing general constraints on values.
/// </summary>
public static partial class GeneralExtensions
{
    /// <summary>
    /// Enforces that the value is equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IEquatable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="other">The value to compare for equality.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not equal to the specified value.</exception>
    public static void IsEqualTo<TValue>(
        this Enforce<TValue> enforce,
        TValue other,
        string? message = null)
        where TValue : IEquatable<TValue>
    {
        if (!enforce.Value.Equals(other))
        {
            throw new ArgumentOutOfRangeException(message ?? "Value must be equal to the other value", enforce.ParamName);
        }
    }
    
    /// <summary>
    /// Enforces that the value is not equal to the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being validated. Must implement IEquatable.</typeparam>
    /// <param name="enforce">The Enforce instance containing the value to validate.</param>
    /// <param name="other">The value to compare for inequality.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is equal to the specified value.</exception>
    public static Enforce<TValue> IsNotEqualTo<TValue>(
        this Enforce<TValue> enforce,
        TValue other,
        string? message = null)
        where TValue : IEquatable<TValue>
    {
        if (enforce.Value.Equals(other))
        {
            throw new ArgumentOutOfRangeException(message ?? "Value must not be equal to the other value", enforce.ParamName);
        }

        return enforce;
    }
}