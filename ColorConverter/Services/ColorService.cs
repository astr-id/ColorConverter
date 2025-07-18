using ColorConverter.Data;
using ColorConverter.DTOs;
using ColorConverter.Models;
using Microsoft.EntityFrameworkCore;

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
                var (r, g, b) = ColorConversionHelper.HexToRgb(request.ColorValue);

                string result;

                switch (request.OutputFormat)
                {
                    case "rgb":
                        result = $"rgb({r}, {g}, {b})";
                        break;
                    case "cmyk":
                        var (c, m, y, k) = ColorConversionHelper.RgbToCmyk(r, g, b);
                        result = $"cmyk({c:F2}, {m:F2}, {y:F2}, {k:F2})";
                        break;
                    case "hsl":
                        var (h, s, l) = ColorConversionHelper.RgbToHsl(r, g, b);
                        result = $"hsl({h:F1}°, {s:F1}%, {l:F1}%)";
                        break;
                    case "lab":
                        var (L, A, B) = ColorConversionHelper.RgbToLab(r, g, b);
                        result = $"lab({L:F1}, {A:F1}, {B:F1})";
                        break;
                    default:
                        throw new NotSupportedException("Format de sortie non supporté");
                }

                return FormatResponse(request, result);

            }
            else if (request.InputFormat == "rgb")
            {
                var values = request.ColorValue
                    .Replace("rgb(", "")
                    .Replace(")", "")
                    .Split(',')
                    .Select(v => int.Parse(v.Trim()))
                    .ToArray();

                int r = values[0], g = values[1], b = values[2];

                string result;

                switch (request.OutputFormat)
                {
                    case "hex":
                        result = ColorConversionHelper.RgbToHex(r, g, b);
                        break;
                    case "cmyk":
                        var (c, m, y, k) = ColorConversionHelper.RgbToCmyk(r, g, b);
                        result = $"cmyk({c:F2}, {m:F2}, {y:F2}, {k:F2})";
                        break;
                    case "hsl":
                        var (h, s, l) = ColorConversionHelper.RgbToHsl(r, g, b);
                        result = $"hsl({h:F1}°, {s:F1}%, {l:F1}%)";
                        break;
                    case "lab":
                        var (L, A, B) = ColorConversionHelper.RgbToLab(r, g, b);
                        result = $"lab({L:F1}, {A:F1}, {B:F1})";
                        break;
                    default:
                        throw new NotSupportedException("Format de sortie non supporté");
                }

                return FormatResponse(request, result);
            }
            else
            {
                throw new NotSupportedException("Format d’entrée non supporté");
            }
        }

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
