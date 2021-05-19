using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.UnitTests.Context
{
    public class AppSettings
    {
        public IConfigurationRoot Config { get; private set; }

        public AppSettings()
        {
            Config = GetAppSettings();
        }

        public IConfigurationRoot GetAppSettings()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            return config;
        }
    }
}
