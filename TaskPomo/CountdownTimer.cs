namespace TaskPomo
{
    public class CountdownTimer : TimerBase
    {
        private int remainingSeconds = 0;

        public int RemainingSeconds => remainingSeconds;
        public int LastDuration { get; private set; } = 0;

        public void SetDuration(int seconds)
        {
            remainingSeconds = seconds;
            LastDuration = seconds;
        }

        public void StartWithCountdown()
        {
            Start();
            StartTimer();
        }

        protected override void OnStart()
        {
            // CountdownTimer開始時の処理
        }

        protected override void OnStop()
        {
            // CountdownTimer停止時の処理
        }

        protected override void OnReset()
        {
            remainingSeconds = LastDuration;
        }

        protected override void OnTimerTick(object? state)
        {
            remainingSeconds--;
            
            if (remainingSeconds <= 0)
            {
                remainingSeconds = 0;
                Stop();
                OnTimerCompleted();
            }
        }
    }
}