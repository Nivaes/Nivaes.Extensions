namespace Nivaes.UnitTest;

[Trait("TestType", "Unit")]
public class AsyncEnumerableTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AsyncEnumerableTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task IEnumerableToAsyncEnumerableTest()
    {
        IEnumerable<int> values = new List<int>() { 1, 2, 3, 4, 5 };

        var asyncValues = values.ToAsyncEnumerable();

        int i = 0;
        await foreach (var value in asyncValues)
        {
            _testOutputHelper.WriteLine($"{value}");
            i++;
        }

        i.ShouldBe(values.Count());
    }

    [Fact]
    public async Task ListToAsyncEnumerableTest()
    {
        var values = new List<int>() { 1, 2, 3, 4, 5 };

        var asyncValues = values.ToAsyncEnumerable();

        int i = 0;
        await foreach (var value in asyncValues)
        {
            _testOutputHelper.WriteLine($"{value}");
            i++;
        }

        i.ShouldBe(values.Count);
    }

    [Fact]
    public async Task ArrayToAsyncEnumerableTest()
    {
        var values = new int[] { 1, 2, 3, 4, 5 };

        var asyncValues = values.ToAsyncEnumerable();

        int i = 0;
        await foreach (var value in asyncValues)
        {
            _testOutputHelper.WriteLine($"{value}");
            i++;
        }

        i.ShouldBe(values.Length);
    }
}
