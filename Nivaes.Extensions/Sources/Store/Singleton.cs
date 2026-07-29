namespace Nivaes
{
    public static class Singleton<TValue>
         where TValue : new()
    {
        private static readonly Lock Lock = new();
        private static TValue? _instance;

        public static void Clear()
        {
            lock (Lock)
            {
                _instance = default;
            }
        }

        public static TValue Instance
        {
            get
            {
                lock (Lock)
                {
                    if (object.ReferenceEquals(_instance, default))
                    {
                        _instance = new TValue();
                    }
                    return _instance;
                }
            }
        }

        public static void Add(TValue instance)
        {
            lock (Lock)
            {
                _instance = instance;
            }
        }
    }
}
