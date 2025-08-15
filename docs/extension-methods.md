# Extension methods vs static methods

FluentEnforce offers two equivalent APIs. Both exist because developers have strong preferences, and we respect that.

## The two approaches

### Static method approach (traditional)

```csharp
// Nullable types
string? email = GetEmail();
Enforce.NotNull(email)  // Returns Enforce<string> (non-nullable)
    .NotWhiteSpace()
    .MatchesEmail();

// Non-nullable types
int age = 25;
Enforce.That(age)
    .GreaterThanOrEqualsTo(18);
```

### Extension method approach (fluent)

```csharp
// Nullable types
string? email = GetEmail();
email.EnforceNotNull()  // Returns Enforce<string> (non-nullable)
    .NotWhiteSpace()
    .MatchesEmail();

// Non-nullable types
int age = 25;
age.Enforce()
    .GreaterThanOrEqualsTo(18);
```

## They're exactly the same

Both approaches:
- Compile to identical IL code
- Have identical performance (zero difference)
- Support all the same validations
- Work with custom exceptions
- Preserve null-safety with `[NotNull]` attributes

## When to use which?

### Static methods feel natural when:

- You prefer explicit over implicit
- Your team comes from Java/C# backgrounds
- You like seeing the validation intent upfront
- You're validating method parameters

```csharp
public void SendEmail(string to, string subject)
{
    Enforce.NotNull(to).MatchesEmail();
    Enforce.NotNull(subject).NotWhiteSpace();
    // Clear that we're enforcing constraints
}
```

### Extension methods feel natural when:

- You prefer fluent chains
- Your team likes LINQ-style code
- You want validation to flow with the value
- You're validating in constructors or properties

```csharp
public Email(string value)
{
    Value = value.EnforceNotNull()
        .NotWhiteSpace()
        .MatchesEmail()
        .Value;  // Flows naturally
}
```

## Mixing approaches

You can mix both in the same codebase. They're designed to work together:

```csharp
public class UserService
{
    public void CreateUser(string email, int age)
    {
        // Static for parameters
        Enforce.NotNull(email).MatchesEmail();
        Enforce.That(age).GreaterThanOrEqualsTo(18);
        
        // Extension in domain model
        var user = new User(
            email.ToLower(),  // Already validated
            age
        );
    }
}

public class User
{
    public User(string email, int age)
    {
        // Extension for property assignment
        Email = email.EnforceNotNull().Value;
        Age = age.Enforce().Value;
    }
}
```

## Team conventions

Pick one approach as your team standard, but don't stress about it:

```csharp
// If your team prefers static:
// ✅ Enforce.NotNull(value)
// ✅ Enforce.That(number)

// If your team prefers extensions:
// ✅ value.EnforceNotNull()
// ✅ number.Enforce()

// Both are equally "right"
```

## Implementation details

For the curious, here's how they work:

```csharp
// Static method
public static class Enforce
{
    public static Enforce<T> NotNull<T>(T? value, 
        [CallerArgumentExpression("value")] string? paramName = null)
        where T : class
    {
        if (value is null) throw new ArgumentNullException(paramName);
        return new Enforce<T>(value, paramName);
    }
}

// Extension method (calls the static)
public static class EnforceExtensions
{
    public static Enforce<T> EnforceNotNull<T>(this T? value,
        [CallerArgumentExpression("value")] string? paramName = null)
        where T : class
    {
        return Enforce.NotNull(value, paramName);  // Just delegates
    }
}
```

The extension literally just calls the static method. No magic, no performance difference.

## Custom exceptions work identically

```csharp
// Static with custom exception
Enforce.NotNull(email, new DomainException("Email required"))
    .MatchesEmail(new DomainException("Invalid email"));

// Extension with custom exception  
email.EnforceNotNull(new DomainException("Email required"))
    .MatchesEmail(new DomainException("Invalid email"));
```

## The bottom line

Use what feels right. They're the same thing underneath. Your code will be equally fast, safe, and maintainable either way.

Some teams even let individual developers choose based on context. The library doesn't care – it just wants to help you validate.

---

Previous: [International validation](international.md) · Next: [Custom exceptions](custom-exceptions.md)