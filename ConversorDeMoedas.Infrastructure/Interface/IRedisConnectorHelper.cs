using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Infrastructure.Interface
{
    public interface IRedisConnectorHelper
    {
        T Get<T>(String cacheKey);
        String GetString(String cacheKey);
        byte[] Get(String cacheKey);
        void Set(String cacheKey, object cacheValue);
        void Set(String cacheKey, object cacheValue, Int32 TempoEmMinutos);
        void SetString(String cacheKey, String cacheValue);
        void SetString(String cacheKey, String cacheValue, Int32 TempoEmMinutos);
    }
}
