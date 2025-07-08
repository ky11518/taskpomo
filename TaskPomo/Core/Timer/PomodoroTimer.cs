using System;

namespace TaskPomo.Core.Timer
{
    /// <summary>
    /// ポモドーロタイマー
    /// </summary>
    public class PomodoroTimer : TimerBase
    {
        private int _workDurationMinutes = 25;
        private int _shortBreakMinutes = 5;
        private int _longBreakMinutes = 15;
        private int _longBreakInterval = 4;
        
        private int _remainingSeconds;
        private PomodoroPhase _currentPhase;
        private int _completedWorkSessions;

        public event EventHandler<PhaseCompletedEventArgs>? PhaseCompleted;

        public PomodoroPhase CurrentPhase => _currentPhase;
        public int CompletedWorkSessions => _completedWorkSessions;

        public PomodoroTimer()
        {
            Reset();
        }

        public void SetWorkDuration(int minutes, int seconds = 0)
        {
            _workDurationMinutes = minutes;
            if (_currentPhase == PomodoroPhase.Work && !_isRunning)
            {
                _remainingSeconds = minutes * 60 + seconds;
            }
        }

        public void SetBreakDuration(int shortBreak, int longBreak)
        {
            _shortBreakMinutes = shortBreak;
            _longBreakMinutes = longBreak;
        }

        public void SetLongBreakInterval(int interval)
        {
            _longBreakInterval = interval;
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
            _currentPhase = PomodoroPhase.Work;
            _completedWorkSessions = 0;
            SetPhaseTime();
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
                    CompletePhase();
                    return; // CompletePhase後はbase.OnTimerTick()を呼ばない
                }
            }
            base.OnTimerTick();
        }

        private void CompletePhase()
        {
            StopTimer();
            
            var previousPhase = _currentPhase;
            
            switch (_currentPhase)
            {
                case PomodoroPhase.Work:
                    _completedWorkSessions++;
                    // TDD Debug: ログ出力
                    System.Console.WriteLine($"[DEBUG] Work completed. Sessions: {_completedWorkSessions}, Interval: {_longBreakInterval}, ShouldTakeLongBreak: {ShouldTakeLongBreak()}");
                    _currentPhase = ShouldTakeLongBreak() ? PomodoroPhase.LongBreak : PomodoroPhase.ShortBreak;
                    break;
                    
                case PomodoroPhase.ShortBreak:
                case PomodoroPhase.LongBreak:
                    _currentPhase = PomodoroPhase.Work;
                    break;
            }
            
            SetPhaseTime();
            OnPhaseCompleted(previousPhase, _currentPhase);
        }

        private bool ShouldTakeLongBreak()
        {
            return _completedWorkSessions > 0 && _completedWorkSessions % _longBreakInterval == 0;
        }

        private void SetPhaseTime()
        {
            _remainingSeconds = _currentPhase switch
            {
                PomodoroPhase.Work => _workDurationMinutes * 60,
                PomodoroPhase.ShortBreak => _shortBreakMinutes * 60,
                PomodoroPhase.LongBreak => _longBreakMinutes * 60,
                _ => _workDurationMinutes * 60
            };
        }

        private void OnPhaseCompleted(PomodoroPhase fromPhase, PomodoroPhase toPhase)
        {
            var args = new PhaseCompletedEventArgs(fromPhase, toPhase, _completedWorkSessions);
            PhaseCompleted?.Invoke(this, args);
            OnTimerCompleted(); // 基底クラスのTimerCompletedイベントも発行
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

    /// <summary>
    /// フェーズ完了イベント引数
    /// </summary>
    public class PhaseCompletedEventArgs : EventArgs
    {
        public PomodoroPhase FromPhase { get; }
        public PomodoroPhase ToPhase { get; }
        public int CompletedWorkSessions { get; }

        public PhaseCompletedEventArgs(PomodoroPhase fromPhase, PomodoroPhase toPhase, int completedSessions)
        {
            FromPhase = fromPhase;
            ToPhase = toPhase;
            CompletedWorkSessions = completedSessions;
        }
    }
}