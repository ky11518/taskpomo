using FluentAssertions;
using NUnit.Framework;

namespace TaskPomo.Tests
{
    // TimerBase抽象クラステスト用の具象実装
    public class TestableTimer : TimerBase
    {
        public int OnStartCallCount { get; private set; } = 0;
        public int OnStopCallCount { get; private set; } = 0;
        public int OnResetCallCount { get; private set; } = 0;
        public int OnTimerTickCallCount { get; private set; } = 0;

        protected override void OnStart()
        {
            OnStartCallCount++;
        }

        protected override void OnStop()
        {
            OnStopCallCount++;
        }

        protected override void OnReset()
        {
            OnResetCallCount++;
        }

        protected override void OnTimerTick(object? state)
        {
            OnTimerTickCallCount++;
        }

        // テスト用の公開メソッド
        public void TestStartTimer()
        {
            StartTimer();
        }

        public void TestOnTimerCompleted()
        {
            OnTimerCompleted();
        }
    }

    [TestFixture]
    public class TimerBaseTests
    {
        [Test]
        public void TimerBase抽象クラスから継承したタイマーを作成できる()
        {
            // Arrange & Act
            var timer = new TestableTimer();

            // Assert
            timer.Should().NotBeNull();
            timer.IsRunning.Should().BeFalse();
        }
    }
}