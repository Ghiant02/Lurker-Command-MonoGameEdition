using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace LurkerCommand.Services {
    public static class ConfigManager {
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
        private static readonly Dictionary<string, string> Settings = new Dictionary<string, string>();

        public static void Initialize()
        {
            if (!File.Exists(ConfigPath))
            {
                Settings["Width"] = "1440";
                Settings["Height"] = "1080";
                Settings["FullScreen"] = "false";
                Settings["WindowTitle"] = "Lurker Command";
                Settings["AltF4"] = "true";
                Settings["AllowResizing"] = "false";
                Settings["FullScreenKey"] = "F11";
                Save();
            }
            else
            {
                Load();
            }
        }

        public static string Get(string key, string defaultValue = "")
        {
            return Settings.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static void Set(string key, string value)
        {
            Settings[key] = value;
            Save();
        }

        private static void Load()
        {
            if (!File.Exists(ConfigPath)) return;

            foreach (var line in File.ReadAllLines(ConfigPath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(new[] { '=' }, 2);

                if (parts.Length == 2)
                {
                    Settings[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }

        private static void Save()
        {
            var lines = Settings.Select(kvp => $"{kvp.Key}={kvp.Value}");
            File.WriteAllLines(ConfigPath, lines);
        }
    }
}