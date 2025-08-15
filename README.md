# FluentEnforce

![FluentEnforce](https://raw.githubusercontent.com/ymjaber/fluent-enforce/main/assets/logo.png)

[![NuGet](https://img.shields.io/nuget/v/FluentEnforce.svg?style=for-the-badge)](https://www.nuget.org/packages/FluentEnforce/)
[![Downloads](https://img.shields.io/nuget/dt/FluentEnforce.svg?style=for-the-badge)](https://www.nuget.org/packages/FluentEnforce/)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg?style=for-the-badge)](https://github.com/ymjaber/fluent-enforce/blob/main/LICENSE)
[![GitHub](https://img.shields.io/badge/GitHub-ymjaber%2Ffluent--enforce-181717?logo=github&style=for-the-badge)](https://github.com/ymjaber/fluent-enforce)

Lightweight, expressive parameter validation for .NET: 100+ built-in validations, fluent API, extensible architecture. Fail fast at method boundaries with zero boilerplate.

## Table of contents

- [Why FluentEnforce](#why-fluentenforce)
- [Install](#install)
- [Quick tour](#quick-tour)
- [Validation categories](#validation-categories)
- [Documentation](#documentation)
- [Links](#links)

## Why FluentEnforce

- **Readable validation chains**: `email.EnforceNotNull().NotWhiteSpace().MatchesEmail()`
- **100+ unique built-in checks**: strings, numbers, dates, collections, enums, GUIDs, and more
- **Dual exception modes**: standard `ArgumentException` (and its inherited types) or your custom exceptions
- **Method boundary defense**: validate early, fail fast
- **Zero overhead**: aggressive inlining (This is removed for now, for more optimization in a later subversion, but the JIT is already smart enough to handle those methods), compiled regex patterns
- **AOT-friendly and fast**: AOT compatibility is enabled, optimized equality, no runtime code emission or reflection

## Install

```bash
dotnet add package FluentEnforce
```

Targets: `net9.0`.

## Quick tour

### Basic validation

```csharp
// Static approach
Enforce.NotNull(email)
    .NotWhiteSpace()
    .MatchesEmail();

Enforce.That(itemsList)
    .HasMinCount(minItemsToShip)
    .AllMatch(item => item.CanBeShipped);

Enforce.That(endDate).GreaterThan(startDate);

// Extension approach (equivalent)
email.EnforceNotNull()
    .NotWhiteSpace()
    .MatchesEmail();

itemsList.Enforce()
    .HasMinCount(minItemsToShip)
    .AllMatch(item => item.CanBeShipped);

endDate.Enforce().GreaterThan(startDate);
```

> Note: For educational purposes, you will see me switch between using the regular static approach and extension approach. For readability, it's preferable to stick to one approach in a single code block. More about this in the documentation, in the `Best Practices` section.

### Constructor defense

```csharp
public sealed class Email
{
    public string Value { get; }

    public Email(string value)
    {
        Value = Enforce.NotNull(value)
            .NotWhiteSpace()
            .MatchesEmail()
            .Value; // Get validated value back
            // [Note that `.Value` is optional. The Enforce type already has implicit conversion to the wrapped type]
    }
}
```

### Custom exceptions

```csharp
email.EnforceNotNull(() => new DomainException("Email required"))
    .MatchesEmail(() => new DomainException("Invalid email format"));
```

### Rich validations

```csharp
// Numbers
Enforce.That(age).InRange(18, 100, RangeBounds.LeftInclusive);
Enforce.That(price).Positive().LessThan(1000);

// Collections
Enforce.NotNull(items).NotEmpty().HasMinCount(1).HasMaxCount(10);

// Dates
Enforce.That(birthDate).InPast();
Enforce.That(appointment).InFuture();

// Extensible (For repeated validations)
Enforce.That(order1).Valid(); // And we define the extension Valid() method below
Enforce.That(order2).Valid();

static class OrderEnforceExtensions
{
    public static Enforce<Order> Valid(this Enforce<Order> enforce)
    {
        var order = enforce.Value;
        var paramName = enforce.ParamName;

        order.Items.Enforce(paramName).NotEmpty();
        order.Total.EnforceNotNull(paramName).Positive();

        return enforce;
    }

    // Alternative approach.
    // The above method is equavalent to:
    public static Enforce<Order> Valid(this Enforce<Order> enforce)
    {
        var order = enforce.Value;
        var paramName = enforce.ParamName;

        if (!order.Items.Any())
            throw new ArgumentException("Order must have items", paramName);

        if(order.Total <= 0)
            throw new ArgumentOutOfRangeException(paramName, order.Total, "Order total must be positive");

        return enforce;
    }
}
```

## Validation categories

### Core types (147 methods)

- **String** (41): length, content, patterns, regex patterns, email/URL/phone/IP/GUID formats
- **Numeric** (15): comparisons, signs, number types, divisibility
- **Collections** (13): count, contained items, uniqueness, predicates (with overloads for the core collection types)
- **DateTime** (8 + 5 numeric comparisons): past/future/today checks, numeric comparisons, with `TimeProvider` support
- **General** (5): equality, null, in/not-in sets (With some overloads)
- **TimeSpan** (6 + 5 numeric comparisons): signs, numeric comparisons
- **Character** (9): letter/digit/whitespace/symbol/special
- **Boolean** (2): true/false
- **GUID** (2): empty/not-empty
- **Enum** (1): defined values

Each validation has both `ArgumentException` and custom exception variants (in addition to overloads for the collection types).

## Documentation

This README stays intentionally brief. Full documentation lives in the docs:

- Index: [docs index](https://github.com/ymjaber/fluent-enforce/tree/main/docs)
- Guide: [docs/guide.md](https://github.com/ymjaber/fluent-enforce/blob/main/docs/guide.md)
- API reference: [docs/validation-reference.md](https://github.com/ymjaber/fluent-enforce/blob/main/docs/validation-reference.md)
- Examples: [docs/examples.md](https://github.com/ymjaber/fluent-enforce/blob/main/docs/examples.md)
- Best practices: [docs/best-practices.md](https://github.com/ymjaber/fluent-enforce/blob/main/docs/best-practices.md)
- International validation: [docs/international.md](https://github.com/ymjaber/fluent-enforce/blob/main/docs/international.md)
- Contributing: [docs/contributing.md](https://github.com/ymjaber/fluent-enforce/blob/main/docs/contributing.md)

## Links

- NuGet: [FluentEnforce on NuGet](https://www.nuget.org/packages/FluentEnforce/)
- Repository: [github.com/ymjaber/fluent-enforce](https://github.com/ymjaber/fluent-enforce)
- License: MIT ([LICENSE](https://github.com/ymjaber/fluent-enforce/blob/main/LICENSE))
