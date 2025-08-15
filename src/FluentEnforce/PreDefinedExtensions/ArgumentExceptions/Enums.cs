namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing enum constraints on values.
/// </summary>
public static partial class EnumExtensions
{
    /// <summary>
    /// Enforces that the enum value is defined in the enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="enforce">The Enforce instance containing the enum value to validate.</param>
    /// <param name="message">Optional custom error message.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the enum value is not defined in the enum type.</exception>
    public static Enforce<TEnum> Defined<TEnum>(
        this Enforce<TEnum> enforce,
        string? message = null)
        where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(enforce.Value))
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                enforce.Value,
                message ?? "Value must be a defined enum value");
        }

        return enforce;
    }
}