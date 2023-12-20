using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Data.FluentApi;
using WebApi.Models;

namespace WebApi.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Pentester> Pentesters { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<PentrationTest> Tests { get; set; }
        public DbSet<Finding> Findings { get; set; }
        public DbSet<PentestingMethodology> PentestingMethodologies { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string? connectionString = config.GetSection("ConnectionStrings")
                                             .GetValue<string>("SqlServer");

            if (connectionString is null)
            {
                throw new Exception("connection string not found !");
            }
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Pentester>().ToTable("Pentesters");

            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            modelBuilder.AddAdminEntity();
            modelBuilder.AddClientEntity();
            modelBuilder.AddPentesterEntity();
            modelBuilder.AddPentrationTestEntity();
            modelBuilder.AddTargetSystemEntity();
            modelBuilder.AddPentestingMethodologyEntity();
            modelBuilder.AddFindingEntity();
        }

        public ApplicationRole[] LoadRoles(string[] roles)
        {
            var loadedRoles = Roles.Where(r => roles.Contains(r.Name));
            return loadedRoles.ToArray();
        }
    }
}
