using ConversorDeMoedas.ACL.Interface;
using ConversorDeMoedas.ACL.Interface.Factory;
using ConversorDeMoedas.Domain;
using ConversorDeMoedas.Domain.Interface;
using ConversorDeMoedas.Domain.Interface.Factory;
using ConversorDeMoedas.Infrastructure.Interface;
using ConversorDeMoedas.Infrastructure.Interface.Factory;
using ConversorDeMoedas.Services.Interface;
using ConversorDeMoedas.Services.Request;
using ConversorDeMoedas.Services.Request.Result;
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
            List<IMoeda> retorno = conversorACL.GetMoedas();
            return retorno;
        }

        public ConverterMoedaResult ConverterMoeda(ConverterMoedaRequest converterMoedaRequest)
        {
            IConversorACL conversorACL = conversorACLFactory.Create();

            IMoeda MoedaOrigem = moedaFactory.Create(converterMoedaRequest.SiglaMoedaOrigem, converterMoedaRequest.ValorParaConversao);
            IMoeda CotacaoEmDolarMoedaOrigem = conversorACL.GetCotacaoComBaseNoDolar(MoedaOrigem.SiglaMoeda);
            IMoeda dinhieroOrigemEmDolar = MoedaOrigem.ConverterParaDolar(CotacaoEmDolarMoedaOrigem);

            IMoeda CotacaoEmDolarMoedaConvertida = conversorACL.GetCotacaoComBaseNoDolar(converterMoedaRequest.MoedaParaConversao);
            Decimal valorDaConversao = dinhieroOrigemEmDolar.ObterValorDaConversaoDeMoeda(CotacaoEmDolarMoedaConvertida);
            IMoeda MoedaConvertida = moedaFactory.Create(converterMoedaRequest.MoedaParaConversao, valorDaConversao);
            ConverterMoedaResult result = new ConverterMoedaResult()
            {
                siglaMoeda = MoedaConvertida.SiglaMoeda,
                valor = MoedaConvertida.Valor
            };
            return result;
        }


        public string TestRedis(string key)
        {
            IConversorACL conversorACL = conversorACLFactory.Create();

            return conversorACL.GetRedis(key);
        }

        public void TestSetRedis()
        {
            IConversorACL conversorACL = conversorACLFactory.Create();

            conversorACL.SetRedis();
        }
    }
}