namespace Nivaes.Async.UnitTest
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    [Trait("TestType", "Unit")]
    public class AsyncLazyTest
    {

        [Fact]
        public async Task AsyncLazySuccess()
        {
            new AsyncLazy<int>(1).IsValueCreated.Should().BeFalse();
            (await new AsyncLazy<int>(1).Value).Should().Be(1);
            new AsyncLazy<int>(1).Value.IsCompleted.Should().BeTrue();
            new AsyncLazy<int>(1).Value.IsCompletedSuccessfully.Should().BeTrue();
            new AsyncLazy<int>(1).Value.IsFaulted.Should().BeFalse();
            new AsyncLazy<int>(1).Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazySyncLoadData()
        {
            new AsyncLazy<int>((() => 1)).IsValueCreated.Should().BeFalse();
            (await new AsyncLazy<int>((() => 1)).Value).Should().Be(1);
            new AsyncLazy<int>((() => 1)).Value.IsCompleted.Should().BeTrue();
            new AsyncLazy<int>((() => 1)).Value.IsCompletedSuccessfully.Should().BeTrue();
            new AsyncLazy<int>((() => 1)).Value.IsFaulted.Should().BeFalse();
            new AsyncLazy<int>((() => 1)).Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazySyncTaskLoadData()
        {
            new AsyncLazy<int>((() => 1)).IsValueCreated.Should().BeFalse();
            var value = await (new AsyncLazy<int>((() => 1)).Value).ConfigureAwait(true);
            value.Should().Be(1);
            new AsyncLazy<int>((() => 1)).Value.IsCompleted.Should().BeTrue();
            new AsyncLazy<int>((() => 1)).Value.IsCompletedSuccessfully.Should().BeTrue();
            new AsyncLazy<int>((() => 1)).Value.IsFaulted.Should().BeFalse();
            new AsyncLazy<int>((() => 1)).Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyDefaultConstructor()
        {
            var lazy = new AsyncLazy<int>();

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(0);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyDefaultConstructorThreadSafe()
        {
            var lazy = new AsyncLazy<int>(true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(0);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyDefaultConstructorLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(0);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyValueTask()
        {
            var lazy = new AsyncLazy<int>(new ValueTask<int>(1));

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyValueTaskThreadSafe()
        {
            var lazy = new AsyncLazy<int>(new ValueTask<int>(1), true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyValueTaskLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(new ValueTask<int>(1), LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyTask()
        {
            var lazy = new AsyncLazy<int>(Task.FromResult<int>(1));

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyTaskThreadSafe()
        {
            var lazy = new AsyncLazy<int>(Task.FromResult<int>(1), true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyTaskLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(Task.FromResult<int>(1), LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyValue()
        {
            var lazy = new AsyncLazy<int>(1);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyValueThreadSafe()
        {
            var lazy = new AsyncLazy<int>(1, true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyValueLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(1, LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyFuncLoadData()
        {
            var lazy = new AsyncLazy<int>(IntValueProvider);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyFuncLoadDataThreadSafe()
        {
            var lazy = new AsyncLazy<int>(IntValueProvider, true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyFuncLoadDataLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(IntValueProvider, LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyAsyncSyncTaskLoadData()
        {
            var lazy = new AsyncLazy<int>(IntTaskValueProvider);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeFalse();
            lazy.Value.IsCompletedSuccessfully.Should().BeFalse();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyAsyncSyncTaskLoadDataThreadSafe()
        {
            var lazy = new AsyncLazy<int>(IntTaskValueProvider, true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeFalse();
            lazy.Value.IsCompletedSuccessfully.Should().BeFalse();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyAsyncSyncTaskLoadDataLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(IntTaskValueProvider, LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeFalse();
            lazy.Value.IsCompletedSuccessfully.Should().BeFalse();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyAsyncSyncValueTaskLoadData()
        {
            var lazy = new AsyncLazy<int>(IntValueTaskValueProvider);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeFalse();
            lazy.Value.IsCompletedSuccessfully.Should().BeFalse();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyAsyncSyncValueTaskLoadDataThreadSafe()
        {
            var lazy = new AsyncLazy<int>(IntValueTaskValueProvider, true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeFalse();
            lazy.Value.IsCompletedSuccessfully.Should().BeFalse();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazyAsyncSyncValueTaskLoadDataLazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(IntValueTaskValueProvider, LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeFalse();
            lazy.Value.IsCompletedSuccessfully.Should().BeFalse();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value).ConfigureAwait(true);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        private int IntValueProvider()
        {
            return 1;
        }

        private async Task<int> IntTaskValueProvider()
        {
            await Task.Delay(10).ConfigureAwait(true);
            return 1;
        }

        private async ValueTask<int> IntValueTaskValueProvider()
        {
            await Task.Delay(10).ConfigureAwait(true);
            return 1;
        }
    }
}
