using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Update;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Xunit;

namespace Todo.Tests
{
    public class TodoListDetailViewmodelFactoryTests
    {
        [Fact]
        public void CreateOrdersItemsByImportanceWhenNotOrderingByRank()
        {
            // Arrange
            const string highImportanceItemTitle = "bread";
            const string mediumImportanceItemTitle = "milk";
            const string lowImportanceItemTitle = "eggs";
            
            var todoList = new TestTodoListBuilder(new IdentityUser("alice@example.com"), "shopping")
                    .WithItem(lowImportanceItemTitle, Importance.Low)
                    .WithItem(highImportanceItemTitle, Importance.High)
                    .WithItem(mediumImportanceItemTitle, Importance.Medium)
                    .Build();
            
            // Act
            var result = TodoListDetailViewmodelFactory.Create(todoList, false);

            // Assert
            Assert.Equal(highImportanceItemTitle, result.Items.Select(o => o.Title).First());
            Assert.Equal(lowImportanceItemTitle, result.Items.Select(o => o.Title).Last());
        }
        
        [Fact]
        public void CreateOrdersItemsByRankWhenRequested()
        {
            // Arrange
            const string highRankItemTitle = "bread";
            const string mediumRankItemTitle = "milk";
            const string lowRankItemTitle = "eggs";
            
            var todoList = new TestTodoListBuilder(new IdentityUser("alice@example.com"), "shopping")
                .WithItem(lowRankItemTitle, Importance.Medium, rank: 3)
                .WithItem(highRankItemTitle, Importance.Medium, rank: 1)
                .WithItem(mediumRankItemTitle, Importance.Medium, rank: 2)
                .Build();
            
            // Act
            var result = TodoListDetailViewmodelFactory.Create(todoList, true);

            // Assert
            Assert.Equal(highRankItemTitle, result.Items.Select(o => o.Title).First());
            Assert.Equal(lowRankItemTitle, result.Items.Select(o => o.Title).Last());
        }
    }
}