using System;
using System.Media;
using System.Windows.Forms;

namespace TaskPomo.Services
{
    /// <summary>
    /// 通知サービス
    /// </summary>
    public class NotificationService : IDisposable
    {
        private SettingsService _settingsService;
        private System.Windows.Forms.Timer _flashTimer;
        private NotifyIcon _currentFlashIcon;
        private bool _flashState;
        private int _flashCount;
        private bool _disposed;

        public NotificationService()
        {
            _settingsService = new SettingsService();
            _flashTimer = new System.Windows.Forms.Timer();
            _flashTimer.Interval = 500; // 0.5秒間隔で点滅
            _flashTimer.Tick += OnFlashTimerTick;
        }

        /// <summary>
        /// Windows通知を表示する
        /// </summary>
        public void ShowNotification(string title, string message)
        {
            var settings = _settingsService.GetSettings();
            
            if (settings.Notification.ShowWindowsNotification)
            {
                // Windows 10/11のトースト通知を使用（簡易実装）
                try
                {
                    var notifyIcon = new NotifyIcon
                    {
                        Visible = true,
                        Icon = System.Drawing.SystemIcons.Information
                    };
                    
                    notifyIcon.ShowBalloonTip(3000, title, message, ToolTipIcon.Info);
                    
                    // 通知後にアイコンを削除
                    System.Windows.Forms.Timer disposeTimer = new System.Windows.Forms.Timer();
                    disposeTimer.Interval = 4000;
                    disposeTimer.Tick += (s, e) =>
                    {
                        notifyIcon.Dispose();
                        disposeTimer.Dispose();
                    };
                    disposeTimer.Start();
                }
                catch (Exception)
                {
                    // 通知に失敗した場合は何もしない
                }
            }
        }

        /// <summary>
        /// システム音を再生する
        /// </summary>
        public void PlaySystemSound()
        {
            var settings = _settingsService.GetSettings();
            
            try
            {
                switch (settings.Notification.SystemSound)
                {
                    case "Beep":
                        SystemSounds.Beep.Play();
                        break;
                    case "Exclamation":
                        SystemSounds.Exclamation.Play();
                        break;
                    case "Hand":
                        SystemSounds.Hand.Play();
                        break;
                    case "Question":
                        SystemSounds.Question.Play();
                        break;
                    case "Asterisk":
                        SystemSounds.Asterisk.Play();
                        break;
                    default:
                        SystemSounds.Beep.Play();
                        break;
                }
            }
            catch (Exception)
            {
                // 音声再生に失敗した場合は何もしない
            }
        }

        /// <summary>
        /// タスクバーアイコンを点滅させる
        /// </summary>
        public void FlashTaskbarIcon(NotifyIcon notifyIcon)
        {
            var settings = _settingsService.GetSettings();
            
            if (!settings.Notification.FlashTaskbarIcon)
            {
                return;
            }

            _currentFlashIcon = notifyIcon;
            _flashState = false;
            _flashCount = 0;
            
            // 音声も再生
            PlaySystemSound();
            
            // 点滅開始
            _flashTimer.Start();
        }

        private void OnFlashTimerTick(object sender, EventArgs e)
        {
            if (_currentFlashIcon == null)
            {
                _flashTimer.Stop();
                return;
            }

            _flashState = !_flashState;
            _flashCount++;

            // 点滅効果（アイコンの表示/非表示）
            _currentFlashIcon.Visible = _flashState;

            // 10回点滅したら停止
            if (_flashCount >= 10)
            {
                _flashTimer.Stop();
                _currentFlashIcon.Visible = true;
                _currentFlashIcon = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _flashTimer?.Stop();
                    _flashTimer?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}