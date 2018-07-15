using ConversorDeMoedas.ACL.Interface;
using ConversorDeMoedas.ACL.Interface.Factory;
using ConversorDeMoedas.Domain.Interface.Factory;

namespace ConversorDeMoedas.ACL.Factory
{
    public class ConversorACLFactory : IConversorACLFactory
    {

        IMoedaFactory moedaFactory;

        public ConversorACLFactory(IMoedaFactory moedaFactory)
        {
            this.moedaFactory = moedaFactory;
        }
        public IConversorACL Create()
        {
            return new ConversorACL(moedaFactory);
        }


    }
}
