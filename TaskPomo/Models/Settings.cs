namespace TaskPomo.Models
{
    /// <summary>
    /// アプリケーション設定
    /// </summary>
    public class Settings
    {
        public PomodoroSettings Pomodoro { get; set; } = new PomodoroSettings();
        public TimerSettings Timer { get; set; } = new TimerSettings();
        public NotificationSettings Notification { get; set; } = new NotificationSettings();
    }

    /// <summary>
    /// ポモドーロ設定
    /// </summary>
    public class PomodoroSettings
    {
        /// <summary>
        /// 作業時間（分）
        /// </summary>
        public int WorkDuration { get; set; } = 25;

        /// <summary>
        /// 短い休憩時間（分）
        /// </summary>
        public int ShortBreakDuration { get; set; } = 5;

        /// <summary>
        /// 長い休憩時間（分）
        /// </summary>
        public int LongBreakDuration { get; set; } = 15;

        /// <summary>
        /// 長い休憩までのサイクル数
        /// </summary>
        public int LongBreakInterval { get; set; } = 4;

        /// <summary>
        /// 長い休憩を使用するかどうか
        /// </summary>
        public bool UseLongBreak { get; set; } = true;
    }

    /// <summary>
    /// タイマー設定
    /// </summary>
    public class TimerSettings
    {
        /// <summary>
        /// 最後に設定した時間（分）
        /// </summary>
        public int LastDuration { get; set; } = 10;
    }

    /// <summary>
    /// 通知設定
    /// </summary>
    public class NotificationSettings
    {
        /// <summary>
        /// システム音
        /// </summary>
        public string SystemSound { get; set; } = "Default";

        /// <summary>
        /// Windows通知を表示するかどうか
        /// </summary>
        public bool ShowWindowsNotification { get; set; } = true;

        /// <summary>
        /// タスクバーアイコンを点滅させるかどうか
        /// </summary>
        public bool FlashTaskbarIcon { get; set; } = true;
    }
}