using System;

namespace TaskPomo
{
    public class BasicTimer : TimerBase
    {
        public override int ElapsedSeconds { get; protected set; } = 0;
        
        public event EventHandler<EventArgs>? TimerTick;

        protected override void OnStart()
        {
            // BasicTimerでは特別な開始処理なし
        }

        protected override void OnStop()
        {
            // BasicTimerでは特別な停止処理なし
        }

        protected override void OnReset()
        {
            ElapsedSeconds = 0;
        }

        protected override void OnTimerTick(object? state)
        {
            // BasicTimerでは特別なティック処理なし
        }
    }
}