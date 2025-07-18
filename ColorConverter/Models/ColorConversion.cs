using System.ComponentModel.DataAnnotations;

namespace ColorConverter.Models
{
    public class ColorConversion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string InputFormat { get; set; }

        [Required]
        public string OutputFormat { get; set; }

        [Required]
        public string InputValue { get; set; }

        [Required]
        public string Result { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
