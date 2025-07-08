using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using TaskPomo.Core.Timer;

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
        public void 開始すると経過時間が増加する()
        {
            // Arrange
            _stopwatchTimer.GetDisplayTime().Should().Be("00:00");

            // Act
            _stopwatchTimer.Start();
            Thread.Sleep(1100); // 1秒以上待機

            // Assert
            _stopwatchTimer.GetDisplayTime().Should().Be("00:01");
        }

        [Test]
        public void 停止すると経過時間が停止する()
        {
            // Arrange
            _stopwatchTimer.Start();
            Thread.Sleep(1100); // 1秒以上待機
            var timeBeforeStop = _stopwatchTimer.GetDisplayTime();

            // Act
            _stopwatchTimer.Stop();
            Thread.Sleep(1100); // さらに1秒待機

            // Assert
            _stopwatchTimer.GetDisplayTime().Should().Be(timeBeforeStop);
        }

        [Test]
        public void リセットすると経過時間が0になる()
        {
            // Arrange
            _stopwatchTimer.Start();
            Thread.Sleep(1100); // 1秒以上待機
            _stopwatchTimer.GetDisplayTime().Should().NotBe("00:00");

            // Act
            _stopwatchTimer.Reset();

            // Assert
            _stopwatchTimer.GetDisplayTime().Should().Be("00:00");
            _stopwatchTimer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void 再開すると前の時間から継続する()
        {
            // Arrange
            _stopwatchTimer.Start();
            Thread.Sleep(1100); // 1秒待機
            _stopwatchTimer.Stop();
            var timeAfterStop = _stopwatchTimer.GetDisplayTime();

            // Act
            _stopwatchTimer.Start();
            Thread.Sleep(1100); // さらに1秒待機

            // Assert
            _stopwatchTimer.GetDisplayTime().Should().Be("00:02");
        }
    }
}