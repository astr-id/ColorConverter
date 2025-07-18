using ColorConverter.DTOs;

namespace ColorConverter.Services
{
    public interface IColorService
    {
        ColorResponseDto ConvertColor(ColorRequestDto request);
    }
}
