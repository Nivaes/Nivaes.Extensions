using System.Diagnostics.CodeAnalysis;

namespace Nivaes
{
    public class KeyContainerManager<TValue>
    {
        private readonly Lock _lock = new();

        private KeyStoreItem[] _values;

        public struct KeyStoreItem
        {
            public nint Key { get; set; }
            public TValue Value { get; set; }
        }

        private static readonly IComparer<KeyStoreItem> Comparer =
                Comparer<KeyStoreItem>.Create(static (x, y) => x.Key.CompareTo(y.Key));


        public KeyContainerManager()
        {
            _values = [];
        }

        public KeyContainerManager(KeyStoreItem[] values)
        {
            lock (_lock)
            {
                _values = values;
                _values.AsSpan().Sort(Comparer);
            }
        }

        public void Merge(KeyStoreItem[] newValues)
        {
            newValues.AsSpan().Sort(Comparer);

            lock (_lock)
            {
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
            var result = TryGetValue(key, out int position);

            if (result)
            {
                var values = Volatile.Read(ref _values);
                presentationType = values[position].Value;
                return true;
            }
            else
            {
                presentationType = default;
                return false;
            }
        }

        protected bool TryGetValue(nint key, [MaybeNullWhen(false)] out int position)
        {
            var values = Volatile.Read(ref _values);
            var high = _values.Length - 1;
            var low = 0;

            while (low <= high)
            {
                int mid = (high + low) / 2;
                var midKey = values[mid].Key;

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
