using ConversorDeMoedas.ACL.Interface.Factory;
using ConversorDeMoedas.Domain.Interface.Factory;
using ConversorDeMoedas.Services.Interface;
using ConversorDeMoedas.Services.Interface.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Services.Factory
{
    public class ConversorServiceFactory : IConversorServiceFactory
    {
        IConversorACLFactory conversorACLFactory;
        IMoedaFactory moedaFactory;
        public ConversorServiceFactory(IConversorACLFactory conversorACLFactory, IMoedaFactory moedaFactory)
        {
            this.conversorACLFactory = conversorACLFactory;
            this.moedaFactory = moedaFactory;
        }

        public IConversorService Create()
        {
            return new ConversorService(conversorACLFactory, moedaFactory);
        }
    }
}
