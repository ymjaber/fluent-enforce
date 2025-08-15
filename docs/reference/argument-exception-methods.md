# Argument exception methods

Standard .NET argument exceptions for all validations.

## Usage

All methods are available on `Enforce.That(value)` and `value.Enforce()`.

Throws one of: `ArgumentNullException`, `ArgumentException`, `ArgumentOutOfRangeException`, depending on the rule.

```csharp
// Strings
email.EnforceNotNull().NotWhiteSpace().MatchesEmail();
username.Enforce().LongerThanOrEqualsTo(3).ShorterThanOrEqualsTo(20);

// Numbers
total.Enforce().Positive();
age.Enforce().InRange(18, 65);

// Collections
items.EnforceNotNull().NotEmpty().HasMaxCount(100);

// Date and time
scheduledAt.Enforce().InFuture(TimeProvider.System);

// General
color.Enforce().In("red", "green", "blue");
```

Notes:

- Prefer argument exceptions at API boundaries and for guard clauses
- Messages are auto‑generated; pass a message if you need specificity

See: [Cheat sheet](../validation-reference.md) · [Custom exception methods](custom-exception-methods.md)

