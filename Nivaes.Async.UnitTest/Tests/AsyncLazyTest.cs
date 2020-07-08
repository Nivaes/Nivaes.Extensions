namespace Nivaes.UnitTest
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;
    using FluentAssertions;
    using System.Threading;

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
        public async Task AsyncLazy_AsyncSync_Task_LoadData()
        {
            var lazy = new AsyncLazy<int>(IntValueProvider);

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

        private async Task<int> IntValueProvider()
        {
            await Task.Delay(10);
            return 1;
        }
    }
}
