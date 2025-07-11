namespace TaskPomo
{
    public enum PomodoroPhase
    {
        Work,
        ShortBreak,
        LongBreak
    }

    public class PomodoroTimer : TimerBase
    {
        private int remainingSeconds = 0;
        private int completedWorkCycles = 0;

        public int WorkDurationMinutes { get; set; } = 25;
        public int ShortBreakDurationMinutes { get; set; } = 5;
        public int LongBreakDurationMinutes { get; set; } = 15;
        public int LongBreakInterval { get; set; } = 4;
        public bool UseLongBreak { get; set; } = true;
        public PomodoroPhase CurrentPhase { get; private set; } = PomodoroPhase.Work;
        public int RemainingSeconds => remainingSeconds;

        public event EventHandler<EventArgs>? PhaseChanged;

        public void StartPomodoro()
        {
            Start();
            SetCurrentPhaseDuration();
            StartTimer();
        }

        protected override void OnStart()
        {
            // PomodoroTimer開始時の処理
        }

        protected override void OnStop()
        {
            // PomodoroTimer停止時の処理
        }

        protected override void OnReset()
        {
            completedWorkCycles = 0;
            CurrentPhase = PomodoroPhase.Work;
            SetCurrentPhaseDuration();
        }

        protected override void OnTimerTick(object? state)
        {
            remainingSeconds--;
            
            if (remainingSeconds <= 0)
            {
                SwitchToNextPhase();
            }
        }

        private void SetCurrentPhaseDuration()
        {
            remainingSeconds = CurrentPhase switch
            {
                PomodoroPhase.Work => WorkDurationMinutes * 60,
                PomodoroPhase.ShortBreak => ShortBreakDurationMinutes * 60,
                PomodoroPhase.LongBreak => LongBreakDurationMinutes * 60,
                _ => WorkDurationMinutes * 60
            };
        }

        private void SwitchToNextPhase()
        {
            if (CurrentPhase == PomodoroPhase.Work)
            {
                completedWorkCycles++;
                CurrentPhase = ShouldUseLongBreak() ? PomodoroPhase.LongBreak : PomodoroPhase.ShortBreak;
            }
            else
            {
                CurrentPhase = PomodoroPhase.Work;
            }

            SetCurrentPhaseDuration();
            PhaseChanged?.Invoke(this, EventArgs.Empty);
        }

        private bool ShouldUseLongBreak()
        {
            return UseLongBreak && completedWorkCycles % LongBreakInterval == 0;
        }
    }
}