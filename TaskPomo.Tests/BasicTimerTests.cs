using FluentAssertions;
using NUnit.Framework;

namespace TaskPomo.Tests
{
    [TestFixture]
    public class BasicTimerTests
    {
        [Test]
        public void タイマーを作成できる()
        {
            // Arrange & Act
            var timer = new BasicTimer();

            // Assert
            timer.Should().NotBeNull();
        }
    }
}