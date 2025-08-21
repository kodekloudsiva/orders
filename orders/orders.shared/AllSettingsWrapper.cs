using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orders.shared
{
    public class AllSettingsWrapper : IAllSettingsWrapper
    {
        private AllSettings _settings;

        public AllSettingsWrapper(IOptions<AllSettings> options)
        {
            _settings = options.Value;

            // Debug output
            Console.WriteLine("=== Loaded Settings ===");
            Console.WriteLine($"Cache: {_settings?.AppSettings?.CacheType}");
            Console.WriteLine($"ConnectionString: {_settings?.DbSettings?.ConnectionString}");
        }
        public AppSettings AppSettings => _settings.AppSettings;
        public DbSettings DbSettings => _settings.DbSettings;

    }
}
