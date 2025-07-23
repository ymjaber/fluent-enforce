using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FluentEnforce;

/// <summary>
/// Provides factory methods for creating <see cref="Enforce{TValue}"/> instances and performing common validation operations.
/// </summary>
public static class Enforce
{
    /// <summary>
    /// Creates a new <see cref="Enforce{TValue}"/> instance for the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to enforce.</typeparam>
    /// <param name="value">The value to enforce preconditions on.</param>
    /// <param name="paramName">The name of the parameter (automatically captured from the caller).</param>
    /// <returns>An <see cref="Enforce{TValue}"/> instance for fluent validation.</returns>
    public static Enforce<TValue> That<TValue>(
        TValue value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        return new Enforce<TValue>(value, paramName);
    }

    /// <summary>
    /// Ensures that the specified value is null.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to check.</typeparam>
    /// <param name="value">The value that must be null.</param>
    /// <param name="paramName">The name of the parameter (automatically captured from the caller).</param>
    /// <param name="message">The error message to use if the value is not null (optional).</param>
    /// <exception cref="ArgumentException">Thrown when the value is not null.</exception>
    public static void Null<TValue>(
        TValue? value,
        [CallerArgumentExpression("value")] string? paramName = null,
        string? message = null)
    {
        if (value is not null)
        {
            throw new ArgumentException(message ?? "Value must be null", paramName);
        }
    }

    /// <summary>
    /// Ensures that the specified value is not null.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to check.</typeparam>
    /// <param name="value">The value that must not be null.</param>
    /// <param name="paramName">The name of the parameter (automatically captured from the caller).</param>
    /// <param name="message">The error message to use if the value is null (optional).</param>
    /// <returns>An <see cref="Enforce{TValue}"/> instance for fluent validation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
    public static Enforce<TValue> NotNull<TValue>(
        [NotNull] TValue? value,
        [CallerArgumentExpression("value")] string? paramName = null,
        string? message = null)
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName, message ?? "Value cannot be null.");
        }

        return new(value, paramName);
    }

    /// <summary>
    /// Ensures that the specified string is not null or empty.
    /// </summary>
    /// <param name="value">The string value that must not be null or empty.</param>
    /// <param name="paramName">The name of the parameter (automatically captured from the caller).</param>
    /// <param name="message">The error message to use if the value is null or empty (optional).</param>
    /// <returns>An <see cref="Enforce{T}"/> instance for fluent validation where T is string.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is null or empty.</exception>
    public static Enforce<string> NotNullOrEmpty(
        [NotNull] string? value,
        [CallerArgumentExpression("value")] string? paramName = null,
        string? message = null)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException(message ?? "Value cannot be null or empty.", paramName);
        }
        
        return new(value, paramName);
    }
    
    /// <summary>
    /// Ensures that the specified string is not null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="value">The string value that must not be null, empty, or whitespace.</param>
    /// <param name="paramName">The name of the parameter (automatically captured from the caller).</param>
    /// <param name="message">The error message to use if the value is null, empty, or whitespace (optional).</param>
    /// <returns>An <see cref="Enforce{T}"/> instance for fluent validation where T is string.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is null, empty, or consists only of white-space characters.</exception>
    public static Enforce<string> NotNullOrWhiteSpace(
        [NotNull] string? value,
        [CallerArgumentExpression("value")] string? paramName = null,
        string? message = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(message ?? "Value cannot be null or whitespace.", paramName);
        }
        
        return new(value, paramName);
    }
}