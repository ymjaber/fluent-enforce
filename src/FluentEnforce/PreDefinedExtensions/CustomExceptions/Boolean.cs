namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing boolean constraints with custom exceptions.
/// </summary>
public static partial class BooleanExtensions
{
    /// <summary>
    /// Enforces that the boolean value is true, throwing a custom exception if it is false.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the boolean value to validate.</param>
    /// <param name="exception">The exception to throw if the value is false.</param>
    /// <exception cref="Exception">The exception when the value is false.</exception>
    public static void True(
        this Enforce<bool> enforce,
        Func<Exception> exception)
    {
        if (!enforce.Value)
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the boolean value is false, throwing a custom exception if it is true.
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the boolean value to validate.</param>
    /// <param name="exception">The exception to throw if the value is true.</param>
    /// <exception cref="Exception">The exception when the value is true.</exception>
    public static void False(
        this Enforce<bool> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value)
        {
            throw exception();
        }
    }
}
