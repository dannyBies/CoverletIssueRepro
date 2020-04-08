using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoverletCoverageIssue
{
    public class Blog
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public User CreatedBy { get; set; }
        public bool HasExpired { get; set; }

        public Guid? SiteId { get; set;}
        public Site Site { get; set; }
        public Guid? RegionId { get; set; }
        public Region Region { get; set; }
    }

    public class Site
    {
        public Guid Id { get; set; }
        public DateTime? EndedOn { get; set; }
    }

    public class Region
    {
        public Guid Id { get; set; }
        public DateTime? EndedOn { get; set; }
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
                : base(options)
        { }

        public DbSet<Blog> Blogs { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        private readonly BloggingContext _dbContext;

        public Program(BloggingContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Blog>> GetBlogs(string name)
        {
            var query = CreateQuery();
            var blogs = await query
                .Where(x => x.Name == name)
                .ToListAsync();

            var activeBlogs = blogs
                .Where(x => !x.HasExpired &&
                            (x.Region == null || (x.Region != null && x.Region.EndedOn == null)) &&
                            (x.Site == null || (x.Site != null && x.Site.EndedOn == null)))
                .ToList();

            return activeBlogs;
        }

        private IIncludableQueryable<Blog, User> CreateQuery()
        {
            var query = _dbContext.Blogs
                .Include(x => x.CreatedBy);

            return query;
        }
    }
}
