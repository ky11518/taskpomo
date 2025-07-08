using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using TaskPomo.Core.Timer;

namespace TaskPomo.Tests.Core.Timer
{
    [TestFixture]
    public class TimerBaseTests
    {
        private TestTimer _timer;

        [SetUp]
        public void SetUp()
        {
            _timer = new TestTimer();
        }

        [TearDown]
        public void TearDown()
        {
            _timer?.Dispose();
        }

        [Test]
        public void 新しいタイマーは停止状態である()
        {
            // Arrange & Act & Assert
            _timer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void Startを呼ぶとタイマーが開始状態になる()
        {
            // Arrange
            _timer.IsRunning.Should().BeFalse();

            // Act
            _timer.Start();

            // Assert
            _timer.IsRunning.Should().BeTrue();
        }

        [Test]
        public void Stopを呼ぶとタイマーが停止状態になる()
        {
            // Arrange
            _timer.Start();
            _timer.IsRunning.Should().BeTrue();

            // Act
            _timer.Stop();

            // Assert
            _timer.IsRunning.Should().BeFalse();
        }

        [Test]
        public void Resetを呼ぶとタイマーがリセットされる()
        {
            // Arrange
            _timer.Start();
            Thread.Sleep(100); // 少し待機
            _timer.IsRunning.Should().BeTrue();

            // Act
            _timer.Reset();

            // Assert
            _timer.IsRunning.Should().BeFalse();
            _timer.GetDisplayTime().Should().Be("00:00");
        }

        [Test]
        public void TimerTickイベントが発生する()
        {
            // Arrange
            bool eventFired = false;
            _timer.TimerTick += (sender, args) => eventFired = true;

            // Act
            _timer.Start();
            Thread.Sleep(1100); // 1秒以上待機

            // Assert
            eventFired.Should().BeTrue();
        }

        [Test]
        public void Disposeを呼ぶとリソースが解放される()
        {
            // Arrange
            _timer.Start();

            // Act
            _timer.Dispose();

            // Assert
            _timer.IsDisposed.Should().BeTrue();
        }
    }

    // テスト用のTimerBase実装
    public class TestTimer : TimerBase
    {
        private int _seconds = 0;
        public bool IsDisposed { get; private set; }

        public override void Start()
        {
            if (_isRunning || _disposed) return;
            _isRunning = true;
            _timer.Start();
        }

        public override void Stop()
        {
            if (!_isRunning || _disposed) return;
            _isRunning = false;
            _timer.Stop();
        }

        public override void Reset()
        {
            Stop();
            _seconds = 0;
        }

        public override string GetDisplayTime()
        {
            var minutes = _seconds / 60;
            var seconds = _seconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        protected override void OnTimerTick(object sender, EventArgs e)
        {
            _seconds++;
            base.OnTimerTick(sender, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                IsDisposed = true;
                base.Dispose(disposing);
            }
        }
    }
}