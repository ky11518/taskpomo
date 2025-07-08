using FluentAssertions;
using NUnit.Framework;
using TaskPomo.Core.Timer;
using System.Threading;

namespace TaskPomo.Tests.Core.Timer
{
    [TestFixture]
    public class CountdownTimerTests
    {
        private CountdownTimer _countdownTimer;

        [SetUp]
        public void SetUp()
        {
            _countdownTimer = new CountdownTimer();
        }

        [TearDown]
        public void TearDown()
        {
            _countdownTimer?.Dispose();
        }

        [Test]
        public void 新しいタイマーは停止状態である()
        {
            // Arrange & Act & Assert
            _countdownTimer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void 時間を設定できる()
        {
            // Act
            _countdownTimer.SetTime(minutes: 5);

            // Assert
            _countdownTimer.GetDisplayTime().Should().Be("05:00");
        }

        [Test]
        public void 開始すると動作状態になる()
        {
            // Arrange
            _countdownTimer.SetTime(minutes: 1);

            // Act
            _countdownTimer.Start();

            // Assert
            _countdownTimer.IsRunning.Should().BeTrue();
        }

        [Test]
        public void 停止すると停止状態になる()
        {
            // Arrange
            _countdownTimer.SetTime(minutes: 1);
            _countdownTimer.Start();

            // Act
            _countdownTimer.Stop();

            // Assert
            _countdownTimer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void カウントダウンが正しく動作する()
        {
            // Arrange
            _countdownTimer.SetTime(minutes: 0, seconds: 2);

            // Act
            _countdownTimer.Start();
            Thread.Sleep(1100); // 1秒以上待機

            // Assert
            _countdownTimer.GetDisplayTime().Should().Be("00:01");
        }

        [Test]
        public void タイマー完了時にイベントが発生する()
        {
            // Arrange
            bool completedEventFired = false;
            _countdownTimer.TimerCompleted += (sender, args) => completedEventFired = true;
            _countdownTimer.SetTime(minutes: 0, seconds: 1);

            // Act
            _countdownTimer.Start();
            Thread.Sleep(1200); // 1.2秒待機

            // Assert
            completedEventFired.Should().BeTrue();
            _countdownTimer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void リセットすると設定時間に戻る()
        {
            // Arrange
            _countdownTimer.SetTime(minutes: 3);
            _countdownTimer.Start();
            Thread.Sleep(100); // 少し時間経過

            // Act
            _countdownTimer.Reset();

            // Assert
            _countdownTimer.GetDisplayTime().Should().Be("03:00");
            _countdownTimer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void ゼロ秒で完了する()
        {
            // Arrange
            _countdownTimer.SetTime(minutes: 0, seconds: 1);
            _countdownTimer.Start();

            // Act & Assert
            Thread.Sleep(1200); // 1.2秒待機
            _countdownTimer.GetDisplayTime().Should().Be("00:00");
        }
    }
}