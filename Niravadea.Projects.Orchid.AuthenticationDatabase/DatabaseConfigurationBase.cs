using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.AuthenticationDatabase
{
    public abstract class DatabaseConfigurationBase
    {
        public abstract string ConnectionString { get; }
    }
}
