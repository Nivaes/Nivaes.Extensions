namespace Nivaes
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    public class AsyncLazy<T> : Lazy<ValueTask<T>>
    {
        [DebuggerHidden]
        public AsyncLazy()
            : base()
        { }

        [DebuggerHidden]
        public AsyncLazy(bool isThreadSafe)
            : base(isThreadSafe)
        { }

        [DebuggerHidden]
        public AsyncLazy(T valueFactory)
            : base(() => new ValueTask<T>(valueFactory))
        { }

        [DebuggerHidden]
        public AsyncLazy(Func<T> valueFactory)
            : base(() => new ValueTask<T>(valueFactory.Invoke()))
        { }

        [DebuggerHidden]
        public AsyncLazy(Func<Task<T>> taskFactory)
            : base(() => new ValueTask<T>(taskFactory.Invoke()))
        { }

        [DebuggerStepperBoundary]
        public AsyncLazy(LazyThreadSafetyMode mode)
            : base(mode)
        { }

        [DebuggerHidden]
        public AsyncLazy(T valueFactory, bool isThreadSafe)
            : base(() => new ValueTask<T>(valueFactory), isThreadSafe)
        { }

        [DebuggerHidden]
        public AsyncLazy(Func<T> valueFactory, bool isThreadSafe)
            : base(() => new ValueTask<T>(Task.Factory.StartNew(valueFactory)), isThreadSafe)
        { }

        [DebuggerHidden]
        public AsyncLazy(Func<Task<T>> taskFactory, bool isThreadSafe)
            : base(() => new ValueTask<T>(taskFactory.Invoke()), isThreadSafe)
        { }

        [DebuggerHidden]
        public AsyncLazy(T valueFactory, LazyThreadSafetyMode mode)
            : base(() => new ValueTask<T>(valueFactory), mode)
        { }

        [DebuggerHidden]
        [DebuggerStepperBoundary]
        public AsyncLazy(Func<T> valueFactory, LazyThreadSafetyMode mode)
            : base(() => new ValueTask<T>(Task.Factory.StartNew(valueFactory)), mode)
        { }

        [DebuggerHidden]
        public AsyncLazy(Func<Task<T>> taskFactory, LazyThreadSafetyMode mode)
            : base(() => new ValueTask<T>(taskFactory.Invoke()), mode)
        { }
    }
}
