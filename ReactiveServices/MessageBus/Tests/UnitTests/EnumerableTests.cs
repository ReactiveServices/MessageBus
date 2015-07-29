using FluentAssertions;
using NUnit.Framework;
using ReactiveServices.Extensions;

namespace ReactiveServices.MessageBus.Tests.UnitTests
{
    [TestFixture]
    class EnumerableTests
    {
        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TestEnumerableToString()
        {
            const string separator = ", ";
            var enumerable = new[] { 0, 1, 2, 3, 4 };
            const string whatEnumerableAsStringShouldBe = "0, 1, 2, 3, 4";
            var enumerableAsString = enumerable.ToString(separator);
            enumerableAsString.Should().NotBeNull();
            enumerableAsString.Should().Be(whatEnumerableAsStringShouldBe);
        }
    }
}
