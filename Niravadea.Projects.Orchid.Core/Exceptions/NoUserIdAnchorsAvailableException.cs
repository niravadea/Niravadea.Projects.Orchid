using System;

namespace Niravadea.Projects.Orchid.Core.Exceptions
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
