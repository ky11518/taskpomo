using System;
using TaskPomo.Core.Timer;

// 簡単なデバッグ用テスト
var timer = new PomodoroTimer();
timer.SetWorkDuration(0, 1);
timer.SetBreakDuration(1, 2);
timer.SetLongBreakInterval(2);

Console.WriteLine($"初期: Phase={timer.CurrentPhase}, Sessions={timer.CompletedWorkSessions}");

// 1回目作業
timer.Start();
System.Threading.Thread.Sleep(1200);
Console.WriteLine($"1回目作業後: Phase={timer.CurrentPhase}, Sessions={timer.CompletedWorkSessions}");

// 1回目休憩
timer.Start();
System.Threading.Thread.Sleep(1200);
Console.WriteLine($"1回目休憩後: Phase={timer.CurrentPhase}, Sessions={timer.CompletedWorkSessions}");

// 2回目作業
timer.Start();
System.Threading.Thread.Sleep(1200);
Console.WriteLine($"2回目作業後: Phase={timer.CurrentPhase}, Sessions={timer.CompletedWorkSessions}");