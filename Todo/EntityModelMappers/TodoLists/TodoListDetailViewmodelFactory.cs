using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoLists;
using Todo.Services;

namespace Todo.EntityModelMappers.TodoLists
{
    public class TodoListDetailViewmodelFactory
    {
        private readonly IGravatarService _gravatarService;

        public TodoListDetailViewmodelFactory(IGravatarService gravatarService)
        {
            _gravatarService = gravatarService ?? throw new ArgumentNullException(nameof(gravatarService));
        }
        
        public async Task<TodoListDetailViewmodel> Create(TodoList todoList, bool orderByRank = false)
        {
            var unprojectedItems = todoList.Items
                .OrderTodoListBy(orderByRank)
                .ToList();

            var distinctUserEmails = unprojectedItems.Select(o => o.ResponsibleParty?.Email)
                .Where(o => !string.IsNullOrWhiteSpace(o))
                .Distinct()
                .ToList();

            var gravatarUsernameDictionary = new Dictionary<string, string>();
            
            foreach (var email in distinctUserEmails)
            {
                var gravatarUsername = await _gravatarService.GetGravatarUsernameFromEmailAddress(email);
                gravatarUsernameDictionary.Add(email, gravatarUsername);
            }

            var items = unprojectedItems
                .Select(o => TodoItemSummaryViewmodelFactory.Create(o, gravatarUsernameDictionary))
                .ToList();
            
            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, orderByRank, items);
        }
    }
}