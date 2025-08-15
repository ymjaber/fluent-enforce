namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing GUID constraints on values with custom exceptions.
/// </summary>
public static partial class GuidExtensions
{
    /// <summary>
    /// Enforces that the GUID value is empty (all zeros).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the GUID value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <exception cref="Exception">The exception when the GUID value is not empty.</exception>
    public static void Empty(
        this Enforce<Guid> enforce,
        Func<Exception> exception)

    {
        if (enforce.Value != Guid.Empty)
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the GUID value is not empty (not all zeros).
    /// </summary>
    /// <param name="enforce">The Enforce instance containing the GUID value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the GUID value is empty.</exception>
    public static Enforce<Guid> NotEmpty(
        this Enforce<Guid> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value == Guid.Empty)
        {
            throw exception();
        }

        return enforce;
    }
}
