namespace TaskPomo
{
    public class BasicTimer
    {
        public bool IsRunning { get; private set; } = false;

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}