namespace Nivaes
{
    using System;
    using System.Threading.Tasks;

    public class AsyncTemporary<T>
    {
        private readonly Func<ValueTask<T>> mFactory;
        private readonly TimeSpan mLifetime;
        private readonly object mValueLock = new object();

        private ValueTask<T> mValue;
        private DateTime mCreationTime ;

        public AsyncTemporary(Func<ValueTask<T>> factory, TimeSpan lifetime)
        {
            mFactory = factory;
            mLifetime = lifetime;
        }

        public bool HasValue
        {
            get
            {
                lock (mValueLock)
                {
                    return !(mValue == null && mCreationTime.Add(mLifetime) < DateTime.UtcNow);
                }
            }
        }

        public ValueTask<T> Value
        {
            get
            {
                DateTime now = DateTime.UtcNow;
                lock (mValueLock)
                {
                    if (mValue == null || mCreationTime.Add(mLifetime) < DateTime.UtcNow)
                    {
                        mValue = mFactory(); 
                        mCreationTime = DateTime.UtcNow;
                    }

                    return mValue;
                }
            }
        }
    }
}
