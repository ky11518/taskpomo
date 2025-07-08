using System;
using System.Windows.Threading;

namespace TaskPomo.Core.Timer
{
    /// <summary>
    /// タイマーの基底クラス
    /// </summary>
    public abstract class TimerBase : IDisposable
    {
        protected DispatcherTimer _timer;
        protected bool _isRunning;
        protected bool _disposed;

        public event EventHandler<TimerEventArgs> TimerTick;
        public event EventHandler<TimerEventArgs> TimerCompleted;

        public bool IsRunning => _isRunning;

        protected TimerBase()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += OnTimerTick;
        }

        public abstract void Start();
        public abstract void Stop();
        public abstract void Reset();
        public abstract string GetDisplayTime();

        protected virtual void OnTimerTick(object sender, EventArgs e)
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