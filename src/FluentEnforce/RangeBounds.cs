namespace FluentEnforce;

/// <summary>
/// Specifies the inclusivity of range boundaries in range validation operations.
/// </summary>
public enum RangeBounds
{
    /// <summary>
    /// Both minimum and maximum values are included in the range [min, max].
    /// </summary>
    Inclusive,
    
    /// <summary>
    /// Both minimum and maximum values are excluded from the range (min, max).
    /// </summary>
    Exclusive,
    
    /// <summary>
    /// Minimum value is included, maximum value is excluded [min, max).
    /// </summary>
    LeftInclusive,
    
    /// <summary>
    /// Minimum value is excluded, maximum value is included (min, max].
    /// </summary>
    RightInclusive
}