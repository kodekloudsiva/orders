using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orders.shared
{
    public interface IAllSettingsWrapper
    {
        AppSettings AppSettings { get; }
        DbSettings DbSettings { get; }
        //string ConnectionString { get; }
        //int DBTimeout { get; }
        //string CacheType { get; }
        //int SessionTimeout { get; }
    }
}
