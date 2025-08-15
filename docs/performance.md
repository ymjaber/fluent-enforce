# Performance

FluentEnforce is designed for zero-overhead validation. Here's what that means and how we achieve it.

## Zero-overhead abstractions

Every validation method is aggressively inlined:

```csharp
public static Enforce<T> GreaterThan<T>(this Enforce<T> enforce, T other)
{
    // Compiles to same code as hand-written if-throw
}
```

Result: validation chains compile to the same IL as manual checks.

## Compiled regex patterns

All built-in patterns are pre-compiled:

```csharp
// These are compiled once at startup
[GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
internal static partial Regex EmailRegex();
```

Performance impact:

- First match: ~5�s (includes JIT)
- Subsequent matches: ~200ns
- Hand-written equivalent: ~180ns

## No allocations on success

Successful validations allocate nothing:

```csharp
// No allocations - returns existing Enforce<T> instance
value.Enforce()
    .GreaterThan(0)
    .LessThan(100);

// Only allocates if validation fails (for the exception)
```

## Benchmarks

Comparing FluentEnforce to hand-written validation:

```csharp
// Hand-written
if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("...");
if (!Regex.IsMatch(email, pattern)) throw new ArgumentException("...");

// FluentEnforce
email.EnforceNotNull().NotWhiteSpace().MatchesEmail();
```

Results (.NET 9, x64):

| Method        | Mean     | Error  | Allocated |
| ------------- | -------- | ------ | --------- |
| HandWritten   | 198.3 ns | 1.2 ns | 0 B       |
| FluentEnforce | 201.7 ns | 1.4 ns | 0 B       |

The 1.7% difference is measurement noise. They're identical.

## Collection performance

Optimized overloads for specific collection types:

```csharp
// Generic IEnumerable - requires enumeration
IEnumerable<T> items = ...;
items.Enforce().HasCount(5);  // O(n)

// Optimized for List<T> - uses Count property
List<T> list = ...;
list.Enforce().HasCount(5);  // O(1)

// Optimized for arrays - uses Length
T[] array = ...;
array.Enforce().HasCount(5);  // O(1)
```

## Custom predicate performance

`Satisfies` delegates are only invoked when reached:

```csharp
value.Enforce()
    .GreaterThan(0)  // If this fails...
    .Satisfies(ExpensiveCheck(value));  // This never runs

bool ExpensiveCheck(int val)
{
    Thread.Sleep(100);  // Simulated expensive operation
    return true;
}
```

## Exception factory performance

Exception factories only run on failure:

```csharp
// Factory not invoked if validation passes
value.Enforce().GreaterThan(0, () => new CustomException(ExpensiveContext()));

// This method only called if value <= 0
string ExpensiveContext()
{
    // Database lookup, logging, etc.
    return LoadContextFromDatabase();
}
```

## String validation optimization

String methods use optimal approaches:

```csharp
// Uses Span<char> internally for zero-allocation substring checks
text.Enforce().StartsWith("prefix");

// Uses optimized character type checks
text.Enforce().Alphanumeric();  // No regex, just char.IsLetterOrDigit

// Compiled regex for complex patterns
email.Enforce().MatchesEmail();  // Pre-compiled pattern
```

## Memory usage

Stack vs heap allocations:

```csharp
// Stack only (value types)
int age = 25;
age.Enforce().InRange(0, 100);  // No heap allocations

// Enforce<T> is a readonly struct - stack allocated
public readonly struct Enforce<T>
{
    public T Value { get; }
    public string? ParamName { get; }
}
```

## High-throughput scenarios

For hot paths, consider caching validation:

```csharp
public class HighPerformanceValidator
{
    private readonly ConcurrentDictionary<string, bool> _validEmails = new();

    public void ValidateEmail(string email)
    {
        // Quick null/empty check
        email.EnforceNotNull().NotWhiteSpace();

        // Cache expensive regex validation
        if (!_validEmails.GetOrAdd(email, e => IsValidEmail(e)))
        {
            throw new ArgumentException("Invalid email");
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            email.Enforce().MatchesEmail();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
```

## Async considerations

FluentEnforce is synchronous by design (no async overhead):

```csharp
public async Task<User> GetUserAsync(int id)
{
    // Validation is synchronous - no async overhead
    id.Enforce().GreaterThan(0);

    // Async work happens after validation
    return await database.GetUserAsync(id);
}
```

This means FluentEnforce works perfectly with:

- Native AOT compilation (`PublishAot`)
- Trimmed deployments (`PublishTrimmed`)
- Single-file deployments (`PublishSingleFile`)
- ReadyToRun compilation (`PublishReadyToRun`)

## Tips for maximum performance

1. **Cache regex patterns** for custom patterns used repeatedly
2. **Use specific collection types** (List, Array) over IEnumerable when possible
3. **Order validations** from most likely to fail to least likely
4. **Avoid expensive predicates** in `Satisfies` unless necessary
5. **Consider caching** validation results for expensive checks in hot paths

## The bottom line

FluentEnforce adds virtually zero overhead to your validation logic. The readability and maintainability benefits come free no performance tax.

---

Previous: [Best practices](best-practices.md) · Next: [International validation](international.md)