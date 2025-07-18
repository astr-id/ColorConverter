using System.ComponentModel.DataAnnotations;

namespace ColorConverter.DTOs
{
    public class ColorRequestDto
    {
        [Required]
        public string InputFormat { get; set; } // ex: "hex", "rgb"

        [Required]
        public string OutputFormat { get; set; } // ex: "rgb", "hex"

        [Required]
        public string ColorValue { get; set; }

    }
}
