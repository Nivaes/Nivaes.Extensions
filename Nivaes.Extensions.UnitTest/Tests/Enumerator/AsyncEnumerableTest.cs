namespace Nivaes.Async.UnitTest
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;
    using FluentAssertions;

    [Trait("TestType", "Unit")]
    public class AsyncEnumerableTest
    {
        private readonly ITestOutputHelper mTestOutputHelper;

        public AsyncEnumerableTest(ITestOutputHelper testOutputHelper)
        {
            mTestOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task IEnumerableToAsyncEnumerableTest()
        {
            IEnumerable<int> values = new List<int>() { 1, 2, 3, 4, 5 };

            var asyncValues = values.ToAsyncEnumerable();

            int i = 0;
            await foreach (var value in asyncValues)
            {
                mTestOutputHelper.WriteLine($"{value}");
                i++;
            }

            i.Should().Be(values.Count());
        }

        [Fact]
        public async Task ListToAsyncEnumerableTest()
        {
            var values = new List<int>() { 1, 2, 3, 4, 5 };

            var asyncValues = values.ToAsyncEnumerable();

            int i = 0;
            await foreach (var value in asyncValues)
            {
                mTestOutputHelper.WriteLine($"{value}");
                i++;
            }

            i.Should().Be(values.Count);
        }

        [Fact]
        public async Task ArrayToAsyncEnumerableTest()
        {
            var values = new int[] { 1, 2, 3, 4, 5 };

            var asyncValues = values.ToAsyncEnumerable();

            int i = 0;
            await foreach (var value in asyncValues)
            {
                mTestOutputHelper.WriteLine($"{value}");
                i++;
            }

            i.Should().Be(values.Length);
        }

        [Fact]
        public async Task ValueToAsyncEnumerableTest()
        {
            const int onlyValue = 3;

            var asyncValues = onlyValue.ToAsyncEnumerable();

            int i = 0;
            await foreach (var value in asyncValues)
            {
                mTestOutputHelper.WriteLine($"{value}");
                i++;
            }

            i.Should().Be(1);
        }
    }
}
