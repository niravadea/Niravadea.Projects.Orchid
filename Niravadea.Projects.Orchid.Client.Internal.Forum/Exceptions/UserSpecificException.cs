using Niravadea.Projects.Orchid.Client.Internal.Forum;
using System;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum.Exceptions
{
    public class UserSpecificException : Exception
    {
        private readonly string displayName;
        private readonly AuthType authenticationType;

        public string UserValue { get; init; }
        public AuthType UserValueType { get; init; }

        public UserSpecificException(string userName)
        {
            UserValueType = AuthType.ByUserName;
            UserValue = userName;
        }

        public UserSpecificException(int userId)
        {
            UserValueType = AuthType.ByUserId;
            UserValue = userId.ToString();
        }
    }
}
