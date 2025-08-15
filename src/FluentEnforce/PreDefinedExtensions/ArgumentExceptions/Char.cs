using System.Runtime.CompilerServices;

namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing char constraints.
/// </summary>
public static partial class CharExtensions
{
    /// <summary>
    /// Enforces that the char value is a letter.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the char is not a letter.</exception>
    public static Enforce<char> Letter(
        this Enforce<char> enforce,
        string? message = null)
    {
        if (!char.IsLetter(enforce.Value))
        {
            throw new ArgumentException(message ?? "Character must be a letter", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a digit.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the char is not a digit.</exception>
    public static Enforce<char> Digit(
        this Enforce<char> enforce,
        string? message = null)
    {
        if (!char.IsDigit(enforce.Value))
        {
            throw new ArgumentException(message ?? "Character must be a digit", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a white-space character.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the char is not a white-space character.</exception>
    public static Enforce<char> WhiteSpace(
        this Enforce<char> enforce,
        string? message = null)
    {
        if (!char.IsWhiteSpace(enforce.Value))
        {
            throw new ArgumentException(message ?? "Character must be white-space", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is an uppercase letter.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the char is not an uppercase letter.</exception>
    public static Enforce<char> Upper(
        this Enforce<char> enforce,
        string? message = null)
    {
        if (!char.IsUpper(enforce.Value))
        {
            throw new ArgumentException(message ?? "Character must be uppercase", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a lowercase letter.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the char is not a lowercase letter.</exception>
    public static Enforce<char> Lower(
        this Enforce<char> enforce,
        string? message = null)
    {
        if (!char.IsLower(enforce.Value))
        {
            throw new ArgumentException(message ?? "Character must be lowercase", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a punctuation mark.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the char is not a punctuation mark.</exception>
    public static Enforce<char> Punctuation(
        this Enforce<char> enforce,
        string? message = null)
    {
        if (!char.IsPunctuation(enforce.Value))
        {
            throw new ArgumentException(message ?? "Character must be punctuation", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a letter or digit.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the char is not a letter or digit.</exception>
    public static Enforce<char> LetterOrDigit(
        this Enforce<char> enforce,
        string? message = null)
    {
        if (!char.IsLetterOrDigit(enforce.Value))
        {
            throw new ArgumentException(message ?? "Character must be a letter or digit", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a symbol character.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the char is not a symbol.</exception>
    public static Enforce<char> Symbol(
        this Enforce<char> enforce,
        string? message = null)
    {
        if (!char.IsSymbol(enforce.Value))
        {
            throw new ArgumentException(message ?? "Character must be a symbol", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the char value is a special character (punctuation or symbol).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the char value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the char is not a special character.</exception>
    public static Enforce<char> Special(
        this Enforce<char> enforce,
        string? message = null)
    {
        if (!(char.IsPunctuation(enforce.Value) || char.IsSymbol(enforce.Value)))
        {
            throw new ArgumentException(message ?? "Character must be a special character", enforce.ParamName);
        }

        return enforce;
    }
}