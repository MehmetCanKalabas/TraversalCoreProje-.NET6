using Microsoft.EntityFrameworkCore;

namespace SignalRApi.DAL
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-5EPP4QL;Database=MyVisitDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        public DbSet<Visitor> Visitors { get; set; }
    }
}
