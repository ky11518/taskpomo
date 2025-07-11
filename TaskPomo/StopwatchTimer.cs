using System;

namespace TaskPomo
{
    public class StopwatchTimer : TimerBase
    {
        private int elapsedSeconds = 0;

        public int ElapsedSeconds => elapsedSeconds;

        public void StartWithRealTimeTracking()
        {
            Start();
            StartTimer();
        }

        protected override void OnStart()
        {
            // StopwatchTimer開始時の処理
        }

        protected override void OnStop()
        {
            // StopwatchTimer停止時の処理
        }

        protected override void OnReset()
        {
            elapsedSeconds = 0;
        }

        protected override void OnTimerTick(object? state)
        {
            elapsedSeconds++;
        }
    }
}