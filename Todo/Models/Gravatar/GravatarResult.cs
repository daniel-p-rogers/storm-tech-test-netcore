using System.Collections.Generic;

namespace Todo.Models.Gravatar
{
    public record GravatarResult(ICollection<GravatarResultEntry> entry);

    public record GravatarResultEntry(string displayName);
}