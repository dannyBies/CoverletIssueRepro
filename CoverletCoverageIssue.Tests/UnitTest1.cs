using System;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using Xunit;

namespace CoverletCoverageIssue.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var dbContext = new BloggingContext(SqliteInMemory.CreateOptions<BloggingContext>());
            dbContext.CreateEmptyViaWipe();

            dbContext.Blogs.Add(new Blog
            {
                Name = "Test",
                HasExpired = true
            });
            await dbContext.SaveChangesAsync();
                
            var program = new Program(dbContext);
            await program.GetBlogs("Test");
        }
    }
}
