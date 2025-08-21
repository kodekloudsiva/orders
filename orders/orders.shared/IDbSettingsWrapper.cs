using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orders.shared
{
    public interface IDbSettingsWrapper
    {
        string ConnectionString { get; }
        int DBTimeout { get; }
    }
}
