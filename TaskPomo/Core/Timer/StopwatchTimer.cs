using System;

namespace TaskPomo.Core.Timer
{
    /// <summary>
    /// ストップウォッチタイマー
    /// </summary>
    public class StopwatchTimer : TimerBase
    {
        private TimeSpan _elapsedTime;

        public StopwatchTimer()
        {
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
            _elapsedTime = TimeSpan.Zero;
        }

        public override string GetDisplayTime()
        {
            return $"{_elapsedTime.Minutes:D2}:{_elapsedTime.Seconds:D2}";
        }

        protected override void OnTimerTick(object sender, EventArgs e)
        {
            _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
            base.OnTimerTick(sender, e);
        }
    }
}