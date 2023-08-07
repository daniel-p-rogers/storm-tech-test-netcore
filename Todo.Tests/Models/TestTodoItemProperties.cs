using Microsoft.AspNetCore.Identity;
using Todo.Data.Entities;

namespace Todo.Tests.Models
{
    public record TestTodoItemProperties(string Title, Importance Importance, IdentityUser AssignedUser, int Rank);
}