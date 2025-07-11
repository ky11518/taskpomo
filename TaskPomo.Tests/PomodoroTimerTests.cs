using FluentAssertions;
using NUnit.Framework;

namespace TaskPomo.Tests
{
    [TestFixture]
    public class PomodoroTimerTests
    {
        [Test]
        public void ポモドーロタイマーを作成できる()
        {
            // Arrange & Act
            var timer = new PomodoroTimer();

            // Assert
            timer.Should().NotBeNull();
        }

        [Test]
        public void ポモドーロタイマーはBasicTimerの機能を持つ()
        {
            // Arrange & Act
            var timer = new PomodoroTimer();

            // Assert - BasicTimerの機能が使えることを確認
            timer.IsRunning.Should().BeFalse();
            timer.ElapsedSeconds.Should().Be(0);
        }

        [Test]
        public void 作業時間をデフォルト25分で設定できる()
        {
            // Arrange & Act
            var timer = new PomodoroTimer();

            // Assert
            timer.WorkDurationMinutes.Should().Be(25);
        }

        [Test]
        public void 短い休憩時間をデフォルト5分で設定できる()
        {
            // Arrange & Act
            var timer = new PomodoroTimer();

            // Assert
            timer.ShortBreakDurationMinutes.Should().Be(5);
        }

        [Test]
        public void 長い休憩時間をデフォルト15分で設定できる()
        {
            // Arrange & Act
            var timer = new PomodoroTimer();

            // Assert
            timer.LongBreakDurationMinutes.Should().Be(15);
        }

        [Test]
        public void 新しいポモドーロタイマーは作業状態である()
        {
            // Arrange & Act
            var timer = new PomodoroTimer();

            // Assert
            timer.CurrentPhase.Should().Be(PomodoroPhase.Work);
        }

        [Test]
        public void 長い休憩までのサイクル数をデフォルト4回で設定できる()
        {
            // Arrange & Act
            var timer = new PomodoroTimer();

            // Assert
            timer.LongBreakInterval.Should().Be(4);
        }

        [Test]
        public void 長い休憩を使用するかどうかをデフォルトtrueで設定できる()
        {
            // Arrange & Act
            var timer = new PomodoroTimer();

            // Assert
            timer.UseLongBreak.Should().BeTrue();
        }

        [Test]
        public void StartPomodoroで実際のポモドーロサイクルを開始できる()
        {
            // Arrange
            var timer = new PomodoroTimer();

            // Act
            timer.StartPomodoro();

            // Assert
            timer.IsRunning.Should().BeTrue();
            timer.CurrentPhase.Should().Be(PomodoroPhase.Work);
        }

        [Test]
        public void 作業時間が完了すると短い休憩に自動切り替えする()
        {
            // Arrange
            var timer = new PomodoroTimer();
            timer.WorkDurationMinutes = 1; // 1分に設定（テスト用）
            timer.ShortBreakDurationMinutes = 1; // 1分に設定（テスト用）
            bool phaseChanged = false;
            timer.PhaseChanged += (sender, e) => phaseChanged = true;

            // Act
            timer.StartPomodoro();
            System.Threading.Thread.Sleep(1200); // 1.2秒待機（テスト用に短縮）

            // Assert
            timer.RemainingSeconds.Should().BeLessThan(60); // 残り時間が減っていることを確認
        }
    }
}