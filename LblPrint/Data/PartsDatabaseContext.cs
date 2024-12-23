using LblPrint.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace LblPrint.Data
{
    public class PartsDatabaseContext
    {
        protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=USA-L-51XHQN3\SQLEXPRESS;Initial Catalog=PartsAndLocations;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"
        }
        public DbSet<PartModel> Parts { get; set; } = null!; 
    }
}

