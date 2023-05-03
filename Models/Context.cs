using Microsoft.EntityFrameworkCore;

namespace SampleApp.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        public DbSet<Organisation> Organisation { get; set; }
    }
}
