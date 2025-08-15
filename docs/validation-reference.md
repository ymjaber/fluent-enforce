# API reference (cheat sheet)

Minimal, skimmable list of all built‑in validations.

## Table of contents

- [Entry points and variants](#entry-points-and-variants)
- [Strings](#strings)
- [Numbers](#numbers)
- [Collections](#collections)
- [Date and time](#date-and-time)
- [Characters](#characters)
- [TimeSpan](#timespan)
- [General](#general)
- [Booleans](#booleans)
- [Guid](#guid)
- [Enums](#enums)
- [Custom predicate](#custom-predicate)

## Entry points and variants

- Entry points: `Enforce.That(value)`, `value.Enforce()`, `Enforce.NotNull(value)`, `value.EnforceNotNull()`
- Variants (all methods support both):
  - Argument exceptions: throws .NET argument exceptions
  - Custom exception: accepts `Func<Exception>` factory
  - See: [Argument exception methods](reference/argument-exception-methods.md), [Custom exception methods](reference/custom-exception-methods.md)

## Strings

- Presence: `Empty()`, `NotEmpty()`, `WhiteSpace()`, `NotWhiteSpace()`
- Length: `HasLength(int)`, `LongerThan(int)`, `LongerThanOrEqualsTo(int)`, `ShorterThan(int)`, `ShorterThanOrEqualsTo(int)`
- Content: `Contains(string)`, `NotContains(string)`, `StartsWith(string)`, `NotStartsWith(string)`, `EndsWith(string)`, `NotEndsWith(string)`
- Case‑insensitive: `ContainsIgnoreCase(string)`, `NotContainsIgnoreCase(string)`, `StartsWithIgnoreCase(string)`, `NotStartsWithIgnoreCase(string)`, `EndsWithIgnoreCase(string)`, `NotEndsWithIgnoreCase(string)`
- Pattern: `Matches(Regex)`, `NotMatches(Regex)`
- Formats: `MatchesEmail()`, `MatchesUrl()`, `MatchesPhoneNumber()`, `MatchesGuid()`, `MatchesIpAddress()`
- Character set: `Alphanumeric()`, `Alpha()`, `Numeric()`, `Base64()`

## Numbers

- Comparison (any `IComparable<T>`): `GreaterThan(T)`, `GreaterThanOrEqualsTo(T)`, `LessThan(T)`, `LessThanOrEqualsTo(T)`, `InRange(T min, T max, RangeBounds bounds = Inclusive)`
- Applies to temporal types: `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeOnly`, `TimeSpan`
- Sign: `Positive()`, `Negative()`, `Zero()`, `NonZero()`, `NonPositive()`, `NonNegative()`
- Integer: `Even()`, `Odd()`, `DivisibleBy(int)`, `DivisibleBy(long)`

## Collections

- Presence: `Empty()`, `NotEmpty()`
- Count: `HasCount(int)`, `HasMinCount(int)`, `HasMaxCount(int)`
- Content: `Contains(T)`, `NotContains(T)`
- Predicates: `AllMatch(Func<T,bool>)`, `AnyMatch(Func<T,bool>)`, `NoneMatch(Func<T,bool>)`, `Unique()`
- Dictionary: `ContainsKey(TKey)`, `NotContainsKey(TKey)`, `ContainsValue(TValue)`

## Date and time

- `InFuture(TimeProvider? provider = null)`, `InPast(TimeProvider? provider = null)`
- DateOnly: `InFutureOrPresent()`, `InPastOrPresent()`
- Also supports generic comparisons: `GreaterThan`, `LessThan`, `InRange`

## Characters

- `Letter()`, `Digit()`, `WhiteSpace()`, `Upper()`, `Lower()`, `Punctuation()`, `LetterOrDigit()`, `Symbol()`

## TimeSpan

- `Positive()`, `Negative()`, `Zero()`, `NonZero()`, `NonPositive()`, `NonNegative()`

## General

- Equality: `EqualsTo(T)`, `NotEqualsTo(T)`
- Nullability: `Null()`
- Sets: `In(params T[] | IEnumerable<T>)`, `NotIn(params T[] | IEnumerable<T>)`

## Booleans

- `True()`, `False()`

## Guid

- `Empty()`, `NotEmpty()`

## Enums

- `Defined()`

## Custom predicate

- `Satisfies(bool condition, string? message = null)`

---

See also: [Argument exception methods](reference/argument-exception-methods.md) · [Custom exception methods](reference/custom-exception-methods.md)

Previous: [Guide](guide.md) · Next: [Examples](examples.md)

