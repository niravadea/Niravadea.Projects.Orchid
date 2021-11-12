using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.Core.Exceptions
{
    public class NonExistentUserException : UserSpecificException
    {
        public NonExistentUserException(string userName) : base(userName)
        {
        }

        public NonExistentUserException(int userId) : base(userId)
        {
        }
    }
}
