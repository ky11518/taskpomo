# CLAUDE.md

必ず日本語で回答してください。

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

TaskPomo is a Windows-only taskbar-resident Pomodoro timer application built with C# and WPF. It operates without windows, using only taskbar interactions for all operations.

## Development Commands

### Build and Test
```bash
# Build the main application
dotnet build TaskPomo/TaskPomo.csproj

# Run tests
dotnet test TaskPomo.Tests/TaskPomo.Tests.csproj

# Run tests with coverage
dotnet test TaskPomo.Tests/TaskPomo.Tests.csproj --collect:"XPlat Code Coverage"

# Publish single-file executable
dotnet publish TaskPomo/TaskPomo.csproj -c Release -r win-x64 --self-contained true --single-file -o ./publish
```

### TDD Workflow
This project follows t-wada style TDD methodology:
1. **RED**: Write failing test
2. **GREEN**: Write minimal code to pass
3. **REFACTOR**: Improve code while keeping tests green

## Architecture

### Core Components
- **TimerBase**: Abstract base class for all timer implementations
- **PomodoroTimer**: Pomodoro technique timer with work/break cycles
- **CountdownTimer**: Arbitrary countdown timer
- **StopwatchTimer**: Elapsed time measurement
- **TimerManager**: Manages timer switching and state
- **TrayIconManager**: Handles taskbar icon and context menu
- **SettingsService**: Persistent settings management
- **NotificationService**: System notifications and taskbar flashing

### Project Structure
```
TaskPomo/
├── Core/Timer/          # Timer implementations
├── UI/TrayIcon/         # Taskbar integration
├── Models/              # Data models
├── Services/            # Business services
└── Properties/          # Resources

TaskPomo.Tests/
├── Core/Timer/          # Timer tests
├── Services/            # Service tests
└── UI/                  # UI tests
```

## CI/CD Pipeline

GitHub Actions workflow runs on push/PR:
1. **Test**: Run all unit tests with coverage
2. **Build**: Create release build
3. **Release**: Publish single-file executable (on main branch)

## Development Notes

- Target Framework: .NET 6.0 Windows
- UI Framework: WPF with Windows Forms (for NotifyIcon)
- Test Framework: NUnit with FluentAssertions and Moq
- Settings: JSON files in %APPDATA%\TaskPomo\
- Package Management: NuGet (Newtonsoft.Json)
- Build Output: Single-file executable (TaskPomo.exe)