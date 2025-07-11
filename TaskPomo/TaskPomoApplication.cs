using System;

namespace TaskPomo
{
    public class TaskPomoApplication : IDisposable
    {
        private TimerManager? timerManager;
        private TrayIconManager? trayIconManager;
        private NotificationService? notificationService;
        private bool disposed = false;

        public bool IsInitialized { get; private set; } = false;

        public TimerManager GetTimerManager()
        {
            if (timerManager == null)
                throw new InvalidOperationException("アプリケーションが初期化されていません。");
            return timerManager;
        }

        public void Initialize()
        {
            // コンポーネント初期化
            timerManager = new TimerManager();
            trayIconManager = new TrayIconManager();
            notificationService = new NotificationService();

            // トレイアイコンのコンテキストメニュー設定
            trayIconManager.SetupContextMenu(timerManager, () => Shutdown());

            // タイマー完了時の通知イベント設定
            SetupTimerCompletedEvents();

            IsInitialized = true;
        }

        private void SetupTimerCompletedEvents()
        {
            if (timerManager != null && notificationService != null)
            {
                timerManager.CurrentTimer.TimerCompleted += OnTimerCompleted;
            }
        }

        private void OnTimerCompleted(object? sender, EventArgs e)
        {
            if (notificationService != null && timerManager != null)
            {
                var timerType = timerManager.CurrentTimerType;
                var message = timerType switch
                {
                    TimerType.Pomodoro => "ポモドーロタイマーが完了しました！",
                    TimerType.Countdown => "カウントダウンタイマーが完了しました！",
                    TimerType.Stopwatch => "ストップウォッチを停止しました。",
                    _ => "タイマーが完了しました！"
                };

                notificationService.ShowNotification("TaskPomo", message);
            }
        }

        public void Shutdown()
        {
            Dispose();
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
                trayIconManager?.Dispose();
                IsInitialized = false;
                disposed = true;
            }
        }
    }
}