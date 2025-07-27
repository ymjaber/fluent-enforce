using System.Runtime.CompilerServices;

namespace FluentEnforce;

/// <summary>
/// Provides a fluent API for enforcing preconditions on values.
/// </summary>
/// <typeparam name="TValue">The type of the value being enforced.</typeparam>
public sealed class Enforce<TValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Enforce{TValue}"/> class.
    /// </summary>
    /// <param name="value">The value to enforce preconditions on.</param>
    /// <param name="paramName">The name of the parameter being enforced (optional).</param>
    internal Enforce(TValue value, string? paramName = null) => (Value, ParamName) = (value, paramName);

    /// <summary>
    /// Gets the value being enforced.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// Gets the name of the parameter being enforced, if any.
    /// </summary>
    public string? ParamName { get; }

    /// <summary>
    /// Implicitly converts an <see cref="Enforce{TValue}"/> instance to its underlying value.
    /// </summary>
    /// <param name="enforce">The enforce instance to convert.</param>
    /// <returns>The underlying value.</returns>
    public static implicit operator TValue(Enforce<TValue> enforce) => enforce.Value;

    /// <summary>
    /// Ensures that the value satisfies the specified predicate.
    /// </summary>
    /// <param name="rule">The predicate that the value must satisfy.</param>
    /// <param name="message">The error message to use if the predicate is not satisfied (optional).</param>
    /// <returns>The current instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the value does not satisfy the predicate.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enforce<TValue> Satisfies(Predicate<TValue> rule, string? message = null)
    {
        if (!rule(Value))
        {
            throw new ArgumentException(message ?? "Condition not satisfied", ParamName);
        }

        return this;
    }

    /// <summary>
    /// Ensures that the value satisfies the specified predicate.
    /// </summary>
    /// <param name="rule">The predicate that the value must satisfy.</param>
    /// <param name="exception">A function that creates the exception to throw if the predicate is not satisfied.</param>
    /// <returns>The current instance for method chaining.</returns>
    /// <exception cref="Exception">The exception created by the exception function when the value does not satisfy the predicate.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enforce<TValue> Satisfies(Predicate<TValue> rule, Func<Exception> exception)
    {
        if (!rule(Value))
        {
            throw exception();
        }

        return this;
    }
}