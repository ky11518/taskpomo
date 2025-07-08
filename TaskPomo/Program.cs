using System;

namespace TaskPomo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TaskPomo - Hello World!");
            Console.WriteLine("Windows専用タスクバー常駐型ポモドーロタイマー");
            Console.WriteLine("開発中...");
            
            // 基本機能テスト
            var timer = new SimpleTimer();
            Console.WriteLine($"タイマー初期状態: {timer.GetStatus()}");
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
    
    // 最小限のタイマークラス
    public class SimpleTimer
    {
        private bool _isRunning = false;
        
        public string GetStatus()
        {
            return _isRunning ? "動作中" : "停止中";
        }
        
        public void Start()
        {
            _isRunning = true;
        }
        
        public void Stop()
        {
            _isRunning = false;
        }
    }
}