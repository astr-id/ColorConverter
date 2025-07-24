using ColorConverter.Data;
using ColorConverter.DTOs;
using ColorConverter.Models;
using System.Text.RegularExpressions;

namespace ColorConverter.Services
{
    public class ColorService : IColorService
    {
        private readonly ColorContext _context;

        public ColorService(ColorContext context)
        {
            _context = context;
        }

        public ColorResponseDto ConvertColor(ColorRequestDto request)
        {
            if (request.InputFormat == "hex")
            {
                if (!Regex.IsMatch(request.ColorValue, @"^#([A-Fa-f0-9]{6})$"))
                {
                    throw new NotSupportedException("Format hexadécimal invalide. Le format attendu est #RRGGBB.");
                }

                var (r, g, b) = ColorConversionHelper.HexToRgb(request.ColorValue);

                string result = request.OutputFormat switch
                {
                    "rgb" => $"rgb({r}, {g}, {b})",
                    "cmyk" => FormatCmyk(ColorConversionHelper.RgbToCmyk(r, g, b)),
                    "hsl" => FormatHsl(ColorConversionHelper.RgbToHsl(r, g, b)),
                    "lab" => FormatLab(ColorConversionHelper.RgbToLab(r, g, b)),
                    _ => throw new NotSupportedException("Format de sortie non supporté")
                };

                return FormatResponse(request, result);
            }
            else if (request.InputFormat == "rgb")
            {
                if (!Regex.IsMatch(request.ColorValue, @"^rgb\(\d{1,3},\s*\d{1,3},\s*\d{1,3}\)$"))
                {
                    throw new NotSupportedException("Format RGB invalide. Le format attendu est rgb(255, 0, 0).");
                }

                var values = request.ColorValue
                    .Replace("rgb(", "")
                    .Replace(")", "")
                    .Split(',')
                    .Select(v => int.Parse(v.Trim()))
                    .ToArray();

                int r = values[0], g = values[1], b = values[2];

                string result = request.OutputFormat switch
                {
                    "hex" => ColorConversionHelper.RgbToHex(r, g, b),
                    "cmyk" => FormatCmyk(ColorConversionHelper.RgbToCmyk(r, g, b)),
                    "hsl" => FormatHsl(ColorConversionHelper.RgbToHsl(r, g, b)),
                    "lab" => FormatLab(ColorConversionHelper.RgbToLab(r, g, b)),
                    _ => throw new NotSupportedException("Format de sortie non supporté")
                };

                return FormatResponse(request, result);
            }
            else
            {
                throw new NotSupportedException("Format d’entrée non supporté");
            }
        }

        private string FormatCmyk((double c, double m, double y, double k) cmyk) =>
    $"cmyk({cmyk.c:F2}, {cmyk.m:F2}, {cmyk.y:F2}, {cmyk.k:F2})";

        private string FormatHsl((double h, double s, double l) hsl) =>
            $"hsl({hsl.h:F1}°, {hsl.s:F1}%, {hsl.l:F1}%)";

        private string FormatLab((double L, double A, double B) lab) =>
            $"lab({lab.L:F1}, {lab.A:F1}, {lab.B:F1})";


        private ColorResponseDto FormatResponse(ColorRequestDto request, string result)
        {
            var conversion = new ColorConversion
            {
                InputFormat = request.InputFormat,
                OutputFormat = request.OutputFormat,
                InputValue = request.ColorValue,
                Result = result
            };

            _context.Conversions.Add(conversion);
            _context.SaveChanges();

            return new ColorResponseDto
            {
                OriginalFormat = request.InputFormat,
                ConvertedFormat = request.OutputFormat,
                Result = result
            };
        }
    }
}
