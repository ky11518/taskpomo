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

        [Test]
        public void 新しいタイマーの経過時間は0秒である()
        {
            // Arrange & Act
            var timer = new BasicTimer();

            // Assert
            timer.ElapsedSeconds.Should().Be(0);
        }

        [Test]
        public void Reset実行後は経過時間が0秒にリセットされる()
        {
            // Arrange
            var timer = new BasicTimer();
            timer.Start();

            // Act
            timer.Reset();

            // Assert
            timer.ElapsedSeconds.Should().Be(0);
        }

        [Test]
        public void TimerTickイベントが存在する()
        {
            // Arrange
            var timer = new BasicTimer();
            var eventFired = false;

            // Act
            timer.TimerTick += (sender, e) => eventFired = true;

            // Assert
            timer.TimerTick.Should().NotBeNull();
        }
    }
}