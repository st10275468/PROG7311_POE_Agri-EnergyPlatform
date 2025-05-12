using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect_Platform.Models;
using Microsoft.AspNetCore.Identity;

namespace Agri_Energy_Connect_Platform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Creating an employee user in the database
            var passwordHasher = new PasswordHasher<User>();
            var employee = new User
            {
                userID = 1,
                name = "Employee",
                surname = "1",
                email = "Employee@gmail.com",
                phoneNumber = "1231231234",
                role = "Employee",
                password = "password",
                farmerProducts = new List<Product>()
            };

  

            modelBuilder.Entity<User>().HasData(employee);
        }

    }

}
