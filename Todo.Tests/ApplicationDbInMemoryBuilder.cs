using Microsoft.EntityFrameworkCore;
using Todo.Data;

namespace Todo.Tests
{
    public static class ApplicationDbInMemoryBuilder
    {
        public static DbContextOptions<ApplicationDbContext> GetInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;
        }
    }
}