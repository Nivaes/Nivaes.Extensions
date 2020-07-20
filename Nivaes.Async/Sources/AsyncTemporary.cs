namespace Nivaes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    public class AsyncTemporary<T>
    {
        private readonly Func<ValueTask<T>> mFactory;
        private readonly TimeSpan mLifetime;

        private readonly AsyncLocal<T> mValue;
        private DateTime mCreationTime;

        public AsyncTemporary(Func<ValueTask<T>> factory, TimeSpan lifetime)
        {
            mValue = new AsyncLocal<T>();
            mFactory = factory;
            mLifetime = lifetime;
        }

        public bool HasValue => !(EqualityComparer<T>.Default.Equals(mValue.Value, default) && mCreationTime.Add(mLifetime) < DateTime.UtcNow);

        public async ValueTask<T> GetValue()
        {
            if (mValue.Value == null || mCreationTime.Add(mLifetime) < DateTime.UtcNow)
            {
                mValue.Value = await mFactory().ConfigureAwait(false);
                mCreationTime = DateTime.UtcNow;
            }

            return mValue.Value;
        }
    }
}
