using FluentAssertions;
using NUnit.Framework;

namespace TaskPomo.Tests
{
    [TestFixture]
    public class NotificationServiceTests
    {
        [Test]
        public void NotificationServiceを作成できる()
        {
            // Arrange & Act
            var notificationService = new NotificationService();

            // Assert
            notificationService.Should().NotBeNull();
        }

        [Test]
        public void 通知を表示できる()
        {
            // Arrange
            var notificationService = new NotificationService();

            // Act & Assert（例外が発生しないことを確認）
            notificationService.ShowNotification("テストタイトル", "テストメッセージ");
        }
    }
}