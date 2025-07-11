using System;

namespace TaskPomo
{
    public enum TimerType
    {
        Stopwatch,
        Countdown,
        Pomodoro
    }

    public class TimerManager
    {
        private StopwatchTimer stopwatchTimer;
        private CountdownTimer countdownTimer;
        private PomodoroTimer pomodoroTimer;

        public TimerType CurrentTimerType { get; private set; } = TimerType.Stopwatch;
        public TimerBase CurrentTimer { get; private set; }

        public TimerManager()
        {
            stopwatchTimer = new StopwatchTimer();
            countdownTimer = new CountdownTimer();
            pomodoroTimer = new PomodoroTimer();
            
            CurrentTimer = stopwatchTimer;
        }

        public void SwitchTimer(TimerType timerType)
        {
            // 現在のタイマーを停止
            CurrentTimer?.Stop();

            // 新しいタイマーに切り替え
            CurrentTimerType = timerType;
            CurrentTimer = timerType switch
            {
                TimerType.Stopwatch => stopwatchTimer,
                TimerType.Countdown => countdownTimer,
                TimerType.Pomodoro => pomodoroTimer,
                _ => stopwatchTimer
            };
        }

        public void StartCurrentTimer()
        {
            switch (CurrentTimerType)
            {
                case TimerType.Stopwatch:
                    ((StopwatchTimer)CurrentTimer).StartWithRealTimeTracking();
                    break;
                case TimerType.Countdown:
                    ((CountdownTimer)CurrentTimer).StartWithCountdown();
                    break;
                case TimerType.Pomodoro:
                    ((PomodoroTimer)CurrentTimer).StartPomodoro();
                    break;
            }
        }

        public void StopCurrentTimer()
        {
            CurrentTimer?.Stop();
        }

        public void ResetCurrentTimer()
        {
            CurrentTimer?.Reset();
        }
    }
}