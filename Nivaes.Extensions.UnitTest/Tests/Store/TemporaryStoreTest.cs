using System.Diagnostics;
using Shouldly;
using Xunit;

namespace Nivaes.UnitTest;


[Trait("TestType", "Unit")]
public class TemporaryStoreTest
{
    #region TestClass
    public class TestClass1()
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    public class TestClass2()
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
    #endregion

    [Fact]
    public void OneTemporaryStoreTest()
    {
        var instance1 = new TestClass1();

        var key1 = Singleton<TemporaryStore<TestClass1>>.Instance.Add(instance1);

        Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key1, out var instance2);
        instance1.ShouldNotBeNull();
        instance2.ShouldNotBeNull();
        instance1.Id.ShouldBe(instance2!.Id);
        instance1.ShouldBeSameAs(instance2);
    }

    [Fact]
    public void ReadnTowSizeTemporaryStoreTest()
    {
        var instance1 = new TestClass1();

        var key1 = Singleton<TemporaryStore<TestClass1>>.Instance.Add(instance1);

        var result1 = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key1, out var instance2);
        result1.ShouldBeTrue();
        instance1.ShouldNotBeNull();
        instance2.ShouldNotBeNull();
        instance1.Id.ShouldBe(instance2!.Id);
        instance1.ShouldBeSameAs(instance2);

        var result2 = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key1, out var instance3);
        result2.ShouldBeFalse();
        instance3.ShouldBeNull();
    }

    [Fact]
    public void TwoTemporaryStoreTest()
    {
        var instance1_1 = new TestClass1();
        var instance2_1 = new TestClass2();

        var key1 = Singleton<TemporaryStore<TestClass1>>.Instance.Add(instance1_1);
        var key2 = Singleton<TemporaryStore<TestClass2>>.Instance.Add(instance2_1);

        var result1 = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key1, out var instance1_2);
        result1.ShouldBeTrue();
        instance1_1.ShouldNotBeNull();
        instance1_2.ShouldNotBeNull();
        instance1_1.Id.ShouldBe(instance1_2!.Id);
        instance1_1.ShouldBeSameAs(instance1_2);

        var result2 = Singleton<TemporaryStore<TestClass2>>.Instance.TryGetAndRemove(key2, out var instance2_2);
        result2.ShouldBeTrue();
        instance2_1.ShouldNotBeNull();
        instance2_2.ShouldNotBeNull();
        instance2_1.Id.ShouldBe(instance2_2!.Id);
        instance2_1.ShouldBeSameAs(instance2_2);
    }

    [Fact]
    public void TwoObjectInTemporaryStoreTest()
    {
        var instance1_1 = new TestClass1();
        var instance2_1 = new TestClass1();

        var key1 = Singleton<TemporaryStore<TestClass1>>.Instance.Add(instance1_1);
        var key2 = Singleton<TemporaryStore<TestClass1>>.Instance.Add(instance2_1);

        var result1 = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key1, out var instance1_2);
        result1.ShouldBeTrue();
        instance1_1.ShouldNotBeNull();
        instance1_2.ShouldNotBeNull();
        instance1_1.Id.ShouldBe(instance1_2!.Id);
        instance1_1.ShouldBeSameAs(instance1_2);

        var result2 = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key2, out var instance2_2);
        result2.ShouldBeTrue();
        instance2_1.ShouldNotBeNull();
        instance2_2.ShouldNotBeNull();
        instance2_1.Id.ShouldBe(instance2_2!.Id);
        instance2_1.ShouldBeSameAs(instance2_2);
    }

    [Fact]
    public async Task MultiTaskTemporaryStoreTest01()
    {
        TestClass1? instance1_1 = null, instance1_2 = null, instance2_1 = null, instance2_2 = null;
        int key1 = 0, key2 = 0;
        Task t1_1 = Task.Run(() =>
        {
            Console.WriteLine("Iniciando T1_1...");
            instance1_1 = new TestClass1();
            key1 = Singleton<TemporaryStore<TestClass1>>.Instance.Add(instance1_1);
            Console.WriteLine("T1_1 completada");
        });

        Task t1_2 = Task.Run(() =>
        {
            Console.WriteLine("Iniciando T1_2...");
            instance2_1 = new TestClass1();
            key2 = Singleton<TemporaryStore<TestClass1>>.Instance.Add(instance2_1);
            Console.WriteLine("T1_2 completada");
        });

        await Task.WhenAll(t1_1, t1_2);

        Task t2_1 = Task.Run(() =>
        {
            Console.WriteLine("Iniciando Task1...");
            var result = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key1, out instance1_2);
            result.ShouldBeTrue();
            Console.WriteLine("T2_1 completada");
        });
        Task t2_2 = Task.Run(() =>
        {
            Debug.WriteLine("Iniciando T1_2...");
            var result = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key2, out instance2_2);
            result.ShouldBeTrue();
            Debug.WriteLine("T2_2 completada ");
        });

        await Task.WhenAll(t2_1, t2_2);

        key1.ShouldBeGreaterThan(0);
        key2.ShouldBeGreaterThan(0);

        instance1_1.ShouldNotBeNull();
        instance1_2.ShouldNotBeNull();
        instance2_1.ShouldNotBeNull();
        instance2_2.ShouldNotBeNull();

        instance1_1!.ShouldBeSameAs(instance1_2);
        instance2_1!.ShouldBeSameAs(instance2_2);
        instance1_1!.Id.ShouldBe(instance1_2!.Id);
        instance2_1!.Id.ShouldBe(instance2_2!.Id);
    }

    [Fact]
    public async Task CleanTemporaryStoreTest()
    {
        var instance1_1 = new TestClass1();
        var instance2_1 = new TestClass1();

        var store = new TemporaryStore<TestClass1>();

        var key1 = store.Add(instance1_1, durationMilliseconds:100);
        var key2 = store.Add(instance2_1);
        await Task.Delay(101);

        var result1 = store.TryGetAndRemove(key1, out var instance1_2);
        result1.ShouldBeFalse();
        instance1_2.ShouldBeNull();

        var result2 = store.TryGetAndRemove(key2, out var instance2_2);
        result2.ShouldBeTrue();
        instance2_1.ShouldNotBeNull();
        instance2_2.ShouldNotBeNull();
        instance2_1.Id.ShouldBe(instance2_2!.Id);
        instance2_1.ShouldBeSameAs(instance2_2);
    }

    [Fact]
    public async Task CleanupTemporaryStoreTest()
    {
        var instance1_1 = new TestClass1();
        var instance2_1 = new TestClass1();

        var store = new TemporaryStore<TestClass1>();

        var key1 = store.Add(instance1_1, durationMilliseconds: 100);
        var key2 = store.Add(instance2_1);
        await Task.Delay(101);

        store.Cleanup();

        var result1 = store.TryGetAndRemove(key1, out var instance1_2);
        result1.ShouldBeFalse();
        instance1_2.ShouldBeNull();

        var result2 = store.TryGetAndRemove(key2, out var instance2_2);
        result2.ShouldBeTrue();
        instance2_1.ShouldNotBeNull();
        instance2_2.ShouldNotBeNull();
        instance2_1.Id.ShouldBe(instance2_2!.Id);
        instance2_1.ShouldBeSameAs(instance2_2);
    }
}
