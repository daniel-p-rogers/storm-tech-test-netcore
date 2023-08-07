using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList, bool orderByRank = false)
        {
            var itemsQueryable = todoList.Items;

            var orderedItemsQueryable = orderByRank
                ? itemsQueryable.OrderBy(o => o.Rank)
                : itemsQueryable.OrderBy(t => t.Importance);
            
            var items = orderedItemsQueryable
                .Select(TodoItemSummaryViewmodelFactory.Create)
                .ToList();
            
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, orderByRank, items);
        }
    }
}