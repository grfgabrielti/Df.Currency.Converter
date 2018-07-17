using ConversorDeMoedas.ACL.Interface;
using ConversorDeMoedas.ACL.Interface.Factory;
using ConversorDeMoedas.Domain.Interface.Factory;
using ConversorDeMoedas.Infrastructure.Interface.Factory;

namespace ConversorDeMoedas.ACL.Factory
{
    public class ConversorACLFactory : IConversorACLFactory
    {

        IMoedaFactory moedaFactory;
        IRedisConnectorHelperFactory redisConnectorHelperFactory;

        public ConversorACLFactory(IMoedaFactory moedaFactory, IRedisConnectorHelperFactory redisConnectorHelperFactory)
        {
            this.moedaFactory = moedaFactory;
            this.redisConnectorHelperFactory = redisConnectorHelperFactory;
        }
        public IConversorACL Create()
        {
            return new ConversorACL(moedaFactory, redisConnectorHelperFactory);
        }


    }
}
