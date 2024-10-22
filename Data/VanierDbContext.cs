using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace VanierAppAPIs.Data
{
    public class VanierDBContext : DbContext
    {
        public VanierDBContext(DbContextOptions<VanierDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }

        //public DbSet<StudentCourse> StudentCourses { get; set; }
        //public DbSet<Grade> Grades { get; set; }
    }

}
