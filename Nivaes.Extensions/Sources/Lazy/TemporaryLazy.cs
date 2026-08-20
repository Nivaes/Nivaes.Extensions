namespace Nivaes
{
    public class TemporaryLazy<T>
    {
        private readonly Func<T> _factory;
        private readonly TimeSpan _lifetime;
        private readonly Lock _valueLock = new();

        private T? mValue;
        private DateTime mCreationTime;

        public TemporaryLazy(Func<T> factory, TimeSpan lifetime)
        {
            _factory = factory;
            _lifetime = lifetime;
        }

        public bool HasValue
        {
            get
            {
                lock (_valueLock)
                {
                    return !(Equals(mValue, default(T)) && mCreationTime.Add(_lifetime) > DateTime.UtcNow);
                }
            }
        }

        public T? Value
        {
            get
            {
                lock (_valueLock)
                {
                    if (Equals(mValue, default(T)) || mCreationTime.Add(_lifetime) > DateTime.UtcNow)
                    {
                        mValue = _factory();
                        mCreationTime = DateTime.UtcNow;
                    }

                    return mValue;
                }
            }
        }
    }
}
