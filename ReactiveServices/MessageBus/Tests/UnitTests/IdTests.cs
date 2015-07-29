using FluentAssertions;
using NUnit.Framework;
using System;

namespace ReactiveServices.MessageBus.Tests.UnitTests
{
    [TestFixture]
    public class IdTests
    {
        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TestIdCreationFromString()
        {
            const string myIdValue = "myIdValue";
            var id = MyId.FromString(myIdValue);
            id.Should().NotBeNull();
            id.Value.Should().NotBeNull();
            id.Value.Should().Be(myIdValue);
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TestIdCreationFromGuid()
        {
            var myIdValue = Guid.NewGuid();
            var id = MyId.FromGuid(myIdValue);
            id.Should().NotBeNull();
            id.Value.Should().NotBeNull();
            id.Value.Should().Be(myIdValue.ToString());
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TestIdValue()
        {
            const string myIdValue = "myIdValue";
            var id = MyId.FromString(myIdValue);
            id.Should().NotBeNull();
            id.Value.Should().NotBeNull();
            id.Value.Should().Be(myIdValue);
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TestIdEquality()
        {
            var anId = MyId.FromString("myIdValue");
            var anotherId = MyId.FromString("myIdValue");
            Assert.True(anId.Equals(anotherId));
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TestUndefinedIdEquality()
        {
            var anId = MyId.Undefined;
            var anotherId = MyId.Undefined;
            anId.Should().NotBeNull();
            anotherId.Should().NotBeNull();
            Assert.True(anId.Equals(anotherId));
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TestNewIdCreation()
        {
            var anId = MyId.New();
            var anotherId = MyId.New();
            anId.Should().NotBeNull();
            anotherId.Should().NotBeNull();
            Assert.False(anId.Equals(anotherId));
        }
    }
}
