using System;
using System.IO;
using Newtonsoft.Json;
using TaskPomo.Models;

namespace TaskPomo.Services
{
    /// <summary>
    /// 設定管理サービス
    /// </summary>
    public class SettingsService
    {
        private const string SettingsFileName = "settings.json";
        private readonly string _settingsPath;
        private Settings _cachedSettings;

        public SettingsService()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(appDataPath, "TaskPomo");
            
            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }
            
            _settingsPath = Path.Combine(appFolder, SettingsFileName);
        }

        /// <summary>
        /// 設定を取得する
        /// </summary>
        public Settings GetSettings()
        {
            if (_cachedSettings != null)
            {
                return _cachedSettings;
            }

            if (!File.Exists(_settingsPath))
            {
                _cachedSettings = new Settings();
                SaveSettings(_cachedSettings);
                return _cachedSettings;
            }

            try
            {
                var json = File.ReadAllText(_settingsPath);
                _cachedSettings = JsonConvert.DeserializeObject<Settings>(json) ?? new Settings();
                return _cachedSettings;
            }
            catch (Exception)
            {
                // 設定ファイルの読み込みに失敗した場合はデフォルト設定を使用
                _cachedSettings = new Settings();
                SaveSettings(_cachedSettings);
                return _cachedSettings;
            }
        }

        /// <summary>
        /// 設定を保存する
        /// </summary>
        public void SaveSettings(Settings settings)
        {
            try
            {
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(_settingsPath, json);
                _cachedSettings = settings;
            }
            catch (Exception)
            {
                // 保存に失敗した場合は何もしない
            }
        }

        /// <summary>
        /// 設定をリセットする
        /// </summary>
        public void ResetSettings()
        {
            _cachedSettings = new Settings();
            SaveSettings(_cachedSettings);
        }
    }
}