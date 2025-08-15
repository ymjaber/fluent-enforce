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
    /// Ensures that the specified condition is true.
    /// </summary>
    /// <param name="condition">The condition that must be true.</param>
    /// <param name="message">The error message to use if the condition is false (optional).</param>
    /// <param name="paramName">The name of the parameter (optional).</param>
    /// <exception cref="ArgumentException">Thrown when the condition is false.</exception>
    public static void That(
            bool condition,
            string? message = null,
            string? paramName = null)
    {
        if (!condition) throw new ArgumentException(message ?? "Condition was not met", paramName);
    }

    /// <summary>
    /// Ensures that the specified condition is true.
    /// </summary>
    /// <param name="condition">The condition that must be true.</param>
    /// <param name="exception">A function that returns the custom exception to throw when the condition is false.</param>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the condition is false.</exception>
    public static void That(
            bool condition,
            Func<Exception> exception)
    {
        if (!condition) throw exception();
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
        where TValue : class
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName, message ?? "Value cannot be null.");
        }

        return new(value, paramName);
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
        where TValue : struct
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName, message ?? "Value cannot be null.");
        }

        return new(value.Value, paramName);
    }

    /// <summary>
    /// Ensures that the specified value is not null.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to check.</typeparam>
    /// <param name="value">The value that must not be null.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <param name="paramName">The name of the parameter (automatically captured from the caller).</param>
    /// <returns>An <see cref="Enforce{TValue}"/> instance for fluent validation.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is null.</exception>
    public static Enforce<TValue> NotNull<TValue>(
        [NotNull] TValue? value,
        Func<Exception> exception,
        [CallerArgumentExpression("value")] string? paramName = null)
        where TValue : class
    {
        if (value is null)
        {
            throw exception();
        }

        return new(value, paramName);
    }

    /// <summary>
    /// Ensures that the specified value is not null.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to check.</typeparam>
    /// <param name="value">The value that must not be null.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <param name="paramName">The name of the parameter (automatically captured from the caller).</param>
    /// <returns>An <see cref="Enforce{TValue}"/> instance for fluent validation.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is null.</exception>
    public static Enforce<TValue> NotNull<TValue>(
        [NotNull] TValue? value,
        Func<Exception> exception,
        [CallerArgumentExpression("value")] string? paramName = null)
        where TValue : struct
    {
        if (value is null)
        {
            throw exception();
        }

        return new(value.Value, paramName);
    }
}
