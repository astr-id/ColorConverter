using System;
using System.Globalization;

namespace ColorConverter.Services
{
    public static class ColorConversionHelper
    {
        // Hex vers RGB
        public static (int R, int G, int B) HexToRgb(string hex)
        {
            hex = hex.TrimStart('#');
            int r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            int g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            int b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            return (r, g, b);
        }

        // RGB vers Hex
        public static string RgbToHex(int r, int g, int b)
        {
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        // RGB vers CMYK
        public static (double C, double M, double Y, double K) RgbToCmyk(int r, int g, int b)
        {
            double rd = r / 255.0, gd = g / 255.0, bd = b / 255.0;
            double k = 1 - Math.Max(rd, Math.Max(gd, bd));
            double c = (1 - rd - k) / (1 - k + 1e-10);
            double m = (1 - gd - k) / (1 - k + 1e-10);
            double y = (1 - bd - k) / (1 - k + 1e-10);
            return (c, m, y, k);
        }

        // RGB vers HSL
        public static (double H, double S, double L) RgbToHsl(int r, int g, int b)
        {
            double rd = r / 255.0, gd = g / 255.0, bd = b / 255.0;
            double max = Math.Max(rd, Math.Max(gd, bd));
            double min = Math.Min(rd, Math.Min(gd, bd));
            double h = 0, s, l = (max + min) / 2;
            double d = max - min;

            if (d != 0)
            {
                s = l > 0.5 ? d / (2.0 - max - min) : d / (max + min);
                if (max == rd) h = (gd - bd) / d + (gd < bd ? 6 : 0);
                else if (max == gd) h = (bd - rd) / d + 2;
                else h = (rd - gd) / d + 4;
                h *= 60;
            }
            else
            {
                s = 0;
                h = 0;
            }

            return (h, s * 100, l * 100);
        }

        // RGB vers LAB
        public static (double L, double A, double B) RgbToLab(int r, int g, int b)
        {
            double[] rgb = { r / 255.0, g / 255.0, b / 255.0 };
            for (int i = 0; i < 3; i++)
                rgb[i] = rgb[i] > 0.04045 ? Math.Pow((rgb[i] + 0.055) / 1.055, 2.4) : rgb[i] / 12.92;

            double x = rgb[0] * 0.4124 + rgb[1] * 0.3576 + rgb[2] * 0.1805;
            double y = rgb[0] * 0.2126 + rgb[1] * 0.7152 + rgb[2] * 0.0722;
            double z = rgb[0] * 0.0193 + rgb[1] * 0.1192 + rgb[2] * 0.9505;

            x /= 0.95047;
            y /= 1.00000;
            z /= 1.08883;

            Func<double, double> f = t => t > 0.008856 ? Math.Pow(t, 1.0 / 3.0) : (7.787 * t) + (16.0 / 116.0);

            double l = (116 * f(y)) - 16;
            double a = 500 * (f(x) - f(y));
            double bVal = 200 * (f(y) - f(z));

            return (l, a, bVal);
        }
    }
}
