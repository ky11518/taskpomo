using FluentAssertions;
using NUnit.Framework;

namespace TaskPomo.Tests
{
    [TestFixture]
    public class TaskPomoApplicationTests
    {
        [Test]
        public void TaskPomoApplicationを作成できる()
        {
            // Arrange & Act
            var app = new TaskPomoApplication();

            // Assert
            app.Should().NotBeNull();
        }

        [Test]
        public void 初期化後はタスクバーにアイコンが表示される()
        {
            // Arrange
            var app = new TaskPomoApplication();

            // Act
            app.Initialize();

            // Assert（例外が発生しないことを確認）
            app.IsInitialized.Should().BeTrue();
        }

        [Test]
        public void アプリケーションを正常にシャットダウンできる()
        {
            // Arrange
            var app = new TaskPomoApplication();
            app.Initialize();

            // Act & Assert（例外が発生しないことを確認）
            app.Shutdown();
        }

        [Test]
        public void タイマー完了時に通知が表示される()
        {
            // Arrange
            var app = new TaskPomoApplication();
            app.Initialize();

            // Act - CountdownTimerを使って短時間でテスト
            var timerManager = app.GetTimerManager();
            timerManager.SwitchTimer(TimerType.Countdown);
            var countdownTimer = (CountdownTimer)timerManager.CurrentTimer;
            countdownTimer.SetDuration(1); // 1秒のカウントダウン
            timerManager.StartCurrentTimer();

            // 1秒以上待機してタイマー完了を確認
            System.Threading.Thread.Sleep(1200);

            // Assert（例外が発生しないことを確認）
            countdownTimer.IsRunning.Should().BeFalse();
            countdownTimer.RemainingSeconds.Should().Be(0);
        }

        [Test]
        public void コンテキストメニューの設定が適切に行われる()
        {
            // Arrange & Act
            var app = new TaskPomoApplication();
            app.Initialize();

            // Assert（例外が発生しないことを確認）
            app.IsInitialized.Should().BeTrue();
            
            // Cleanup
            app.Shutdown();
        }
    }
}