using System;

namespace TaskPomo.Core.Timer
{
    /// <summary>
    /// カウントダウンタイマー
    /// </summary>
    public class CountdownTimer : TimerBase
    {
        private int _totalSeconds;
        private int _remainingSeconds;

        public CountdownTimer()
        {
            Reset();
        }

        public void SetTime(int minutes, int seconds = 0)
        {
            _totalSeconds = minutes * 60 + seconds;
            _remainingSeconds = _totalSeconds;
        }

        public override void Start()
        {
            if (_remainingSeconds > 0)
            {
                StartTimer();
            }
        }

        public override void Stop()
        {
            StopTimer();
        }

        public override void Reset()
        {
            StopTimer();
            _remainingSeconds = _totalSeconds;
        }

        public override string GetDisplayTime()
        {
            var minutes = _remainingSeconds / 60;
            var seconds = _remainingSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        protected override void OnTimerTick()
        {
            if (_isRunning && _remainingSeconds > 0)
            {
                _remainingSeconds--;
                
                if (_remainingSeconds <= 0)
                {
                    StopTimer();
                    OnTimerCompleted();
                }
            }
            base.OnTimerTick();
        }
    }
}