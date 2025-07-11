# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

**必ず日本語で回答しなさい。**

## Project Overview

TaskPomo is a Windows-only taskbar-resident Pomodoro timer application being developed with strict **t-wada style TDD methodology**. The project implements three timer types: BasicTimer (base class), StopwatchTimer, CountdownTimer, and PomodoroTimer. Currently **24 TDD cycles complete** with core timer functionality implemented.

## Development Commands

### WSL2 Environment Setup
This project runs in WSL2 (Linux) but executes Windows .NET commands. Use the full path to dotnet.exe:

```bash
# Basic test execution
"/mnt/c/Program Files/dotnet/dotnet.exe" test TaskPomo.Tests/TaskPomo.Tests.csproj --verbosity normal

# Run specific test class
"/mnt/c/Program Files/dotnet/dotnet.exe" test TaskPomo.Tests/TaskPomo.Tests.csproj --filter "FullyQualifiedName~BasicTimerTests"

# Run specific test method
"/mnt/c/Program Files/dotnet/dotnet.exe" test TaskPomo.Tests/TaskPomo.Tests.csproj --filter "Name=タイマーを作成できる"

# Build the application
"/mnt/c/Program Files/dotnet/dotnet.exe" build TaskPomo/TaskPomo.csproj

# Run the application
"/mnt/c/Program Files/dotnet/dotnet.exe" run --project TaskPomo/TaskPomo.csproj
```

### TDD Workflow (CRITICAL)
This project strictly follows t-wada style TDD methodology:
1. **RED**: Write a failing test first
2. **GREEN**: Write minimal code to make the test pass  
3. **REFACTOR**: Improve code while keeping tests green

**Always respond in Japanese** as this is a Japanese development project.

## Current Architecture & Implementation Status

### Implemented Timer Classes (24 TDD Cycles Complete)

#### 1. BasicTimer (✅ Complete - 8 cycles)
- Base class for all timer implementations
- State management (IsRunning, ElapsedSeconds)
- Basic operations (Start, Stop, Reset)
- Event system (TimerTick, TimerCompleted)
- Protected OnTimerCompleted() method for inheritance

#### 2. StopwatchTimer (✅ Complete - 8 cycles)
- Inherits from BasicTimer
- **Real-time tracking** with System.Threading.Timer
- StartWithRealTimeTracking() method
- Automatic elapsed time incrementation
- Proper resource management (Timer disposal)

#### 3. CountdownTimer (✅ Complete - 8 cycles)
- Inherits from BasicTimer
- SetDuration() and RemainingSeconds property
- **Real countdown functionality** with StartWithCountdown()
- LastDuration memory feature
- Automatic TimerCompleted event when countdown reaches zero
- Proper timer resource management

#### 4. PomodoroTimer (🚧 In Progress - 2 cycles)
- Inherits from BasicTimer
- **Currently**: Basic inheritance skeleton only
- **Next**: Work/break cycle implementation

### Key Architecture Patterns

#### Inheritance-Based Design
- **BasicTimer** as foundation class
- Specialized timers extend with specific functionality
- Event-driven notification system

#### Real-Time Implementation
- **System.Threading.Timer** for actual time tracking
- Thread-safe time updates
- Proper disposal pattern for resources

#### Resource Management
- Timer disposal in Stop/Reset methods
- Nullable reference types for safety
- Clean state management

### Test Implementation
- **Japanese method names** for test clarity
- **Real-time tests** using Thread.Sleep
- **FluentAssertions** for readable assertions
- **Complete coverage** of all implemented features

### Technology Stack
- **Framework**: .NET 8.0 Windows (upgraded from .NET 6.0)
- **UI**: WPF with Windows Forms (for NotifyIcon taskbar integration)
- **Test Framework**: NUnit 3.13.3 + FluentAssertions 6.10.0
- **Build Target**: Single-file executable (TaskPomo.exe)
- **Nullable Reference Types**: Enabled

## Development Notes

- **Language**: All development communication must be in Japanese
- **TDD Discipline**: Never write implementation code before a failing test
- **Commit Strategy**: Each RED-GREEN-REFACTOR cycle should be committed separately
- **Test Naming**: Use Japanese method names for test clarity
- **WSL2 Integration**: Use Windows dotnet.exe via `/mnt/c/Program Files/dotnet/dotnet.exe`
- **Resource Management**: Always dispose System.Threading.Timer in Stop/Reset methods

## Next Implementation Priority

The next major development phase is **PomodoroTimer** implementation:
- Work/break cycle management
- Long break calculation
- Customizable time settings
- State transitions between work and break phases

See `WSL2-Windows-CLI実行要領書.md` for detailed WSL2 command execution patterns.