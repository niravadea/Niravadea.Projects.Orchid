using DSharpPlus.SlashCommands;

namespace Niravadea.Projects.Orchid.Client.External.Discord
{
    public enum AuthType
    {
        [ChoiceName("Authentication by user ID")]
        ByUserId,
        [ChoiceName("Authentication by username")]
        ByUserName
    }
}
