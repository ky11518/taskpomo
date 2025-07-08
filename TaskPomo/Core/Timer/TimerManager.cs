using System;
using TaskPomo.Models;

namespace TaskPomo.Core.Timer
{
    /// <summary>
    /// タイマー管理クラス
    /// </summary>
    public class TimerManager : IDisposable
    {
        private TimerBase _currentTimer;
        private TimerMode _currentMode;
        private bool _disposed;

        public event EventHandler<TimerEventArgs> TimerTick;
        public event EventHandler<TimerEventArgs> TimerCompleted;
        public event EventHandler<TimerModeChangedEventArgs> TimerModeChanged;

        public TimerMode CurrentMode => _currentMode;
        public bool IsRunning => _currentTimer?.IsRunning ?? false;

        public TimerManager()
        {
            _currentMode = TimerMode.Pomodoro;
            SwitchToMode(_currentMode);
        }

        public void SwitchToMode(TimerMode mode)
        {
            if (_currentMode == mode) return;

            // 現在のタイマーを停止・破棄
            if (_currentTimer != null)
            {
                _currentTimer.Stop();
                _currentTimer.TimerTick -= OnTimerTick;
                _currentTimer.TimerCompleted -= OnTimerCompleted;
                _currentTimer.Dispose();
            }

            // 新しいタイマーを作成
            _currentMode = mode;
            _currentTimer = CreateTimer(mode);
            _currentTimer.TimerTick += OnTimerTick;
            _currentTimer.TimerCompleted += OnTimerCompleted;

            TimerModeChanged?.Invoke(this, new TimerModeChangedEventArgs(mode));
        }

        private TimerBase CreateTimer(TimerMode mode)
        {
            return mode switch
            {
                TimerMode.Pomodoro => new PomodoroTimer(),
                TimerMode.Timer => new CountdownTimer(),
                TimerMode.Stopwatch => new StopwatchTimer(),
                _ => throw new ArgumentException($"Unknown timer mode: {mode}")
            };
        }

        public void StartStop()
        {
            if (_currentTimer == null) return;

            if (_currentTimer.IsRunning)
            {
                _currentTimer.Stop();
            }
            else
            {
                _currentTimer.Start();
            }
        }

        public void Reset()
        {
            _currentTimer?.Reset();
        }

        public string GetDisplayTime()
        {
            return _currentTimer?.GetDisplayTime() ?? "00:00";
        }

        private void OnTimerTick(object sender, TimerEventArgs e)
        {
            TimerTick?.Invoke(sender, e);
        }

        private void OnTimerCompleted(object sender, TimerEventArgs e)
        {
            TimerCompleted?.Invoke(sender, e);
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
                    _currentTimer?.Dispose();
                }
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// タイマーモード変更イベント引数
    /// </summary>
    public class TimerModeChangedEventArgs : EventArgs
    {
        public TimerMode NewMode { get; }

        public TimerModeChangedEventArgs(TimerMode newMode)
        {
            NewMode = newMode;
        }
    }
}