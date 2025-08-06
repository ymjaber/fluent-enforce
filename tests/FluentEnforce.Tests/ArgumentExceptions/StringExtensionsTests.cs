namespace FluentEnforce.Tests.ArgumentExceptions;

public class StringExtensionsTests
{
    [Fact]
    public void Empty_WhenStringIsEmpty_DoesNotThrow()
    {
        // Arrange
        var enforce = Enforce.That("");

        // Act
        var action = () => enforce.Empty();

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Empty_WhenStringIsNotEmpty_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("not empty");

        // Act
        var action = () => enforce.Empty();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be empty*")
            .And.ParamName.Should().Be("\"not empty\"");
    }

    [Fact]
    public void NotEmpty_WhenStringIsNotEmpty_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("not empty");

        // Act
        var result = enforce.NotEmpty();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void NotEmpty_WhenStringIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("");

        // Act
        var action = () => enforce.NotEmpty();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String cannot be empty*")
            .And.ParamName.Should().Be("\"\"");
    }

    [Fact]
    public void HasLength_WhenLengthMatches_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var result = enforce.HasLength(4);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void HasLength_WhenLengthDoesNotMatch_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var action = () => enforce.HasLength(5);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must have exactly 5 characters*")
            .And.ParamName.Should().Be("\"test\"");
    }

    [Fact]
    public void LongerThan_WhenStringIsLonger_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var result = enforce.LongerThan(3);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void LongerThan_WhenStringIsNotLonger_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var action = () => enforce.LongerThan(4);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be longer than 4 characters*")
            .And.ParamName.Should().Be("\"test\"");
    }

    [Fact]
    public void LongerThan_WhenStringLengthEquals_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var action = () => enforce.LongerThan(4);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be longer than 4 characters*");
    }

    [Fact]
    public void ShorterThan_WhenStringIsShorter_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var result = enforce.ShorterThan(5);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void ShorterThan_WhenStringIsNotShorter_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var action = () => enforce.ShorterThan(4);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be shorter than 4 characters*");
    }

    [Fact]
    public void Contains_WhenStringContainsSubstring_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("hello world");

        // Act
        var result = enforce.Contains("world");

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Contains_WhenStringDoesNotContainSubstring_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("hello world");

        // Act
        var action = () => enforce.Contains("goodbye");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must contain the specified substring*");
    }

    [Fact]
    public void StartsWith_WhenStringStartsWithPrefix_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("hello world");

        // Act
        var result = enforce.StartsWith("hello");

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void StartsWith_WhenStringDoesNotStartWithPrefix_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("hello world");

        // Act
        var action = () => enforce.StartsWith("world");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must start with the specified substring*");
    }

    [Fact]
    public void EndsWith_WhenStringEndsWithSuffix_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("hello world");

        // Act
        var result = enforce.EndsWith("world");

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void EndsWith_WhenStringDoesNotEndWithSuffix_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("hello world");

        // Act
        var action = () => enforce.EndsWith("hello");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must end with the specified substring*");
    }

    [Fact]
    public void Matches_WhenStringMatchesPattern_ReturnsEnforce()
    {
        // Arrange
        var enforce = Enforce.That("test123");
        var pattern = new System.Text.RegularExpressions.Regex(@"^test\d+$");

        // Act
        var result = enforce.Matches(pattern);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Fact]
    public void Matches_WhenStringDoesNotMatchPattern_ThrowsArgumentException()
    {
        // Arrange
        var enforce = Enforce.That("test");
        var pattern = new System.Text.RegularExpressions.Regex(@"^\d+$");

        // Act
        var action = () => enforce.Matches(pattern);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String does not match the pattern*");
    }

    [Fact]
    public void MethodChaining_ThrowsOnFirstFailedCondition()
    {
        // Arrange
        var enforce = Enforce.That("test");

        // Act
        var action = () => enforce
            .NotEmpty()
            .LongerThan(10)  // This should fail
            .Contains("test");

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be longer than 10 characters*");
    }

    [Fact]
    public void Empty_FollowedByOtherValidation_CannotChain()
    {
        // Arrange
        var enforce = Enforce.That("");

        // Act & Assert
        enforce.Empty();  // This returns void
        
        // Can still use the original enforce variable for other validations
        var result = enforce.HasLength(0);
        result.Should().BeSameAs(enforce);
    }

    #region Email Validation Tests

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@example.com")]
    [InlineData("user+tag@example.co.uk")]
    [InlineData("test_email@subdomain.example.com")]
    [InlineData("123@example.com")]
    [InlineData("test@123.456.789.012")]
    [InlineData("a@example.com")] // Single character local part
    [InlineData("test@example.international")] // Long TLD
    public void MatchesEmail_WhenValidEmail_ReturnsEnforce(string email)
    {
        // Arrange
        var enforce = Enforce.That(email);

        // Act
        var result = enforce.MatchesEmail();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-an-email")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    [InlineData("test@.com")]
    [InlineData("test..email@example.com")]
    [InlineData("test @example.com")] // Space in local part
    [InlineData("test@example")] // Missing TLD
    [InlineData("test@.example.com")] // Domain starts with dot
    public void MatchesEmail_WhenInvalidEmail_ThrowsArgumentException(string email)
    {
        // Arrange
        var enforce = Enforce.That(email);

        // Act
        var action = () => enforce.MatchesEmail();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be a valid email address*");
    }

    #endregion

    #region URL Validation Tests

    [Theory]
    [InlineData("http://example.com")]
    [InlineData("https://example.com")]
    [InlineData("https://www.example.com")]
    [InlineData("https://subdomain.example.com")]
    [InlineData("https://example.com:8080")]
    [InlineData("https://example.com/path/to/resource")]
    [InlineData("https://example.com/path?query=value")]
    [InlineData("https://example.com/path?query=value&another=value2")]
    [InlineData("https://example.com/path#fragment")]
    [InlineData("https://example.com/path?query=value#fragment")]
    [InlineData("ftp://ftp.example.com")]
    [InlineData("ftps://secure.example.com")]
    [InlineData("https://user:pass@example.com")]
    [InlineData("https://[2001:db8::8a2e:370:7334]")] // IPv6
    [InlineData("https://example.com/path%20with%20spaces")]
    public void MatchesUrl_WhenValidUrl_ReturnsEnforce(string url)
    {
        // Arrange
        var enforce = Enforce.That(url);

        // Act
        var result = enforce.MatchesUrl();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-a-url")]
    [InlineData("//example.com")] // Missing scheme
    [InlineData("http://")] // Missing host
    [InlineData("example.com")] // Missing scheme
    [InlineData("javascript:alert('test')")] // Not allowed scheme
    public void MatchesUrl_WhenInvalidUrl_ThrowsArgumentException(string url)
    {
        // Arrange
        var enforce = Enforce.That(url);

        // Act
        var action = () => enforce.MatchesUrl();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be a valid URL*");
    }

    #endregion

    #region Phone Number Validation Tests

    [Theory]
    [InlineData("+970591234567")] // Palestine
    [InlineData("+971501234567")] // UAE
    [InlineData("+962791234567")] // Jordan
    [InlineData("970591234567")] // Without + prefix
    [InlineData("12345")] // Short valid number
    public void MatchesPhoneNumber_WhenValidE164Format_ReturnsEnforce(string phoneNumber)
    {
        // Arrange
        var enforce = Enforce.That(phoneNumber);

        // Act
        var result = enforce.MatchesPhoneNumber();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Theory]
    [InlineData("")]
    [InlineData("059-123-4567")] // Local format with dashes
    [InlineData("0591234567")] // Local format without country code
    [InlineData("+9")] // Too short
    [InlineData("+97059123456789012")] // Too long (>15 digits)
    [InlineData("++970591234567")] // Double plus
    [InlineData("+970 59 123 4567")] // Contains spaces
    [InlineData("00970591234567")] // International prefix 00 instead of +
    public void MatchesPhoneNumber_WhenInvalidE164Format_ThrowsArgumentException(string phoneNumber)
    {
        // Arrange
        var enforce = Enforce.That(phoneNumber);

        // Act
        var action = () => enforce.MatchesPhoneNumber();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be a valid E.164 phone number format*");
    }

    #endregion

    #region GUID Validation Tests

    [Theory]
    [InlineData("550e8400-e29b-41d4-a716-446655440000")]
    [InlineData("550E8400-E29B-41D4-A716-446655440000")] // Uppercase
    [InlineData("{550e8400-e29b-41d4-a716-446655440000}")] // With braces
    [InlineData("(550e8400-e29b-41d4-a716-446655440000)")] // With parentheses
    [InlineData("550e8400e29b41d4a716446655440000")] // Without hyphens
    [InlineData("{550e8400e29b41d4a716446655440000}")] // Without hyphens, with braces
    public void MatchesGuid_WhenValidGuid_ReturnsEnforce(string guid)
    {
        // Arrange
        var enforce = Enforce.That(guid);

        // Act
        var result = enforce.MatchesGuid();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-a-guid")]
    [InlineData("550e8400-e29b-41d4-a716")] // Too short
    [InlineData("550e8400-e29b-41d4-a716-446655440000-extra")] // Too long
    [InlineData("550g8400-e29b-41d4-a716-446655440000")] // Invalid character 'g'
    [InlineData("550e8400_e29b_41d4_a716_446655440000")] // Underscores instead of hyphens
    [InlineData("[550e8400-e29b-41d4-a716-446655440000]")] // Square brackets
    public void MatchesGuid_WhenInvalidGuid_ThrowsArgumentException(string guid)
    {
        // Arrange
        var enforce = Enforce.That(guid);

        // Act
        var action = () => enforce.MatchesGuid();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be a valid GUID*");
    }

    #endregion

    #region IP Address Validation Tests

    [Theory]
    [InlineData("192.168.1.1")]
    [InlineData("10.0.0.0")]
    [InlineData("172.16.0.1")]
    [InlineData("255.255.255.255")]
    [InlineData("0.0.0.0")]
    [InlineData("2001:0db8:85a3:0000:0000:8a2e:0370:7334")] // IPv6
    [InlineData("2001:db8:85a3::8a2e:370:7334")] // IPv6 compressed
    [InlineData("::")] // IPv6 all zeros
    [InlineData("::1")] // IPv6 loopback
    [InlineData("fe80::1")] // IPv6 link-local
    [InlineData("::ffff:192.0.2.1")] // IPv6 mapped IPv4
    public void MatchesIpAddress_WhenValidIpAddress_ReturnsEnforce(string ipAddress)
    {
        // Arrange
        var enforce = Enforce.That(ipAddress);

        // Act
        var result = enforce.MatchesIpAddress();

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-an-ip")]
    [InlineData("256.1.1.1")] // First octet too large
    [InlineData("1.256.1.1")] // Second octet too large
    [InlineData("1.1.256.1")] // Third octet too large
    [InlineData("1.1.1.256")] // Fourth octet too large
    [InlineData("192.168.1")] // Missing octet
    [InlineData("192.168.1.1.1")] // Too many octets
    [InlineData("192.168.1.")] // Trailing dot
    [InlineData(".192.168.1.1")] // Leading dot
    [InlineData("192.168..1.1")] // Double dot
    [InlineData("gggg::1")] // Invalid IPv6 character
    [InlineData("2001:0db8:85a3::8a2e:370g:7334")] // Invalid character in IPv6
    public void MatchesIpAddress_WhenInvalidIpAddress_ThrowsArgumentException(string ipAddress)
    {
        // Arrange
        var enforce = Enforce.That(ipAddress);

        // Act
        var action = () => enforce.MatchesIpAddress();

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String must be a valid IP address*");
    }

    #endregion

    #region String Pattern Matching Tests

    [Theory]
    [InlineData("abc123", @"^[a-z]+\d+$")]
    [InlineData("test@example", @"^[^@]+@[^@]+$")]
    [InlineData("123-45-6789", @"^\d{3}-\d{2}-\d{4}$")]
    public void Matches_WithStringPattern_WhenMatches_ReturnsEnforce(string value, string pattern)
    {
        // Arrange
        var enforce = Enforce.That(value);

        // Act
        var result = enforce.Matches(pattern);

        // Assert
        result.Should().BeSameAs(enforce);
    }

    [Theory]
    [InlineData("abc123", @"^\d+$")]
    [InlineData("test", @"^[^@]+@[^@]+$")]
    [InlineData("123456789", @"^\d{3}-\d{2}-\d{4}$")]
    public void Matches_WithStringPattern_WhenDoesNotMatch_ThrowsArgumentException(string value, string pattern)
    {
        // Arrange
        var enforce = Enforce.That(value);

        // Act
        var action = () => enforce.Matches(pattern);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("String does not match the pattern*");
    }

    #endregion
}