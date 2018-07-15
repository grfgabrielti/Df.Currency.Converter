using ConversorDeMoedas.ACL.Interface;
using ConversorDeMoedas.ACL.Interface.Factory;
using ConversorDeMoedas.Domain;
using ConversorDeMoedas.Domain.Interface;
using ConversorDeMoedas.Domain.Interface.Factory;
using ConversorDeMoedas.Services.Interface;
using System;
using System.Collections.Generic;

namespace ConversorDeMoedas.Services
{
    public class ConversorService : IConversorService
    {
        IConversorACLFactory conversorACLFactory;
        IMoedaFactory moedaFactory;
        public ConversorService(IConversorACLFactory conversorACLFactory, IMoedaFactory moedaFactory)
        {
            this.conversorACLFactory = conversorACLFactory;
            this.moedaFactory = moedaFactory;
        }

        public List<IMoeda> GetMoedas()
        {
            IConversorACL conversorACL = conversorACLFactory.Create();
            return conversorACL.GetMoedas();
        }

        public IMoeda ConverterMoeda(String MoedaOrigemSiglas, String MoedaParaConversao, Decimal ValorParaConversao )
        {
            Decimal Resultado = 00.00M;
            IConversorACL conversorACL = conversorACLFactory.Create();

            IMoeda MoedaOrigem = moedaFactory.Create(MoedaOrigemSiglas, ValorParaConversao);
            IMoeda CotacaoEmDolarMoedaOrigem = conversorACL.GetCotacaoComBaseNoDolar(MoedaOrigem.SiglaMoeda);
            IMoeda dinhieroOrigemEmDolar = MoedaOrigem.ConverterParaDolar(CotacaoEmDolarMoedaOrigem);

            IMoeda DinheiroResultado = moedaFactory.Create(MoedaParaConversao, Resultado);
            IMoeda CotacaoEmDolarMoedaConvertida = conversorACL.GetCotacaoComBaseNoDolar(MoedaParaConversao);

            return moedaFactory.Create(MoedaParaConversao, dinhieroOrigemEmDolar.ObterValorDaConversaoDeMoeda(CotacaoEmDolarMoedaConvertida));
        }
    }
}
