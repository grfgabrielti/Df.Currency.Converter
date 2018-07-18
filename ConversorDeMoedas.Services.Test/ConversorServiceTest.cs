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
using Microsoft.Extensions.Caching.Distributed;
using ConversorDeMoedas.Infrastructure.Factory;

namespace ConversorDeMoedas.Services.Test
{
    public class ConversorServiceTest
    {
        [Fact]
        public void TesteListarMoedas()
        {
            List<IMoeda> result = new List<IMoeda>();
            result.Add(new Moeda("USD", 10));
            result.Add(new Moeda("BRL", 10));

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
            byte[] resultmockNull = null;
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            mckcache.Setup(x => x.Get("GetMoedasList")).Returns(resultmockNull);

            IConversorService service = new ConversorService(new ConversorACLFactory(new MoedaFactory(), new RedisConnectorHelperFactory(mckcache.Object)), new MoedaFactory());

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

            aclMck.Setup(x => x.GetCotacaoComBaseNoDolar(request.SiglaMoedaOrigem)).Returns(new Moeda("BRL", 3.84M));
            aclMck.Setup(x => x.GetCotacaoComBaseNoDolar(request.MoedaParaConversao)).Returns(new Moeda("USD", 1));

            aclFactoryMck.Setup(x => x.Create()).Returns(aclMck.Object);
            moedaFactoryMock.Setup(x => x.Create("BRL", 1m)).Returns(moedaOrigem);
            IMoeda MoedaParaConversao = new Moeda("USD", 0.25M);
            moedaFactoryMock.Setup(x => x.Create("USD", It.IsAny<Decimal>())).Returns(MoedaParaConversao);
            IConversorService service = new ConversorService(aclFactoryMck.Object, moedaFactoryMock.Object);
            var result = service.ConverterMoeda(request);

            Assert.True(result != null);
            Assert.True(result.valor.Equals(MoedaParaConversao.Valor));

            aclMck.VerifyAll();
            aclFactoryMck.VerifyAll();
        }

        [Fact]
        public void TestDeConversaoDeMoedaAPIExterna()
        {
            byte[] resultmockNull = null;
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            mckcache.Setup(x => x.Get("GetCotacaoComBaseNoDolarBRL")).Returns(resultmockNull);
            mckcache.Setup(x => x.Get("GetCotacaoComBaseNoDolarUSD")).Returns(resultmockNull);

            IConversorService service = new ConversorService(new ConversorACLFactory(new MoedaFactory(),new RedisConnectorHelperFactory(mckcache.Object)), new MoedaFactory());
            ConverterMoedaRequest request = new ConverterMoedaRequest()
            {
                SiglaMoedaOrigem = "BRL",
                MoedaParaConversao = "USD",
                ValorParaConversao = 1M
            };

            var result = service.ConverterMoeda(request);

            Assert.True(result != null);
            Assert.True(result.valor > 0 || result.valor < 0);
        }
    }
}
