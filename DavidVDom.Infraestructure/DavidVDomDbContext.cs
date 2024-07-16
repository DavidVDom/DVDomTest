using DavidVDom.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DavidVDom.Infraestructure
{
    public class DavidVDomDbContext : DbContext
    {
        public DavidVDomDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Bank> Banks { get; set; }
    }
}
