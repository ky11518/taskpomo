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

        [Test]
        public void 新しいタイマーは停止状態である()
        {
            // Arrange & Act
            var timer = new BasicTimer();

            // Assert
            timer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void Start実行後は動作状態になる()
        {
            // Arrange
            var timer = new BasicTimer();

            // Act
            timer.Start();

            // Assert
            timer.IsRunning.Should().BeTrue();
        }

        [Test]
        public void Stop実行後は停止状態になる()
        {
            // Arrange
            var timer = new BasicTimer();
            timer.Start();

            // Act
            timer.Stop();

            // Assert
            timer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void Reset実行後は停止状態になる()
        {
            // Arrange
            var timer = new BasicTimer();
            timer.Start();

            // Act
            timer.Reset();

            // Assert
            timer.IsRunning.Should().BeFalse();
        }
    }
}