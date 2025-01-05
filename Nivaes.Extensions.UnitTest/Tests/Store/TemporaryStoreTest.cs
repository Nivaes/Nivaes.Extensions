namespace Nivaes.UnitTest
{
    using System;
    using System.Diagnostics;
    using FluentAssertions;
    using Xunit;

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
            instance1.Should().NotBeNull();
            instance2.Should().NotBeNull();
            instance1.Id.Should().Be(instance2!.Id);
            instance1.Should().BeSameAs(instance2);
        }

        [Fact]
        public void ReadnTowSizeTemporaryStoreTest()
        {
            var instance1 = new TestClass1();

            var key1 = Singleton<TemporaryStore<TestClass1>>.Instance.Add(instance1);

            var result1 = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key1, out var instance2);
            result1.Should().BeTrue();
            instance1.Should().NotBeNull();
            instance2.Should().NotBeNull();
            instance1.Id.Should().Be(instance2!.Id);
            instance1.Should().BeSameAs(instance2);

            var result2 = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key1, out var instance3);
            result2.Should().BeFalse();
            instance3.Should().BeNull();
        }

        [Fact]
        public void TwoTemporaryStoreTest()
        {
            var instance1_1 = new TestClass1();
            var instance2_1 = new TestClass2();

            var key1 = Singleton<TemporaryStore<TestClass1>>.Instance.Add(instance1_1);
            var key2 = Singleton<TemporaryStore<TestClass2>>.Instance.Add(instance2_1);

            var result1 = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key1, out var instance1_2);
            result1.Should().BeTrue();
            instance1_1.Should().NotBeNull();
            instance1_2.Should().NotBeNull();
            instance1_1.Id.Should().Be(instance1_2!.Id);
            instance1_1.Should().BeSameAs(instance1_2);

            var result2 = Singleton<TemporaryStore<TestClass2>>.Instance.TryGetAndRemove(key2, out var instance2_2);
            result2.Should().BeTrue();
            instance2_1.Should().NotBeNull();
            instance2_2.Should().NotBeNull();
            instance2_1.Id.Should().Be(instance2_2!.Id);
            instance2_1.Should().BeSameAs(instance2_2);
        }

        [Fact]
        public void TwoObjectInTemporaryStoreTest()
        {
            var instance1_1 = new TestClass1();
            var instance2_1 = new TestClass1();

            var key1 = Singleton<TemporaryStore<TestClass1>>.Instance.Add(instance1_1);
            var key2 = Singleton<TemporaryStore<TestClass1>>.Instance.Add(instance2_1);

            var result1 = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key1, out var instance1_2);
            result1.Should().BeTrue();
            instance1_1.Should().NotBeNull();
            instance1_2.Should().NotBeNull();
            instance1_1.Id.Should().Be(instance1_2!.Id);
            instance1_1.Should().BeSameAs(instance1_2);

            var result2 = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key2, out var instance2_2);
            result2.Should().BeTrue();
            instance2_1.Should().NotBeNull();
            instance2_2.Should().NotBeNull();
            instance2_1.Id.Should().Be(instance2_2!.Id);
            instance2_1.Should().BeSameAs(instance2_2);
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
                result.Should().BeTrue();
                Console.WriteLine("T2_1 completada");
            });
            Task t2_2 = Task.Run(() =>
            {
                Debug.WriteLine("Iniciando T1_2...");
                var result = Singleton<TemporaryStore<TestClass1>>.Instance.TryGetAndRemove(key2, out instance2_2);
                result.Should().BeTrue();
                Debug.WriteLine("T2_2 completada ");
            });

            await Task.WhenAll(t2_1, t2_2);

            key1.Should().BeGreaterThan(0);
            key2.Should().BeGreaterThan(0);

            instance1_1.Should().NotBeNull();
            instance1_2.Should().NotBeNull();
            instance2_1.Should().NotBeNull();
            instance2_2.Should().NotBeNull();

            instance1_1!.Should().BeSameAs(instance1_2);
            instance2_1!.Should().BeSameAs(instance2_2);
            instance1_1!.Id.Should().Be(instance1_2!.Id);
            instance2_1!.Id.Should().Be(instance2_2!.Id);
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
            result1.Should().BeFalse();
            instance1_2.Should().BeNull();

            var result2 = store.TryGetAndRemove(key2, out var instance2_2);
            result2.Should().BeTrue();
            instance2_1.Should().NotBeNull();
            instance2_2.Should().NotBeNull();
            instance2_1.Id.Should().Be(instance2_2!.Id);
            instance2_1.Should().BeSameAs(instance2_2);
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
            result1.Should().BeFalse();
            instance1_2.Should().BeNull();

            var result2 = store.TryGetAndRemove(key2, out var instance2_2);
            result2.Should().BeTrue();
            instance2_1.Should().NotBeNull();
            instance2_2.Should().NotBeNull();
            instance2_1.Id.Should().Be(instance2_2!.Id);
            instance2_1.Should().BeSameAs(instance2_2);
        }
    }
}
