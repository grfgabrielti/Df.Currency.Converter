using ConversorDeMoedas.Infrastructure.Interface;
using ConversorDeMoedas.Infrastructure.Interface.Factory;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Infrastructure.Factory
{
    public class RedisConnectorHelperFactory : IRedisConnectorHelperFactory
    {
        IDistributedCache cache;

        public RedisConnectorHelperFactory(IDistributedCache cache)
        {
            this.cache = cache;
        }
        public IRedisConnectorHelper Create()
        {
            return new RedisConnectorHelper(cache);
        }
    }
}
