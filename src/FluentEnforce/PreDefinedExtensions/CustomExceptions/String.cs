using System.Text.RegularExpressions;

namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing string constraints on values with custom exceptions.
/// </summary>
public static partial class StringExtensions
{
    /// <summary>
    /// Enforces that the string is empty.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string is not empty.</exception>
    public static void Empty(
        this Enforce<string> enforce,
        Func<Exception> exception)

    {
        if (!string.IsNullOrEmpty(enforce.Value))
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the string is not null or empty.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string is null or empty.</exception>
    public static Enforce<string> NotEmpty(
        this Enforce<string> enforce,
        Func<Exception> exception)
    {
        if (string.IsNullOrEmpty(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string has exactly the specified length.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="length">The exact length the string must have.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string does not have the specified length.</exception>
    public static Enforce<string> HasLength(
        this Enforce<string> enforce,
        int length,
        Func<Exception> exception)
    {
        if (enforce.Value.Length != length)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is longer than the specified length.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="length">The minimum length the string must exceed.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string is not longer than the specified length.</exception>
    public static Enforce<string> LongerThan(
        this Enforce<string> enforce,
        int length,
        Func<Exception> exception)
    {
        if (enforce.Value.Length <= length)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is longer than or equal to the specified length.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="length">The minimum length the string must have.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string is shorter than the specified length.</exception>
    public static Enforce<string> LongerThanOrEqualTo(
        this Enforce<string> enforce,
        int length,
        Func<Exception> exception)
    {
        if (enforce.Value.Length < length)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is shorter than the specified length.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="length">The maximum length the string must not reach.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string is not shorter than the specified length.</exception>
    public static Enforce<string> ShorterThan(
        this Enforce<string> enforce,
        int length,
        Func<Exception> exception)
    {
        if (enforce.Value.Length >= length)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is shorter than or equal to the specified length.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="length">The maximum length the string can have.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string is longer than the specified length.</exception>
    public static Enforce<string> ShorterThanOrEqualTo(
        this Enforce<string> enforce,
        int length,
        Func<Exception> exception)
    {
        if (enforce.Value.Length > length)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string matches the specified regular expression pattern.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="pattern">The regular expression pattern to match against.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string does not match the pattern.</exception>
    public static Enforce<string> Matches(
        this Enforce<string> enforce,
        Regex pattern,
        Func<Exception> exception)
    {
        if (!pattern.IsMatch(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string does not match the specified regular expression pattern.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="pattern">The regular expression pattern that must not match.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string matches the pattern.</exception>
    public static Enforce<string> NotMatch(
        this Enforce<string> enforce,
        Regex pattern,
        Func<Exception> exception)
    {
        if (pattern.IsMatch(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string contains the specified substring.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must be present.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string does not contain the substring.</exception>
    public static Enforce<string> Contains(
        this Enforce<string> enforce,
        string substring,
        Func<Exception> exception)
    {
        if (!enforce.Value.Contains(substring))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string does not contain the specified substring.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must not be present.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string contains the substring.</exception>
    public static Enforce<string> NotContain(
        this Enforce<string> enforce,
        string substring,
        Func<Exception> exception)
    {
        if (enforce.Value.Contains(substring))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string starts with the specified substring.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must be at the start.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string does not start with the substring.</exception>
    public static Enforce<string> StartsWith(
        this Enforce<string> enforce,
        string substring,
        Func<Exception> exception)
    {
        if (!enforce.Value.StartsWith(substring))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string does not start with the specified substring.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must not be at the start.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string starts with the substring.</exception>
    public static Enforce<string> NotStartWith(
        this Enforce<string> enforce,
        string substring,
        Func<Exception> exception)
    {
        if (enforce.Value.StartsWith(substring))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string ends with the specified substring.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must be at the end.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string does not end with the substring.</exception>
    public static Enforce<string> EndsWith(
        this Enforce<string> enforce,
        string substring,
        Func<Exception> exception)
    {
        if (!enforce.Value.EndsWith(substring))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string does not end with the specified substring.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must not be at the end.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string ends with the substring.</exception>
    public static Enforce<string> NotEndWith(
        this Enforce<string> enforce,
        string substring,
        Func<Exception> exception)
    {
        if (enforce.Value.EndsWith(substring))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is a valid email address.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string is not a valid email address.</exception>
    public static Enforce<string> MatchesEmail(
        this Enforce<string> enforce,
        Func<Exception> exception)
    {
        if (!Regex.IsMatch(enforce.Value, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is a valid URL.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string is not a valid URL.</exception>
    public static Enforce<string> MatchesUrl(
        this Enforce<string> enforce,
        Func<Exception> exception)
    {
        if (!Regex.IsMatch(enforce.Value, @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$"))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is a valid phone number.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string is not a valid phone number.</exception>
    public static Enforce<string> MatchesPhoneNumber(
        this Enforce<string> enforce,
        Func<Exception> exception)
    {
        if (!Regex.IsMatch(enforce.Value, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$"))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is a valid IP address.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string is not a valid IP address.</exception>
    public static Enforce<string> MatchesIpAddress(
        this Enforce<string> enforce,
        Func<Exception> exception)
    {
        if (!Regex.IsMatch(enforce.Value, @"^(\d{1,3}\.){3}\d{1,3}$"))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is a valid GUID.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the string is not a valid GUID.</exception>
    public static Enforce<string> MatchesGuid(
        this Enforce<string> enforce,
        Func<Exception> exception)
    {
        if (!Regex.IsMatch(
                enforce.Value,
                @"^(\{){0,1}[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}(\}){0,1}$"))
        {
            throw exception();
        }

        return enforce;
    }
}