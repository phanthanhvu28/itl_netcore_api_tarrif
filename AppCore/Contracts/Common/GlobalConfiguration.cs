

using Microsoft.Extensions.Configuration;

namespace AppCore.Contracts.Common
{
    public static class GlobalConfiguration
    {
        public static IConfiguration Configuration { get; private set; }

        public static IConfiguration Bind(IConfiguration configuration)
        {
            return Configuration ??= configuration;
        }
    }
}
