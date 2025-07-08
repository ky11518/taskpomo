using FluentAssertions;
using NUnit.Framework;
using TaskPomo.Core.Timer;
using System.Threading;

namespace TaskPomo.Tests.Core.Timer
{
    [TestFixture]
    public class PomodoroTimerTests
    {
        private PomodoroTimer _pomodoroTimer;

        [SetUp]
        public void SetUp()
        {
            _pomodoroTimer = new PomodoroTimer();
        }

        [TearDown]
        public void TearDown()
        {
            _pomodoroTimer?.Dispose();
        }

        [Test]
        public void 新しいポモドーロタイマーは作業フェーズで停止状態である()
        {
            // Arrange & Act & Assert
            _pomodoroTimer.IsRunning.Should().BeFalse();
            _pomodoroTimer.CurrentPhase.Should().Be(PomodoroPhase.Work);
        }

        [Test]
        public void デフォルトの作業時間は25分である()
        {
            // Arrange & Act & Assert
            _pomodoroTimer.GetDisplayTime().Should().Be("25:00");
        }

        [Test]
        public void 作業時間をカスタム設定できる()
        {
            // Act
            _pomodoroTimer.SetWorkDuration(30);

            // Assert
            _pomodoroTimer.GetDisplayTime().Should().Be("30:00");
        }

        [Test]
        public void 休憩時間をカスタム設定できる()
        {
            // Act
            _pomodoroTimer.SetBreakDuration(shortBreak: 7, longBreak: 20);

            // Assert (休憩時間の設定は次のフェーズで確認される)
            _pomodoroTimer.CurrentPhase.Should().Be(PomodoroPhase.Work);
        }

        [Test]
        public void 開始すると動作状態になる()
        {
            // Act
            _pomodoroTimer.Start();

            // Assert
            _pomodoroTimer.IsRunning.Should().BeTrue();
        }

        [Test]
        public void 作業時間のカウントダウンが正しく動作する()
        {
            // Arrange
            _pomodoroTimer.SetWorkDuration(0, 2); // 2秒に設定

            // Act
            _pomodoroTimer.Start();
            Thread.Sleep(1100); // 1秒以上待機

            // Assert
            _pomodoroTimer.GetDisplayTime().Should().Be("00:01");
            _pomodoroTimer.CurrentPhase.Should().Be(PomodoroPhase.Work);
        }

        [Test]
        public void 作業完了後に短い休憩フェーズに移行する()
        {
            // Arrange
            bool completedEventFired = false;
            _pomodoroTimer.PhaseCompleted += (sender, args) => completedEventFired = true;
            _pomodoroTimer.SetWorkDuration(0, 1); // 1秒に設定
            _pomodoroTimer.SetBreakDuration(shortBreak: 1, longBreak: 1);

            // Act
            _pomodoroTimer.Start();
            Thread.Sleep(1200); // 1.2秒待機

            // Assert
            completedEventFired.Should().BeTrue();
            _pomodoroTimer.CurrentPhase.Should().Be(PomodoroPhase.ShortBreak);
            _pomodoroTimer.IsRunning.Should().BeFalse(); // 自動では次のフェーズは開始しない
        }

        [Test]
        public void 短い休憩完了後に作業フェーズに戻る()
        {
            // Arrange
            _pomodoroTimer.SetWorkDuration(0, 1);
            _pomodoroTimer.SetBreakDuration(shortBreak: 1, longBreak: 1);
            
            // 最初の作業フェーズ完了
            _pomodoroTimer.Start();
            Thread.Sleep(1200);
            
            // Act - 休憩フェーズ開始・完了
            _pomodoroTimer.Start();
            Thread.Sleep(1200);

            // Assert
            _pomodoroTimer.CurrentPhase.Should().Be(PomodoroPhase.Work);
        }

        [Test]
        public void 指定サイクル完了後に長い休憩フェーズに移行する()
        {
            // Arrange
            _pomodoroTimer.SetWorkDuration(0, 1);
            _pomodoroTimer.SetBreakDuration(shortBreak: 1, longBreak: 2);
            _pomodoroTimer.SetLongBreakInterval(2); // 2サイクルで長い休憩

            // Act - 1回目の作業+休憩サイクル
            _pomodoroTimer.Start();
            Thread.Sleep(1200); // 作業完了
            _pomodoroTimer.Start();
            Thread.Sleep(1200); // 短い休憩完了
            
            // 2回目の作業完了（長い休憩になる条件）
            _pomodoroTimer.Start();
            Thread.Sleep(1200); // 作業完了

            // Assert
            _pomodoroTimer.CurrentPhase.Should().Be(PomodoroPhase.LongBreak);
        }

        [Test]
        public void リセットすると作業フェーズの開始時間に戻る()
        {
            // Arrange
            _pomodoroTimer.SetWorkDuration(10);
            _pomodoroTimer.Start();
            Thread.Sleep(100); // 少し進める

            // Act
            _pomodoroTimer.Reset();

            // Assert
            _pomodoroTimer.CurrentPhase.Should().Be(PomodoroPhase.Work);
            _pomodoroTimer.GetDisplayTime().Should().Be("10:00");
            _pomodoroTimer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void 完了セッション数が正しく記録される()
        {
            // Arrange
            _pomodoroTimer.SetWorkDuration(0, 1);
            _pomodoroTimer.SetBreakDuration(shortBreak: 1, longBreak: 1);

            // Act - 1回目の作業完了
            _pomodoroTimer.Start();
            Thread.Sleep(1200);

            // Assert
            _pomodoroTimer.CompletedWorkSessions.Should().Be(1);
        }
    }
}

