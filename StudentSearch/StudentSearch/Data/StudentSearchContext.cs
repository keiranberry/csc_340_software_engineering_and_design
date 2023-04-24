using StudentSearch.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentSearch.Data
{
    public class StudentSearchContext : IdentityDbContext<ApplicationUser>
    {
        public StudentSearchContext(DbContextOptions<StudentSearchContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Student { get; set; } = default!;
        public DbSet<Comment> Comment { get; set; } = default!;
        public DbSet<ApplicationUser> ApplicationUser { get; set; } = default!;
    }
}
