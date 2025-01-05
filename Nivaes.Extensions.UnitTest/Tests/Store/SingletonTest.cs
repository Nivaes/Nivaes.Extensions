namespace Nivaes.UnitTest
{
    using System;
    using FluentAssertions;
    using Xunit;

    [Trait("TestType", "Unit")]
    public class SingletonTest
    {
        public class TestClass1()
        {
            public Guid Id { get; } = Guid.NewGuid();
        }

        public class TestClass2()
        {
            public Guid Id { get; } = Guid.NewGuid();
        }

        [Fact]
        public void OneSingletonTest()
        {
            var instance1 = Singleton<TestClass1>.Instance;
            instance1.Should().NotBeNull();

            var instance2 = Singleton<TestClass1>.Instance;
            instance2.Should().NotBeNull();

            instance1.Id.Should().Be(instance2.Id);
        }

        [Fact]
        public void TwoSingletonTest()
        {
            var instance1 = Singleton<TestClass1>.Instance;
            instance1.Should().NotBeNull();

            var instance2 = Singleton<TestClass2>.Instance;
            instance2.Should().NotBeNull();

            instance1.Id.Should().NotBe(instance2.Id);
        }

        [Fact]
        public void MultipleTest()
        {
            for (int i = 0; i < 10000; i++)
            {
                var instance1 = Singleton<TestClass1>.Instance;
                instance1.Should().NotBeNull();

                var instance2 = Singleton<TestClass2>.Instance;
                instance2.Should().NotBeNull();

                instance1.Id.Should().NotBe(instance2.Id);
            }
        }

        [Fact]
        public void ClearSingletonTest()
        {
            var instance1 = Singleton<TestClass1>.Instance;
            instance1.Should().NotBeNull();

            Singleton<TestClass1>.Clear();

            var instance2 = Singleton<TestClass1>.Instance;
            instance2.Should().NotBeNull();

            instance1.Should().NotBe(instance2);
            instance1.Should().NotBeSameAs(instance2);
            instance1.Id.Should().NotBe(instance2.Id);
        }

        [Fact]
        public void AddSingletonTest()
        {
            var instance1 = new TestClass1();

            Singleton<TestClass1>.Add(instance1);

            var instance2 = Singleton<TestClass1>.Instance;

            instance1.Id.Should().Be(instance2.Id);
            instance1.Should().BeSameAs(instance2);
        }

        [Fact]
        public async Task MultiTaskSingletonTest01()
        {
            TestClass1? instance1 = null, instance2 = null, instance3 = null;
            Task t1 = Task.Run(() =>
            {
                //instance1 = new TestClass1();
                //Singleton<TestClass1>.Add(instance1);
                instance1 = Singleton<TestClass1>.Instance;
            });

            Task t2 = Task.Run(() =>
            {
                instance2 = Singleton<TestClass1>.Instance;
            });
            Task t3 = Task.Run(() =>
            {
                instance3 = Singleton<TestClass1>.Instance;
            });

            await Task.WhenAll(t1, t2, t3);

            instance1.Should().NotBeNull();
            instance2.Should().NotBeNull();
            instance3.Should().NotBeNull();

            instance1!.Should().BeSameAs(instance2);
            instance1!.Should().BeSameAs(instance3);
            instance1!.Id.Should().Be(instance2!.Id);
            instance1!.Id.Should().Be(instance3!.Id);
        }

        [Fact]
        public async Task MultiTaskSingletonTest02()
        {
            TestClass1? instance1 = null, instance2 = null, instance3 = null;
            Task t1 = Task.Run(() =>
            {
                instance1 = new TestClass1();
                Singleton<TestClass1>.Add(instance1);
            });

            Task t2 = Task.Run(() =>
            {
                instance2 = Singleton<TestClass1>.Instance;
            });
            Task t3 = Task.Run(() =>
            {
                instance3 = Singleton<TestClass1>.Instance;
            });

            await t1;
            await Task.WhenAll(t2, t3);

            instance1.Should().NotBeNull();
            instance2.Should().NotBeNull();
            instance3.Should().NotBeNull();

            instance1!.Should().BeSameAs(instance2);
            instance1!.Should().BeSameAs(instance3);
            instance1!.Id.Should().Be(instance2!.Id);
            instance1!.Id.Should().Be(instance3!.Id);
        }
    }
}
