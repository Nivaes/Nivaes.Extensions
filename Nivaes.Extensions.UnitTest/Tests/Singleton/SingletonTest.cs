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

            Singleton<TestClass2>.Clear();

            var instance2 = Singleton<TestClass1>.Instance;
            instance2.Should().NotBeNull();

            instance1.Id.Should().NotBe(instance2.Id);
        }
    }
}
