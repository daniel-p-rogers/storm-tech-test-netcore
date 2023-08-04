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
        public void CreateOrdersItemsByImportance()
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
            var result = TodoListDetailViewmodelFactory.Create(todoList);

            // Assert
            Assert.Equal(highImportanceItemTitle, result.Items.Select(o => o.Title).First());
            Assert.Equal(lowImportanceItemTitle, result.Items.Select(o => o.Title).Last());
        }
    }
}