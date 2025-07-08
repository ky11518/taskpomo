using System;
using TaskPomo.Services;

namespace TaskPomo.Core.Timer
{
    /// <summary>
    /// カウントダウンタイマー
    /// </summary>
    public class CountdownTimer : TimerBase
    {
        private TimeSpan _remainingTime;
        private TimeSpan _initialTime;
        private SettingsService _settingsService;

        public CountdownTimer()
        {
            _settingsService = new SettingsService();
            Reset();
        }

        public override void Start()
        {
            if (_isRunning) return;

            _isRunning = true;
            _timer.Start();
        }

        public override void Stop()
        {
            if (!_isRunning) return;

            _isRunning = false;
            _timer.Stop();
        }

        public override void Reset()
        {
            Stop();
            var settings = _settingsService.GetSettings();
            _initialTime = TimeSpan.FromMinutes(settings.Timer.LastDuration);
            _remainingTime = _initialTime;
        }

        public override string GetDisplayTime()
        {
            return $"{_remainingTime.Minutes:D2}:{_remainingTime.Seconds:D2}";
        }

        public void SetTime(int minutes)
        {
            _initialTime = TimeSpan.FromMinutes(minutes);
            _remainingTime = _initialTime;
            
            // 最後の設定時間を保存
            var settings = _settingsService.GetSettings();
            settings.Timer.LastDuration = minutes;
            _settingsService.SaveSettings(settings);
        }

        protected override void OnTimerTick(object sender, EventArgs e)
        {
            if (_remainingTime.TotalSeconds <= 0)
            {
                Stop();
                OnTimerCompleted();
                return;
            }

            _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));
            base.OnTimerTick(sender, e);
        }
    }
}