using System.Reflection;

namespace WebAppDashboard
{
    // Liest die Branding-Werte aus den per MSBuild gesetzten Assembly-Metadaten.
    // Die Werte stammen aus WebAppDashboard.brand.targets (<AssemblyMetadata .../>)
    // und unterscheiden sich pro Variante.
    internal static class Brand
    {
        private static readonly Assembly s_assembly = typeof(Brand).Assembly;
        private static readonly Dictionary<string, string> s_values = Load();

        public static string Id              => Get("BrandId");
        public static string DisplayName     => Get("BrandDisplayName");
        public static string DefaultPageName => Get("BrandDefaultPageName");
        public static string DefaultUrl      => Get("BrandDefaultUrl");
        public static string MutexPrefix     => Get("BrandMutexPrefix");
        public static string ExeBaseName     => s_assembly.GetName().Name ?? Id;

        private static string Get(string key) =>
            s_values.TryGetValue(key, out var value) ? value : string.Empty;

        private static Dictionary<string, string> Load()
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var attribute in s_assembly.GetCustomAttributes<AssemblyMetadataAttribute>())
            {
                if (attribute.Key is { } key)
                    map[key] = attribute.Value ?? string.Empty;
            }
            return map;
        }
    }
}