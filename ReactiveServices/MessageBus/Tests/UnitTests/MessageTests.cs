using NUnit.Framework;
using FluentAssertions;

namespace ReactiveServices.MessageBus.Tests.UnitTests
{
    [TestFixture]
    public class MessageTests
    {
        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TestMessageReferenceInequality()
        {
            var aMessage = new AMessage();
            var anotherMessage = new AMessage();
            Assert.False(aMessage.Equals(anotherMessage), "Message inequality failed: {0} != {1}", aMessage, anotherMessage);
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TestMessageToString()
        {
            var message = new AMessage();
            var myIdValue = message.MessageId.Value;
            message.Should().NotBeNull();
            message.ToString().Should().Be(myIdValue);
        }
    }
}
