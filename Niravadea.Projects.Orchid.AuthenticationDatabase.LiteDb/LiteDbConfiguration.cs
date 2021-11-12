using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niravadea.Projects.Orchid.AuthenticationDatabase.LiteDb
{
    public class LiteDbConfiguration
        : DatabaseConfigurationBase
    {
        public string Filename { get; set; }
        public string Password { get; set; }
        public override string ConnectionString => $"Filename=\"{Filename}\"; Password=\"{Password}\"";
    }
}
