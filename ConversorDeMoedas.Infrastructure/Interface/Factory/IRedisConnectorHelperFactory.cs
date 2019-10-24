using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Infrastructure.Interface.Factory
{
    public interface IRedisConnectorHelperFactory
    {
        IRedisConnectorHelper Create();
    }
}
