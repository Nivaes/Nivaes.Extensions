namespace Nivaes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Threading;

    public static class AsyncEnumerableHelper
    {
        public static async Task<T> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            var enumerator = values.GetAsyncEnumerator();

            if (await enumerator.MoveNextAsync())
                return enumerator.Current;
            else
                return default;
        }

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this T[] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            await Task.CompletedTask.ConfigureAwait(false);

            foreach (var value in values)
            {
                yield return value;
            }
        }

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            await Task.CompletedTask.ConfigureAwait(false);

            foreach (var value in values)
            {
                yield return value;
            }
        }

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this List<T> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

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
