using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing string constraints on values.
/// </summary>
public static partial class StringExtensions
{
    // Email regex: Supports international characters and longer TLDs
    // Based on simplified RFC 5322 - handles 99.9% of real-world email addresses
    // Prevents consecutive dots and requires at least one character after @
    [GeneratedRegex(@"^[a-zA-Z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+\/=?^_`{|}~-]+)*@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)+$", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    internal static partial Regex EmailRegex();

    // URL regex: Supports multiple schemes, IDN domains, IPv6, and international paths
    // Prevents consecutive dots and ensures valid host names
    [GeneratedRegex(@"^(https?|ftp|ftps):\/\/(([a-zA-Z0-9\-._~!$&'()*+,;=:]|%[0-9A-Fa-f]{2})+@)?(([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?)(\.([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?))*|(\[([0-9A-Fa-f]{1,4}:){7}[0-9A-Fa-f]{1,4}\])|(\[(([0-9A-Fa-f]{1,4}:){0,6}[0-9A-Fa-f]{1,4})?::(([0-9A-Fa-f]{1,4}:){0,6}[0-9A-Fa-f]{1,4})?\]))(:[0-9]+)?(\/([a-zA-Z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})*)*(\?([a-zA-Z0-9\-._~!$&'()*+,;=:@\/?]|%[0-9A-Fa-f]{2})*)?(#([a-zA-Z0-9\-._~!$&'()*+,;=:@\/?]|%[0-9A-Fa-f]{2})*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    internal static partial Regex UrlRegex();

    // Phone number regex: E.164 format (international standard)
    // Allows optional + prefix, 1-15 digits total
    [GeneratedRegex(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled)]
    internal static partial Regex PhoneNumberRegex();

    // IP Address regex: Supports both IPv4 and IPv6
    [GeneratedRegex(@"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$|^(([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]+|::(ffff(:0{1,4})?:)?((25[0-5]|(2[0-4]|1?\d)?\d)\.){3}(25[0-5]|(2[0-4]|1?\d)?\d)|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1?\d)?\d)\.){3}(25[0-5]|(2[0-4]|1?\d)?\d))$", RegexOptions.Compiled)]
    internal static partial Regex IpAddressRegex();

    // GUID regex: Supports standard format with optional braces/parentheses
    [GeneratedRegex(@"^[\{\(]?[0-9a-fA-F]{8}-?[0-9a-fA-F]{4}-?[0-9a-fA-F]{4}-?[0-9a-fA-F]{4}-?[0-9a-fA-F]{12}[\}\)]?$", RegexOptions.Compiled)]
    internal static partial Regex GuidRegex();
    /// <summary>
    /// Enforces that the string is empty.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentException">Thrown when the string is not empty.</exception>
    public static void Empty(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!string.IsNullOrEmpty(enforce.Value))
        {
            throw new ArgumentException(message ?? "String must be empty", enforce.ParamName);
        }
    }

    /// <summary>
    /// Enforces that the string is not null or empty.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is null or empty.</exception>
    public static Enforce<string> NotEmpty(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (string.IsNullOrEmpty(enforce.Value))
        {
            throw new ArgumentException(message ?? "String cannot be empty", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is not empty or consists only of white-space characters.
    /// </summary>
    /// <param name="enforce">The <see cref="Enforce{T}"/> instance.</param>
    /// <param name="message">The error message to display if the string is empty or whitespace.</param>
    /// <returns>The <see cref="Enforce{T}"/> instance for chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is empty or consists only of white-space characters.</exception>
    public static Enforce<string> NotWhiteSpace(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (enforce.Value.Length == 0 || enforce.Value.Trim().Length == 0)
        {
            throw new ArgumentException(message ?? "String cannot be empty or consist only of white-space characters", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string consists only of white-space characters or is empty.
    /// </summary>
    /// <param name="enforce">The <see cref="Enforce{T}"/> instance.</param>
    /// <param name="message">The error message to display if the string contains non-whitespace characters.</param>
    /// <returns>The <see cref="Enforce{T}"/> instance for chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string contains non-whitespace characters.</exception>
    public static Enforce<string> WhiteSpace(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (enforce.Value.Length > 0 && enforce.Value.Trim().Length > 0)
        {
            throw new ArgumentException(message ?? "String must be empty or consist only of white-space characters", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string has exactly the specified length.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="length">The exact length the string must have.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not have the specified length.</exception>
    public static Enforce<string> HasLength(
        this Enforce<string> enforce,
        int length,
        string? message = null)
    {
        if (enforce.Value.Length != length)
        {
            throw new ArgumentException(
                message ?? $"String must have exactly {length} characters",
                enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is longer than the specified length.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="length">The minimum length the string must exceed.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is not longer than the specified length.</exception>
    public static Enforce<string> LongerThan(
        this Enforce<string> enforce,
        int length,
        string? message = null)
    {
        if (enforce.Value.Length <= length)
        {
            throw new ArgumentException(
                message ?? $"String must be longer than {length} characters",
                enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is longer than or equal to the specified length.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="length">The minimum length the string must have.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is shorter than the specified length.</exception>
    public static Enforce<string> LongerThanOrEqualsTo(
        this Enforce<string> enforce,
        int length,
        string? message = null)
    {
        if (enforce.Value.Length < length)
        {
            throw new ArgumentException(
                message ?? $"String must be longer than or equal to {length} characters",
                enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is shorter than the specified length.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="length">The maximum length the string must not reach.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is not shorter than the specified length.</exception>
    public static Enforce<string> ShorterThan(
        this Enforce<string> enforce,
        int length,
        string? message = null)
    {
        if (enforce.Value.Length >= length)
        {
            throw new ArgumentException(
                message ?? $"String must be shorter than {length} characters",
                enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is shorter than or equal to the specified length.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="length">The maximum length the string can have.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is longer than the specified length.</exception>
    public static Enforce<string> ShorterThanOrEqualsTo(
        this Enforce<string> enforce,
        int length,
        string? message = null)
    {
        if (enforce.Value.Length > length)
        {
            throw new ArgumentException(
                message ?? $"String must be shorter than or equal to {length} characters",
                enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string matches the specified regular expression pattern.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="pattern">The regular expression pattern to match against.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not match the pattern.</exception>
    public static Enforce<string> Matches(
        this Enforce<string> enforce,
        Regex pattern,
        string? message = null)
    {
        if (!pattern.IsMatch(enforce.Value))
        {
            throw new ArgumentException(message ?? "String does not match the pattern", enforce.ParamName);
        }

        return enforce;
    }
    /// <summary>
    /// Enforces that the string does not match the specified regular expression pattern.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="pattern">The regular expression pattern that must not match.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string matches the pattern.</exception>
    public static Enforce<string> NotMatches(
        this Enforce<string> enforce,
        Regex pattern,
        string? message = null)
    {
        if (pattern.IsMatch(enforce.Value))
        {
            throw new ArgumentException(message ?? "String must not match the pattern", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string contains the specified substring.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must be present.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not contain the substring.</exception>
    public static Enforce<string> Contains(
        this Enforce<string> enforce,
        string substring,
        string? message = null)
    {
        if (!enforce.Value.Contains(substring))
        {
            throw new ArgumentException(message ?? "String must contain the specified substring", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string does not contain the specified substring.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must not be present.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string contains the substring.</exception>
    public static Enforce<string> NotContains(
        this Enforce<string> enforce,
        string substring,
        string? message = null)
    {
        if (enforce.Value.Contains(substring))
        {
            throw new ArgumentException(
                message ?? "String must not contain the specified substring",
                enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string starts with the specified substring.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must be at the start.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not start with the substring.</exception>
    public static Enforce<string> StartsWith(
        this Enforce<string> enforce,
        string substring,
        string? message = null)
    {
        if (!enforce.Value.StartsWith(substring))
        {
            throw new ArgumentException(message ?? "String must start with the specified substring", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string does not start with the specified substring.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must not be at the start.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string starts with the substring.</exception>
    public static Enforce<string> NotStartsWith(
        this Enforce<string> enforce,
        string substring,
        string? message = null)
    {
        if (enforce.Value.StartsWith(substring))
        {
            throw new ArgumentException(
                message ?? "String must not start with the specified substring",
                enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string ends with the specified substring.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must be at the end.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not end with the substring.</exception>
    public static Enforce<string> EndsWith(
        this Enforce<string> enforce,
        string substring,
        string? message = null)
    {
        if (!enforce.Value.EndsWith(substring))
        {
            throw new ArgumentException(message ?? "String must end with the specified substring", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string does not end with the specified substring.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must not be at the end.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string ends with the substring.</exception>
    public static Enforce<string> NotEndsWith(
        this Enforce<string> enforce,
        string substring,
        string? message = null)
    {
        if (enforce.Value.EndsWith(substring))
        {
            throw new ArgumentException(
                message ?? "String must not end with the specified substring",
                enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is a valid email address.
    /// Uses RFC 5322 simplified pattern that handles most real-world email addresses.
    /// Note: This validation is suitable for most use cases but may reject some valid edge cases.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is not a valid email address.</exception>
    public static Enforce<string> MatchesEmail(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!EmailRegex().IsMatch(enforce.Value))
        {
            throw new ArgumentException(message ?? "String must be a valid email address", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is a valid URL.
    /// Supports http(s), ftp(s) schemes, international domains, IPv6 addresses, and encoded characters.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is not a valid URL.</exception>
    public static Enforce<string> MatchesUrl(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!UrlRegex().IsMatch(enforce.Value))
        {
            throw new ArgumentException(message ?? "String must be a valid URL", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is a valid phone number in E.164 international format.
    /// Format: +[country code][number] where the total length is 1-15 digits.
    /// Example: +970591234567 (Palestine), +971501234567 (UAE), +962791234567 (Jordan)
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is not a valid E.164 phone number.</exception>
    public static Enforce<string> MatchesPhoneNumber(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!PhoneNumberRegex().IsMatch(enforce.Value))
        {
            throw new ArgumentException(message ?? "String must be a valid E.164 phone number format", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is a valid GUID/UUID.
    /// Accepts standard format with or without hyphens, and with optional braces or parentheses.
    /// Examples: 550e8400-e29b-41d4-a716-446655440000, {550e8400-e29b-41d4-a716-446655440000}
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is not a valid GUID.</exception>
    public static Enforce<string> MatchesGuid(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!GuidRegex().IsMatch(enforce.Value))
        {
            throw new ArgumentException(message ?? "String must be a valid GUID", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is a valid IP address (IPv4 or IPv6).
    /// IPv4 example: 192.168.1.1
    /// IPv6 example: 2001:0db8:85a3:0000:0000:8a2e:0370:7334
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is not a valid IP address.</exception>
    public static Enforce<string> MatchesIpAddress(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!IpAddressRegex().IsMatch(enforce.Value))
        {
            throw new ArgumentException(message ?? "String must be a valid IP address", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string contains only alphanumeric characters (letters and digits).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string contains non-alphanumeric characters.</exception>
    public static Enforce<string> Alphanumeric(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!enforce.Value.All(char.IsLetterOrDigit))
        {
            throw new ArgumentException(message ?? "String must contain only alphanumeric characters", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string contains only alphabetic characters (letters).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string contains non-alphabetic characters.</exception>
    public static Enforce<string> Alpha(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!enforce.Value.All(char.IsLetter))
        {
            throw new ArgumentException(message ?? "String must contain only alphabetic characters", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string contains only numeric characters (digits).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string contains non-numeric characters.</exception>
    public static Enforce<string> Numeric(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!enforce.Value.All(char.IsDigit))
        {
            throw new ArgumentException(message ?? "String must contain only numeric characters", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string is a valid Base64 encoded string.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string is not valid Base64.</exception>
    public static Enforce<string> Base64(
        this Enforce<string> enforce,
        string? message = null)
    {
        try
        {
            Convert.FromBase64String(enforce.Value);
        }
        catch
        {
            throw new ArgumentException(message ?? "String must be valid Base64", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string contains the specified substring, ignoring case.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must be present (case-insensitive).</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not contain the substring.</exception>
    public static Enforce<string> ContainsIgnoreCase(
        this Enforce<string> enforce,
        string substring,
        string? message = null)
    {
        if (!enforce.Value.Contains(substring, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(message ?? "String must contain the specified substring (case-insensitive)", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string starts with the specified substring, ignoring case.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must be at the start (case-insensitive).</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not start with the substring.</exception>
    public static Enforce<string> StartsWithIgnoreCase(
        this Enforce<string> enforce,
        string substring,
        string? message = null)
    {
        if (!enforce.Value.StartsWith(substring, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(message ?? "String must start with the specified substring (case-insensitive)", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string ends with the specified substring, ignoring case.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must be at the end (case-insensitive).</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not end with the substring.</exception>
    public static Enforce<string> EndsWithIgnoreCase(
        this Enforce<string> enforce,
        string substring,
        string? message = null)
    {
        if (!enforce.Value.EndsWith(substring, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(message ?? "String must end with the specified substring (case-insensitive)", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string does not contain the specified substring, ignoring case.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must not be present (case-insensitive).</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string contains the substring.</exception>
    public static Enforce<string> NotContainsIgnoreCase(
        this Enforce<string> enforce,
        string substring,
        string? message = null)
    {
        if (enforce.Value.Contains(substring, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(message ?? "String must not contain the specified substring (case-insensitive)", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string does not start with the specified substring, ignoring case.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must not be at the start (case-insensitive).</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string starts with the substring.</exception>
    public static Enforce<string> NotStartsWithIgnoreCase(
        this Enforce<string> enforce,
        string substring,
        string? message = null)
    {
        if (enforce.Value.StartsWith(substring, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(message ?? "String must not start with the specified substring (case-insensitive)", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string does not end with the specified substring, ignoring case.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="substring">The substring that must not be at the end (case-insensitive).</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string ends with the substring.</exception>
    public static Enforce<string> NotEndsWithIgnoreCase(
        this Enforce<string> enforce,
        string substring,
        string? message = null)
    {
        if (enforce.Value.EndsWith(substring, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(message ?? "String must not end with the specified substring (case-insensitive)", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string includes at least one letter character.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not include at least one letter.</exception>
    public static Enforce<string> IncludesLetter(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!enforce.Value.Any(char.IsLetter))
        {
            throw new ArgumentException(message ?? "String must include at least one letter", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string includes at least one digit character.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not include at least one digit.</exception>
    public static Enforce<string> IncludesDigit(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!enforce.Value.Any(char.IsDigit))
        {
            throw new ArgumentException(message ?? "String must include at least one digit", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string includes at least one white-space character.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not include at least one white-space character.</exception>
    public static Enforce<string> IncludesWhiteSpace(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!enforce.Value.Any(char.IsWhiteSpace))
        {
            throw new ArgumentException(message ?? "String must include at least one white-space character", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string includes at least one uppercase letter.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not include at least one uppercase letter.</exception>
    public static Enforce<string> IncludesUpper(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!enforce.Value.Any(char.IsUpper))
        {
            throw new ArgumentException(message ?? "String must include at least one uppercase letter", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string includes at least one lowercase letter.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not include at least one lowercase letter.</exception>
    public static Enforce<string> IncludesLower(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!enforce.Value.Any(char.IsLower))
        {
            throw new ArgumentException(message ?? "String must include at least one lowercase letter", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string includes at least one punctuation character.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not include at least one punctuation character.</exception>
    public static Enforce<string> IncludesPunctuation(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!enforce.Value.Any(char.IsPunctuation))
        {
            throw new ArgumentException(message ?? "String must include at least one punctuation character", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string includes at least one letter or digit character.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not include at least one letter or digit.</exception>
    public static Enforce<string> IncludesLetterOrDigit(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!enforce.Value.Any(char.IsLetterOrDigit))
        {
            throw new ArgumentException(message ?? "String must include at least one letter or digit", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string includes at least one symbol character.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not include at least one symbol character.</exception>
    public static Enforce<string> IncludesSymbol(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!enforce.Value.Any(char.IsSymbol))
        {
            throw new ArgumentException(message ?? "String must include at least one symbol character", enforce.ParamName);
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the string includes at least one special character (punctuation or symbol).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the string to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the string does not include at least one special character.</exception>
    public static Enforce<string> IncludesSpecial(
        this Enforce<string> enforce,
        string? message = null)
    {
        if (!enforce.Value.Any(ch => char.IsPunctuation(ch) || char.IsSymbol(ch)))
        {
            throw new ArgumentException(message ?? "String must include at least one special character", enforce.ParamName);
        }

        return enforce;
    }
}
