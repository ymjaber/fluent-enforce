namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing enum constraints on values with custom exceptions.
/// </summary>
public static partial class EnumExtensions
{
    /// <summary>
    /// Enforces that the enum value is defined in the enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="enforce">The Enforce instance containing the enum value to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the enum value is not defined in the enum type.</exception>
    public static Enforce<TEnum> Defined<TEnum>(
        this Enforce<TEnum> enforce,
        Func<Exception> exception)
        where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(enforce.Value))
        {
            throw exception();
        }

        return enforce;
    }
}
