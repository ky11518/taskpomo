using System;

namespace TaskPomo.Core.Timer
{
    /// <summary>
    /// タイマーの基底クラス
    /// </summary>
    public abstract class TimerBase : IDisposable
    {
        private System.Timers.Timer _timer;
        protected bool _isRunning;
        protected bool _disposed;

        public event EventHandler<TimerEventArgs>? TimerTick;
        public event EventHandler<TimerEventArgs>? TimerCompleted;

        public bool IsRunning => _isRunning;

        protected TimerBase()
        {
            _timer = new System.Timers.Timer(1000); // 1秒間隔
            _timer.Elapsed += OnTimerElapsed;
        }

        public abstract void Start();
        public abstract void Stop();
        public abstract void Reset();
        public abstract string GetDisplayTime();

        private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            OnTimerTick();
        }

        protected virtual void OnTimerTick()
        {
            var args = new TimerEventArgs(GetDisplayTime());
            TimerTick?.Invoke(this, args);
        }

        protected virtual void OnTimerCompleted()
        {
            var args = new TimerEventArgs(GetDisplayTime());
            TimerCompleted?.Invoke(this, args);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _timer?.Stop();
                    _timer?.Dispose();
                }
                _disposed = true;
            }
        }

        protected void StartTimer()
        {
            if (!_disposed && !_isRunning)
            {
                _isRunning = true;
                _timer.Start();
            }
        }

        protected void StopTimer()
        {
            if (!_disposed && _isRunning)
            {
                _isRunning = false;
                _timer.Stop();
            }
        }
    }

    /// <summary>
    /// タイマーイベント引数
    /// </summary>
    public class TimerEventArgs : EventArgs
    {
        public string DisplayTime { get; }

        public TimerEventArgs(string displayTime)
        {
            DisplayTime = displayTime;
        }
    }
}