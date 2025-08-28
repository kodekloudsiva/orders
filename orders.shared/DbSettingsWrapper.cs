using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orders.shared
{
    public class DbSettingsWrapper : IDbSettingsWrapper
    {
        private DbSettings settings;
        public DbSettingsWrapper(IOptions<DbSettings> options)
        {
            settings = options.Value;   
        }
        public string ConnectionString => settings.ConnectionString;

        public int DBTimeout => settings.DBTimeout;
    }
}
