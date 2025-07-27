namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing boolean value constraints.
/// </summary>
public static partial class BooleanExtensions
{
    /// <summary>
    /// Ensures that the boolean value is true.
    /// </summary>
    /// <param name="enforce">The enforce instance containing the boolean value to validate.</param>
    /// <param name="message">The error message to use if the value is false (optional).</param>
    /// <exception cref="ArgumentException">Thrown when the value is false.</exception>
    public static void True(
        this Enforce<bool> enforce,
        string? message = null)
    {
        if (!enforce.Value)
        {
            throw new ArgumentException(message ?? "Value must be true", enforce.ParamName);
        }
    }
    
    /// <summary>
    /// Ensures that the boolean value is false.
    /// </summary>
    /// <param name="enforce">The enforce instance containing the boolean value to validate.</param>
    /// <param name="message">The error message to use if the value is true (optional).</param>
    /// <exception cref="ArgumentException">Thrown when the value is true.</exception>
    public static void False(
        this Enforce<bool> enforce,
        string? message = null)
    {
        if (enforce.Value)
        {
            throw new ArgumentException(message ?? "Value must be false", enforce.ParamName);
        }
    }
}