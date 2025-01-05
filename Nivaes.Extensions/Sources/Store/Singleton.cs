namespace Nivaes
{
    public static class Singleton<TValue>
         where TValue : new()
    {
#if NET8_0
        private static readonly object Lock = new();
#elif NET9_0_OR_GREATER
        private static readonly System.Threading.Lock Lock = new();
#endif
        private static TValue? mInstance;

        public static void Clear()
        {
            lock (Lock)
            {
                mInstance = default;
            }
        }

        public static TValue Instance
        {
            get
            {
                lock (Lock)
                {
                    if (object.ReferenceEquals(mInstance, default))
                    {
                        mInstance = new TValue();
                    }
                    return mInstance;
                }
            }
        }

        public static void Add(TValue instance)
        {
            lock (Lock)
            {
                mInstance = instance;
            }
        }
    }
}
