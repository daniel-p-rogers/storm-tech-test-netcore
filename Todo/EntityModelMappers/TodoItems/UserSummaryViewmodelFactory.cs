using Microsoft.AspNetCore.Identity;
using Todo.Models.TodoItems;

namespace Todo.EntityModelMappers.TodoItems
{
    public static class UserSummaryViewmodelFactory
    {
        public static UserSummaryViewmodel Create(IdentityUser identityUser, string gravatarUsername)
        {
            return new UserSummaryViewmodel(identityUser.UserName, identityUser.Email, gravatarUsername);
        }
    }
}