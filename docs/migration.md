# Migration guide

Moving to FluentEnforce from other validation libraries? Here's how.

## From manual validation

### Before (manual if-throw)

```csharp
public void ProcessOrder(string orderId, decimal amount, string email)
{
    if (string.IsNullOrWhiteSpace(orderId))
        throw new ArgumentException("Order ID is required", nameof(orderId));

    if (amount <= 0)
        throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive");

    if (amount > 10000)
        throw new ArgumentOutOfRangeException(nameof(amount), "Amount exceeds limit");

    if (string.IsNullOrWhiteSpace(email))
        throw new ArgumentException("Email is required", nameof(email));

    if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        throw new ArgumentException("Invalid email format", nameof(email));
}
```

### After (FluentEnforce)

```csharp
public void ProcessOrder(string orderId, decimal amount, string email)
{
    orderId.EnforceNotNull().NotWhiteSpace();
    amount.Enforce().Positive().LessThanOrEqualsTo(10000);
    email.EnforceNotNull().NotWhiteSpace().MatchesEmail();
}
```

## From Ardalis.GuardClauses

FluentEnforce was inspired by `GuardClauses` but offers a fluent API and checks for rules enforcement (positive side) rather than errors (negative sides):
| Note: If you prefer to check for the negative side, then I advice you to stick with the GuardClauses library. FluentEnforce adea came from my other library `FluentUnions`, to maintain consistency with the `Ensure` (Result<T>) and `Filter` (Option<T>) methods.

### GuardClauses

```csharp
public class UserService
{
    public void CreateUser(string email, int age, string password)
    {
        Guard.Against.NullOrWhiteSpace(email, nameof(email));
        Guard.Against.InvalidEmail(email, nameof(email));

        Guard.Against.OutOfRange(age, nameof(age), 18, 100);

        Guard.Against.NullOrWhiteSpace(password, nameof(password));
        Guard.Against.LengthOutOfRange(password, nameof(password), 8, 100);
    }
}
```

### FluentEnforce

```csharp
public class UserService
{
    public void CreateUser(string email, int age, string password)
    {
        email.EnforceNotNull()
            .NotWhiteSpace()
            .MatchesEmail();

        age.Enforce().InRange(18, 100);

        password.EnforceNotNull()
            .NotWhiteSpace()
            .LongerThanOrEqualsTo(8)
            .ShorterThanOrEqualsTo(100);
    }
}
```

## From FluentValidation

FluentValidation is for model validation; FluentEnforce is for parameter validation:

### FluentValidation (model validation)

```csharp
public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Age)
            .InclusiveBetween(18, 100);
    }
}

// Usage
var validator = new CustomerValidator();
var result = validator.Validate(customer);
if (!result.IsValid)
{
    // Handle errors
}
```

### FluentEnforce (parameter validation)

```csharp
public class Customer
{
    private string _name;
    private string _email;
    private int _age;

    public Customer(string name, string email, int age)
    {
        // Immediate validation at boundaries
        _name = name.EnforceNotNull()
            .NotWhiteSpace()
            .LongerThanOrEqualsTo(3)
            .ShorterThanOrEqualsTo(100)
            .Value;

        _email = email.EnforceNotNull()
            .NotWhiteSpace()
            .MatchesEmail()
            .Value;

        _age = age.Enforce()
            .InRange(18, 100)
            .Value;
    }
}
```

### When to use which

**Use FluentValidation when:**

- Validating complex models
- Need validation rules separate from models
- Want to collect all errors
- Building APIs with model validation

**Use FluentEnforce when:**

- Validating method parameters
- Want fail-fast behavior
- Building domain models
- Need inline validation

## From Data Annotations

### Data Annotations

```csharp
public class User
{
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [Range(18, 100)]
    public int Age { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Username { get; set; }
}

// Validation requires separate call
var context = new ValidationContext(user);
var results = new List<ValidationResult>();
if (!Validator.TryValidateObject(user, context, results, true))
{
    // Handle errors
}
```

### FluentEnforce

```csharp
public class User
{
    private string _email;
    private int _age;
    private string _username;

    public string Email
    {
        get => _email;
        set => _email = value.EnforceNotNull()
            .NotWhiteSpace()
            .MatchesEmail()
            .ShorterThanOrEqualsTo(255)
            .Value;
    }

    public int Age
    {
        get => _age;
        set => _age = value.Enforce()
            .InRange(18, 100)
            .Value;
    }

    public string Username
    {
        get => _username;
        set => _username = value.EnforceNotNull()
            .NotWhiteSpace()
            .LongerThanOrEqualsTo(3)
            .ShorterThanOrEqualsTo(100)
            .Value;
    }
}
```

## Migration strategy

### Phase 1: Add FluentEnforce package

```bash
dotnet add package FluentEnforce
```

### Phase 2: Migrate critical paths

Start with constructors and public APIs:

```csharp
public class Order
{
    public Order(int customerId, List<OrderItem> items)
    {
        // Old validation
        // if (customerId <= 0) throw new ArgumentException(...);
        // if (items == null || !items.Any()) throw new ArgumentException(...);

        // New validation
        customerId.Enforce().GreaterThan(0);
        items.EnforceNotNull().NotEmpty();
    }
}
```

### Phase 3: Create custom extensions (Optional - useful when validations happen repeatedly)

Extract repeated patterns:

```csharp
public static class DomainValidations
{
    public static Enforce<decimal> IsValidPrice(this Enforce<decimal> enforce)
    {
        return enforce.Positive().LessThanOrEqualsTo(1_000_000);
    }

    public static Enforce<string> IsValidku(this Enforce<string> enforce)
    {
        return enforce.NotWhiteSpace().Matches(new Regex(@"^[A-Z]{3}-\d{5}$"));
    }
}
```

## Coexistence strategy

You can use FluentEnforce alongside existing validation:

```csharp
public class UserService
{
    public void CreateUser(UserDto dto)
    {
        // FluentEnforce for parameters
        dto.EnforceNotNull();

        // FluentValidation for model
        var validator = new UserDtoValidator();
        var result = validator.Validate(dto);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        // FluentEnforce for domain construction
        var user = new User(
            dto.Email.EnforceNotNull().MatchesEmail().Value,
            dto.Age.Enforce().InRange(18, 100).Value
        );
    }
}
```

## Performance considerations

- FluentEnforce has zero overhead for successful validations
- No reflection or runtime code generation
- Compiled regex patterns for performance
- Consider caching expensive validations during migration

## Common migration issues

### Issue: Parameter names not captured

```csharp
// Problem: nameof() is verbose
Enforce.NotNull(value, nameof(value));

// Solution: CallerArgumentExpression handles it
Enforce.NotNull(value);  // Parameter name captured automatically
```

### Issue: Collecting multiple errors

```csharp
// FluentEnforce is fail-fast by design
// If you need to collect errors:

var errors = new List<string>();

try { email.EnforceNotNull().MatchesEmail(); }
catch (Exception ex) { errors.Add($"Email: {ex.Message}"); }

try { age.Enforce().InRange(18, 100); }
catch (Exception ex) { errors.Add($"Age: {ex.Message}"); }

if (errors.Any())
    throw new ValidationException(string.Join("; ", errors));
```

---

Previous: [Extending FluentEnforce](extending.md) · Next: [Contributing](contributing.md)