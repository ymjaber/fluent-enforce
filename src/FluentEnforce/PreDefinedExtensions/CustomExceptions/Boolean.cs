namespace FluentEnforce;

public static partial class BooleanExtensions
{
    public static void True(
        this Enforce<bool> enforce,
        Func<Exception> exception)
    {
        if (!enforce.Value)
        {
            throw exception();
        }
    }
    
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