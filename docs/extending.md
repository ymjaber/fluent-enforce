# Extending FluentEnforce

FluentEnforce is designed to be extended. Add your own validations that feel native to the library.

## Basic extension method

The simplest way to extend:

```csharp
public static class MyValidations
{
    public static Enforce<string> ValidateUsername(this Enforce<string> enforce)
    {
        return enforce
            .NotWhiteSpace()
            .LongerThanOrEqualsTo(3)
            .ShorterThanOrEqualsTo(20)
            .Matches(new Regex("^[a-zA-Z0-9_]+$"), "Username can only contain letters, numbers, and underscores");
    }
}

// Usage feels native
username.EnforceNotNull().ValidateUsername();
```

## Generic validations

Create validations that work with multiple types:

```csharp
public static class RangeValidations
{
    public static Enforce<T> Between<T>(this Enforce<T> enforce, T min, T max)
        where T : IComparable<T>
    {
        return enforce
            .GreaterThanOrEqualsTo(min)
            .LessThanOrEqualsTo(max);
    }
    
    public static Enforce<T> Outside<T>(this Enforce<T> enforce, T min, T max)
        where T : IComparable<T>
    {
        if (enforce.Value.CompareTo(min) >= 0 && enforce.Value.CompareTo(max) <= 0)
        {
            throw new ArgumentOutOfRangeException(
                enforce.ParamName,
                $"Value must be outside range {min} to {max}");
        }
        return enforce;
    }
}

// Works with any comparable type
age.Enforce().Between(18, 65);
temperature.Enforce().Outside(-10.0, 40.0);
```

## Domain-specific validations

### Financial validations

```csharp
public static class FinancialValidations
{
    public static Enforce<string> ValidateIBAN(this Enforce<string> enforce)
    {
        return enforce
            .NotWhiteSpace()
            .Matches(new Regex(@"^[A-Z]{2}\d{2}[A-Z0-9]+$"))
            .Satisfies(IsValidIBAN(enforce.Value), "Invalid IBAN checksum");
    }
    
    public static Enforce<string> ValidateCreditCard(this Enforce<string> enforce)
    {
        return enforce
            .NotWhiteSpace()
            .Matches(new Regex(@"^\d{13,19}$"))
            .Satisfies(LuhnCheck(enforce.Value), "Invalid credit card number");
    }
    
    public static Enforce<decimal> ValidateCurrency(
        this Enforce<decimal> enforce,
        int decimalPlaces = 2)
    {
        var multiplier = (decimal)Math.Pow(10, decimalPlaces);
        var scaled = enforce.Value * multiplier;
        
        if (scaled != Math.Floor(scaled))
        {
            throw new ArgumentException(
                $"Value can only have {decimalPlaces} decimal places",
                enforce.ParamName);
        }
        
        return enforce.Positive();
    }
    
    private static bool IsValidIBAN(string iban)
    {
        // IBAN validation algorithm
        // Move first 4 chars to end, replace letters with numbers
        // Calculate mod 97, should equal 1
        return true; // Simplified
    }
    
    private static bool LuhnCheck(string number)
    {
        // Luhn algorithm implementation
        return true; // Simplified
    }
}
```

### Geographic validations

```csharp
public static class GeoValidations
{
    public static Enforce<double> ValidateLatitude(this Enforce<double> enforce)
    {
        return enforce.InRange(-90, 90, 
            () => new ArgumentException("Latitude must be between -90 and 90"));
    }
    
    public static Enforce<double> ValidateLongitude(this Enforce<double> enforce)
    {
        return enforce.InRange(-180, 180,
            () => new ArgumentException("Longitude must be between -180 and 180"));
    }
    
    public static Enforce<(double lat, double lon)> ValidateCoordinates(
        this Enforce<(double lat, double lon)> enforce)
    {
        enforce.Value.lat.Enforce().ValidateLatitude();
        enforce.Value.lon.Enforce().ValidateLongitude();
        return enforce;
    }
    
    public static Enforce<string> ValidateCountryCode(
        this Enforce<string> enforce,
        CountryCodeFormat format = CountryCodeFormat.ISO3166_Alpha2)
    {
        return format switch
        {
            CountryCodeFormat.ISO3166_Alpha2 => 
                enforce.HasLength(2).Matches(new Regex("^[A-Z]{2}$")),
            CountryCodeFormat.ISO3166_Alpha3 => 
                enforce.HasLength(3).Matches(new Regex("^[A-Z]{3}$")),
            CountryCodeFormat.ISO3166_Numeric => 
                enforce.HasLength(3).Matches(new Regex("^\\d{3}$")),
            _ => enforce
        };
    }
}

public enum CountryCodeFormat
{
    ISO3166_Alpha2,  // US, GB, CN
    ISO3166_Alpha3,  // USA, GBR, CHN
    ISO3166_Numeric  // 840, 826, 156
}
```

## Async validations

For validations that require I/O:

