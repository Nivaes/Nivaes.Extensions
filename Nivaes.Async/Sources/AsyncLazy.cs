namespace Nivaes
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    public class AsyncLazy<T> : Lazy<ValueTask<T>>
    {
        public AsyncLazy()
            : base()
        { }

        public AsyncLazy(bool isThreadSafe)
            : base(isThreadSafe)
        { }

        public AsyncLazy(T valueFactory)
            : base(() => new ValueTask<T>(valueFactory))
        { }

        public AsyncLazy(Func<T> valueFactory)
            : base(() => new ValueTask<T>(valueFactory.Invoke()))
        { }

        public AsyncLazy(Func<ValueTask<T>> taskFactory)
           : base(() => taskFactory.Invoke())
        { }

        public AsyncLazy(Func<Task<T>> taskFactory)
            : base(() => new ValueTask<T>(taskFactory.Invoke()))
        { }

        public AsyncLazy(LazyThreadSafetyMode mode)
            : base(mode)
        { }

        public AsyncLazy(T valueFactory, bool isThreadSafe)
            : base(() => new ValueTask<T>(valueFactory), isThreadSafe)
        { }

        public AsyncLazy(Func<T> valueFactory, bool isThreadSafe)
            : base(() => new ValueTask<T>(Task.Factory.StartNew(valueFactory)), isThreadSafe)
        { }

        public AsyncLazy(Func<ValueTask<T>> taskFactory, bool isThreadSafe)
            : base(() => taskFactory.Invoke(), isThreadSafe)
        { }

        public AsyncLazy(Func<Task<T>> taskFactory, bool isThreadSafe)
            : base(() => new ValueTask<T>(taskFactory.Invoke()), isThreadSafe)
        { }

        public AsyncLazy(T valueFactory, LazyThreadSafetyMode mode)
            : base(() => new ValueTask<T>(valueFactory), mode)
        { }

        public AsyncLazy(Func<T> valueFactory, LazyThreadSafetyMode mode)
            : base(() => new ValueTask<T>(Task.Factory.StartNew(valueFactory)), mode)
        { }

        public AsyncLazy(Func<ValueTask<T>> taskFactory, LazyThreadSafetyMode mode)
           : base(() => taskFactory.Invoke(), mode)
        { }

        public AsyncLazy(Func<Task<T>> taskFactory, LazyThreadSafetyMode mode)
            : base(() => new ValueTask<T>(taskFactory.Invoke()), mode)
        { }
    }
}
