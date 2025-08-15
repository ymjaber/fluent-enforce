namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing char constraints with custom exceptions.
/// </summary>
public static partial class CharExtensions
{
    /// <summary>
    /// Enforces that the char value is a letter.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the char is not a letter.</exception>
    public static Enforce<char> Letter(
        this Enforce<char> enforce,
        Func<Exception> exception)
    {
        if (!char.IsLetter(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a digit.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the char is not a digit.</exception>
    public static Enforce<char> Digit(
        this Enforce<char> enforce,
        Func<Exception> exception)
    {
        if (!char.IsDigit(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a white-space character.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the char is not a white-space character.</exception>
    public static Enforce<char> WhiteSpace(
        this Enforce<char> enforce,
        Func<Exception> exception)
    {
        if (!char.IsWhiteSpace(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is an uppercase letter.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the char is not an uppercase letter.</exception>
    public static Enforce<char> Upper(
        this Enforce<char> enforce,
        Func<Exception> exception)
    {
        if (!char.IsUpper(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a lowercase letter.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the char is not a lowercase letter.</exception>
    public static Enforce<char> Lower(
        this Enforce<char> enforce,
        Func<Exception> exception)
    {
        if (!char.IsLower(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a punctuation mark.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the char is not a punctuation mark.</exception>
    public static Enforce<char> Punctuation(
        this Enforce<char> enforce,
        Func<Exception> exception)
    {
        if (!char.IsPunctuation(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a letter or digit.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the char is not a letter or digit.</exception>
    public static Enforce<char> LetterOrDigit(
        this Enforce<char> enforce,
        Func<Exception> exception)
    {
        if (!char.IsLetterOrDigit(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a symbol character.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the char is not a symbol.</exception>
    public static Enforce<char> Symbol(
        this Enforce<char> enforce,
        Func<Exception> exception)
    {
        if (!char.IsSymbol(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a special character (punctuation or symbol).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the char is not a special character.</exception>
    public static Enforce<char> Special(
        this Enforce<char> enforce,
        Func<Exception> exception)
    {
        if (!(char.IsPunctuation(enforce.Value) || char.IsSymbol(enforce.Value)))
        {
            throw exception();
        }

        return enforce;
    }
}
