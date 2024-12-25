using LblPrint.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace LblPrint.Data
{
    public class PartsDatabaseContext
    {
        public DbSet<PartModel> Parts { get; set; } = null!; 
    }
}

