using System.Text.RegularExpressions;

namespace WebAppDashboard
{
    // Zentrale Hex-Farben-Validierung: ausschließlich "#RRGGBB" (6-stellig).
    // 8-stellig (#AARRGGBB), 3-stellig, >8 Zeichen oder ungültige Zeichen → ungültig.
    internal static class HexColor
    {
        public static bool IsValid6(string? value) =>
            !string.IsNullOrWhiteSpace(value) &&
            Regex.IsMatch(value, @"^#[0-9a-fA-F]{6}$");

        public static bool TryParse6(string? value, out Color color)
        {
            color = default;
            if (!IsValid6(value)) return false;
            color = ColorTranslator.FromHtml(value!);
            return color.A == 255;
        }
    }
}