```csharp
public static class AsyncValidations
{
    public static async Task<Enforce<string>> ValidateEmailNotExistsAsync(
        this Enforce<string> enforce,
        IUserRepository repository)
    {
        enforce.MatchesEmail();
        
        var exists = await repository.EmailExistsAsync(enforce.Value);
        if (exists)
        {
            throw new ConflictException($"Email {enforce.Value} already exists");
        }
        
        return enforce;
    }
    
    public static async Task<Enforce<string>> ValidateUrlAccessibleAsync(
        this Enforce<string> enforce,
        HttpClient httpClient,
        TimeSpan? timeout = null)
    {
        enforce.MatchesUrl();
        
        using var cts = new CancellationTokenSource(timeout ?? TimeSpan.FromSeconds(5));
        
        try
        {
            var response = await httpClient.GetAsync(enforce.Value, cts.Token);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new ValidationException(
                $"URL {enforce.Value} is not accessible: {ex.Message}");
        }
        
        return enforce;
    }
}

// Usage
await email.EnforceNotNull().ValidateEmailNotExistsAsync(repository);
await url.EnforceNotNull().ValidateUrlAccessibleAsync(httpClient);
```

## Conditional validations

```csharp
public static class ConditionalValidations
{
    public static Enforce<T> When<T>(
        this Enforce<T> enforce,
        bool condition,
        Action<Enforce<T>> validation)
    {
        if (condition)
        {
            validation(enforce);
        }
        return enforce;
    }
    
    public static Enforce<T> Unless<T>(
        this Enforce<T> enforce,
        bool condition,
        Action<Enforce<T>> validation)
    {
        if (!condition)
        {
            validation(enforce);
        }
        return enforce;
    }
    
    public static Enforce<T> RequiredWhen<T>(
        this Enforce<T?> enforce,
        bool condition)
        where T : class
    {
        if (condition && enforce.Value == null)
        {
            throw new ArgumentNullException(enforce.ParamName, "Value is required");
        }
        return new Enforce<T>(enforce.Value!, enforce.ParamName);
    }
}

// Usage
email.Enforce()
    .When(isNewUser, e => e.MatchesEmail())
    .Unless(isAdmin, e => e.NotContains("admin"));

optionalField.RequiredWhen(isRequired)
    .NotWhiteSpace();
```

## Composite validations

```csharp
public static class CompositeValidations
{
    public static Enforce<T> ValidateAll<T>(
        this Enforce<T> enforce,
        params Action<Enforce<T>>[] validations)
    {
        foreach (var validation in validations)
        {
            validation(enforce);
        }
        return enforce;
    }
    
    public static Enforce<T> ValidateAny<T>(
        this Enforce<T> enforce,
        params Action<Enforce<T>>[] validations)
    {
        var exceptions = new List<Exception>();
        
        foreach (var validation in validations)
        {
            try
            {
                validation(enforce);
                return enforce; // At least one passed
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }
        
        throw new AggregateException(
            "None of the validations passed", exceptions);
    }
}

// Usage
value.Enforce().ValidateAll(
    e => e.NotEmpty(),
    e => e.LongerThan(5),
    e => e.Matches(new Regex("[A-Z]"))
);

identifier.Enforce().ValidateAny(
    e => e.MatchesEmail(),
    e => e.MatchesPhoneNumber(),
    e => e.Matches(new Regex(@"^\\d{9}$")) // SSN
);
```

## Custom validation attributes

```csharp
[AttributeUsage(AttributeTargets.Method)]
public class ValidatesAttribute : Attribute
{
    public Type[] Types { get; }
    public string Description { get; }
    
    public ValidatesAttribute(string description, params Type[] types)
    {
        Description = description;
        Types = types;
    }
}

public static class DocumentedValidations
{
    [Validates("Validates strong passwords", typeof(string))]
    public static Enforce<string> ValidateStrongPassword(
        this Enforce<string> enforce)
    {
        return enforce
            .NotWhiteSpace()
            .LongerThanOrEqualsTo(12)
            .Matches(new Regex("(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])"),
                "Password must contain lowercase, uppercase, number, and special character");
    }
}
```

## Testing custom validations

```csharp
[TestFixture]
public class CustomValidationTests
{
    [TestCase("user123", true)]
    [TestCase("us", false)]  // Too short
    [TestCase("user@name", false)]  // Invalid character
    [TestCase("verylongusernamethatexceedslimit", false)]  // Too long
    public void ValidateUsername_ValidatesCorrectly(string username, bool shouldPass)
    {
        if (shouldPass)
        {
            Assert.DoesNotThrow(() => 
                username.Enforce().ValidateUsername());
        }
        else
        {
            Assert.Throws<ArgumentException>(() => 
                username.Enforce().ValidateUsername());
        }
    }
}
```

## Best practices for extensions

1. **Follow naming conventions** - Start with `Validate` for complex validations
2. **Return Enforce<T>** - Enable chaining
3. **Use generic constraints** - Make validations type-safe
4. **Provide good messages** - Include context in error messages
5. **Consider performance** - Cache regex patterns, avoid unnecessary allocations
6. **Document thoroughly** - Explain what validation does and why
7. **Test edge cases** - Ensure validations work correctly
8. **Keep it focused** - One validation per method

---

Previous: [Custom exceptions](custom-exceptions.md) · Next: [Migration](migration.md)