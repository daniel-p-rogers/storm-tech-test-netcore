using System.Threading.Tasks;

namespace Todo.Services
{
    public interface IGravatarService
    {
        Task<string> GetGravatarUsernameFromEmailAddress(string emailAddress);
    }
}