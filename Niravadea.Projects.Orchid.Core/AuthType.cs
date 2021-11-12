using DSharpPlus.SlashCommands;

namespace Niravadea.Projects.Orchid.Core
{
    public enum AuthType
    {
        [ChoiceName("Authentication by user ID")]
        ByUserId,
        [ChoiceName("Authentication by username")]
        ByUserName
    }
}
