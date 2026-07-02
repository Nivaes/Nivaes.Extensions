using System;
using Shouldly;
using Xunit;

namespace Nivaes.Extensions.UnitTest;

[Trait("TestType", "Unit")]
public class SingletonTest
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
    public void OneSingletonTest()
    {
        var instance1 = Singleton<TestClass1>.Instance;
        instance1.ShouldNotBeNull();

        var instance2 = Singleton<TestClass1>.Instance;
        instance2.ShouldNotBeNull();

        instance1.Id.ShouldBe(instance2.Id);
    }

    [Fact]
    public void TwoSingletonTest()
    {
        var instance1 = Singleton<TestClass1>.Instance;
        instance1.ShouldNotBeNull();

        var instance2 = Singleton<TestClass2>.Instance;
        instance2.ShouldNotBeNull();

        instance1.Id.ShouldNotBe(instance2.Id);
    }

    [Fact]
    public void MultipleTest()
    {
        for (int i = 0; i < 10000; i++)
        {
            var instance1 = Singleton<TestClass1>.Instance;
            instance1.ShouldNotBeNull();

            var instance2 = Singleton<TestClass2>.Instance;
            instance2.ShouldNotBeNull();

            instance1.Id.ShouldNotBe(instance2.Id);
        }
    }

    [Fact]
    public void ClearSingletonTest()
    {
        var instance1 = Singleton<TestClass1>.Instance;
        instance1.ShouldNotBeNull();

        Singleton<TestClass1>.Clear();

        var instance2 = Singleton<TestClass1>.Instance;
        instance2.ShouldNotBeNull();

        instance1.ShouldNotBe(instance2);
        instance1.ShouldNotBeSameAs(instance2);
        instance1.Id.ShouldNotBe(instance2.Id);
    }

    [Fact]
    public void AddSingletonTest()
    {
        var instance1 = new TestClass1();

        Singleton<TestClass1>.Add(instance1);

        var instance2 = Singleton<TestClass1>.Instance;

        instance1.Id.ShouldBe(instance2.Id);
        instance1.ShouldBeSameAs(instance2);
    }

    [Fact]
    public async Task MultiTaskSingletonTest01()
    {
        TestClass1? instance1 = null, instance2 = null, instance3 = null;
        Task t1 = Task.Run(() =>
        {
            instance1 = Singleton<TestClass1>.Instance;
        }, TestContext.Current.CancellationToken);

        Task t2 = Task.Run(() =>
        {
            instance2 = Singleton<TestClass1>.Instance;
        }, TestContext.Current.CancellationToken);

        Task t3 = Task.Run(() =>
        {
            instance3 = Singleton<TestClass1>.Instance;
        }, TestContext.Current.CancellationToken);

        await Task.WhenAll(t1, t2, t3);

        instance1.ShouldNotBeNull();
        instance2.ShouldNotBeNull();
        instance3.ShouldNotBeNull();

        instance1!.ShouldBeSameAs(instance2);
        instance1!.ShouldBeSameAs(instance3);
        instance1!.Id.ShouldBe(instance2!.Id);
        instance1!.Id.ShouldBe(instance3!.Id);
    }

    [Fact]
    public async Task MultiTaskSingletonTest02()
    {
        TestClass1? instance1 = null, instance2 = null, instance3 = null;

        Task t1 = Task.Run(() =>
        {
            instance1 = new TestClass1();
            Singleton<TestClass1>.Add(instance1);
        }, TestContext.Current.CancellationToken);
        await Task.WhenAll(t1);

        Task t2 = Task.Run(() =>
        {
            instance2 = Singleton<TestClass1>.Instance;
        }, TestContext.Current.CancellationToken);

        Task t3 = Task.Run(() =>
        {
            instance3 = Singleton<TestClass1>.Instance;
        }, TestContext.Current.CancellationToken);

        await Task.WhenAll(t2, t3);

        instance1.ShouldNotBeNull();
        instance2.ShouldNotBeNull();
        instance3.ShouldNotBeNull();

        instance1!.ShouldBeSameAs(instance2);
        instance1!.ShouldBeSameAs(instance3);
        instance1!.Id.ShouldBe(instance2!.Id);
        instance1!.Id.ShouldBe(instance3!.Id);
    }
}
