namespace Nivaes.App.UnitTest
{
    [Trait("TestType", "Unit")]
    public class KeyContainerManagerTest
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
        public void LoadContainerManagerTest()
        {
            var instance1_1 = new TestClass1();
            var instance2_1 = new TestClass1();
            const int key1 = 56226, key2 = 88951;

            var manager = new KeyContainerManager<TestClass1>(new[]
            {
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 1232, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 553681, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 8851, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 88442, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 87942, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = key1, Value = instance1_1 },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 615882, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 2233584, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 5584, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = key2, Value = instance2_1},
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 223213622, Value = new TestClass1() },
            });

            var result1 = manager.TryGetValue(key1, out var instance1_2);
            result1.ShouldBeTrue();

            instance1_1.ShouldNotBeNull();
            instance1_2.ShouldNotBeNull();
            instance1_1.Id.ShouldBe(instance1_2!.Id);
            instance1_1.ShouldBeSameAs(instance1_2);

            var result2 = manager.TryGetValue(key2, out var instance2_2);
            result2.ShouldBeTrue();

            instance2_1.ShouldNotBeNull();
            instance2_2.ShouldNotBeNull();
            instance2_1.Id.ShouldBe(instance2_2!.Id);
            instance2_1.ShouldBeSameAs(instance2_2);
        }

        [Fact]
        public void MergeContainerManagerTest()
        {
            var instance1_1 = new TestClass1();
            var instance2_1 = new TestClass1();
            int key1 = 56226, key2 = 88951;

            var manager = new KeyContainerManager<TestClass1>(new[]
            {
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 1232, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 553681, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 8851, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 88442, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 87942, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = key1, Value = instance1_1 },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 615882, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 2233584, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 5584, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 223213622, Value = new TestClass1() },
            });

            manager.Merge(new[]
            {
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 24232, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 912793, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 15217, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 3432, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 1685, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 1345234, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 3242113, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 123423, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = key2, Value = instance2_1},
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 3151233, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 23423425, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 23432, Value = new TestClass1() },
                new KeyContainerManager<TestClass1>.KeyStoreItem { Key = 2572435, Value = new TestClass1() },
            });

            var result1 = manager.TryGetValue(key1, out var instance1_2);
            result1.ShouldBeTrue();

            instance1_1.ShouldNotBeNull();
            instance1_2.ShouldNotBeNull();
            instance1_1.Id.ShouldBe(instance1_2!.Id);
            instance1_1.ShouldBeSameAs(instance1_2);

            var result2 = manager.TryGetValue(key2, out var instance2_2);
            result2.ShouldBeTrue();

            instance2_1.ShouldNotBeNull();
            instance2_2.ShouldNotBeNull();
            instance2_1.Id.ShouldBe(instance2_2!.Id);
            instance2_1.ShouldBeSameAs(instance2_2);
        }
    }
}
