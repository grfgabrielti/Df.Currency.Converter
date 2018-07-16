using ConversorDeMoedas.Services.Interface;
using System;
using Xunit;
using Moq;
using ConversorDeMoedas.ACL.Interface.Factory;
using ConversorDeMoedas.ACL.Interface;
using ConversorDeMoedas.Domain.Interface.Factory;
using System.Collections.Generic;
using ConversorDeMoedas.Domain.Interface;
using ConversorDeMoedas.Domain;
using ConversorDeMoedas.ACL.Factory;
using ConversorDeMoedas.Domain.Factory;
using ConversorDeMoedas.Services.Request;

namespace ConversorDeMoedas.Services.Test
{
    public class ConversorServiceTest
    {
        [Fact]
        public void TesteListarMoedas()
        {
            List<IMoeda> result = new List<IMoeda>();
            result.Add(new Moeda("GABRIEL", 10));
            result.Add(new Moeda("VIADO", 10));

            Mock<IMoedaFactory> moedaFactoryMock = new Mock<IMoedaFactory>();
            Mock<IConversorACL> aclMck = new Mock<IConversorACL>();
            Mock<IConversorACLFactory> aclFactoryMck = new Mock<IConversorACLFactory>();

            aclMck.Setup(x => x.GetMoedas()).Returns(result);
            aclFactoryMck.Setup(x => x.Create()).Returns(aclMck.Object);

            IConversorService service = new ConversorService(aclFactoryMck.Object, moedaFactoryMock.Object);

            List<IMoeda> retorno = service.GetMoedas();

            Assert.True(retorno.Count == 2);

            aclMck.VerifyAll();
            aclFactoryMck.VerifyAll();
        }


        [Fact]
        public void TestListarMoedasUsandoAAPIExterna()
        {
            IConversorService service = new ConversorService(new ConversorACLFactory(new MoedaFactory()), new MoedaFactory());

            List<IMoeda> result = service.GetMoedas();

            Assert.True(result.Count > 0);
        }

        [Fact]
        public void TestDeConversaoDeMoeda()
        {

            Mock<IMoedaFactory> moedaFactoryMock = new Mock<IMoedaFactory>();
            Mock<IConversorACL> aclMck = new Mock<IConversorACL>();
            Mock<IConversorACLFactory> aclFactoryMck = new Mock<IConversorACLFactory>();


            ConverterMoedaRequest request = new ConverterMoedaRequest()
            {
                SiglaMoedaOrigem = "BRL",
                MoedaParaConversao = "USD",
                ValorParaConversao = 1M
            };
            IMoeda moedaOrigem = new Moeda(request.SiglaMoedaOrigem, request.ValorParaConversao);

            IMoeda CotacaoDodolarDaMoedaOrigem = new Moeda("BRL", 3.84M);
            aclMck.Setup(x => x.GetCotacaoComBaseNoDolar(moedaOrigem.SiglaMoeda)).Returns(CotacaoDodolarDaMoedaOrigem);

            IMoeda dinhieroOrigemEmDolar = moedaOrigem.ConverterParaDolar(CotacaoDodolarDaMoedaOrigem);

            IMoeda CotacaoEmDolarMoedaConvertida = new Moeda("USD", 1);
            aclMck.Setup(x => x.GetCotacaoComBaseNoDolar(request.MoedaParaConversao)).Returns(CotacaoEmDolarMoedaConvertida);


            IMoeda rsultmock = new Moeda(request.MoedaParaConversao, dinhieroOrigemEmDolar.ObterValorDaConversaoDeMoeda(CotacaoEmDolarMoedaConvertida));


            aclFactoryMck.Setup(x => x.Create()).Returns(aclMck.Object);
            IConversorService service = new ConversorService(aclFactoryMck.Object, moedaFactoryMock.Object);
            var result = service.ConverterMoeda(request);

            Assert.True(rsultmock.Equals(result));

        }

    }
}
