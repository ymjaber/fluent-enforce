namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing GUID constraints on values.
/// </summary>
public static partial class GuidExtensions
{
    /// <summary>
    /// Enforces that the GUID value is empty (all zeros).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the GUID value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="ArgumentException">Thrown when the GUID value is not empty.</exception>
    public static void Empty(
        this Enforce<Guid> enforce,
        string? message = null)
    {
        if (enforce.Value != Guid.Empty)
        {
            throw new ArgumentException(message ?? "Value must be empty", enforce.ParamName);
        }
    }
    
    /// <summary>
    /// Enforces that the GUID value is not empty (not all zeros).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the GUID value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the GUID value is empty.</exception>
    public static Enforce<Guid> NotEmpty(
        this Enforce<Guid> enforce,
        string? message = null)
    {
        if (enforce.Value == Guid.Empty)
        {
            throw new ArgumentException(message ?? "Value cannot be empty", enforce.ParamName);
        }

        return enforce;
    }
}