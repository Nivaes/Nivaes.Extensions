using System.Diagnostics.CodeAnalysis;

namespace Nivaes 
{
    public class KeyContainerManager<TValue>
    {
        private readonly Lock @lock = new();

        private KeyStoreItem[] mValues;

        public struct KeyStoreItem
        {
            public nint Key { get; set; }
            public TValue Value { get; set; }
        }

        private sealed class KeyPresentationComparer
            : IComparer<KeyStoreItem>
        {
            public int Compare(KeyStoreItem x, KeyStoreItem y)
            {
                return x.Key.CompareTo(y.Key);
            }
        }

        public KeyContainerManager()
        {
            mValues = [];
        }

        public KeyContainerManager(KeyStoreItem[] values)
        {
            lock (@lock)
            {
                mValues = values;
                var keyInstanceResolverValues = new Span<KeyStoreItem>(mValues);
                keyInstanceResolverValues.Sort(new KeyPresentationComparer());
            }
        }

        public void Merge(KeyStoreItem[] newValues)
        {
            lock (@lock)
            {
                var keyInstanceResolverValues = new Span<KeyStoreItem>(newValues);
                keyInstanceResolverValues.Sort(new KeyPresentationComparer());

                var oldValues = mValues;
                var allValues = new KeyStoreItem[oldValues.Length + newValues.Length];
                int i = 0, j = 0, m = 0;

                while (i < oldValues.Length && j < newValues.Length)
                {
                    if (oldValues[i].Key < newValues[j].Key)
                    {
                        allValues[m++] = oldValues[i++];
                    }
                    else
                    {
                        allValues[m++] = newValues[j++];
                    }
                }
                while (i < oldValues.Length)
                {
                    allValues[m++] = oldValues[i++];
                }
                while (j < newValues.Length)
                {
                    allValues[m++] = newValues[j++];
                }

                mValues = allValues;
            }
        }

        protected internal bool TryGetValue(nint key, [MaybeNullWhen(false)] out TValue presentationType)
        {
            lock (@lock)
            {
                var result = TryGetValue(key, out int position);

                if (result)
                {
                    presentationType = mValues[position].Value;
                    return true;
                }
                else
                {
                    presentationType = default;
                    return false;
                }
            }
        }

        protected bool TryGetValue(nint key, [MaybeNullWhen(false)] out int position)
        {
            lock (@lock)
            {
                var high = mValues.Length - 1;
                var low = 0;

                while (low <= high)
                {
                    int mid = (high + low) / 2;
                    var midKey = mValues[mid].Key;

                    if (midKey == key)
                    {
                        position = mid;
                        return true;
                    }
                    else
                    {
                        if (key < midKey)
                            high = mid - 1;
                        else
                            low = mid + 1;
                    }
                }
                position = -1;
                return false;
            }
        }
    }
}
