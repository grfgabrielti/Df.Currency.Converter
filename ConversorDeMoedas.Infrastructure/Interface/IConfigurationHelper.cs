using ConversorDeMoedas.Infrastructure.Interface;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;

namespace ConversorDeMoedas.Infrastructure.Interface
{
    public interface IConfigurationHelper 
    {
        String GetSection(String Key);
        
    }
}