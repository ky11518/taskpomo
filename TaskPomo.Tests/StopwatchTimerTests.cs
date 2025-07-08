using FluentAssertions;
using NUnit.Framework;

namespace TaskPomo.Tests
{
    [TestFixture]
    public class StopwatchTimerTests
    {
        [Test]
        public void ストップウォッチタイマーを作成できる()
        {
            // Arrange & Act
            var timer = new StopwatchTimer();

            // Assert
            timer.Should().NotBeNull();
        }
    }
}