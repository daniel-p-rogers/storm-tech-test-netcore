using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Todo.Data.Entities;
using Todo.Tests.Models;

namespace Todo.Tests
{
    /*
     * This class makes it easier for tests to create new TodoLists with TodoItems correctly hooked up
     */
    public class TestTodoListBuilder
    {
        private readonly string title;
        private readonly IdentityUser owner;
        private readonly List<TestTodoItemProperties> items = new List<TestTodoItemProperties>();

        public TestTodoListBuilder(IdentityUser owner, string title)
        {
            this.title = title;
            this.owner = owner;
        }

        public TestTodoListBuilder WithItem(
            string itemTitle,
            Importance importance,
            IdentityUser assignedUser = null,
            int rank = 0)
        {
            items.Add(new TestTodoItemProperties(itemTitle, importance, assignedUser, rank));
            return this;
        }

        public TodoList Build()
        {
            var todoList = new TodoList(owner, title);
            var todoItems = items.Select(itm => new TodoItem(todoList.TodoListId, itm.AssignedUser ?? owner, itm.Title, itm.Importance, itm.Rank));
            todoItems.ToList().ForEach(tlItm =>
            {
                todoList.Items.Add(tlItm);
                tlItm.TodoList = todoList;
            });
            return todoList;
        }
    }
}