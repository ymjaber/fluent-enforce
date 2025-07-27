# FluentEnforce

[![NuGet](https://img.shields.io/nuget/v/FluentEnforce.svg)](https://www.nuget.org/packages/FluentEnforce/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

FluentEnforce is a lightweight and expressive library for parameter validation in C#/.NET that provides a fluent API for enforcing preconditions. It enables defensive programming practices with readable validation chains, comprehensive built-in validations for common types, and support for both standard exceptions and custom exception types. Write cleaner, more maintainable code by validating inputs at method boundaries with minimal boilerplate.

## Why FluentEnforce?

Traditional parameter validation in C# often leads to:

- **Verbose boilerplate** - Repetitive if-throw statements clutter your methods
- **Inconsistent validation** - Different developers write checks differently
- **Poor readability** - Intent gets lost in implementation details
- **Scattered validation logic** - No central place for common validations
- **Missing edge cases** - Easy to forget important checks

FluentEnforce solves these problems by providing a consistent, readable, and comprehensive validation framework.

## Key Features

- **🎯 Fluent API** - Chain validations for expressive, readable code
- **💯 Comprehensive Validations** - Built-in checks for strings, numbers, dates, GUIDs, and more
- **🛡️ Defensive Programming** - Fail fast at method boundaries
- **🔗 Method Chaining** - Combine multiple validations seamlessly
- **⚡ Zero Overhead** - Optimized with aggressive inlining
- **🎨 Extensible** - Easy to add custom validations
- **📝 Automatic Parameter Names** - Uses CallerArgumentExpression for better error messages
- **🔄 Dual Exception Support** - Use ArgumentException or custom exceptions
- **✨ Clean Syntax** - Minimal ceremony, maximum clarity
- **🚀 Performance Focused** - Designed for high-throughput scenarios

## Installation

### Core Package

```bash
dotnet add package FluentEnforce
```

Or via Package Manager:

```powershell
Install-Package FluentEnforce
```

## Quick Start

### Basic Parameter Validation

```csharp
using FluentEnforce;

public class UserService
{
    public User CreateUser(string email, string password, int age)
    {
        // Validate all parameters with fluent chains
        Enforce.NotNullOrWhiteSpace(email)
            .MatchesEmail();

        Enforce.NotNullOrWhiteSpace(password)
            .LongerThanOrEqualTo(8)
            .Contains("@", "Password must contain @");

        Enforce.That(age)
            .GreaterThanOrEqualTo(18)
            .LessThan(150);

        return new User(email, password, age);
    }
}
```

### Constructor Validation

```csharp
public class Email
{
    public string Value { get; }

    public Email(string value)
    {
        Value = Enforce.NotNullOrWhiteSpace(value)
            .MatchesEmail()
            .ShorterThanOrEqualTo(255)
            .Value; // Implicit conversion returns the validated value
    }
}
```

### Custom Predicates

```csharp
public void ProcessOrder(Order order, decimal discount)
{
    Enforce.NotNull(order)
        .Satisfies(o => o.Items.Any(), "Order must have items")
        .Satisfies(o => o.Total > 0, "Order total must be positive");

    Enforce.That(discount)
        .InRange(0, 100)
        .Satisfies(d => d % 5 == 0, "Discount must be in increments of 5");
}
```

### Custom Exceptions

```csharp
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public void ValidateEmail(string email)
{
    // Use custom exception instead of ArgumentException
    Enforce.That(email)
        .Satisfies(e => !string.IsNullOrWhiteSpace(e),
            () => new DomainException("Email is required"))
        .Satisfies(e => e.Contains("@"),
            () => new DomainException("Invalid email format"));
}
```

## Common Patterns

### 1. Method Parameter Validation

```csharp
public async Task<User> GetUserAsync(int userId, bool includeDetails)
{
    Enforce.That(userId).GreaterThan(0);

    var user = await repository.GetByIdAsync(userId);
    return includeDetails ? await LoadDetailsAsync(user) : user;
}
```

### 2. Domain Model Validation

```csharp
public class Product
{
    public string Name { get; }
    public decimal Price { get; }
    public int StockLevel { get; }

    public Product(string name, decimal price, int stockLevel)
    {
        Name = Enforce.NotNullOrWhiteSpace(name)
            .ShorterThanOrEqualTo(100)
            .Value;

        Price = Enforce.That(price)
            .GreaterThan(0)
            .LessThanOrEqualTo(1_000_000)
            .Value;

        StockLevel = Enforce.That(stockLevel)
            .GreaterThanOrEqualTo(0)
            .Value;
    }
}
```

### 3. API Input Validation

```csharp
[HttpPost]
public IActionResult CreateAccount([FromBody] CreateAccountRequest request)
{
    Enforce.NotNull(request);

    Enforce.NotNullOrWhiteSpace(request.Username)
        .LongerThanOrEqualTo(3)
        .ShorterThanOrEqualTo(20)
        .Matches("^[a-zA-Z0-9_]+$", "Username can only contain letters, numbers, and underscores");

    Enforce.NotNullOrWhiteSpace(request.Email)
        .MatchesEmail();

    Enforce.NotNullOrWhiteSpace(request.Password)
        .LongerThanOrEqualTo(8)
        .Matches("[A-Z]", "Password must contain uppercase letter")
        .Matches("[0-9]", "Password must contain number");

    // Process the valid request
    return Ok(accountService.CreateAccount(request));
}
```

### 4. Collection Validation

```csharp
public void ProcessItems<T>(IEnumerable<T> items, int batchSize)
{
    Enforce.NotNull(items)
        .NotEmpty()
        .Satisfies(i => i.Count() <= 1000, "Too many items to process");

    Enforce.That(batchSize)
        .InRange(1, 100);

    // Process items in batches
}
```

## Predefined Validations

FluentEnforce provides extensive built-in validations:

### String Validations

- `NotEmpty()`, `Empty()`, `HasLength()`, `LongerThan()`, `ShorterThan()`
- `Contains()`, `StartsWith()`, `EndsWith()`
- `Matches()` for regex patterns
- `MatchesEmail()`, `MatchesUrl()`, `MatchesPhoneNumber()`, `MatchesGuid()`

### Numeric Validations

- `GreaterThan()`, `LessThan()`, `InRange()`, `NotInRange()`
- `Zero()`, `NotZero()`, `Positive()`, `Negative()`
- `Even()`, `Odd()`, `DivisibleBy()`

### DateTime Validations

- `Past()`, `Future()`, `Today()`, `NotToday()`
- `After()`, `Before()`, `InRange()`
- `Weekday()`, `Weekend()`

### Boolean Validations

- `True()`, `False()`

### Enum Validations

- `Defined()`, `NotDefined()`
- `HasFlag()`, `NotHaveFlag()`

### Collection Validations

- `Empty()`, `NotEmpty()`
- `MinCount()`, `MaxCount()`, `ExactCount()`

## Best Practices

1. **Validate Early**

    ```csharp
    public void ProcessData(string input, int count)
    {
        // Validate at method entry
        Enforce.NotNullOrWhiteSpace(input);
        Enforce.That(count).Positive();

        // Now safe to use parameters
    }
    ```

2. **Use Descriptive Messages**

    ```csharp
    Enforce.That(age)
        .GreaterThanOrEqualTo(18, "User must be 18 or older")
        .LessThan(100, "Invalid age provided");
    ```

3. **Chain Related Validations**

    ```csharp
    Enforce.NotNullOrWhiteSpace(email)
        .MatchesEmail()
        .NotContain("tempmail", "Temporary emails not allowed")
        .ShorterThanOrEqualTo(255);
    ```

4. **Extract Complex Validations**

    ```csharp
    public static Enforce<string> ValidatePassword(this Enforce<string> enforce)
    {
        return enforce
            .NotEmpty()
            .LongerThanOrEqualTo(8)
            .Matches("[A-Z]", "Must contain uppercase")
            .Matches("[a-z]", "Must contain lowercase")
            .Matches("[0-9]", "Must contain number");
    }

    // Usage
    Enforce.That(password).ValidatePassword();
    ```

## Performance

FluentEnforce is designed for high performance:

- All validation methods use `AggressiveInlining` for zero-overhead abstractions
- Predefined regex patterns are compiled and cached
- No allocations for successful validations
- Minimal overhead compared to manual if-throw statements

## Real-World Example

```csharp
public class OrderService
{
    private readonly IOrderRepository repository;
    private readonly IPaymentService paymentService;

    public async Task<Order> CreateOrderAsync(
        int customerId,
        List<OrderItem> items,
        string promoCode = null)
    {
        // Validate inputs
        Enforce.That(customerId).GreaterThan(0);

        Enforce.NotNull(items)
            .NotEmpty("Order must contain at least one item")
            .Satisfies(i => i.All(item => item.Quantity > 0),
                "All items must have positive quantity")
            .Satisfies(i => i.Sum(item => item.Price * item.Quantity) <= 10_000,
                "Order total exceeds maximum allowed");

        if (promoCode != null)
        {
            Enforce.That(promoCode)
                .Matches("^[A-Z0-9]{4,10}$", "Invalid promo code format");
        }

        // Business logic with validated inputs
        var order = new Order(customerId);
        foreach (var item in items)
        {
            order.AddItem(item);
        }

        if (promoCode != null)
        {
            await ApplyPromoCodeAsync(order, promoCode);
        }

        return await repository.SaveAsync(order);
    }
}
```

## Documentation

For comprehensive documentation, visit the [docs](./docs) folder:

- [Getting Started Guide](./docs/getting-started/quick-start.md)
- [Core Concepts](./docs/core-concepts/)
- [API Reference](./docs/api-reference/)
- [Examples](./docs/examples/)
- [Best Practices](./docs/guides/best-practices.md)

## Contributing

We welcome contributions! Please see our [Contributing Guide](./docs/contributing.md) for details.

## Acknowledgments

This library was inspired by **[Ardalis.GuardClauses](https://github.com/ardalis/GuardClauses)** by Steve Smith.
