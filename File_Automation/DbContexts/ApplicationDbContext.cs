using File_Automation.Models;
using Microsoft.EntityFrameworkCore;

namespace File_Automation.DbContexts
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Upload> Uploads { get; set; }
        public DbSet<Move> Copies { get; set; }
        public DbSet<Delete> Deletes { get; set; }
    }
}
