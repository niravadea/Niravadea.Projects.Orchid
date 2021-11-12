using System;

namespace Niravadea.Projects.Orchid.Core.Exceptions
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
