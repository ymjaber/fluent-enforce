using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FluentEnforce;

/// <summary>
/// Provides extension methods for fluent validation using the Enforce pattern.
/// These methods offer an alternative syntax to the static factory methods.
/// </summary>
public static class EnforceExtensions
{
    /// <summary>
    /// Creates a new <see cref="Enforce{TValue}"/> instance for the specified value.
    /// This is an extension method alternative to <see cref="Enforce.That{TValue}"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to enforce.</typeparam>
    /// <param name="value">The value to enforce preconditions on.</param>
    /// <param name="paramName">The name of the parameter (automatically captured from the caller).</param>
    /// <returns>An <see cref="Enforce{TValue}"/> instance for fluent validation.</returns>
    /// <example>
    /// <code>
    /// int age = 25;
    /// age.Enforce().GreaterThanOrEqualsTo(18);
    /// </code>
    /// </example>
    public static Enforce<TValue> Enforce<TValue>(
        this TValue value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        return FluentEnforce.Enforce.That(value, paramName);
    }

    /// <summary>
    /// Ensures that the specified value is not null.
    /// This is an extension method alternative to Enforce.NotNull.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to check.</typeparam>
    /// <param name="value">The value that must not be null.</param>
    /// <param name="message">The error message to use if the value is null (optional).</param>
    /// <param name="paramName">The name of the parameter (automatically captured from the caller).</param>
    /// <returns>An <see cref="Enforce{TValue}"/> instance for fluent validation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
    /// <example>
    /// <code>
    /// string? name = GetName();
    /// var validName = name.EnforceNotNull().StartsWith("A");
    /// </code>
    /// </example>
    public static Enforce<TValue> EnforceNotNull<TValue>(
        [NotNull] this TValue? value,
        string? message = null,
        [CallerArgumentExpression("value")] string? paramName = null)
        where TValue : class
    {
        return FluentEnforce.Enforce.NotNull(value, paramName, message);
    }

    /// <summary>
    /// Ensures that the specified value is not null.
    /// This is an extension method alternative to Enforce.NotNull.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to check.</typeparam>
    /// <param name="value">The value that must not be null.</param>
    /// <param name="message">The error message to use if the value is null (optional).</param>
    /// <param name="paramName">The name of the parameter (automatically captured from the caller).</param>
    /// <returns>An <see cref="Enforce{TValue}"/> instance for fluent validation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
    /// <example>
    /// <code>
    /// string? name = GetName();
    /// var validName = name.EnforceNotNull().StartsWith("A");
    /// </code>
    /// </example>
    public static Enforce<TValue> EnforceNotNull<TValue>(
        [NotNull] this TValue? value,
        string? message = null,
        [CallerArgumentExpression("value")] string? paramName = null)
        where TValue : struct
    {
        return FluentEnforce.Enforce.NotNull(value, paramName, message);
    }

    /// <summary>
    /// Ensures that the specified value is not null.
    /// This is an extension method alternative to <see cref="Enforce.NotNull{TValue}(TValue?, Func{Exception}, string?)"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to check.</typeparam>
    /// <param name="value">The value that must not be null.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <param name="paramName">The name of the parameter (automatically captured from the caller).</param>
    /// <returns>An <see cref="Enforce{TValue}"/> instance for fluent validation.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is null.</exception>
    public static Enforce<TValue> EnforceNotNull<TValue>(
        [NotNull] this TValue? value,
        Func<Exception> exception,
        [CallerArgumentExpression("value")] string? paramName = null)
        where TValue : class
    {
        return FluentEnforce.Enforce.NotNull(value, exception, paramName);
    }

    /// <summary>
    /// Ensures that the specified value is not null.
    /// This is an extension method alternative to <see cref="Enforce.NotNull{TValue}(TValue?, Func{Exception}, string?)"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to check.</typeparam>
    /// <param name="value">The value that must not be null.</param>
    /// <param name="exception">A function that returns the custom exception to throw when validation fails.</param>
    /// <param name="paramName">The name of the parameter (automatically captured from the caller).</param>
    /// <returns>An <see cref="Enforce{TValue}"/> instance for fluent validation.</returns>
    /// <exception cref="Exception">Throws the custom exception returned by the exception function when the value is null.</exception>
    public static Enforce<TValue> EnforceNotNull<TValue>(
        [NotNull] this TValue? value,
        Func<Exception> exception,
        [CallerArgumentExpression("value")] string? paramName = null)
        where TValue : struct
    {
        return FluentEnforce.Enforce.NotNull(value, exception, paramName);
    }

}
