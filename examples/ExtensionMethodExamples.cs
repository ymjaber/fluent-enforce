using FluentEnforce;

namespace Examples;

/// <summary>
/// Examples demonstrating the extension method approach for FluentEnforce
/// </summary>
public class ExtensionMethodExamples
{
    /// <summary>
    /// Example: User registration with extension method validation
    /// </summary>
    public class UserRegistration
    {
        public void RegisterUser(string email, string password, string phoneNumber, int? age)
        {
            // Extension method approach - reads naturally from left to right
            email.EnforceNotNull("Email is required")
                .NotWhiteSpace()
                .MatchesEmail()
                .ShorterThanOrEqualTo(255);

            password.EnforceNotNull("Password is required")
                .NotWhiteSpace()
                .LongerThanOrEqualTo(8)
                .Matches(@"[A-Z]", "Password must contain uppercase letter")
                .Matches(@"[0-9]", "Password must contain number");

            // Phone number validation with E.164 format
            phoneNumber.EnforceNotNull()
                .NotWhiteSpace()
                .MatchesPhoneNumber();

            // Nullable value type handling
            if (age.HasValue)
            {
                age.EnforceNotNull()
                    .GreaterThanOrEqualTo(13)
                    .LessThanOrEqualTo(120);
            }
        }
    }

    /// <summary>
    /// Example: Using custom exceptions with extension methods
    /// </summary>
    public class OrderProcessing
    {
        public class OrderValidationException : Exception
        {
            public OrderValidationException(string message) : base(message) { }
        }

        public void ProcessOrder(Order? order, decimal amount)
        {
            // Custom exception with extension method
            order.EnforceNotNull(
                () => new OrderValidationException("Order cannot be null"));

            // Chain validations after null check
            order.Id.Enforce()
                .GreaterThan(0, () => new OrderValidationException("Invalid order ID"));

            amount.Enforce()
                .GreaterThan(0m)
                .LessThanOrEqualTo(10_000m, 
                    () => new OrderValidationException("Amount exceeds maximum allowed"));

            // Check for null values within the order
            order.Items.Enforce()
                .NotEmpty(() => new OrderValidationException("Order must contain items"));
        }
    }

    /// <summary>
    /// Example: Comparing static vs extension method approaches
    /// </summary>
    public class ValidationComparison
    {
        public void ValidateWithStaticMethods(string name, int age)
        {
            // Traditional static method approach
            Enforce.NotNull(name)
                .NotWhiteSpace()
                .LongerThanOrEqualTo(2)
                .ShorterThanOrEqualTo(50);

            Enforce.That(age)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(150);
        }

        public void ValidateWithExtensionMethods(string name, int age)
        {
            // Extension method approach - same validation, different syntax
            name.EnforceNotNull()
                .NotWhiteSpace()
                .LongerThanOrEqualTo(2)
                .ShorterThanOrEqualTo(50);

            age.Enforce()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(150);
        }
    }

    /// <summary>
    /// Example: Real-world API endpoint validation
    /// </summary>
    public class ApiController
    {
        public record CreateProductRequest(
            string Name,
            string? Description,
            decimal Price,
            string Sku,
            int StockQuantity);

        public IActionResult CreateProduct(CreateProductRequest? request)
        {
            // Validate the entire request
            request.EnforceNotNull("Request body is required");

            // Validate individual fields using extension methods
            request.Name.EnforceNotNull("Product name is required")
                .NotWhiteSpace()
                .LongerThanOrEqualTo(3, "Product name too short")
                .ShorterThanOrEqualTo(100, "Product name too long");

            // Optional field validation
            if (request.Description != null)
            {
                request.Description.Enforce()
                    .ShorterThanOrEqualTo(500, "Description too long");
            }

            request.Price.Enforce()
                .GreaterThan(0m, "Price must be positive")
                .LessThanOrEqualTo(999_999.99m, "Price exceeds maximum");

            request.Sku.EnforceNotNull("SKU is required")
                .NotWhiteSpace()
                .Matches(@"^[A-Z0-9\-]+$", "Invalid SKU format");

            request.StockQuantity.Enforce()
                .GreaterThanOrEqualTo(0, "Stock quantity cannot be negative");

            // Create product...
            return new OkResult();
        }
    }

    // Supporting classes for examples
    public class Order
    {
        public int Id { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        public string ProductId { get; set; } = "";
        public int Quantity { get; set; }
    }

    public interface IActionResult { }
    public class OkResult : IActionResult { }
}