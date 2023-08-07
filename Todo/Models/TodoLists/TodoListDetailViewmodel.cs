using System.Collections.Generic;
using Todo.Models.TodoItems;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }
        public ICollection<TodoItemSummaryViewmodel> Items { get; }
        public bool OrderByRank { get; set; }

        public TodoListDetailViewmodel(int todoListId, string title, bool orderByRank, ICollection<TodoItemSummaryViewmodel> items)
        {
            Items = items;
            TodoListId = todoListId;
            OrderByRank = orderByRank;
            Title = title;
        }
    }
}