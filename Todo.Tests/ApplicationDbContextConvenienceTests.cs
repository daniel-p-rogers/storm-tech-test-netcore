using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Services;
using Xunit;

namespace Todo.Tests
{
    public class ApplicationDbContextConvenienceTests
    {
        [Fact]
        public async Task RelevantTodoListsReturnsListsWhereUserOwnsList()
        {
            // Arrange
            var ownerUser = new IdentityUser("alice@example.com");
            var assignedUser = new IdentityUser("bob@example.com");

            var todoList = new TestTodoListBuilder(ownerUser, "shopping")
                .WithItem("bread", Importance.Medium, assignedUser)
                .Build();

            var dbOptions = ApplicationDbInMemoryBuilder.GetInMemoryDbContextOptions(); 

            using var context = new ApplicationDbContext(dbOptions);
            context.TodoLists.Add(todoList);
            await context.SaveChangesAsync();

            // Act
            var relevantTodoLists = context.RelevantTodoLists(assignedUser.Id);

            // Assert
            Assert.Single(relevantTodoLists);
        }
        
        [Fact]
        public async Task RelevantTodoListsReturnsNoListWhereUserDoesNotOwnList()
        {
            // Arrange
            var ownerUser = new IdentityUser("alice@example.com");
            var queryingUser = new IdentityUser("bob@example.com");

            var todoList = new TestTodoListBuilder(ownerUser, "shopping")
                .WithItem("bread", Importance.Medium, ownerUser)
                .Build();

            var dbOptions = ApplicationDbInMemoryBuilder.GetInMemoryDbContextOptions(); 

            using var context = new ApplicationDbContext(dbOptions);
            context.TodoLists.Add(todoList);
            await context.SaveChangesAsync();

            // Act
            var relevantTodoLists = context.RelevantTodoLists(queryingUser.Id);

            // Assert
            Assert.Empty(relevantTodoLists);
        }
        
        [Fact]
        public async Task RelevantTodoListsReturnsListsWhereUserIsAssigned()
        {
            // Arrange
            var ownerUser = new IdentityUser("alice@example.com");
            var assignedUser = new IdentityUser("bob@example.com");

            var todoList = new TestTodoListBuilder(ownerUser, "shopping")
                .WithItem("bread", Importance.Medium, assignedUser)
                .Build();

            var dbOptions = ApplicationDbInMemoryBuilder.GetInMemoryDbContextOptions(); 

            using var context = new ApplicationDbContext(dbOptions);
            context.TodoLists.Add(todoList);
            await context.SaveChangesAsync();

            // Act
            var relevantTodoLists = context.RelevantTodoLists(assignedUser.Id);

            // Assert
            Assert.Single(relevantTodoLists);
        }
    }
}