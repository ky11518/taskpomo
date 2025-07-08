namespace TaskPomo
{
    public class BasicTimer
    {
        public bool IsRunning { get; private set; } = false;
        public int ElapsedSeconds { get; private set; } = 0;

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Reset()
        {
            IsRunning = false;
            ElapsedSeconds = 0;
        }
    }
}