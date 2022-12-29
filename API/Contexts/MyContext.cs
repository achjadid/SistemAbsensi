using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace API.Contexts
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<AttendanceHistory> AttendanceHistories { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(a => a.Employee)
                .WithOne(b => b.User)
                .HasForeignKey<Employee>(b => b.NIK);

            modelBuilder.Entity<Role>()
                .HasMany(a => a.Users)
                .WithOne(b => b.Role);

            modelBuilder.Entity<Department>()
                .HasOne(a => a.EmployeeSupervisor)
                .WithOne(b => b.DepartmentSupervisor)
                .HasForeignKey<Department>(b => b.SupervisorNIK);

            modelBuilder.Entity<Department>()
                .HasMany(a => a.EmployeeDepartments)
                .WithOne(b => b.Departments);

            modelBuilder.Entity<Employee>()
                .HasMany(a => a.AttendanceHistories)
                .WithOne(b => b.Employee);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
