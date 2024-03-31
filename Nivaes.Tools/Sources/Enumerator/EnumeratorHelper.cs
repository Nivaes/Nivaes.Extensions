namespace Nivaes
{
    using System;
    using System.Collections.Generic;

    public static class EnumeratorHelper
    {
        public static IEnumerable<T> Take<T>(this IEnumerator<T> enumerator, int n)
        {
            ArgumentNullException.ThrowIfNull(enumerator);

            int i = 0;
            while (i++ < n && enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }
}
