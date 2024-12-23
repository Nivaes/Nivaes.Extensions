namespace Nivaes.UnitTest
{
    using System;
    using Xunit;

    [Trait("TestType", "Unit")]
    public class GuidHelperTest
    {
        [Fact]
        public void GuidHelperTest01()
        {
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();

            var combineGuid1 = GuidHelper.Combine(guid1, guid2);
            var combineGuid2 = GuidHelper.Combine(guid2, guid1);

            Assert.Equal(combineGuid1, combineGuid2);
        }

        [Fact]
        public void GuidHelperTest02()
        {
            for (var i = 0; i < 1000; i++)
            {
                var guid1 = Guid.NewGuid();
                var guid2 = Guid.NewGuid();

                var combineGuid1 = GuidHelper.Combine(guid1, guid2);
                var combineGuid2 = GuidHelper.Combine(guid2, guid1);

                Assert.Equal(combineGuid1, combineGuid2);
            }
        }

        [Fact]
        public void GuidHelperTest03()
        {
            for (var i = 0; i < 1000; i++)
            {
                var guid1 = Guid.NewGuid();
                var guid2 = Guid.NewGuid();
                var guid3 = Guid.NewGuid();
                var guid4 = Guid.NewGuid();
                var guid5 = Guid.NewGuid();
                var guid6 = Guid.NewGuid();
                var guid7 = Guid.NewGuid();
                var guid8 = Guid.NewGuid();

                var combineGuid1 = GuidHelper.Combine(guid1, guid2, guid3, guid4, guid5, guid6, guid7, guid8);
                var combineGuid2 = GuidHelper.Combine(guid2, guid5, guid6, guid8, guid7, guid1, guid3, guid4);

                Assert.Equal(combineGuid1, combineGuid2);
            }
        }
    }
}
