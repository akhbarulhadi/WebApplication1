using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        public DbSet<Genders> Genders { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<EmployeeDept> EmployeeDept { get; set; }
        public DbSet<EmployeeVM> EmployeeVM { get; set; }
        public DbSet<GenderVM> GenderVM { get; set; }
        public DbSet<DepartmentVM> DepartmentVM { get; set; }
        public DbSet<DepartmentChartVM> DepartmentChartVM { get; set; }
        public DbSet<GenderChartVM> GenderChartVM { get; set; }
        public DbSet<ChartVM> ChartVM { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeVM>().HasNoKey();
            modelBuilder.Entity<GenderVM>().HasNoKey();
            modelBuilder.Entity<DepartmentVM>().HasNoKey();
            modelBuilder.Entity<DepartmentChartVM>().HasNoKey();
            modelBuilder.Entity<GenderChartVM>().HasNoKey();
            modelBuilder.Entity<ChartVM>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }

    }
}
