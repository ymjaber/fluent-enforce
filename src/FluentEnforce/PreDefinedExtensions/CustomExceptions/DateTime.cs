namespace FluentEnforce;

public static partial class DateTimeExtensions
{
    public static Enforce<DateTime> InFuture(
        this Enforce<DateTime> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value <= DateTime.Now)
        {
            throw exception();
        }

        return enforce;
    }
    
    public static Enforce<DateOnly> InFuture(
        this Enforce<DateOnly> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value <= DateOnly.FromDateTime(DateTime.Now))
        {
            throw exception();
        }

        return enforce;
    }
    
    public static Enforce<TimeOnly> InFuture(
        this Enforce<TimeOnly> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value <= TimeOnly.FromDateTime(DateTime.Now))
        {
            throw exception();
        }

        return enforce;
    }
    
    public static Enforce<DateTime> InPast(
        this Enforce<DateTime> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value >= DateTime.Now)
        {
            throw exception();
        }

        return enforce;
    }
    
    public static Enforce<DateOnly> InPast(
        this Enforce<DateOnly> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value >= DateOnly.FromDateTime(DateTime.Now))
        {
            throw exception();
        }

        return enforce;
    }
    
    public static Enforce<TimeOnly> InPast(
        this Enforce<TimeOnly> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value >= TimeOnly.FromDateTime(DateTime.Now))
        {
            throw exception();
        }

        return enforce;
    }
    
    public static Enforce<DateOnly> InFutureOrPresent(
        this Enforce<DateOnly> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value < DateOnly.FromDateTime(DateTime.Now))
        {
            throw exception();
        }

        return enforce;
    }
    
    public static Enforce<DateOnly> InPastOrPresent(
        this Enforce<DateOnly> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value > DateOnly.FromDateTime(DateTime.Now))
        {
            throw exception();
        }

        return enforce;
    }
}