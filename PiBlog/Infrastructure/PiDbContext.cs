using Microsoft.EntityFrameworkCore;
using PiBlog.Model.Blog;
using System;
namespace PiBlog.Infrastructure
{
    public class PiDbContext:DbContext
    {
        public PiDbContext(DbContextOptions<PiDbContext> options):base (options)
        {
            CreateFakeData(5);
        }

        public DbSet<Post> Posts {get;set;}

        private void CreateFakeData(int blogCount)
        {
            for(int i=1;i<=blogCount;i++)
            {
                if(Posts.Find(i)==null)
                {
                    var fakePost = new Post(){
                    Title = $"title_{i}",
                    Body = $"body_{i}",
                    Author = $"author_{i}",
                    CreationTime = DateTime.Now.AddDays(-i),
                    };

                    Posts.Add(fakePost);
                    this.SaveChanges();
                }

                
            }
        }
    }
}