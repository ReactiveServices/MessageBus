using FluentAssertions;
using NUnit.Framework;

namespace ReactiveServices.Extensions.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TesteWithLengthComTamanhoIgual()
        {
            // Arrange
            const string input = "abcd"; 
            const string expectedOutput = "abcd";

            // Action
            var output = input.WithLength(input.Length);
            
            // Assert
            output.Should().Be(expectedOutput);
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TesteWithLengthComTamanhoInferior()
        {
            // Arrange
            const string input = "abcd";
            const string expectedOutput = "abcd";

            // Action
            var output = input.WithLength(input.Length - 1);

            // Assert
            output.Should().Be(expectedOutput);
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TesteWithLengthComTamanhoSuperior()
        {
            // Arrange
            const string input = "abcd";
            const string expectedOutput = " abcd";

            // Action
            var output = input.WithLength(input.Length + 1);

            // Assert
            output.Should().Be(expectedOutput);
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TesteWithLengthComTamanhoZero()
        {
            // Arrange
            const string input = "abcd";
            const string expectedOutput = "abcd";

            // Action
            var output = input.WithLength(0);

            // Assert
            output.Should().Be(expectedOutput);
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TesteWithLengthComTamanhoNegativo()
        {
            // Arrange
            const string input = "abcd";
            const string expectedOutput = "abcd";

            // Action
            var output = input.WithLength(int.MinValue); // MinValue: -2147483648

            // Assert
            output.Should().Be(expectedOutput);
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TesteWithLengthAtLeft()
        {
            // Arrange
            const string input = "abcd";
            const string expectedOutput = " abcd";

            // Action
            var output = input.WithLength(input.Length + 1).AtLeft(); 

            // Assert
            output.Should().Be(expectedOutput);
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TesteWithLengthAtRight()
        {
            // Arrange
            const string input = "abcd";
            const string expectedOutput = "abcd ";

            // Action
            var output = input.WithLength(input.Length + 1).AtRight();

            // Assert
            output.Should().Be(expectedOutput);
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TesteWithLengthAtLeftCompletedWithAsterisco()
        {
            // Arrange
            const string input = "abcd";
            const string expectedOutput = "***abcd";

            // Action
            var output = input.WithLength(input.Length + 3).AtLeft().CompletedWith('*');

            // Assert
            output.Should().Be(expectedOutput);
        }

        [Test]
        [Category("stable")]
        [Category("fast")]
        public void TesteWithLengthAtRightCompletedWithAsterisco()
        {
            // Arrange
            const string input = "abcd";
            const string expectedOutput = "abcd***";

            // Action
            var output = input.WithLength(input.Length + 3).AtRight().CompletedWith('*');

            // Assert
            output.Should().Be(expectedOutput);
        }
    }
}
