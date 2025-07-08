using FluentAssertions;
using NUnit.Framework;
using TaskPomo.Core.Timer;
using System.Threading;

namespace TaskPomo.Tests.Core.Timer
{
    [TestFixture]
    public class StopwatchTimerTests
    {
        private StopwatchTimer _stopwatchTimer;

        [SetUp]
        public void SetUp()
        {
            _stopwatchTimer = new StopwatchTimer();
        }

        [TearDown]
        public void TearDown()
        {
            _stopwatchTimer?.Dispose();
        }

        [Test]
        public void 新しいストップウォッチは0秒を表示する()
        {
            // Arrange & Act & Assert
            _stopwatchTimer.GetDisplayTime().Should().Be("00:00");
        }

        [Test]
        public void 新しいストップウォッチは停止状態である()
        {
            // Arrange & Act & Assert
            _stopwatchTimer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void 開始すると動作状態になる()
        {
            // Act
            _stopwatchTimer.Start();

            // Assert
            _stopwatchTimer.IsRunning.Should().BeTrue();
        }

        [Test]
        public void 停止すると停止状態になる()
        {
            // Arrange
            _stopwatchTimer.Start();
            _stopwatchTimer.IsRunning.Should().BeTrue();

            // Act
            _stopwatchTimer.Stop();

            // Assert
            _stopwatchTimer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void リセットすると停止状態で0秒になる()
        {
            // Arrange
            _stopwatchTimer.Start();
            Thread.Sleep(100); // 少し時間を経過させる

            // Act
            _stopwatchTimer.Reset();

            // Assert
            _stopwatchTimer.IsRunning.Should().BeFalse();
            _stopwatchTimer.GetDisplayTime().Should().Be("00:00");
        }

        [Test]
        public void TimerTickイベントが発生する()
        {
            // Arrange
            bool eventFired = false;
            string lastDisplayTime = "";
            _stopwatchTimer.TimerTick += (sender, args) => 
            {
                eventFired = true;
                lastDisplayTime = args.DisplayTime;
            };

            // Act
            _stopwatchTimer.Start();
            Thread.Sleep(1100); // 1秒以上待機

            // Assert
            eventFired.Should().BeTrue();
            lastDisplayTime.Should().NotBeEmpty();
        }

        [Test]
        public void 経過時間が正しく表示される()
        {
            // Act
            _stopwatchTimer.Start();
            Thread.Sleep(1100); // 1秒以上待機

            // Assert
            _stopwatchTimer.GetDisplayTime().Should().Be("00:01");
        }
    }
}