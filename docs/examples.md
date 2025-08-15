# Real-world examples

Practical validation patterns from production applications.

## Table of contents

- [E-commerce order processing](#e-commerce-order-processing)
- [User registration with multi-tier validation](#user-registration-with-multi-tier-validation)
- [API rate limiting](#api-rate-limiting)
- [File upload validation](#file-upload-validation)
- [Configuration validation](#configuration-validation)
- [Data import validation](#data-import-validation)
- [ASP.NET Minimal API guard endpoints](#aspnet-minimal-api-guard-endpoints)
- [Exception mapping to ProblemDetails](#exception-mapping-to-problemdetails)
- [Constructor guard clauses (domain entities)](#constructor-guard-clauses-domain-entities)
- [Reusable validation extensions](#reusable-validation-extensions)
- [TimeProvider testing](#timeprovider-testing)
- [Options/config validation](#optionsconfig-validation)
- [Enum and Guid quick checks](#enum-and-guid-quick-checks)
- [Normalization + validation](#normalization--validation)
- [Performance: precompiled Regex](#performance-precompiled-regex)

## E-commerce order processing

```csharp
public class OrderService
{
    private readonly IInventoryService _inventory;
    private readonly IPricingService _pricing;
    
    public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
    {
        // Validate customer
        request.CustomerId.Enforce()
            .GreaterThan(0, () => new ValidationException("Invalid customer"));
        
        // Validate items
        request.Items.EnforceNotNull()
            .NotEmpty(() => new BusinessException("Order must have items"))
            .HasMaxCount(50, () => new BusinessException("Too many items in order"))
            .AllMatch(item => item.Quantity > 0, 
                () => new ValidationException("All items must have positive quantity"))
            .AllMatch(item => item.ProductId > 0,
                () => new ValidationException("Invalid product ID"));
        
        // Validate shipping
        request.ShippingAddress.EnforceNotNull();
        ValidateAddress(request.ShippingAddress);
        
        // Validate payment
        ValidatePaymentMethod(request.PaymentMethod);
        
        // Business rule validations
        await ValidateInventory(request.Items);
        ValidatePricing(request);
        
        return await ProcessOrder(request);
    }
    
    private void ValidateAddress(Address address)
    {
        address.Street.EnforceNotNull()
            .NotWhiteSpace()
            .ShorterThanOrEqualsTo(100);
        
        address.City.EnforceNotNull()
            .NotWhiteSpace()
            .ShorterThanOrEqualsTo(50);
        
        address.PostalCode.EnforceNotNull()
            .Matches(new Regex(@"^\d{5}(-\d{4})?$"), "Invalid US postal code");
        
        address.Country.EnforceNotNull()
            .HasLength(2)  // ISO country code
            .Matches(new Regex("^[A-Z]{2}$"));
    }
    
    private void ValidatePaymentMethod(PaymentMethod payment)
    {
        payment.EnforceNotNull();
        
        switch (payment.Type)
        {
            case PaymentType.CreditCard:
                payment.CardNumber.EnforceNotNull()
                    .Matches(new Regex(@"^\d{4}[\s-]?\d{4}[\s-]?\d{4}[\s-]?\d{4}$"))
                    .Satisfies(LuhnCheck(payment.CardNumber), "Invalid credit card number");
                
                payment.ExpiryMonth.Enforce()
                    .InRange(1, 12);
                
                payment.ExpiryYear.Enforce()
                    .GreaterThanOrEqualsTo(DateTime.Now.Year);
                
                payment.Cvv.EnforceNotNull()
                    .Matches(new Regex(@"^\d{3,4}$"));
                break;
                
            case PaymentType.PayPal:
                payment.Email.EnforceNotNull()
                    .MatchesEmail();
                break;
                
            case PaymentType.BankTransfer:
                payment.AccountNumber.EnforceNotNull()
                    .Matches(new Regex(@"^\d{8,12}$"));
                
                payment.RoutingNumber.EnforceNotNull()
                    .Matches(new Regex(@"^\d{9}$"));
                break;
        }
    }
}
```

## User registration with multi-tier validation

```csharp
public class UserRegistrationService
{
    private readonly IUserRepository _repository;
    private readonly IEmailService _emailService;
    
    public async Task<User> RegisterAsync(RegistrationRequest request)
    {
        // Layer 1: Input validation
        ValidateInput(request);
        
        // Layer 2: Format validation
        ValidateFormats(request);
        
        // Layer 3: Business rules
        await ValidateBusinessRules(request);
        
        // Layer 4: Security validation
        ValidateSecurity(request);
        
        // Create user
        var user = new User
        {
            Email = request.Email.ToLowerInvariant(),
            Username = request.Username,
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = NormalizePhoneNumber(request.PhoneNumber)
        };
        
        user.SetPassword(request.Password);
        return await _repository.CreateAsync(user);
    }
    
    private void ValidateInput(RegistrationRequest request)
    {
        request.EnforceNotNull();
        
        request.Email.EnforceNotNull().NotWhiteSpace();
        request.Username.EnforceNotNull().NotWhiteSpace();
        request.Password.EnforceNotNull().NotWhiteSpace();
        request.DateOfBirth.Enforce().InPast();
    }
    
    private void ValidateFormats(RegistrationRequest request)
    {
        // Email format
        request.Email.Enforce()
            .MatchesEmail()
            .ShorterThanOrEqualsTo(255)
            .NotContains("+", "Plus addressing not allowed");
        
        // Username format
        request.Username.Enforce()
            .LongerThanOrEqualsTo(3)
            .ShorterThanOrEqualsTo(20)
            .Matches(new Regex("^[a-zA-Z0-9_]+$"), "Username can only contain letters, numbers, and underscores")
            .NotMatches(new Regex("^[0-9]"), "Username cannot start with a number");
        
        // Phone format (optional)
        if (request.PhoneNumber != null)
        {
            request.PhoneNumber.Enforce()
                .MatchesPhoneNumber();
        }
        
        // Age validation
        var age = DateTime.Today.Year - request.DateOfBirth.Year;
        age.Enforce()
            .GreaterThanOrEqualsTo(13, () => new BusinessException("Must be 13 or older"))
            .LessThan(120, () => new ValidationException("Invalid age"));
    }
    
    private async Task ValidateBusinessRules(RegistrationRequest request)
    {
        // Check email uniqueness
        var emailExists = await _repository.EmailExistsAsync(request.Email);
        emailExists.Enforce()
            .False(() => new ConflictException($"Email {request.Email} is already registered"));
        
        // Check username uniqueness
        var usernameExists = await _repository.UsernameExistsAsync(request.Username);
        usernameExists.Enforce()
            .False(() => new ConflictException($"Username {request.Username} is taken"));
        
        // Check against banned list
        var isBanned = await CheckBannedEmail(request.Email);
        isBanned.Enforce()
            .False(() => new SecurityException("Email domain is not allowed"));
    }
    
    private void ValidateSecurity(RegistrationRequest request)
    {
        // Password strength
        request.Password.Enforce()
            .LongerThanOrEqualsTo(8)
            .ShorterThanOrEqualsTo(128)
            .Matches(new Regex("[A-Z]"), "Password must contain uppercase letter")
            .Matches(new Regex("[a-z]"), "Password must contain lowercase letter")
            .Matches(new Regex("[0-9]"), "Password must contain number")
            .Matches(@"[!@#$%^&*(),.?\"":{}|<>]", "Password must contain special character")
            .NotContains(request.Username, "Password cannot contain username")
            .NotContains(request.Email.Split('@')[0], "Password cannot contain email username");
        
        // Check for common passwords
        IsCommonPassword(request.Password).Enforce()
            .False(() => new SecurityException("Password is too common"));
    }
}
```

## API rate limiting

```csharp
public class RateLimiter
{
    private readonly IMemoryCache _cache;
    
    public void ValidateRequest(string clientId, RateLimitPolicy policy)
    {
        clientId.EnforceNotNull().NotWhiteSpace();
        policy.EnforceNotNull();
        
        // Validate policy
        policy.RequestsPerMinute.Enforce()
            .Positive()
            .LessThanOrEqualsTo(1000);
        
        policy.BurstSize.Enforce()
            .Positive()
            .LessThanOrEqualsTo(policy.RequestsPerMinute);
        
        // Check current usage
        var usage = GetCurrentUsage(clientId);
        
        usage.RequestCount.Enforce()
            .LessThan(policy.RequestsPerMinute,
                () => new RateLimitException($"Rate limit exceeded: {policy.RequestsPerMinute}/min"));
        
        usage.BurstCount.Enforce()
            .LessThan(policy.BurstSize,
                () => new RateLimitException($"Burst limit exceeded: {policy.BurstSize}"));
        
        UpdateUsage(clientId, usage);
    }
}
```

## File upload validation

```csharp
public class FileUploadService
{
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
    private readonly string[] _allowedMimeTypes = { 
        "image/jpeg", "image/png", "image/gif", "application/pdf" 
    };
    
    public async Task<FileUploadResult> UploadFileAsync(IFormFile file, UploadOptions options)
    {
        // Basic validation
        file.EnforceNotNull(() => new ValidationException("No file provided"));
        
        file.Length.Enforce()
            .GreaterThan(0, () => new ValidationException("File is empty"))
            .LessThanOrEqualsTo(options.MaxSizeBytes, 
                () => new ValidationException($"File exceeds maximum size of {options.MaxSizeBytes / 1048576}MB"));
        
        // File name validation
        file.FileName.EnforceNotNull()
            .NotWhiteSpace()
            .ShorterThanOrEqualsTo(255)
            .NotMatches(new Regex(@"[<>:""/\\|?*]"), "File name contains invalid characters");
        
        // Extension validation
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        extension.Enforce()
            .NotEmpty(() => new ValidationException("File must have an extension"))
            .In(_allowedExtensions, 
                () => new ValidationException($"File type {extension} is not allowed"));
        
        // MIME type validation
        file.ContentType.Enforce()
            .In(_allowedMimeTypes,
                () => new SecurityException("Invalid file content type"));
        
        // Content validation (check file signature)
        await ValidateFileContent(file);
        
        // Virus scan
        if (options.RequireVirusScan)
        {
            var scanResult = await ScanForViruses(file);
            scanResult.IsClean.Enforce()
                .True(() => new SecurityException($"File contains malware: {scanResult.ThreatName}"));
        }
        
        return await StoreFile(file, options);
    }
    
    private async Task ValidateFileContent(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var buffer = new byte[8];
        await stream.ReadAsync(buffer, 0, 8);
        
        // Check file signatures (magic numbers)
        var signature = BitConverter.ToString(buffer).Replace("-", "");
        
        var isValid = file.ContentType switch
        {
            "image/jpeg" => signature.StartsWith("FFD8FF"),
            "image/png" => signature.StartsWith("89504E47"),
            "image/gif" => signature.StartsWith("474946"),
            "application/pdf" => signature.StartsWith("255044462D"),
            _ => false
        };
        
        isValid.Enforce()
            .True(() => new SecurityException("File content doesn't match declared type"));
    }
}
```

## Configuration validation

```csharp
public class AppConfiguration
{
    public DatabaseConfig Database { get; set; }
    public ApiConfig Api { get; set; }
    public EmailConfig Email { get; set; }
    
    public void Validate()
    {
        Database.EnforceNotNull();
        ValidateDatabase(Database);
        
        Api.EnforceNotNull();
        ValidateApi(Api);
        
        Email.EnforceNotNull();
        ValidateEmail(Email);
    }
    
    private void ValidateDatabase(DatabaseConfig config)
    {
        config.ConnectionString.EnforceNotNull()
            .NotWhiteSpace()
            .Contains("Server=", "Invalid connection string format")
            .Contains("Database=", "Invalid connection string format");
        
        config.MaxConnections.Enforce()
            .GreaterThan(0)
            .LessThanOrEqualsTo(1000);
        
        config.CommandTimeout.Enforce()
            .GreaterThan(0)
            .LessThanOrEqualsTo(300);
        
        config.RetryCount.Enforce()
            .GreaterThanOrEqualsTo(0)
            .LessThanOrEqualsTo(10);
    }
    
    private void ValidateApi(ApiConfig config)
    {
        config.BaseUrl.EnforceNotNull()
            .NotWhiteSpace()
            .MatchesUrl()
            .EndsWith("/", "API base URL must end with /");
        
        config.ApiKey.EnforceNotNull()
            .NotWhiteSpace()
            .HasLength(32);
        
        config.Timeout.Enforce()
            .GreaterThan(TimeSpan.Zero)
            .LessThanOrEqualsTo(TimeSpan.FromMinutes(5));
        
        config.MaxRetries.Enforce()
            .GreaterThanOrEqualsTo(0)
            .LessThanOrEqualsTo(5);
    }
    
    private void ValidateEmail(EmailConfig config)
    {
        config.SmtpHost.EnforceNotNull()
            .NotWhiteSpace()
            .ShorterThanOrEqualsTo(255);
        
        config.SmtpPort.Enforce()
            .InRange(1, 65535);
        
        config.FromAddress.EnforceNotNull()
            .MatchesEmail();
        
        if (config.RequiresAuthentication)
        {
            config.Username.EnforceNotNull().NotWhiteSpace();
            config.Password.EnforceNotNull().NotWhiteSpace();
        }
        
        config.EnableSsl.Enforce()
            .True(() => new SecurityException("SSL must be enabled for email"));
    }
}
```

## Data import validation

```csharp
public class CsvImporter
{
    public async Task<ImportResult> ImportCustomersAsync(Stream csvStream)
    {
        csvStream.EnforceNotNull();
        csvStream.CanRead.Enforce().True("Stream must be readable");
        
        var result = new ImportResult();
        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        var rowNumber = 0;
        await foreach (var row in csv.GetRecordsAsync<CustomerImportRow>())
        {
            rowNumber++;
            
            try
            {
                ValidateRow(row, rowNumber);
                await ProcessRow(row);
                result.SuccessCount++;
            }
            catch (Exception ex)
            {
                result.Errors.Add(new ImportError
                {
                    Row = rowNumber,
                    Error = ex.Message
                });
            }
        }
        
        // Validate totals
        result.SuccessCount.Enforce()
            .GreaterThan(0, () => new BusinessException("No valid rows imported"));
        
        var errorRate = (double)result.Errors.Count / rowNumber;
        errorRate.Enforce()
            .LessThan(0.1, () => new BusinessException($"Too many errors: {errorRate:P0}"));
        
        return result;
    }
    
    private void ValidateRow(CustomerImportRow row, int rowNumber)
    {
        // Required fields
        row.Email.EnforceNotNull(() => new ImportException(rowNumber, "Email", "Required"))
            .NotWhiteSpace()
            .MatchesEmail(() => new ImportException(rowNumber, "Email", "Invalid format"));
        
        row.FirstName.EnforceNotNull(() => new ImportException(rowNumber, "FirstName", "Required"))
            .NotWhiteSpace()
            .ShorterThanOrEqualsTo(50);
        
        row.LastName.EnforceNotNull(() => new ImportException(rowNumber, "LastName", "Required"))
            .NotWhiteSpace()
            .ShorterThanOrEqualsTo(50);
        
        // Optional fields with validation when present
        if (!string.IsNullOrEmpty(row.PhoneNumber))
        {
            row.PhoneNumber.Enforce()
                .MatchesPhoneNumber(() => new ImportException(rowNumber, "PhoneNumber", "Invalid format"));
        }
        
        if (row.DateOfBirth.HasValue)
        {
            row.DateOfBirth.Value.Enforce()
                .InPast(() => new ImportException(rowNumber, "DateOfBirth", "Must be in past"))
                .GreaterThan(DateTime.Today.AddYears(-120), 
                    () => new ImportException(rowNumber, "DateOfBirth", "Invalid date"));
        }
        
        // Business rules
        if (row.CreditLimit.HasValue)
        {
            row.CreditLimit.Value.Enforce()
                .GreaterThanOrEqualsTo(0)
                .LessThanOrEqualsTo(100000);
        }
    }
}
```

## ASP.NET Minimal API guard endpoints

```csharp
var app = WebApplication.Create();

app.MapPost("/users", (CreateUserRequest dto) =>
{
    dto.EnforceNotNull();

    dto.Email.EnforceNotNull().NotWhiteSpace().MatchesEmail().ShorterThanOrEqualsTo(255);
    dto.Username.EnforceNotNull().NotWhiteSpace().LongerThanOrEqualsTo(3).ShorterThanOrEqualsTo(20);
    dto.Password.EnforceNotNull().NotWhiteSpace().LongerThanOrEqualsTo(8);

    // ... create user
    return Results.Created($"/users/{Guid.NewGuid()}", null);
});
```

## Exception mapping to ProblemDetails

Map argument exceptions to 400 and custom domain exceptions to 422.

```csharp
app.Use(async (ctx, next) =>
{
    try { await next(); }
    catch (ArgumentNullException ex)
    { await WriteProblem(ctx, 400, ex.Message, nameof(ArgumentNullException)); }
    catch (ArgumentOutOfRangeException ex)
    { await WriteProblem(ctx, 400, ex.Message, nameof(ArgumentOutOfRangeException)); }
    catch (ArgumentException ex)
    { await WriteProblem(ctx, 400, ex.Message, nameof(ArgumentException)); }
    catch (DomainException ex)
    { await WriteProblem(ctx, 422, ex.Message, ex.GetType().Name); }
});

static Task WriteProblem(HttpContext ctx, int status, string detail, string type)
{
    ctx.Response.StatusCode = status;
    ctx.Response.ContentType = "application/problem+json";
    var problem = new
    {
        type = type,
        title = StatusCodes.Status422UnprocessableEntity == status ? "Unprocessable Entity" : "Bad Request",
        status,
        detail
    };
    return ctx.Response.WriteAsJsonAsync(problem);
}
```

## Constructor guard clauses (domain entities)

```csharp
public sealed class Customer
{
    public Customer(Guid id, string email, string name)
    {
        id.Enforce().NotEmpty();
        Email = email.EnforceNotNull().NotWhiteSpace().MatchesEmail();
        Name  = name.EnforceNotNull().NotWhiteSpace().ShorterThanOrEqualsTo(100);
    }

    public Guid Id { get; }
    public string Email { get; }
    public string Name { get; }
}
```

## Reusable validation extensions

```csharp
public static class ValidationExtensions
{
    public static Enforce<string> EnforceEmail(this string? value) =>
        value.EnforceNotNull().NotWhiteSpace().MatchesEmail().ShorterThanOrEqualsTo(255);

    public static Enforce<string> EnforceUsername(this string? value) =>
        value.EnforceNotNull()
             .NotWhiteSpace()
             .LongerThanOrEqualsTo(3)
             .ShorterThanOrEqualsTo(20)
             .Matches(new Regex("^[a-zA-Z0-9_]+$", RegexOptions.Compiled), "Only letters, numbers, underscores")
             .NotMatches(new Regex("^[0-9]", RegexOptions.Compiled), "Cannot start with a number");
}

// Usage
request.Email.EnforceEmail();
request.Username.EnforceUsername();
```

## TimeProvider testing

```csharp
sealed class FakeTimeProvider : TimeProvider
{
    private DateTimeOffset _now;
    public FakeTimeProvider(DateTimeOffset now) => _now = now;
    public override DateTimeOffset GetUtcNow() => _now;
}

[Fact]
public void Delivery_in_future_is_valid()
{
    var clock = new FakeTimeProvider(DateTimeOffset.Parse("2025-01-01T00:00:00Z"));
    var delivery = new DateTime(2025, 01, 02);
    delivery.Enforce().InFuture(clock);
}
```

## Options/config validation

```csharp
public sealed class RateLimiterOptions
{
    public int RequestsPerMinute { get; init; }
    public int BurstSize { get; init; }

    public void Validate()
    {
        RequestsPerMinute.Enforce().InRange(1, 10_000);
        BurstSize.Enforce().InRange(1, RequestsPerMinute);
    }
}

// During startup
builder.Services.PostConfigure<RateLimiterOptions>(o => o.Validate());
```

## Enum and Guid quick checks

```csharp
id.Enforce().NotEmpty();

public enum Status { Active = 1, Inactive = 2 }
var value = (Status)1;
value.Enforce().Defined();
```

## Normalization + validation

```csharp
static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

var email = NormalizeEmail(dto.Email.EnforceNotNull().NotWhiteSpace());
email.Enforce().MatchesEmail();
```

## Performance: precompiled Regex

```csharp
// Compile once, reuse
static readonly Regex UsernameRegex = new("^[a-zA-Z0-9_]+$", RegexOptions.Compiled);

username.Enforce()
    .LongerThanOrEqualsTo(3)
    .ShorterThanOrEqualsTo(20)
    .Matches(UsernameRegex, "Only letters, numbers, underscores");
```

---

Previous: [API Reference](validation-reference.md) · Next: [Best practices](best-practices.md)
