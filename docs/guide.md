# FluentEnforce Guide

If you just want the package: NuGet `FluentEnforce` and you’re set. If you want to understand what you get and how to use it well, read on.

## Table of contents

- [Overview and install](#overview-and-install)
- [Core concepts and entry points](#core-concepts-and-entry-points)
- [API styles and chaining patterns](#api-styles-and-chaining-patterns)
- [Exceptions and messages](#exceptions-and-messages)
- [Strings and text](#strings-and-text)
- [Numbers and ranges](#numbers-and-ranges)
- [Collections and dictionaries](#collections-and-dictionaries)
- [Date and time with TimeProvider](#date-and-time-with-timeprovider)
- [Booleans and general rules](#booleans-and-general-rules)
- [Guid and enums](#guid-and-enums)
- [Custom predicates](#custom-predicates)
- [Integration patterns and best practices](#integration-patterns-and-best-practices)
- [Performance notes](#performance-notes)
- [Extensibility](#extensibility)
- [Migration](#migration)
- [Real‑world example](#real-world-example)

---

## Overview and install

FluentEnforce provides fast, fluent validation for .NET with two ergonomic styles:

- Static: `Enforce.That(value)` / `Enforce.NotNull(value)`
- Extension: `value.Enforce()` / `value.EnforceNotNull()`

Install:

```bash
dotnet add package FluentEnforce
```

Targets: app `net9.0`.

Notes:

- All methods are allocation‑free on the success path and aggressively inlined
- No analyzers or source generators required
- Full method list: see the [API reference (cheat sheet)](validation-reference.md)

---

## Core concepts and entry points

- `Enforce<T>` wraps a value and exposes fluent validations
- Choose one style per codebase (static or extension) to keep usage consistent

Entry points:

```csharp
var nonNull  = Enforce.NotNull(input); // narrows T? → T
var builder  = Enforce.That(value);    // Enforce<T>

// Extension equivalents
var alsoNull = input.EnforceNotNull();
var alsoBld  = value.Enforce();
```

Edge cases:

- Null‑narrowing helpers return `Enforce<T>` so you can keep chaining
- Prefer short, contextual messages for external inputs; omit for internal invariants

---

## API styles and chaining patterns

- Pick one style (static or extension) and stick to it project‑wide
- Prefer ordering: presence → size/shape → content → business rules

Examples:

```csharp
// Static style
Enforce.NotNull(dto.Email)
    .NotWhiteSpace()
    .MatchesEmail()
    .ShorterThanOrEqualsTo(255);

// Extension style
dto.Items.EnforceNotNull()
    .NotEmpty()
    .HasMaxCount(100)
    .AllMatch(i => i.Quantity > 0);
```

---

## Exceptions and messages

All validations support two variants:

- Argument exceptions (default): throws `ArgumentNullException`, `ArgumentException`, or `ArgumentOutOfRangeException`
- Custom exception: pass a `Func<Exception>` factory; nothing allocates on the success path

```csharp
// Boundary validation (argument exceptions)
id.Enforce().GreaterThan(0);

// Domain/business rule (custom exception)
price.Enforce().Positive(() => new DomainException("Price must be > 0"));
```

Guidance:

- Use argument exceptions at API boundaries and guard clauses
- Use domain exceptions for business rules and cross‑aggregate policies

See: [Argument exception methods](reference/argument-exception-methods.md), [Custom exception methods](reference/custom-exception-methods.md)

---

## Strings and text

Common patterns:

```csharp
email.EnforceNotNull()
    .NotWhiteSpace()
    .MatchesEmail()
    .ShorterThanOrEqualsTo(255);

username.Enforce()
    .LongerThanOrEqualsTo(3)
    .ShorterThanOrEqualsTo(20)
    .Matches(new Regex("^[a-zA-Z0-9_]+$"), "Only letters, numbers, underscores")
    .NotMatches(new Regex("^[0-9]"), "Cannot start with a number");

url.Enforce().MatchesUrl();
phone.Enforce().MatchesPhoneNumber();
```

Notes:

- Prefer `NotWhiteSpace()` over `NotEmpty()` for user inputs
- Use case‑insensitive variants for user‑facing checks (`ContainsIgnoreCase`, etc.)
- Full list: see [Strings](validation-reference.md#strings)

---

## Numbers and ranges

```csharp
age.Enforce().InRange(18, 65);
total.Enforce().Positive().LessThan(10_000);

quantity.Enforce().Even();
percent.Enforce().InRange(0m, 100m);
```

Notes:

- Comparison methods work with any `IComparable<T>` (e.g., `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeOnly`, `TimeSpan`)
- Use `InRange(min, max, bounds)` to control inclusivity
- Full list: see [Numbers](validation-reference.md#numbers)

---

## Collections and dictionaries

```csharp
items.EnforceNotNull()
    .NotEmpty()
    .HasMaxCount(100)
    .AllMatch(i => i != null);

dict.Enforce().ContainsKey("id");
```

Notes:

- Validate presence and size before content
- Use `Unique()` to enforce no duplicates
- Full list: see [Collections](validation-reference.md#collections)

---

## Date and time with TimeProvider

Use `TimeProvider` for deterministic, testable time comparisons.

```csharp
var clock = TimeProvider.System; // or a fake in tests

// DateTime uses UTC comparison
appointment.Enforce().InFuture(clock);

// DateOnly / TimeOnly use local date/time from the provider
startDate.Enforce().InFutureOrPresent(clock);
cutoffTime.Enforce().InPast(clock);
```

Notes:

- `DateTime` values are compared in UTC (`ToUniversalTime()` when needed)
- `DateOnly`/`TimeOnly` use `GetLocalNow()` to compute today/now
- You can also use `GreaterThan`/`LessThan`/`InRange` for temporal comparisons

---

## Booleans and general rules

```csharp
isEnabled.Enforce().True();

color.Enforce().In("red", "green", "blue");
status.Enforce().NotEqualsTo(Status.Inactive);

string? s = null;
Enforce.That(s).Null();
```

Notes:

- `In`/`NotIn` accept params or any `IEnumerable<T>`
- Use `Null()` for reference‑type null checks when you don’t need null‑narrowing

---

## Guid and enums

```csharp
id.Enforce().NotEmpty();
orderStatus.Enforce().Defined();
```

Notes:

- `Defined()` validates enum values against declared members

---

## Custom predicates

```csharp
// Simple predicate
value.Enforce().Satisfies(value % 2 == 0, "Must be even");

// Complex business rule
order.Enforce().Satisfies(
    order.Items.Sum(i => i.Quantity * i.Price) == order.Total,
    "Order total doesn't match items");
```

Guidance:

- Prefer specific validations when available; use `Satisfies` for bespoke rules

---

## Integration patterns and best practices

Validate at boundaries (controllers, public methods, constructors). Extract repeated chains into small helpers or extension methods.

```csharp
public static class ValidationExtensions
{
    public static Enforce<string> EnforceEmail(this string? value) =>
        value.EnforceNotNull().NotWhiteSpace().MatchesEmail().ShorterThanOrEqualsTo(255);
}
```

Tips:

- Keep chains short and intention‑revealing
- Surface domain rules with custom exceptions
- Use the cheat sheet to discover available methods: [validation-reference.md](validation-reference.md)

See also: `docs/best-practices.md`

---

## Performance notes

- Success path is allocation‑free; exception factories execute only on failure
- Methods are aggressively inlined; avoid unnecessary captures in lambdas
- Prefer pre‑compiled `Regex` for hot paths

See: `docs/performance.md`

---

## Extensibility

- Add project‑specific helper chains with extension methods
- Author custom validations when needed

See: `docs/extension-methods.md`, `docs/extending.md`

---

## Migration

Guidance for moving from other validation libraries and guard helpers.

See: `docs/migration.md`

---

## Real‑world example

```csharp
public sealed class OrderRequest
{
    public int CustomerId { get; init; }
    public IReadOnlyList<OrderItem> Items { get; init; } = Array.Empty<OrderItem>();
    public DateTime DeliveryDate { get; init; }
}

public static class OrderValidations
{
    public static void Validate(OrderRequest request, TimeProvider? clock = null)
    {
        request.EnforceNotNull();

        request.CustomerId.Enforce().GreaterThan(0);
        request.Items.EnforceNotNull().NotEmpty().AllMatch(i => i.Quantity > 0);

        var provider = clock ?? TimeProvider.System;
        request.DeliveryDate.Enforce().InFuture(provider);
    }
}
```

---

Next: [API reference (cheat sheet)](validation-reference.md)