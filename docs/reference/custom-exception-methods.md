# Custom exception methods

All validations accept a custom exception via `Func<Exception>`.

## Usage

Pass a factory that returns your exception type. Nothing is allocated on the success path.

```csharp
// Domain rule
price.Enforce().Positive(() => new DomainException("Price must be > 0"));

// Conflict
exists.Enforce().False(() => new ConflictException($"Resource already exists"));

// Import pipeline with coordinates
row.PhoneNumber.Enforce()
    .MatchesPhoneNumber(() => new ImportException(rowNumber, "PhoneNumber", "Invalid format"));
```

Guidance:

- Use domain‑specific exceptions for business rules
- Keep messages short and specific to the failing rule
- Reuse factories for repeated validations

See: [Cheat sheet](../validation-reference.md) · [Argument exception methods](argument-exception-methods.md)

