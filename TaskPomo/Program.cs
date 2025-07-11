using System;
using System.Windows.Forms;

namespace TaskPomo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Windows Formsアプリケーションとして初期化
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            using (var app = new TaskPomoApplication())
            {
                try
                {
                    app.Initialize();
                    
                    // アプリケーションメッセージループを開始
                    Application.Run();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"アプリケーションエラー: {ex.Message}", "TaskPomo", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}