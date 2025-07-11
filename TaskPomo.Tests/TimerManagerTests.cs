using FluentAssertions;
using NUnit.Framework;

namespace TaskPomo.Tests
{
    [TestFixture]
    public class TimerManagerTests
    {
        [Test]
        public void TimerManagerを作成できる()
        {
            // Arrange & Act
            var timerManager = new TimerManager();

            // Assert
            timerManager.Should().NotBeNull();
        }

        [Test]
        public void 初期状態はStopwatchTimerモードである()
        {
            // Arrange & Act
            var timerManager = new TimerManager();

            // Assert
            timerManager.CurrentTimerType.Should().Be(TimerType.Stopwatch);
        }

        [Test]
        public void 現在のタイマーを取得できる()
        {
            // Arrange & Act
            var timerManager = new TimerManager();

            // Assert
            timerManager.CurrentTimer.Should().NotBeNull();
            timerManager.CurrentTimer.Should().BeOfType<StopwatchTimer>();
        }

        [Test]
        public void タイマーを切り替えできる()
        {
            // Arrange
            var timerManager = new TimerManager();

            // Act
            timerManager.SwitchTimer(TimerType.Pomodoro);

            // Assert
            timerManager.CurrentTimerType.Should().Be(TimerType.Pomodoro);
            timerManager.CurrentTimer.Should().BeOfType<PomodoroTimer>();
        }

        [Test]
        public void 現在のタイマーを開始できる()
        {
            // Arrange
            var timerManager = new TimerManager();

            // Act
            timerManager.StartCurrentTimer();

            // Assert
            timerManager.CurrentTimer.IsRunning.Should().BeTrue();
        }

        [Test]
        public void Countdownタイマーに切り替えて開始できる()
        {
            // Arrange
            var timerManager = new TimerManager();
            timerManager.SwitchTimer(TimerType.Countdown);
            var countdownTimer = (CountdownTimer)timerManager.CurrentTimer;
            countdownTimer.SetDuration(60);

            // Act
            timerManager.StartCurrentTimer();

            // Assert
            timerManager.CurrentTimer.IsRunning.Should().BeTrue();
            countdownTimer.RemainingSeconds.Should().Be(60);
        }

        [Test]
        public void Pomodoroタイマーに切り替えて開始できる()
        {
            // Arrange
            var timerManager = new TimerManager();
            timerManager.SwitchTimer(TimerType.Pomodoro);

            // Act
            timerManager.StartCurrentTimer();

            // Assert
            timerManager.CurrentTimer.IsRunning.Should().BeTrue();
            var pomodoroTimer = (PomodoroTimer)timerManager.CurrentTimer;
            pomodoroTimer.CurrentPhase.Should().Be(PomodoroPhase.Work);
        }

        [Test]
        public void 現在のタイマーを停止できる()
        {
            // Arrange
            var timerManager = new TimerManager();
            timerManager.StartCurrentTimer();

            // Act
            timerManager.StopCurrentTimer();

            // Assert
            timerManager.CurrentTimer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void 現在のタイマーをリセットできる()
        {
            // Arrange
            var timerManager = new TimerManager();
            timerManager.StartCurrentTimer();
            System.Threading.Thread.Sleep(100); // わずかに時間を進める

            // Act
            timerManager.ResetCurrentTimer();

            // Assert
            timerManager.CurrentTimer.IsRunning.Should().BeFalse();
            timerManager.CurrentTimer.ElapsedSeconds.Should().Be(0);
        }
    }
}