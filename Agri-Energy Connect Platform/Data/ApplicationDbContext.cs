using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect_Platform.Models;

namespace Agri_Energy_Connect_Platform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
