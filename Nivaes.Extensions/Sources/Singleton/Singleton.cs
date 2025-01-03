namespace Nivaes
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    public abstract class Singleton
    {
        protected Singleton()
        {
        }

        protected static readonly ConcurrentDictionary<Type, Singleton> Instances = new();

        public abstract void ClearInstance();

        public static void Clear()
        {
            foreach (var instance in Instances)
            {
                if (Instances.TryRemove(instance.Key, out Singleton? value))
                {
                    value.ClearInstance();
                }
            }
        }
    }

    public class Singleton<TValue> : Singleton
         where TValue : new()
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2743:Static fields should not be used in generic types", Justification = "<Pending>")]
        private static object? mInstance;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "<Pending>")]
        public override void ClearInstance()
        {
            mInstance = null;
        }

        public static TValue Instance
        {
            get
            {
                if (mInstance == null)
                {
                    Instances.GetOrAdd(typeof(TValue), (_) =>
                    {
                        mInstance = new TValue();
                        return new Singleton<TValue>();
                    });
                }
                return (TValue)mInstance!;
            }
        }

        public static void Add(TValue instance)
        {
            Instances.AddOrUpdate(typeof(TValue),
                (_) =>
                {
                    mInstance = instance;
                    return new Singleton<TValue>();
                },
                (_, _) =>
                {
                    mInstance = instance;
                    return new Singleton<TValue>();
                }
            );
        }
    }
}
