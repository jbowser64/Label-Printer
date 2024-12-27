using LblPrint.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace LblPrint.Data
{
    public class PartsDatabaseContext : DbContext
    {
        public PartsDatabaseContext (DbContextOptions<PartsDatabaseContext> options) : base(options) { }    
        public DbSet<PartModel> Parts { get; set; } = null!; 
    }
}

