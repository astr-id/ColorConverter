using Microsoft.EntityFrameworkCore;
using ColorConverter.Models;

namespace ColorConverter.Data
{
    public class ColorContext : DbContext
    {
        public ColorContext(DbContextOptions<ColorContext> options) : base(options) { }

        public DbSet<ColorConversion> Conversions { get; set; }
    }
}
