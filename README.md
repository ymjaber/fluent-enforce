# FluentEnforce

[![NuGet](https://img.shields.io/nuget/v/FluentEnforce.svg)](https://www.nuget.org/packages/FluentEnforce/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

FluentEnforce provides a fluent API for handling unexpected errors with predefined validations. Features flexible custom validations through the `Satisfies` method, enabling clean and readable precondition checks.

## 📦 Installation

```bash
dotnet add package FluentEnforce
```

## 🚀 Features

- **Fluent API** - Chain multiple validations with clear, readable syntax
- **Built-in Validations** - Common validations out of the box
- **Custom Validations** - Flexible `Satisfies` method for custom rules
- **Detailed Error Messages** - Clear messages for debugging
- **Type-Safe** - Full IntelliSense support
- **Lightweight** - No external dependencies

## 📖 Usage

### Basic Validations

```csharp
using FluentEnforce;

// Simple null check
Enforce.That(value).IsNotNull();

// String validations
Enforce.That(email).IsNotNullOrWhiteSpace();
Enforce.That(name).HasMinLength(2);
Enforce.That(code).HasMaxLength(10);
Enforce.That(id).Matches(@"^\d{4}-\d{4}$");

// Numeric validations
Enforce.That(age).IsGreaterThan(0);
Enforce.That(price).IsGreaterThanOrEqualTo(0);
Enforce.That(discount).IsLessThan(100);
Enforce.That(percentage).IsInRange(0, 100);

// Collection validations
Enforce.That(items).IsNotEmpty();
Enforce.That(list).HasCount(5);
Enforce.That(array).HasMinCount(1);
Enforce.That(collection).HasMaxCount(10);
```

### Custom Validations with Satisfies

```csharp
// Single custom validation
Enforce.That(password)
    .Satisfies(p => p.Length >= 8, "Password must be at least 8 characters");

// Multiple custom validations
Enforce.That(username)
    .Satisfies(u => !string.IsNullOrWhiteSpace(u), "Username cannot be empty")
    .Satisfies(u => u.Length >= 3, "Username must be at least 3 characters")
    .Satisfies(u => u.All(char.IsLetterOrDigit), "Username must be alphanumeric")
    .Satisfies(u => char.IsLetter(u[0]), "Username must start with a letter");

// Complex object validation
Enforce.That(order)
    .IsNotNull()
    .Satisfies(o => o.Items.Any(), "Order must have at least one item")
    .Satisfies(o => o.TotalAmount > 0, "Order total must be positive")
    .Satisfies(o => o.CustomerId != Guid.Empty, "Order must have a valid customer");
```

### Chaining Validations

```csharp
public class UserService
{
    public void CreateUser(string email, string password, int age)
    {
        // Validate all inputs
        Enforce.That(email)
            .IsNotNullOrWhiteSpace()
            .Satisfies(IsValidEmail, "Invalid email format");
            
        Enforce.That(password)
            .IsNotNullOrWhiteSpace()
            .HasMinLength(8)
            .Satisfies(HasUpperCase, "Password must contain uppercase letter")
            .Satisfies(HasLowerCase, "Password must contain lowercase letter")
            .Satisfies(HasDigit, "Password must contain at least one digit")
            .Satisfies(HasSpecialChar, "Password must contain special character");
            
        Enforce.That(age)
            .IsGreaterThanOrEqualTo(18, "Must be 18 or older")
            .IsLessThan(120, "Invalid age");
    }
    
    private bool IsValidEmail(string email) => 
        email.Contains('@') && email.Contains('.');
        
    private bool HasUpperCase(string str) => 
        str.Any(char.IsUpper);
        
    private bool HasLowerCase(string str) => 
        str.Any(char.IsLower);
        
    private bool HasDigit(string str) => 
        str.Any(char.IsDigit);
        
    private bool HasSpecialChar(string str) => 
        str.Any(c => !char.IsLetterOrDigit(c));
}
```

### Guard Clauses in Constructors

```csharp
public class Product
{
    public string Name { get; }
    public decimal Price { get; }
    public int StockQuantity { get; }
    
    public Product(string name, decimal price, int stockQuantity)
    {
        Name = Enforce.That(name)
            .IsNotNullOrWhiteSpace()
            .HasMinLength(3)
            .Value;
            
        Price = Enforce.That(price)
            .IsGreaterThan(0, "Price must be positive")
            .Value;
            
        StockQuantity = Enforce.That(stockQuantity)
            .IsGreaterThanOrEqualTo(0, "Stock cannot be negative")
            .Value;
    }
}
```

### Method Parameter Validation

```csharp
public class OrderService
{
    public Order PlaceOrder(Customer customer, List<OrderItem> items, Address shippingAddress)
    {
        Enforce.That(customer).IsNotNull();
        Enforce.That(customer.Id).Satisfies(id => id != Guid.Empty, "Invalid customer ID");
        
        Enforce.That(items)
            .IsNotNull()
            .IsNotEmpty()
            .Satisfies(list => list.All(item => item.Quantity > 0), 
                      "All items must have positive quantity");
        
        Enforce.That(shippingAddress).IsNotNull();
        Enforce.That(shippingAddress.ZipCode).Matches(@"^\d{5}(-\d{4})?$");
        
        // Process order...
    }
}
```

## 🏗️ Architecture Guidelines

### When to Use FluentEnforce

1. **Constructor Guards** - Ensure object validity at creation
2. **Method Preconditions** - Validate inputs before processing
3. **Domain Invariants** - Enforce business rules
4. **API Validation** - Validate external inputs
5. **Configuration Validation** - Ensure settings are valid

### Best Practices

1. **Fail Fast** - Validate as early as possible
2. **Clear Messages** - Provide descriptive error messages
3. **Chain Related Validations** - Group validations logically
4. **Custom Validations** - Use `Satisfies` for domain-specific rules
5. **Avoid Over-Validation** - Don't validate what's already guaranteed

### Integration with Result Pattern

FluentEnforce is designed for unexpected errors (exceptions), while FluentUnions handles expected errors. Use them together:

```csharp
public Result<User> CreateUser(string email, string password)
{
    try
    {
        // Use FluentEnforce for preconditions (unexpected errors)
        Enforce.That(email).IsNotNullOrWhiteSpace();
        Enforce.That(password).IsNotNullOrWhiteSpace();
        
        // Use Result for business logic (expected errors)
        if (!IsValidEmailFormat(email))
            return Result<User>.Failure("Invalid email format");
            
        if (await UserExists(email))
            return Result<User>.Failure("Email already registered");
        
        var user = new User(email, password);
        return Result<User>.Success(user);
    }
    catch (ArgumentException ex)
    {
        // Unexpected validation failures
        return Result<User>.Failure($"Validation error: {ex.Message}");
    }
}
```

## 🧪 Testing

```csharp
[Test]
public void Enforce_That_ThrowsException_WhenConditionNotMet()
{
    // Arrange
    string? nullValue = null;
    
    // Act & Assert
    Assert.Throws<ArgumentNullException>(() => 
        Enforce.That(nullValue).IsNotNull());
}

[Test]
public void Enforce_Satisfies_ThrowsWithCustomMessage()
{
    // Arrange
    var password = "weak";
    
    // Act & Assert
    var ex = Assert.Throws<ArgumentException>(() => 
        Enforce.That(password)
            .Satisfies(p => p.Length >= 8, "Password too short"));
            
    Assert.That(ex.Message, Contains.Substring("Password too short"));
}
```

## 🔧 Advanced Usage

### Custom Extensions

```csharp
public static class EnforceExtensions
{
    public static IEnforcer<string> IsValidEmail(this IEnforcer<string> enforcer)
    {
        return enforcer.Satisfies(email => 
            !string.IsNullOrWhiteSpace(email) && 
            email.Contains('@') && 
            email.Contains('.'), 
            "Must be a valid email address");
    }
    
    public static IEnforcer<string> IsStrongPassword(this IEnforcer<string> enforcer)
    {
        return enforcer
            .HasMinLength(8)
            .Satisfies(p => p.Any(char.IsUpper), "Must contain uppercase")
            .Satisfies(p => p.Any(char.IsLower), "Must contain lowercase")
            .Satisfies(p => p.Any(char.IsDigit), "Must contain digit")
            .Satisfies(p => p.Any(c => !char.IsLetterOrDigit(c)), "Must contain special character");
    }
}

// Usage
Enforce.That(email).IsValidEmail();
Enforce.That(password).IsStrongPassword();
```

## 📚 Additional Resources

- [Defensive Programming](https://en.wikipedia.org/wiki/Defensive_programming)
- [Guard Clauses](https://refactoring.com/catalog/replaceNestedConditionalWithGuardClauses.html)
- [Fail-Fast Principle](https://en.wikipedia.org/wiki/Fail-fast)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.