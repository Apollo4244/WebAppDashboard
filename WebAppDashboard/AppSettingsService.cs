using System.Text.Json;

namespace WebAppDashboard
{
    public static class AppSettingsService
    {
        internal static string FilePath =
            Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        public static bool IsFirstRun { get; private set; }

        public static AppSettings Load()
        {
            if (!File.Exists(FilePath))
            {
                IsFirstRun = true;
                var defaults = new AppSettings();
                Save(defaults);
                return defaults;
            }

            try
            {
                string json = File.ReadAllText(FilePath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions)
                               ?? new AppSettings();

                // Migration: legacy "StartUrl" string → first Pages entry
                if (settings.Pages.Count == 0)
                {
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("StartUrl", out var urlProp))
                    {
                        string url = urlProp.GetString() ?? Brand.DefaultUrl;
                        settings.Pages.Add(new PageEntry { Name = Brand.DefaultPageName, Url = url });
                        Save(settings);
                    }
                }

                return settings;
            }
            catch
            {
                return new AppSettings();
            }
        }

        public static void Save(AppSettings settings)
        {
            string json = JsonSerializer.Serialize(settings, JsonOptions);
            File.WriteAllText(FilePath, json);
        }
    }
}
