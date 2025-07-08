using System;

namespace TaskPomo.Core.Timer
{
    /// <summary>
    /// ストップウォッチタイマー
    /// </summary>
    public class StopwatchTimer : TimerBase
    {
        private int _elapsedSeconds;

        public StopwatchTimer()
        {
            Reset();
        }

        public override void Start()
        {
            StartTimer();
        }

        public override void Stop()
        {
            StopTimer();
        }

        public override void Reset()
        {
            StopTimer();
            _elapsedSeconds = 0;
        }

        public override string GetDisplayTime()
        {
            var minutes = _elapsedSeconds / 60;
            var seconds = _elapsedSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        protected override void OnTimerTick()
        {
            if (_isRunning)
            {
                _elapsedSeconds++;
            }
            base.OnTimerTick();
        }
    }
}