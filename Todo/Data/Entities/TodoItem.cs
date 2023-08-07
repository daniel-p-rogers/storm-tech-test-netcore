﻿using Microsoft.AspNetCore.Identity;

namespace Todo.Data.Entities {
    public class TodoItem
    {
        public int TodoItemId { get; set; }
        public string Title { get; set; }
        public string ResponsiblePartyId { get; set; }
        public IdentityUser ResponsibleParty { get; set; }
        public bool IsDone { get; set; }
        public Importance Importance { get; set; }

        public int TodoListId { get; set; }
        public TodoList TodoList { get; set; }

        public int Rank { get; set; } = 0;

        protected TodoItem() { }

        public TodoItem(int todoListId, string responsiblePartyId, string title, Importance importance)
        {
            TodoListId = todoListId;
            ResponsiblePartyId = responsiblePartyId;
            Title = title;
            Importance = importance;
        }

        public TodoItem(int todoListId, IdentityUser responsibleParty, string title, Importance importance)
        : this (todoListId, responsibleParty?.Id, title, importance)
        {
            ResponsibleParty = responsibleParty;
        }
    }
}