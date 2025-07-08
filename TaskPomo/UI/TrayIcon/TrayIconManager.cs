using System;
using System.Drawing;
using System.Windows.Forms;
using TaskPomo.Core.Timer;
using TaskPomo.Models;
using TaskPomo.Services;

namespace TaskPomo.UI.TrayIcon
{
    /// <summary>
    /// タスクバーアイコン管理クラス
    /// </summary>
    public class TrayIconManager : IDisposable
    {
        private NotifyIcon _notifyIcon;
        private TimerManager _timerManager;
        private NotificationService _notificationService;
        private ContextMenuStrip _contextMenu;
        private bool _disposed;

        public void Initialize()
        {
            _timerManager = new TimerManager();
            _notificationService = new NotificationService();
            
            InitializeNotifyIcon();
            InitializeContextMenu();
            SubscribeToTimerEvents();
        }

        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon()
            {
                Icon = GetDefaultIcon(),
                Text = "TaskPomo",
                Visible = true
            };

            _notifyIcon.MouseClick += OnNotifyIconClick;
        }

        private void InitializeContextMenu()
        {
            _contextMenu = new ContextMenuStrip();
            
            // モード選択メニュー
            var pomodoroItem = new ToolStripMenuItem("ポモドーロ", null, (s, e) => _timerManager.SwitchToMode(TimerMode.Pomodoro));
            var timerItem = new ToolStripMenuItem("タイマー", null, (s, e) => _timerManager.SwitchToMode(TimerMode.Timer));
            var stopwatchItem = new ToolStripMenuItem("ストップウォッチ", null, (s, e) => _timerManager.SwitchToMode(TimerMode.Stopwatch));
            
            _contextMenu.Items.Add(pomodoroItem);
            _contextMenu.Items.Add(timerItem);
            _contextMenu.Items.Add(stopwatchItem);
            _contextMenu.Items.Add(new ToolStripSeparator());
            
            // 設定メニュー
            var settingsItem = new ToolStripMenuItem("設定", null, OnSettingsClick);
            _contextMenu.Items.Add(settingsItem);
            _contextMenu.Items.Add(new ToolStripSeparator());
            
            // 終了メニュー
            var exitItem = new ToolStripMenuItem("終了", null, OnExitClick);
            _contextMenu.Items.Add(exitItem);
            
            _notifyIcon.ContextMenuStrip = _contextMenu;
        }

        private void SubscribeToTimerEvents()
        {
            _timerManager.TimerTick += OnTimerTick;
            _timerManager.TimerCompleted += OnTimerCompleted;
            _timerManager.TimerModeChanged += OnTimerModeChanged;
        }

        private void OnNotifyIconClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _timerManager.StartStop();
            }
        }

        private void OnTimerTick(object sender, TimerEventArgs e)
        {
            UpdateToolTip(e.DisplayTime);
        }

        private void OnTimerCompleted(object sender, TimerEventArgs e)
        {
            _notificationService.ShowNotification("TaskPomo", "タイマーが完了しました");
            _notificationService.FlashTaskbarIcon(_notifyIcon);
        }

        private void OnTimerModeChanged(object sender, TimerModeChangedEventArgs e)
        {
            UpdateToolTip($"{GetModeDisplayName(e.NewMode)} - {_timerManager.GetDisplayTime()}");
        }

        private void OnSettingsClick(object sender, EventArgs e)
        {
            // 設定画面を表示
            // TODO: 設定画面の実装
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void UpdateToolTip(string time)
        {
            var mode = GetModeDisplayName(_timerManager.CurrentMode);
            var status = _timerManager.IsRunning ? "実行中" : "停止中";
            _notifyIcon.Text = $"TaskPomo - {mode} ({status}) - {time}";
        }

        private string GetModeDisplayName(TimerMode mode)
        {
            return mode switch
            {
                TimerMode.Pomodoro => "ポモドーロ",
                TimerMode.Timer => "タイマー",
                TimerMode.Stopwatch => "ストップウォッチ",
                _ => "不明"
            };
        }

        private Icon GetDefaultIcon()
        {
            // 簡単なアイコンを作成（実際の実装では適切なアイコンファイルを使用）
            var bitmap = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.FillEllipse(Brushes.Red, 2, 2, 12, 12);
            }
            return Icon.FromHandle(bitmap.GetHicon());
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
                    _timerManager?.Dispose();
                    _notificationService?.Dispose();
                    _contextMenu?.Dispose();
                    _notifyIcon?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}