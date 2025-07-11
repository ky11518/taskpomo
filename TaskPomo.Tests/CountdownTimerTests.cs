using FluentAssertions;
using NUnit.Framework;

namespace TaskPomo.Tests
{
    [TestFixture]
    public class CountdownTimerTests
    {
        [Test]
        public void カウントダウンタイマーを作成できる()
        {
            // Arrange & Act
            var timer = new CountdownTimer();

            // Assert
            timer.Should().NotBeNull();
        }

        [Test]
        public void カウントダウンタイマーはBasicTimerの機能を持つ()
        {
            // Arrange & Act
            var timer = new CountdownTimer();

            // Assert - BasicTimerの機能が使えることを確認
            timer.IsRunning.Should().BeFalse();
            timer.ElapsedSeconds.Should().Be(0);
        }

        [Test]
        public void SetDurationメソッドで残り時間を設定できる()
        {
            // Arrange
            var timer = new CountdownTimer();
            var durationSeconds = 60;

            // Act
            timer.SetDuration(durationSeconds);

            // Assert
            timer.RemainingSeconds.Should().Be(durationSeconds);
        }

        [Test]
        public void StartWithCountdownメソッドで実際のカウントダウンを開始できる()
        {
            // Arrange
            var timer = new CountdownTimer();
            timer.SetDuration(60);

            // Act
            timer.StartWithCountdown();

            // Assert
            timer.IsRunning.Should().BeTrue();
        }

        [Test]
        public void StartWithCountdown実行後は1秒後にRemainingSecondsが1減る()
        {
            // Arrange
            var timer = new CountdownTimer();
            timer.SetDuration(60);

            // Act
            timer.StartWithCountdown();
            System.Threading.Thread.Sleep(1100); // 1.1秒待機

            // Assert
            timer.RemainingSeconds.Should().Be(59);
        }

        [Test]
        public void カウントダウンが0になるとTimerCompletedイベントが発生する()
        {
            // Arrange
            var timer = new CountdownTimer();
            timer.SetDuration(1); // 1秒で完了
            bool eventFired = false;
            timer.TimerCompleted += (sender, e) => eventFired = true;

            // Act
            timer.StartWithCountdown();
            System.Threading.Thread.Sleep(1200); // 1.2秒待機

            // Assert
            eventFired.Should().BeTrue();
            timer.RemainingSeconds.Should().Be(0);
        }

        [Test]
        public void SetDurationで設定した時間がLastDurationに記憶される()
        {
            // Arrange
            var timer = new CountdownTimer();
            var duration = 120;

            // Act
            timer.SetDuration(duration);

            // Assert
            timer.LastDuration.Should().Be(duration);
        }

        [Test]
        public void Stop実行後は実際のカウントダウンも停止する()
        {
            // Arrange
            var timer = new CountdownTimer();
            timer.SetDuration(60);
            timer.StartWithCountdown();
            System.Threading.Thread.Sleep(100); // 少し待機

            // Act
            timer.Stop();
            var remainingBeforeWait = timer.RemainingSeconds;
            System.Threading.Thread.Sleep(1200); // 1.2秒待機

            // Assert
            timer.IsRunning.Should().BeFalse();
            timer.RemainingSeconds.Should().Be(remainingBeforeWait); // 変化しない
        }

        [Test]
        public void Reset実行後は実際のカウントダウンもリセットされる()
        {
            // Arrange
            var timer = new CountdownTimer();
            timer.SetDuration(60);
            timer.StartWithCountdown();
            System.Threading.Thread.Sleep(100); // 少し待機

            // Act
            timer.Reset();

            // Assert
            timer.IsRunning.Should().BeFalse();
            timer.RemainingSeconds.Should().Be(timer.LastDuration); // LastDurationに戻る
        }
    }
}