using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TaskPomo.Models;
using TaskPomo.Services;

namespace TaskPomo.Tests.Services
{
    [TestFixture]
    public class SettingsServiceTests
    {
        private SettingsService _settingsService;
        private string _testSettingsPath;

        [SetUp]
        public void SetUp()
        {
            // テスト用の一時ディレクトリを作成
            _testSettingsPath = Path.Combine(Path.GetTempPath(), "TaskPomoTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testSettingsPath);
            
            // 環境変数を設定してテスト用パスを使用
            Environment.SetEnvironmentVariable("APPDATA", _testSettingsPath);
            
            _settingsService = new SettingsService();
        }

        [TearDown]
        public void TearDown()
        {
            // テスト用ディレクトリを削除
            if (Directory.Exists(_testSettingsPath))
            {
                Directory.Delete(_testSettingsPath, true);
            }
        }

        [Test]
        public void 初回起動時はデフォルト設定を返す()
        {
            // Arrange & Act
            var settings = _settingsService.GetSettings();

            // Assert
            settings.Should().NotBeNull();
            settings.Pomodoro.WorkDuration.Should().Be(25);
            settings.Pomodoro.ShortBreakDuration.Should().Be(5);
            settings.Pomodoro.LongBreakDuration.Should().Be(15);
            settings.Pomodoro.LongBreakInterval.Should().Be(4);
            settings.Pomodoro.UseLongBreak.Should().BeTrue();
            settings.Timer.LastDuration.Should().Be(10);
            settings.Notification.SystemSound.Should().Be("Default");
            settings.Notification.ShowWindowsNotification.Should().BeTrue();
            settings.Notification.FlashTaskbarIcon.Should().BeTrue();
        }

        [Test]
        public void 設定を保存できる()
        {
            // Arrange
            var settings = _settingsService.GetSettings();
            settings.Pomodoro.WorkDuration = 30;
            settings.Timer.LastDuration = 15;

            // Act
            _settingsService.SaveSettings(settings);

            // Assert
            var loadedSettings = _settingsService.GetSettings();
            loadedSettings.Pomodoro.WorkDuration.Should().Be(30);
            loadedSettings.Timer.LastDuration.Should().Be(15);
        }

        [Test]
        public void 設定ファイルが存在する場合は読み込む()
        {
            // Arrange
            var originalSettings = _settingsService.GetSettings();
            originalSettings.Pomodoro.WorkDuration = 45;
            _settingsService.SaveSettings(originalSettings);

            // Act
            var newService = new SettingsService();
            var loadedSettings = newService.GetSettings();

            // Assert
            loadedSettings.Pomodoro.WorkDuration.Should().Be(45);
        }

        [Test]
        public void 設定をリセットできる()
        {
            // Arrange
            var settings = _settingsService.GetSettings();
            settings.Pomodoro.WorkDuration = 60;
            _settingsService.SaveSettings(settings);

            // Act
            _settingsService.ResetSettings();

            // Assert
            var resetSettings = _settingsService.GetSettings();
            resetSettings.Pomodoro.WorkDuration.Should().Be(25);
        }

        [Test]
        public void 不正な設定ファイルの場合はデフォルト設定を使用する()
        {
            // Arrange
            var settingsDir = Path.Combine(_testSettingsPath, "TaskPomo");
            Directory.CreateDirectory(settingsDir);
            var settingsFile = Path.Combine(settingsDir, "settings.json");
            File.WriteAllText(settingsFile, "invalid json content");

            // Act
            var newService = new SettingsService();
            var settings = newService.GetSettings();

            // Assert
            settings.Should().NotBeNull();
            settings.Pomodoro.WorkDuration.Should().Be(25); // デフォルト値
        }
    }
}