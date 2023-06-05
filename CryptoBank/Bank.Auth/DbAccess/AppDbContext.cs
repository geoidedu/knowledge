using Bank.Auth.Features.Auth.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bank.Auth.DbAccess
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("DbConString"));
        }



        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

    }
}
