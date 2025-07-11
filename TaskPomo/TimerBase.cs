using System;

namespace TaskPomo
{
    public abstract class TimerBase
    {
        // 共通プロパティ
        public bool IsRunning { get; protected set; } = false;
        public virtual int ElapsedSeconds { get; protected set; } = 0;
        
        // 共通イベント
        public event EventHandler<EventArgs>? TimerCompleted;
        
        // 内部タイマー管理
        protected System.Threading.Timer? timer;
        
        // 基本制御メソッド（具象実装）
        public virtual void Start()
        {
            IsRunning = true;
            OnStart();
        }
        
        public virtual void Stop()
        {
            IsRunning = false;
            timer?.Dispose();
            timer = null;
            OnStop();
        }
        
        public virtual void Reset()
        {
            Stop();
            ElapsedSeconds = 0;
            OnReset();
        }
        
        // 抽象メソッド（サブクラスで実装必須）
        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract void OnReset();
        protected abstract void OnTimerTick(object? state);
        
        // テンプレートメソッド
        protected void StartTimer()
        {
            timer = new System.Threading.Timer(OnTimerTick, null, 
                TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }
        
        // イベント発火メソッド
        protected virtual void OnTimerCompleted()
        {
            TimerCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}