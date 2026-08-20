using System.Diagnostics.CodeAnalysis;

namespace Nivaes 
{
    public class KeyContainerManager<TValue>
    {
        private readonly Lock @lock = new();

        private KeyStoreItem[] _values;

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
            _values = [];
        }

        public KeyContainerManager(KeyStoreItem[] values)
        {
            lock (@lock)
            {
                _values = values;
                var keyInstanceResolverValues = new Span<KeyStoreItem>(_values);
                keyInstanceResolverValues.Sort(new KeyPresentationComparer());
            }
        }

        public void Merge(KeyStoreItem[] newValues)
        {
            lock (@lock)
            {
                var keyInstanceResolverValues = new Span<KeyStoreItem>(newValues);
                keyInstanceResolverValues.Sort(new KeyPresentationComparer());

                var oldValues = _values;
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

                _values = allValues;
            }
        }

        protected internal bool TryGetValue(nint key, [MaybeNullWhen(false)] out TValue presentationType)
        {
            lock (@lock)
            {
                var result = TryGetValue(key, out int position);

                if (result)
                {
                    presentationType = _values[position].Value;
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
                var high = _values.Length - 1;
                var low = 0;

                while (low <= high)
                {
                    int mid = (high + low) / 2;
                    var midKey = _values[mid].Key;

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
