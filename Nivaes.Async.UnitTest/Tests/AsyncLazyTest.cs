namespace Nivaes.UnitTest
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    [Trait("TestType", "Unit")]
    public class AsyncLazyTest
    {

        [Fact]
        public void AsyncLazy_Success()
        {
            new AsyncLazy<int>(1).IsValueCreated.Should().BeFalse();
            new AsyncLazy<int>(1).Value.Result.Should().Be(1);
            new AsyncLazy<int>(1).Value.IsCompleted.Should().BeTrue();
            new AsyncLazy<int>(1).Value.IsCompletedSuccessfully.Should().BeTrue();
            new AsyncLazy<int>(1).Value.IsFaulted.Should().BeFalse();
            new AsyncLazy<int>(1).Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public void AsyncLazy_Sync_LoadData()
        {
            new AsyncLazy<int>((() => 1)).IsValueCreated.Should().BeFalse();
            new AsyncLazy<int>((() => 1)).Value.Result.Should().Be(1);
            new AsyncLazy<int>((() => 1)).Value.IsCompleted.Should().BeTrue();
            new AsyncLazy<int>((() => 1)).Value.IsCompletedSuccessfully.Should().BeTrue();
            new AsyncLazy<int>((() => 1)).Value.IsFaulted.Should().BeFalse();
            new AsyncLazy<int>((() => 1)).Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_Sync_Task_LoadData()
        {
            new AsyncLazy<int>((() => 1)).IsValueCreated.Should().BeFalse();
            var value = await (new AsyncLazy<int>((() => 1)).Value);
            value.Should().Be(1);
            new AsyncLazy<int>((() => 1)).Value.IsCompleted.Should().BeTrue();
            new AsyncLazy<int>((() => 1)).Value.IsCompletedSuccessfully.Should().BeTrue();
            new AsyncLazy<int>((() => 1)).Value.IsFaulted.Should().BeFalse();
            new AsyncLazy<int>((() => 1)).Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_DefaultConstructor()
        {
            var lazy = new AsyncLazy<int>();

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(0);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_DefaultConstructor_ThreadSafe()
        {
            var lazy = new AsyncLazy<int>(true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(0);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_DefaultConstructor_LazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(0);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_ValueTask()
        {
            var lazy = new AsyncLazy<int>(new ValueTask<int>(1));

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_ValueTask_ThreadSafe()
        {
            var lazy = new AsyncLazy<int>(new ValueTask<int>(1), true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_ValueTask_LazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(new ValueTask<int>(1), LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_Task()
        {
            var lazy = new AsyncLazy<int>(Task.FromResult<int>(1));

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_Task_ThreadSafe()
        {
            var lazy = new AsyncLazy<int>(Task.FromResult<int>(1), true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_Task_LazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(Task.FromResult<int>(1), LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_Valu()
        {
            var lazy = new AsyncLazy<int>(1);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_Value_ThreadSafe()
        {
            var lazy = new AsyncLazy<int>(1, true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_Value_LazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(1, LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_Func_LoadData()
        {
            var lazy = new AsyncLazy<int>(IntValueProvider);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_Func_LoadData_ThreadSafe()
        {
            var lazy = new AsyncLazy<int>(IntValueProvider, true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_Func_LoadData_LazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(IntValueProvider, LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_AsyncSync_Task_LoadData()
        {
            var lazy = new AsyncLazy<int>(IntTaskValueProvider);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeFalse();
            lazy.Value.IsCompletedSuccessfully.Should().BeFalse();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_AsyncSync_Task_LoadData_ThreadSafe()
        {
            var lazy = new AsyncLazy<int>(IntTaskValueProvider, true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeFalse();
            lazy.Value.IsCompletedSuccessfully.Should().BeFalse();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_AsyncSync_Task_LoadData_LazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(IntTaskValueProvider, LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeFalse();
            lazy.Value.IsCompletedSuccessfully.Should().BeFalse();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_AsyncSync_ValueTask_LoadData()
        {
            var lazy = new AsyncLazy<int>(IntValueTaskValueProvider);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeFalse();
            lazy.Value.IsCompletedSuccessfully.Should().BeFalse();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_AsyncSync_ValueTask_LoadData_ThreadSafe()
        {
            var lazy = new AsyncLazy<int>(IntValueTaskValueProvider, true);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeFalse();
            lazy.Value.IsCompletedSuccessfully.Should().BeFalse();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
            value.Should().Be(1);

            lazy.IsValueCreated.Should().BeTrue();
            lazy.Value.IsCompleted.Should().BeTrue();
            lazy.Value.IsCompletedSuccessfully.Should().BeTrue();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();
        }

        [Fact]
        public async Task AsyncLazy_AsyncSync_ValueTask_LoadData_LazyThreadSafetyMode()
        {
            var lazy = new AsyncLazy<int>(IntValueTaskValueProvider, LazyThreadSafetyMode.ExecutionAndPublication);

            lazy.IsValueCreated.Should().BeFalse();
            lazy.Value.IsCompleted.Should().BeFalse();
            lazy.Value.IsCompletedSuccessfully.Should().BeFalse();
            lazy.Value.IsFaulted.Should().BeFalse();
            lazy.Value.IsCanceled.Should().BeFalse();

            var value = await (lazy.Value);
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
            await Task.Delay(10);
            return 1;
        }

        private async ValueTask<int> IntValueTaskValueProvider()
        {
            await Task.Delay(10);
            return 1;
        }
    }
}
