using FluentAssertions;
using NUnit.Framework;

namespace TaskPomo.Tests
{
    [TestFixture]
    public class TrayIconManagerTests
    {
        [Test]
        public void TrayIconManagerを作成できる()
        {
            // Arrange & Act
            var trayIconManager = new TrayIconManager();

            // Assert
            trayIconManager.Should().NotBeNull();
        }

        [Test]
        public void トレイアイコンを表示できる()
        {
            // Arrange
            var trayIconManager = new TrayIconManager();

            // Act & Assert（例外が発生しないことを確認）
            trayIconManager.Show();
        }

        [Test]
        public void トレイアイコンを非表示にできる()
        {
            // Arrange
            var trayIconManager = new TrayIconManager();

            // Act & Assert（例外が発生しないことを確認）
            trayIconManager.Hide();
        }

        [Test]
        public void Disposeできる()
        {
            // Arrange
            var trayIconManager = new TrayIconManager();

            // Act & Assert（例外が発生しないことを確認）
            trayIconManager.Dispose();
        }

        [Test]
        public void コンテキストメニューを設定できる()
        {
            // Arrange
            var trayIconManager = new TrayIconManager();

            // Act & Assert（例外が発生しないことを確認）
            trayIconManager.SetupContextMenu();
        }
    }
}