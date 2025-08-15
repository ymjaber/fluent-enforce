using System.Runtime.CompilerServices;

namespace FluentEnforce;

/// <summary>
/// Provides extension methods for enforcing collection constraints on values with custom exceptions.
/// </summary>
public static partial class CollectionExtensions
{
    #region Base IEnumerable<T> Methods

    /// <summary>
    /// Enforces that the collection is not empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="enforce">The Enforce instance containing the collection to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the collection is empty.</exception>
    public static Enforce<IEnumerable<T>> NotEmpty<T>(
        this Enforce<IEnumerable<T>> enforce,
        Func<Exception> exception)
    {
        if (!enforce.Value.Any())
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the collection is empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="enforce">The Enforce instance containing the collection to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <exception cref="Exception">The exception when the collection is not empty.</exception>
    public static void Empty<T>(
        this Enforce<IEnumerable<T>> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Any())
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the collection has exactly the specified number of elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="enforce">The Enforce instance containing the collection to validate.</param>
    /// <param name="expected">The expected number of elements.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the collection does not have the expected count.</exception>
    public static Enforce<IEnumerable<T>> HasCount<T>(
        this Enforce<IEnumerable<T>> enforce,
        int expected,
        Func<Exception> exception)
    {
        var count = enforce.Value.Count();
        if (count != expected)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the collection has at least the specified number of elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="enforce">The Enforce instance containing the collection to validate.</param>
    /// <param name="min">The minimum number of elements.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the collection has fewer than the minimum count.</exception>
    public static Enforce<IEnumerable<T>> HasMinCount<T>(
        this Enforce<IEnumerable<T>> enforce,
        int min,
        Func<Exception> exception)
    {
        var count = enforce.Value.Count();
        if (count < min)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the collection has at most the specified number of elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="enforce">The Enforce instance containing the collection to validate.</param>
    /// <param name="max">The maximum number of elements.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the collection has more than the maximum count.</exception>
    public static Enforce<IEnumerable<T>> HasMaxCount<T>(
        this Enforce<IEnumerable<T>> enforce,
        int max,
        Func<Exception> exception)
    {
        var count = enforce.Value.Count();
        if (count > max)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the collection contains the specified item.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="enforce">The Enforce instance containing the collection to validate.</param>
    /// <param name="item">The item that must be present in the collection.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the collection does not contain the item.</exception>
    public static Enforce<IEnumerable<T>> Contains<T>(
        this Enforce<IEnumerable<T>> enforce,
        T item,
        Func<Exception> exception)
    {
        if (!enforce.Value.Contains(item))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the collection does not contain the specified item.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="enforce">The Enforce instance containing the collection to validate.</param>
    /// <param name="item">The item that must not be present in the collection.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the collection contains the item.</exception>
    public static Enforce<IEnumerable<T>> NotContains<T>(
        this Enforce<IEnumerable<T>> enforce,
        T item,
        Func<Exception> exception)
    {
        if (enforce.Value.Contains(item))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that all elements in the collection satisfy the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="enforce">The Enforce instance containing the collection to validate.</param>
    /// <param name="predicate">The predicate that all elements must satisfy.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when any element does not satisfy the predicate.</exception>
    public static Enforce<IEnumerable<T>> AllMatch<T>(
        this Enforce<IEnumerable<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (!enforce.Value.All(x => predicate(x)))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that at least one element in the collection satisfies the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="enforce">The Enforce instance containing the collection to validate.</param>
    /// <param name="predicate">The predicate that at least one element must satisfy.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when no elements satisfy the predicate.</exception>
    public static Enforce<IEnumerable<T>> AnyMatch<T>(
        this Enforce<IEnumerable<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (!enforce.Value.Any(x => predicate(x)))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that no elements in the collection satisfy the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="enforce">The Enforce instance containing the collection to validate.</param>
    /// <param name="predicate">The predicate that no elements must satisfy.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when any element satisfies the predicate.</exception>
    public static Enforce<IEnumerable<T>> NoneMatch<T>(
        this Enforce<IEnumerable<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (enforce.Value.Any(x => predicate(x)))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that all elements in the collection are unique (no duplicates).
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="enforce">The Enforce instance containing the collection to validate.</param>
    /// <param name="exception">The exception to throw when validation fails.</param>
    /// <returns>The Enforce instance for method chaining.</returns>
    /// <exception cref="Exception">The exception when the collection contains duplicate elements.</exception>
    public static Enforce<IEnumerable<T>> Unique<T>(
        this Enforce<IEnumerable<T>> enforce,
        Func<Exception> exception)
    {
        var list = enforce.Value.ToList();
        if (list.Count != list.Distinct().Count())
        {
            throw exception();
        }

        return enforce;
    }

    #endregion

    #region List<T> Overloads

    /// <summary>
    /// Enforces that the list is not empty.
    /// </summary>
    public static Enforce<List<T>> NotEmpty<T>(
        this Enforce<List<T>> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Count == 0)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the list is empty.
    /// </summary>
    public static void Empty<T>(
        this Enforce<List<T>> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Count > 0)
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the list has exactly the specified number of elements.
    /// </summary>
    public static Enforce<List<T>> HasCount<T>(
        this Enforce<List<T>> enforce,
        int expected,
        Func<Exception> exception)
    {
        if (enforce.Value.Count != expected)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the list has at least the specified number of elements.
    /// </summary>
    public static Enforce<List<T>> HasMinCount<T>(
        this Enforce<List<T>> enforce,
        int min,
        Func<Exception> exception)
    {
        if (enforce.Value.Count < min)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the list has at most the specified number of elements.
    /// </summary>
    public static Enforce<List<T>> HasMaxCount<T>(
        this Enforce<List<T>> enforce,
        int max,
        Func<Exception> exception)
    {
        if (enforce.Value.Count > max)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the list contains the specified item.
    /// </summary>
    public static Enforce<List<T>> Contains<T>(
        this Enforce<List<T>> enforce,
        T item,
        Func<Exception> exception)
    {
        if (!enforce.Value.Contains(item))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the list does not contain the specified item.
    /// </summary>
    public static Enforce<List<T>> NotContains<T>(
        this Enforce<List<T>> enforce,
        T item,
        Func<Exception> exception)
    {
        if (enforce.Value.Contains(item))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that all elements in the list satisfy the specified predicate.
    /// </summary>
    public static Enforce<List<T>> AllMatch<T>(
        this Enforce<List<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (!enforce.Value.TrueForAll(predicate))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that at least one element in the list satisfies the specified predicate.
    /// </summary>
    public static Enforce<List<T>> AnyMatch<T>(
        this Enforce<List<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (!enforce.Value.Exists(predicate))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that no elements in the list satisfy the specified predicate.
    /// </summary>
    public static Enforce<List<T>> NoneMatch<T>(
        this Enforce<List<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (enforce.Value.Exists(predicate))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that all elements in the list are unique (no duplicates).
    /// </summary>
    public static Enforce<List<T>> Unique<T>(
        this Enforce<List<T>> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Count != enforce.Value.Distinct().Count())
        {
            throw exception();
        }

        return enforce;
    }

    #endregion

    #region T[] Array Overloads

    /// <summary>
    /// Enforces that the array is not empty.
    /// </summary>
    public static Enforce<T[]> NotEmpty<T>(
        this Enforce<T[]> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Length == 0)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the array is empty.
    /// </summary>
    public static void Empty<T>(
        this Enforce<T[]> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Length > 0)
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the array has exactly the specified number of elements.
    /// </summary>
    public static Enforce<T[]> HasCount<T>(
        this Enforce<T[]> enforce,
        int expected,
        Func<Exception> exception)
    {
        if (enforce.Value.Length != expected)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the array has at least the specified number of elements.
    /// </summary>
    public static Enforce<T[]> HasMinCount<T>(
        this Enforce<T[]> enforce,
        int min,
        Func<Exception> exception)
    {
        if (enforce.Value.Length < min)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the array has at most the specified number of elements.
    /// </summary>
    public static Enforce<T[]> HasMaxCount<T>(
        this Enforce<T[]> enforce,
        int max,
        Func<Exception> exception)
    {
        if (enforce.Value.Length > max)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the array contains the specified item.
    /// </summary>
    public static Enforce<T[]> Contains<T>(
        this Enforce<T[]> enforce,
        T item,
        Func<Exception> exception)
    {
        if (Array.IndexOf(enforce.Value, item) == -1)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the array does not contain the specified item.
    /// </summary>
    public static Enforce<T[]> NotContains<T>(
        this Enforce<T[]> enforce,
        T item,
        Func<Exception> exception)
    {
        if (Array.IndexOf(enforce.Value, item) != -1)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that all elements in the array satisfy the specified predicate.
    /// </summary>
    public static Enforce<T[]> AllMatch<T>(
        this Enforce<T[]> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (!Array.TrueForAll(enforce.Value, predicate))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that at least one element in the array satisfies the specified predicate.
    /// </summary>
    public static Enforce<T[]> AnyMatch<T>(
        this Enforce<T[]> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (!Array.Exists(enforce.Value, predicate))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that no elements in the array satisfy the specified predicate.
    /// </summary>
    public static Enforce<T[]> NoneMatch<T>(
        this Enforce<T[]> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (Array.Exists(enforce.Value, predicate))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that all elements in the array are unique (no duplicates).
    /// </summary>
    public static Enforce<T[]> Unique<T>(
        this Enforce<T[]> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Length != enforce.Value.Distinct().Count())
        {
            throw exception();
        }

        return enforce;
    }

    #endregion

    #region HashSet<T> Overloads

    /// <summary>
    /// Enforces that the HashSet is not empty.
    /// </summary>
    public static Enforce<HashSet<T>> NotEmpty<T>(
        this Enforce<HashSet<T>> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Count == 0)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the HashSet is empty.
    /// </summary>
    public static void Empty<T>(
        this Enforce<HashSet<T>> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Count > 0)
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the HashSet has exactly the specified number of elements.
    /// </summary>
    public static Enforce<HashSet<T>> HasCount<T>(
        this Enforce<HashSet<T>> enforce,
        int expected,
        Func<Exception> exception)
    {
        if (enforce.Value.Count != expected)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the HashSet has at least the specified number of elements.
    /// </summary>
    public static Enforce<HashSet<T>> HasMinCount<T>(
        this Enforce<HashSet<T>> enforce,
        int min,
        Func<Exception> exception)
    {
        if (enforce.Value.Count < min)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the HashSet has at most the specified number of elements.
    /// </summary>
    public static Enforce<HashSet<T>> HasMaxCount<T>(
        this Enforce<HashSet<T>> enforce,
        int max,
        Func<Exception> exception)
    {
        if (enforce.Value.Count > max)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the HashSet contains the specified item.
    /// </summary>
    public static Enforce<HashSet<T>> Contains<T>(
        this Enforce<HashSet<T>> enforce,
        T item,
        Func<Exception> exception)
    {
        if (!enforce.Value.Contains(item))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the HashSet does not contain the specified item.
    /// </summary>
    public static Enforce<HashSet<T>> NotContains<T>(
        this Enforce<HashSet<T>> enforce,
        T item,
        Func<Exception> exception)
    {
        if (enforce.Value.Contains(item))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that all elements in the HashSet satisfy the specified predicate.
    /// </summary>
    public static Enforce<HashSet<T>> AllMatch<T>(
        this Enforce<HashSet<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (!enforce.Value.All(x => predicate(x)))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that at least one element in the HashSet satisfies the specified predicate.
    /// </summary>
    public static Enforce<HashSet<T>> AnyMatch<T>(
        this Enforce<HashSet<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (!enforce.Value.Any(x => predicate(x)))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that no elements in the HashSet satisfy the specified predicate.
    /// </summary>
    public static Enforce<HashSet<T>> NoneMatch<T>(
        this Enforce<HashSet<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (enforce.Value.Any(x => predicate(x)))
        {
            throw exception();
        }

        return enforce;
    }

    // Note: Unique is not needed for HashSet as it inherently contains unique elements

    #endregion

    #region Queue<T> Overloads

    /// <summary>
    /// Enforces that the queue is not empty.
    /// </summary>
    public static Enforce<Queue<T>> NotEmpty<T>(
        this Enforce<Queue<T>> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Count == 0)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the queue is empty.
    /// </summary>
    public static void Empty<T>(
        this Enforce<Queue<T>> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Count > 0)
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the queue has exactly the specified number of elements.
    /// </summary>
    public static Enforce<Queue<T>> HasCount<T>(
        this Enforce<Queue<T>> enforce,
        int expected,
        Func<Exception> exception)
    {
        if (enforce.Value.Count != expected)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the queue has at least the specified number of elements.
    /// </summary>
    public static Enforce<Queue<T>> HasMinCount<T>(
        this Enforce<Queue<T>> enforce,
        int min,
        Func<Exception> exception)
    {
        if (enforce.Value.Count < min)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the queue has at most the specified number of elements.
    /// </summary>
    public static Enforce<Queue<T>> HasMaxCount<T>(
        this Enforce<Queue<T>> enforce,
        int max,
        Func<Exception> exception)
    {
        if (enforce.Value.Count > max)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the queue contains the specified item.
    /// </summary>
    public static Enforce<Queue<T>> Contains<T>(
        this Enforce<Queue<T>> enforce,
        T item,
        Func<Exception> exception)
    {
        if (!enforce.Value.Contains(item))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the queue does not contain the specified item.
    /// </summary>
    public static Enforce<Queue<T>> NotContains<T>(
        this Enforce<Queue<T>> enforce,
        T item,
        Func<Exception> exception)
    {
        if (enforce.Value.Contains(item))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that all elements in the queue satisfy the specified predicate.
    /// </summary>
    public static Enforce<Queue<T>> AllMatch<T>(
        this Enforce<Queue<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (!enforce.Value.All(x => predicate(x)))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that at least one element in the queue satisfies the specified predicate.
    /// </summary>
    public static Enforce<Queue<T>> AnyMatch<T>(
        this Enforce<Queue<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (!enforce.Value.Any(x => predicate(x)))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that no elements in the queue satisfy the specified predicate.
    /// </summary>
    public static Enforce<Queue<T>> NoneMatch<T>(
        this Enforce<Queue<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (enforce.Value.Any(x => predicate(x)))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that all elements in the queue are unique (no duplicates).
    /// </summary>
    public static Enforce<Queue<T>> Unique<T>(
        this Enforce<Queue<T>> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Count != enforce.Value.Distinct().Count())
        {
            throw exception();
        }

        return enforce;
    }

    #endregion

    #region Stack<T> Overloads

    /// <summary>
    /// Enforces that the stack is not empty.
    /// </summary>
    public static Enforce<Stack<T>> NotEmpty<T>(
        this Enforce<Stack<T>> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Count == 0)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the stack is empty.
    /// </summary>
    public static void Empty<T>(
        this Enforce<Stack<T>> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Count > 0)
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the stack has exactly the specified number of elements.
    /// </summary>
    public static Enforce<Stack<T>> HasCount<T>(
        this Enforce<Stack<T>> enforce,
        int expected,
        Func<Exception> exception)
    {
        if (enforce.Value.Count != expected)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the stack has at least the specified number of elements.
    /// </summary>
    public static Enforce<Stack<T>> HasMinCount<T>(
        this Enforce<Stack<T>> enforce,
        int min,
        Func<Exception> exception)
    {
        if (enforce.Value.Count < min)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the stack has at most the specified number of elements.
    /// </summary>
    public static Enforce<Stack<T>> HasMaxCount<T>(
        this Enforce<Stack<T>> enforce,
        int max,
        Func<Exception> exception)
    {
        if (enforce.Value.Count > max)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the stack contains the specified item.
    /// </summary>
    public static Enforce<Stack<T>> Contains<T>(
        this Enforce<Stack<T>> enforce,
        T item,
        Func<Exception> exception)
    {
        if (!enforce.Value.Contains(item))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the stack does not contain the specified item.
    /// </summary>
    public static Enforce<Stack<T>> NotContains<T>(
        this Enforce<Stack<T>> enforce,
        T item,
        Func<Exception> exception)
    {
        if (enforce.Value.Contains(item))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that all elements in the stack satisfy the specified predicate.
    /// </summary>
    public static Enforce<Stack<T>> AllMatch<T>(
        this Enforce<Stack<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (!enforce.Value.All(x => predicate(x)))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that at least one element in the stack satisfies the specified predicate.
    /// </summary>
    public static Enforce<Stack<T>> AnyMatch<T>(
        this Enforce<Stack<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (!enforce.Value.Any(x => predicate(x)))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that no elements in the stack satisfy the specified predicate.
    /// </summary>
    public static Enforce<Stack<T>> NoneMatch<T>(
        this Enforce<Stack<T>> enforce,
        Predicate<T> predicate,
        Func<Exception> exception)
    {
        if (enforce.Value.Any(x => predicate(x)))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that all elements in the stack are unique (no duplicates).
    /// </summary>
    public static Enforce<Stack<T>> Unique<T>(
        this Enforce<Stack<T>> enforce,
        Func<Exception> exception)
    {
        if (enforce.Value.Count != enforce.Value.Distinct().Count())
        {
            throw exception();
        }

        return enforce;
    }

    #endregion

    #region Dictionary<TKey, TValue> Overloads

    /// <summary>
    /// Enforces that the dictionary is not empty.
    /// </summary>
    public static Enforce<Dictionary<TKey, TValue>> NotEmpty<TKey, TValue>(
        this Enforce<Dictionary<TKey, TValue>> enforce,
        Func<Exception> exception)
        where TKey : notnull
    {
        if (enforce.Value.Count == 0)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the dictionary is empty.
    /// </summary>
    public static void Empty<TKey, TValue>(
        this Enforce<Dictionary<TKey, TValue>> enforce,
        Func<Exception> exception)
        where TKey : notnull
    {
        if (enforce.Value.Count > 0)
        {
            throw exception();
        }
    }

    /// <summary>
    /// Enforces that the dictionary has exactly the specified number of key-value pairs.
    /// </summary>
    public static Enforce<Dictionary<TKey, TValue>> HasCount<TKey, TValue>(
        this Enforce<Dictionary<TKey, TValue>> enforce,
        int expected,
        Func<Exception> exception)
        where TKey : notnull
    {
        if (enforce.Value.Count != expected)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the dictionary has at least the specified number of key-value pairs.
    /// </summary>
    public static Enforce<Dictionary<TKey, TValue>> HasMinCount<TKey, TValue>(
        this Enforce<Dictionary<TKey, TValue>> enforce,
        int min,
        Func<Exception> exception)
        where TKey : notnull
    {
        if (enforce.Value.Count < min)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the dictionary has at most the specified number of key-value pairs.
    /// </summary>
    public static Enforce<Dictionary<TKey, TValue>> HasMaxCount<TKey, TValue>(
        this Enforce<Dictionary<TKey, TValue>> enforce,
        int max,
        Func<Exception> exception)
        where TKey : notnull
    {
        if (enforce.Value.Count > max)
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the dictionary contains the specified key.
    /// </summary>
    public static Enforce<Dictionary<TKey, TValue>> ContainsKey<TKey, TValue>(
        this Enforce<Dictionary<TKey, TValue>> enforce,
        TKey key,
        Func<Exception> exception)
        where TKey : notnull
    {
        if (!enforce.Value.ContainsKey(key))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the dictionary does not contain the specified key.
    /// </summary>
    public static Enforce<Dictionary<TKey, TValue>> NotContainsKey<TKey, TValue>(
        this Enforce<Dictionary<TKey, TValue>> enforce,
        TKey key,
        Func<Exception> exception)
        where TKey : notnull
    {
        if (enforce.Value.ContainsKey(key))
        {
            throw exception();
        }

        return enforce;
    }

    /// <summary>
    /// Enforces that the dictionary contains the specified value.
    /// </summary>
    public static Enforce<Dictionary<TKey, TValue>> ContainsValue<TKey, TValue>(
        this Enforce<Dictionary<TKey, TValue>> enforce,
        TValue value,
        Func<Exception> exception)
        where TKey : notnull
    {
        if (!enforce.Value.ContainsValue(value))
        {
            throw exception();
        }

        return enforce;
    }

    #endregion
}
