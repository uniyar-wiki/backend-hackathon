using Microsoft.EntityFrameworkCore;
using UniYarWiki.Models;

namespace UniYarWiki.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<TeacherRatings> TeacherRatings { get; set; }
    }
}

