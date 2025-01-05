namespace Nivaes
{
    using System.Collections.Concurrent;

    public class TemporaryStore<TValue>
    {
        private readonly ConcurrentDictionary<int, StoreItem> values = new();

        private int countKey = 0;

        private sealed class StoreItem
        {
            public TValue Value { get; }
            public DateTime ExpiryTime { get; }

            public StoreItem(TValue value, DateTime expiryTime)
            {
                Value = value;
                ExpiryTime = expiryTime;
            }
        }

        public int Add(TValue value, int durationMilliseconds = int.MaxValue)
        {
            var key = Interlocked.Increment(ref countKey);

            var expiryTime = DateTime.UtcNow.AddMilliseconds(durationMilliseconds);
            var storeItem = new StoreItem(value: value, expiryTime: expiryTime);
            values[key] = storeItem;

            return key;
        }

        public bool TryGetAndRemove(int key, out TValue? value)
        {
            if (values.TryRemove(key, out var storeItem))
            {
                if (storeItem.ExpiryTime > DateTime.UtcNow)
                {
                    value = storeItem.Value;
                    return true;
                }
            }
            value = default;
            return false;
        }

        public void Cleanup()
        {
            foreach (var key in values.Keys)
            {
                if (values.TryGetValue(key, out var storeItem) && storeItem.ExpiryTime <= DateTime.UtcNow)
                {
                    values.TryRemove(key, out _);
                }
            }
        }
    }
}
