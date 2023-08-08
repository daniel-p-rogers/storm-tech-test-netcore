using System.Collections.Generic;
using System.Linq;
using Todo.Data.Entities;
using Todo.Models.TodoItems;

namespace Todo.EntityModelMappers.TodoItems
{
    public static class TodoItemSummaryViewmodelFactory
    {
        public static TodoItemSummaryViewmodel Create(TodoItem ti, IDictionary<string, string> gravatarUsernameDictionary)
        {
            var gravatarUsername = !string.IsNullOrWhiteSpace(ti.ResponsibleParty?.Email) && gravatarUsernameDictionary.ContainsKey(ti.ResponsibleParty?.Email)
                ? gravatarUsernameDictionary[ti.ResponsibleParty?.Email]
                : null;
            
            return new TodoItemSummaryViewmodel(ti.TodoItemId, ti.Title, ti.IsDone, UserSummaryViewmodelFactory.Create(ti.ResponsibleParty, gravatarUsername), ti.Importance);
        }

        public static IOrderedEnumerable<TodoItem> OrderTodoListBy(this IEnumerable<TodoItem> listItems, bool orderByRank)
        {
            return orderByRank
                ? listItems.OrderBy(o => o.Rank)
                : listItems.OrderBy(o => o.Importance);
        }
    }
}