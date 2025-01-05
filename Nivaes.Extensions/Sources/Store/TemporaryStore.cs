using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes
{
    public class TemporaryStore<TValue>
    {
        private readonly ConcurrentDictionary<int, StoreItem> _store = new();

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
            _store[key] = storeItem;

            return key;
        }

        public bool TryGetAndRemove(int key, out TValue? value)
        {
            if (_store.TryRemove(key, out var storeItem))
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
            foreach (var key in _store.Keys)
            {
                if (_store.TryGetValue(key, out var storeItem) && storeItem.ExpiryTime <= DateTime.UtcNow)
                {
                    _store.TryRemove(key, out _);
                }
            }
        }
    }
}
