using System;

namespace Niravadea.Projects.Orchid.Client.Internal.Forum.Exceptions
{
    public class InconsistentUserIdException : UserSpecificException
    {
        public InconsistentUserIdException(string userName) : base(userName)
        {
        }

        public InconsistentUserIdException(int userId) : base(userId)
        {
        }
    }
}
