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

        [Test]
        public void ストップウォッチタイマーはBasicTimerの機能を持つ()
        {
            // Arrange & Act
            var timer = new StopwatchTimer();

            // Assert - BasicTimerの機能が使えることを確認
            timer.IsRunning.Should().BeFalse();
            timer.ElapsedSeconds.Should().Be(0);
        }

        [Test]
        public void StartWithRealTimeTracking実行後は実際の時間計測を開始する()
        {
            // Arrange
            var timer = new StopwatchTimer();
            
            // Act
            timer.StartWithRealTimeTracking();
            
            // Assert - 実際の時間計測が開始されることを確認
            timer.IsRunning.Should().BeTrue();
        }

        [Test]
        public void StartWithRealTimeTracking実行後は1秒後にElapsedSecondsが1になる()
        {
            // Arrange
            var timer = new StopwatchTimer();
            
            // Act
            timer.StartWithRealTimeTracking();
            System.Threading.Thread.Sleep(1100); // 1.1秒待機
            
            // Assert - 1秒経過後にElapsedSecondsが1になることを確認
            timer.ElapsedSeconds.Should().Be(1);
        }

        [Test]
        public void Stop実行後は実際の時間計測も停止する()
        {
            // Arrange
            var timer = new StopwatchTimer();
            timer.StartWithRealTimeTracking();
            System.Threading.Thread.Sleep(500); // 0.5秒待機
            
            // Act
            timer.Stop();
            System.Threading.Thread.Sleep(1000); // 1秒待機
            
            // Assert - Stop後は時間が進まないことを確認
            timer.ElapsedSeconds.Should().Be(0);
        }

        [Test]
        public void Reset実行後は実際の時間計測もリセットされる()
        {
            // Arrange
            var timer = new StopwatchTimer();
            timer.StartWithRealTimeTracking();
            System.Threading.Thread.Sleep(1100); // 1.1秒待機
            
            // Act
            timer.Reset();
            
            // Assert - Reset後はElapsedSecondsが0になることを確認
            timer.ElapsedSeconds.Should().Be(0);
            timer.IsRunning.Should().BeFalse();
        }
    }
}