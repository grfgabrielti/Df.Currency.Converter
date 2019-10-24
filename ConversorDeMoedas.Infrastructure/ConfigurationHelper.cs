using ConversorDeMoedas.Infrastructure.Interface;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using Microsoft.Extensions.Configuration;


namespace ConversorDeMoedas.Infrastructure
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        IConfiguration configuration;
        public ConfigurationHelper(IConfiguration configuration)
        {
            this.configuration = configuration;

        }
        public String GetSection(String Key)
        {
            var result = configuration.GetSection(Key).Value;
            return result;
        }
    }
}