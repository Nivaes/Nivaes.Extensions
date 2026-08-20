namespace Nivaes
{
    public static class CollectionHelper
    {
        public static void AddRange<T>(this IList<T> collection, IEnumerable<T> query)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(query);

            foreach (T value in query)
            {
                collection.Add(value);
            }
        }
    }
}
