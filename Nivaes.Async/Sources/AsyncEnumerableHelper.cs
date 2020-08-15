// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class AsyncEnumerableHelper
    {
        public static async ValueTask<T> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

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

            if (values == null) throw new ArgumentNullException(nameof(values));

            foreach (var value in values)
            {
                yield return value;
            }
        }

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> values)
        {
            await Task.CompletedTask.ConfigureAwait(false);

            if (values == null) throw new ArgumentNullException(nameof(values));

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
