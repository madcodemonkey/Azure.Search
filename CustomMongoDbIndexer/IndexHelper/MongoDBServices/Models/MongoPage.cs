namespace MongoDBServices;

public class MongoPage<T>
{
    /// <summary>
    /// The one based paged number (should be greater than zero).
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// The total number of items on all pages.
    /// </summary>
    public long Total { get; set; }

    /// <summary>
    /// The items for the current <see cref="PageNumber"/>!
    /// </summary>
    public List<T> Items { get; set; } = new List<T>();
}