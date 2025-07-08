using System.Windows;
using TaskPomo.UI.TrayIcon;

namespace TaskPomo
{
    public partial class App : Application
    {
        private TrayIconManager _trayIconManager;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // ウィンドウを表示せずにタスクバーのみで動作
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            
            // タスクバーアイコンを初期化
            _trayIconManager = new TrayIconManager();
            _trayIconManager.Initialize();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _trayIconManager?.Dispose();
            base.OnExit(e);
        }
    }
}