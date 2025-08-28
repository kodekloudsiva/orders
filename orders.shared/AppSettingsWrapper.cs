using Microsoft.Extensions.Options;

namespace orders.shared
{
    public class AppSettingsWrapper : IAppSettingsWrapper
    {
        private readonly AppSettings settings;

        public AppSettingsWrapper(IOptions<AppSettings> options)
        {
            settings = options.Value;
        }
        public string CacheType => settings.CacheType;

        public int SessionTimeout => settings.SessionTimeout;
    }
}
