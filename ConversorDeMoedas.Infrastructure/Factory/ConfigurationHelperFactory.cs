using System;
using ConversorDeMoedas.Infrastructure.Interface;
using ConversorDeMoedas.Infrastructure.Interface.Factory;
using Microsoft.Extensions.Configuration;

namespace ConversorDeMoedas.Infrastructure.Factory                           
{
    public class ConfigurationHelperFactory :IConfigurationHelperFactory
    {

        IConfiguration configuration;

        public ConfigurationHelperFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public IConfigurationHelper Create()
        {
            return new ConfigurationHelper(configuration);

        }
    }
}
