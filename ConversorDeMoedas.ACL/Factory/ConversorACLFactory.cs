using ConversorDeMoedas.ACL.Interface;
using ConversorDeMoedas.ACL.Interface.Factory;
using ConversorDeMoedas.Domain.Interface.Factory;
using ConversorDeMoedas.Infrastructure.Interface.Factory;
using Microsoft.Extensions.Configuration;

namespace ConversorDeMoedas.ACL.Factory
{
    public class ConversorACLFactory : IConversorACLFactory
    {

        IMoedaFactory moedaFactory;
        IRedisConnectorHelperFactory redisConnectorHelperFactory;
        IConfiguration Configuration;

        public ConversorACLFactory(IMoedaFactory moedaFactory, IRedisConnectorHelperFactory redisConnectorHelperFactory,IConfiguration Configuration)
        {
            this.moedaFactory = moedaFactory;
            this.redisConnectorHelperFactory = redisConnectorHelperFactory;
            this.Configuration = Configuration;
        }
        public IConversorACL Create()
        {
            return new ConversorACL(moedaFactory, redisConnectorHelperFactory,Configuration);
        }


    }
}
