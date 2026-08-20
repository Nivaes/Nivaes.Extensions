namespace Nivaes.Extensions.UnitTest;

[Trait("TestType", "Unit")]

public class CollectionTest
{
    [Fact]
    public void AddRangeTest()
    {
        var collection = new List<int>() { 1, 2, 3 };
        var query = new List<int>() { 4, 5, 6 };
        collection.AddRange(query);
        collection.Count.ShouldBe(6);
        collection.ShouldContain(1);
        collection.ShouldContain(2);
        collection.ShouldContain(3);
        collection.ShouldContain(4);
        collection.ShouldContain(5);
        collection.ShouldContain(6);
    }
}
