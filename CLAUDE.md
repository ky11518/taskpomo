# CLAUDE.md

必ず日本語で回答しなさい。

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

TaskPomo is a Windows-only taskbar-resident Pomodoro timer application being developed with TDD methodology. The project was recently reset to start from scratch using proper t-wada style TDD. Currently in the early RED-GREEN-REFACTOR cycle phase.

## Development Commands

### Basic Operations
```bash
# Build the main application
dotnet build TaskPomo/TaskPomo.csproj

# Run tests (current focus of development)
dotnet test TaskPomo.Tests/TaskPomo.Tests.csproj --verbosity normal

# Run a specific test class
dotnet test TaskPomo.Tests/TaskPomo.Tests.csproj --filter "FullyQualifiedName~BasicTimerTests"

# Run a specific test method
dotnet test TaskPomo.Tests/TaskPomo.Tests.csproj --filter "Name=タイマーを作成できる"

# Run the application (minimal console app currently)
dotnet run --project TaskPomo/TaskPomo.csproj
```

### TDD Workflow (CRITICAL)
This project strictly follows t-wada style TDD methodology:
1. **RED**: Write a failing test first
2. **GREEN**: Write minimal code to make the test pass
3. **REFACTOR**: Improve code while keeping tests green

**Always respond in Japanese** as this is a Japanese development project.

## Current State

The project is in early TDD development phase:
- **Current Test**: `BasicTimerTests.タイマーを作成できる()` - expects a `BasicTimer` class
- **Current Status**: RED (test fails because `BasicTimer` class doesn't exist)
- **Next Step**: Implement minimal `BasicTimer` class to achieve GREEN state

## Architecture (Target State)

### Planned Core Components
The final application will include:
- **TimerBase**: Abstract base for timer implementations  
- **PomodoroTimer**: Pomodoro technique with work/break cycles
- **CountdownTimer**: Arbitrary countdown functionality
- **StopwatchTimer**: Elapsed time measurement
- **TimerManager**: Timer switching and state management
- **TrayIconManager**: Taskbar integration (WPF + Windows Forms)
- **NotificationService**: System notifications and taskbar alerts

### Technology Stack
- **Target Framework**: .NET 6.0 Windows
- **UI**: WPF with Windows Forms (for NotifyIcon taskbar integration)
- **Test Framework**: NUnit with FluentAssertions
- **Build Target**: Single-file executable (TaskPomo.exe)
- **Nullable Reference Types**: Enabled

## CI/CD Pipeline

Simple GitHub Actions workflow (`.github/workflows/test.yml`):
- Triggers on push to master branch
- Runs on windows-latest
- Executes: `dotnet test TaskPomo.Tests/TaskPomo.Tests.csproj --verbosity normal`

## Development Notes

- **Language**: All development communication must be in Japanese
- **TDD Discipline**: Never write implementation code before a failing test
- **Commit Strategy**: Each RED-GREEN-REFACTOR cycle should be committed separately
- **Test Naming**: Use Japanese method names for test clarity
- **Project Structure**: Keep tests in `TaskPomo.Tests/` with same namespace structure as main project