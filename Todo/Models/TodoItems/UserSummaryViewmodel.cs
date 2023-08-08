namespace Todo.Models.TodoItems
{
    public class UserSummaryViewmodel
    {
        public string UserName { get; }
        public string Email { get; }
        
        public string GravatarUserName { get; set; }

        public UserSummaryViewmodel(string userName, string email, string gravatarUserName)
        {
            UserName = userName;
            Email = email;
            GravatarUserName = gravatarUserName;
        }
    }
}