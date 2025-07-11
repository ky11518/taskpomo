using System;
using System.Windows.Forms;

namespace TaskPomo
{
    public class TrayIconManager : IDisposable
    {
        private NotifyIcon? notifyIcon;
        private bool disposed = false;

        public TrayIconManager()
        {
            InitializeTrayIcon();
        }

        private void InitializeTrayIcon()
        {
            notifyIcon = new NotifyIcon
            {
                Text = "TaskPomo",
                Visible = true
            };

            // 簡易アイコン（後で適切なアイコンに置き換え）
            notifyIcon.Icon = System.Drawing.SystemIcons.Application;
        }

        public void Show()
        {
            if (notifyIcon != null)
            {
                notifyIcon.Visible = true;
            }
        }

        public void Hide()
        {
            if (notifyIcon != null)
            {
                notifyIcon.Visible = false;
            }
        }

        public void SetupContextMenu(TimerManager? timerManager = null, Action? exitAction = null)
        {
            if (notifyIcon != null)
            {
                var contextMenuStrip = new ContextMenuStrip();
                
                // タイマー開始メニュー
                var startMenuItem = new ToolStripMenuItem("開始");
                if (timerManager != null)
                {
                    startMenuItem.Click += (sender, e) => timerManager.StartCurrentTimer();
                }
                contextMenuStrip.Items.Add(startMenuItem);
                
                // タイマー停止メニュー
                var stopMenuItem = new ToolStripMenuItem("停止");
                if (timerManager != null)
                {
                    stopMenuItem.Click += (sender, e) => timerManager.StopCurrentTimer();
                }
                contextMenuStrip.Items.Add(stopMenuItem);
                
                // タイマーリセットメニュー
                var resetMenuItem = new ToolStripMenuItem("リセット");
                if (timerManager != null)
                {
                    resetMenuItem.Click += (sender, e) => timerManager.ResetCurrentTimer();
                }
                contextMenuStrip.Items.Add(resetMenuItem);
                
                // セパレーター
                contextMenuStrip.Items.Add(new ToolStripSeparator());
                
                // 終了メニュー
                var exitMenuItem = new ToolStripMenuItem("終了");
                if (exitAction != null)
                {
                    exitMenuItem.Click += (sender, e) => exitAction();
                }
                contextMenuStrip.Items.Add(exitMenuItem);
                
                notifyIcon.ContextMenuStrip = contextMenuStrip;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                notifyIcon?.Dispose();
                disposed = true;
            }
        }
    }
}