using System;
using TaskPomo.Services;

namespace TaskPomo.Core.Timer
{
    /// <summary>
    /// ポモドーロタイマー
    /// </summary>
    public class PomodoroTimer : TimerBase
    {
        private TimeSpan _remainingTime;
        private PomodoroPhase _currentPhase;
        private int _completedWorkSessions;
        private SettingsService _settingsService;

        public PomodoroPhase CurrentPhase => _currentPhase;
        public int CompletedWorkSessions => _completedWorkSessions;

        public PomodoroTimer()
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
            _currentPhase = PomodoroPhase.Work;
            _completedWorkSessions = 0;
            SetPhaseTime();
        }

        public override string GetDisplayTime()
        {
            return $"{_remainingTime.Minutes:D2}:{_remainingTime.Seconds:D2}";
        }

        protected override void OnTimerTick(object sender, EventArgs e)
        {
            if (_remainingTime.TotalSeconds <= 0)
            {
                CompletePhase();
                return;
            }

            _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));
            base.OnTimerTick(sender, e);
        }

        private void CompletePhase()
        {
            Stop();
            OnTimerCompleted();

            switch (_currentPhase)
            {
                case PomodoroPhase.Work:
                    _completedWorkSessions++;
                    _currentPhase = ShouldTakeLongBreak() ? PomodoroPhase.LongBreak : PomodoroPhase.ShortBreak;
                    break;
                case PomodoroPhase.ShortBreak:
                case PomodoroPhase.LongBreak:
                    _currentPhase = PomodoroPhase.Work;
                    break;
            }

            SetPhaseTime();
        }

        private bool ShouldTakeLongBreak()
        {
            var settings = _settingsService.GetSettings();
            return settings.Pomodoro.UseLongBreak && 
                   _completedWorkSessions > 0 && 
                   _completedWorkSessions % settings.Pomodoro.LongBreakInterval == 0;
        }

        private void SetPhaseTime()
        {
            var settings = _settingsService.GetSettings();
            
            _remainingTime = _currentPhase switch
            {
                PomodoroPhase.Work => TimeSpan.FromMinutes(settings.Pomodoro.WorkDuration),
                PomodoroPhase.ShortBreak => TimeSpan.FromMinutes(settings.Pomodoro.ShortBreakDuration),
                PomodoroPhase.LongBreak => TimeSpan.FromMinutes(settings.Pomodoro.LongBreakDuration),
                _ => TimeSpan.FromMinutes(25)
            };
        }
    }

    /// <summary>
    /// ポモドーロフェーズ
    /// </summary>
    public enum PomodoroPhase
    {
        Work,
        ShortBreak,
        LongBreak
    }
}