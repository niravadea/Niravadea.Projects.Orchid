using System;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum.Exceptions
{
    public class NoUserIdAnchorsAvailableException : UserSpecificException
    {
        public NoUserIdAnchorsAvailableException(string userName) : base(userName)
        {
        }

        public NoUserIdAnchorsAvailableException(int userId) : base(userId)
        {
        }
    }
}
