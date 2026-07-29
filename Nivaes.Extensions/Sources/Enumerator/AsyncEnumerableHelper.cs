namespace Nivaes
{
    public static class AsyncEnumerableHelper
    {
        public static async ValueTask<T> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> values)
        {
            ArgumentNullException.ThrowIfNull(values);

            var enumerator = values.GetAsyncEnumerator();

            if (await enumerator.MoveNextAsync().ConfigureAwait(false))
            {
                return enumerator.Current;
            }
            else
            {
                return default!;
            }
        }

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this T[] values)
        {
            await Task.CompletedTask.ConfigureAwait(false);

            ArgumentNullException.ThrowIfNull(values);

            foreach (var value in values)
            {
                yield return value;
            }
        }

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> values)
        {
            await Task.CompletedTask.ConfigureAwait(false);

            ArgumentNullException.ThrowIfNull(values);

            foreach (var value in values)
            {
                yield return value;
            }
        }

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this List<T> values)
        {
            ArgumentNullException.ThrowIfNull(values);

            await Task.CompletedTask.ConfigureAwait(false);

            foreach (var value in values)
            {
                yield return value;
            }
        }

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this T value)
        {
            await Task.CompletedTask.ConfigureAwait(false);

            yield return value;
        }
    }
}
