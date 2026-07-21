using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mystic_Foods.Managers
{
    public class SettingsData
    {
        public float MusicVolume { get; set; } = 0.5f;
        public float SfxVolume { get; set; } = 0.5f;
        public bool IsMuted { get; set; } = false;

        private static string FilePath => Path.Combine(AppContext.BaseDirectory, "settings.json");

        public void Save()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public static SettingsData Load()
        {
            if (!File.Exists(FilePath))
                return new SettingsData();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<SettingsData>(json) ?? new SettingsData();
        }
    }
}
