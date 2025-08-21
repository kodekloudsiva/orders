using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orders.shared
{
    public interface IAppSettingsWrapper
    {
        string CacheType { get; }
        int SessionTimeout { get; }
    }
}
