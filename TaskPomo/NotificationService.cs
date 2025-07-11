using System;
using System.Windows.Forms;

namespace TaskPomo
{
    public class NotificationService
    {
        public void ShowNotification(string title, string message)
        {
            // Windowsシステム通知を使用した簡易実装
            var notifyIcon = new NotifyIcon
            {
                Icon = System.Drawing.SystemIcons.Information,
                Visible = true
            };

            notifyIcon.ShowBalloonTip(
                timeout: 3000,
                tipTitle: title,
                tipText: message,
                tipIcon: ToolTipIcon.Info
            );

            // 通知後にクリーンアップ
            System.Threading.Timer? timer = null;
            timer = new System.Threading.Timer(_ =>
            {
                notifyIcon?.Dispose();
                timer?.Dispose();
            }, null, TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
        }
    }
}