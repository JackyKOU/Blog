using Microsoft.EntityFrameworkCore;
using PiBlog.Model.Blog;
namespace PiBlog.Infrastructure
{
    public class PiDbContext:DbContext
    {
        public DbSet<Post> Posts {get;set;}
    }
}