using Microsoft.EntityFrameworkCore;
using User.Models;
using UserModel = User.Models.User;
using User.Data;

namespace User.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<Run> Runs { get; set; }
        public DbSet<LocationPoint> LocationPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Certifique-se de que o LocationPoint tenha uma chave primária configurada
            modelBuilder.Entity<LocationPoint>()
                .HasKey(lp => lp.Id);  // Definindo a chave primária (se ainda não estiver definida)
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<UserModel>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
    }
}
