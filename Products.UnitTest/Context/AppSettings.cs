using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.UnitTest.Context
{
    public class AppSettings
    {
        public IConfigurationRoot Config { get; private set; }

        public AppSettings()
        {
            Config = GetAppSettings();
        }

        /// <summary>
        /// Get appsettings.json from main project
        /// </summary>
        /// <returns></returns>
        public static IConfigurationRoot GetAppSettings()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            return config;
        }
    }
}
