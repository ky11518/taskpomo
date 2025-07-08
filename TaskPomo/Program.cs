using System;
using System.Threading;
using TaskPomo.Core.Timer;

namespace TaskPomo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TaskPomo - ポモドーロタイマー");
            Console.WriteLine("Windows専用タスクバー常駐型ポモドーロタイマー");
            Console.WriteLine("=====================================");
            
            // タイマー機能デモ
            DemoTimerFunctionality();
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
        
        static void DemoTimerFunctionality()
        {
            Console.WriteLine("\n=== タイマー機能デモ ===");
            
            using var timerManager = new TimerManager();
            
            // イベントハンドラー設定
            timerManager.TimerTick += (sender, e) => 
            {
                Console.Write($"\r{GetModeDisplayName(timerManager.CurrentMode)}: {e.DisplayTime}");
            };
            
            timerManager.TimerCompleted += (sender, e) => 
            {
                Console.WriteLine($"\n✓ {GetModeDisplayName(timerManager.CurrentMode)} 完了!");
            };

            // 1. ストップウォッチテスト
            Console.WriteLine("\n1. ストップウォッチテスト (3秒)");
            timerManager.SwitchToMode(TimerMode.Stopwatch);
            timerManager.StartStop();
            Thread.Sleep(3000);
            timerManager.StartStop();
            Console.WriteLine($"\n   結果: {timerManager.GetDisplayTime()}");
            
            // 2. カウントダウンタイマーテスト
            Console.WriteLine("\n2. カウントダウンタイマーテスト (3秒)");
            timerManager.SwitchToMode(TimerMode.Timer);
            timerManager.SetCountdownTime(0, 3);
            timerManager.StartStop();
            Thread.Sleep(3500);
            
            // 3. ポモドーロタイマーテスト
            Console.WriteLine("\n3. ポモドーロタイマーテスト (短縮版: 3秒作業)");
            timerManager.SwitchToMode(TimerMode.Pomodoro);
            timerManager.SetPomodoroSettings(workMinutes: 0, shortBreak: 2, longBreak: 3, longBreakInterval: 2);
            Console.WriteLine($"   フェーズ: {timerManager.GetPomodoroPhase()}");
            timerManager.StartStop();
            Thread.Sleep(3500);
            Console.WriteLine($"   新しいフェーズ: {timerManager.GetPomodoroPhase()}");
            
            Console.WriteLine("\n=== デモ完了 ===");
        }
        
        static string GetModeDisplayName(TimerMode mode)
        {
            return mode switch
            {
                TimerMode.Pomodoro => "ポモドーロ",
                TimerMode.Timer => "タイマー",
                TimerMode.Stopwatch => "ストップウォッチ",
                _ => "不明"
            };
        }
    }
}