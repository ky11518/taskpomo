using FluentAssertions;
using NUnit.Framework;

namespace TaskPomo.Tests
{
    [TestFixture]
    public class SimpleTimerTests
    {
        private SimpleTimer _timer;

        [SetUp]
        public void SetUp()
        {
            _timer = new SimpleTimer();
        }

        [Test]
        public void 新しいタイマーは停止状態である()
        {
            // Arrange & Act & Assert
            _timer.GetStatus().Should().Be("停止中");
        }

        [Test]
        public void Start呼び出し後は動作中になる()
        {
            // Arrange
            _timer.GetStatus().Should().Be("停止中");

            // Act
            _timer.Start();

            // Assert
            _timer.GetStatus().Should().Be("動作中");
        }

        [Test]
        public void Stop呼び出し後は停止中になる()
        {
            // Arrange
            _timer.Start();
            _timer.GetStatus().Should().Be("動作中");

            // Act
            _timer.Stop();

            // Assert
            _timer.GetStatus().Should().Be("停止中");
        }
    }
}