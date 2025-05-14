using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DreamNucleus.Commands.Playground.Commands
{
    // https://docs.efproject.net/en/latest/platforms/full-dotnet/new-db.html with some tweaks
    public class BloggingContext : DbContext
    {
        public Guid Id { get; } = Guid.NewGuid();

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public BloggingContext()
        {
            Console.WriteLine($"BloggingContext created {Id}");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\Projects;Initial Catalog=Slipstream.CommonDotNet.Commands.Debug;Integrated Security=True;Connect Timeout=30;" +
                                         "Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public override void Dispose()
        {
            base.Dispose();
            Console.WriteLine($"BloggingContext disposed {Id}");
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public string PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
