# Best practices and code smells

## Core rules

- Validate at boundaries (controllers, public methods, constructors)
- Extract repeated chains into small extension methods
- Order by likelihood and cost: null/empty → format → business rule/I/O
- Pick one API style per codebase (static or extension)

## Messaging

- Prefer short, contextual messages for user/input validation
- Omit messages for internal invariants unless useful for logs

## Nullability and narrowing

- Use `Enforce.NotNull(T?)` or `.EnforceNotNull()` to narrow `T?` → `T`

## Collections

- Validate presence and sizes before content
- Use `AllMatch`/`AnyMatch`/`NoneMatch` for content rules

```csharp
items.EnforceNotNull()
    .NotEmpty()
    .HasMaxCount(100)
    .AllMatch(i => i.IsValid);
```

## Custom predicates and composition

- Prefer `Satisfies` for domain rules; extract common rules into extensions

## Custom exceptions

- Every validation accepts a `Func<Exception>`; use domain exceptions where helpful

```csharp
price.Enforce().Positive(() => new DomainException("Price must be > 0"));
```

## Performance

- Success path is allocation‑free and aggressively inlined
- Built‑in regexes are compiled and cached
- Cache your own expensive checks in hot paths

---

Previous: [Examples](examples.md) · Back to index: [Docs index](README.md)
