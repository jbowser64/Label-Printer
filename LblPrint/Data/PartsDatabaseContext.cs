using LblPrint.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace LblPrint.Data
{
    public class PartsDatabaseContext : DbContext
    {
        public PartsDatabaseContext (DbContextOptions<PartsDatabaseContext> options) : base(options)
        { }    
        public DbSet<PrintPartModel> Parts { get; set; } = null!; 
        public DbSet<ViewPartModel> ViewParts { get; set; }
    }
}

