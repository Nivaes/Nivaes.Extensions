namespace Nivaes.Extensions.UnitTest
{
    [Trait("TestType", "Unit")]
    public class AsyncLazyTest
    {

        [Fact]
        public async Task AsyncLazySuccess()
        {
            new AsyncLazy<int>(1).IsValueCreated.ShouldBeFalse();
            (await new AsyncLazy<int>(1).Value).ShouldBe(1);
            new AsyncLazy<int>(1).Value.IsCompleted.ShouldBeTrue();
            new AsyncLazy<int>(1).Value.IsCompletedSuccessfully.ShouldBeTrue();
            new AsyncLazy<int>(1).Value.IsFaulted.ShouldBeFalse();
            new AsyncLazy<int>(1).Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazySyncLoadData()
        {
            new AsyncLazy<int>((() => 1)).IsValueCreated.ShouldBeFalse();
            (await new AsyncLazy<int>((() => 1)).Value).ShouldBe(1);
            new AsyncLazy<int>((() => 1)).Value.IsCompleted.ShouldBeTrue();
            new AsyncLazy<int>((() => 1)).Value.IsCompletedSuccessfully.ShouldBeTrue();
            new AsyncLazy<int>((() => 1)).Value.IsFaulted.ShouldBeFalse();
            new AsyncLazy<int>((() => 1)).Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazySyncTaskLoadData()
        {
            new AsyncLazy<int>((() => 1)).IsValueCreated.ShouldBeFalse();
            var value = await (new AsyncLazy<int>((() => 1)).Value).ConfigureAwait(true);
            value.ShouldBe(1);
            new AsyncLazy<int>((() => 1)).Value.IsCompleted.ShouldBeTrue();
            new AsyncLazy<int>((() => 1)).Value.IsCompletedSuccessfully.ShouldBeTrue();
            new AsyncLazy<int>((() => 1)).Value.IsFaulted.ShouldBeFalse();
            new AsyncLazy<int>((() => 1)).Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyDefaultConstructor()
        {
            var lazy = new AsyncLazy<int>();

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(0);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyDefaultConstructorThreadSafe()
        {
            var lazy = new AsyncLazy<int>(true);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(0);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyDefaultConstructorLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(0);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyValueTask()
        {
            var lazy = new AsyncLazy<int>(new ValueTask<int>(1));

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyValueTaskThreadSafe()
        {
            var lazy = new AsyncLazy<int>(new ValueTask<int>(1), true);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyValueTaskLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(new ValueTask<int>(1), LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyTask()
        {
            var lazy = new AsyncLazy<int>(Task.FromResult<int>(1));

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyTaskThreadSafe()
        {
            var lazy = new AsyncLazy<int>(Task.FromResult<int>(1), true);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyTaskLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(Task.FromResult<int>(1), LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyValue()
        {
            var lazy = new AsyncLazy<int>(1);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyValueThreadSafe()
        {
            var lazy = new AsyncLazy<int>(1, true);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyValueLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(1, LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyFuncLoadData()
        {
            var lazy = new AsyncLazy<int>(IntValueProvider);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyFuncLoadDataThreadSafe()
        {
            var lazy = new AsyncLazy<int>(IntValueProvider, true);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyFuncLoadDataLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(IntValueProvider, LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyAsyncSyncTaskLoadData()
        {
            var lazy = new AsyncLazy<int>(IntTaskValueProvider);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeFalse();
            lazy.Value.IsCompletedSuccessfully.ShouldBeFalse();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyAsyncSyncTaskLoadDataThreadSafe()
        {
            var lazy = new AsyncLazy<int>(IntTaskValueProvider, true);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeFalse();
            lazy.Value.IsCompletedSuccessfully.ShouldBeFalse();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyAsyncSyncTaskLoadDataLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(IntTaskValueProvider, LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeFalse();
            lazy.Value.IsCompletedSuccessfully.ShouldBeFalse();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyAsyncSyncValueTaskLoadData()
        {
            var lazy = new AsyncLazy<int>(IntValueTaskValueProvider);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeFalse();
            lazy.Value.IsCompletedSuccessfully.ShouldBeFalse();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyAsyncSyncValueTaskLoadDataThreadSafe()
        {
            var lazy = new AsyncLazy<int>(IntValueTaskValueProvider, true);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeFalse();
            lazy.Value.IsCompletedSuccessfully.ShouldBeFalse();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        [Fact]
        public async Task AsyncLazyAsyncSyncValueTaskLoadDataLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(IntValueTaskValueProvider, LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.ShouldBeFalse();
            lazy.Value.IsCompleted.ShouldBeFalse();
            lazy.Value.IsCompletedSuccessfully.ShouldBeFalse();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.ShouldBe(1);

            lazy.IsValueCreated.ShouldBeTrue();
            lazy.Value.IsCompleted.ShouldBeTrue();
            lazy.Value.IsCompletedSuccessfully.ShouldBeTrue();
            lazy.Value.IsFaulted.ShouldBeFalse();
            lazy.Value.IsCanceled.ShouldBeFalse();
        }

        private static int IntValueProvider()
        {
            return 1;
        }

        private static async Task<int> IntTaskValueProvider()
        {
            await Task.Delay(10).ConfigureAwait(true);
            return 1;
        }

        private static async ValueTask<int> IntValueTaskValueProvider()
        {
            await Task.Delay(10).ConfigureAwait(true);
            return 1;
        }
    }
}
