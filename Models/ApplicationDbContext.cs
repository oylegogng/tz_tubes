using Microsoft.EntityFrameworkCore;

namespace tz_tubes.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Pipe> Pipes { get; set; }
        public DbSet<Packet> Packets { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = LAPTOP-FU2ARVTC\\SQLEXPRESS02; User = oleg; Password = 123; Database = TubeDatabase; TrustServerCertificate = True;");
        }
    }
}


