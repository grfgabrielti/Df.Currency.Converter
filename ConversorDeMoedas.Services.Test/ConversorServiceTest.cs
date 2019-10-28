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
using ConversorDeMoedas.Infrastructure;
using ConversorDeMoedas.Infrastructure.Interface;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Microsoft.Extensions.Configuration;

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
            Mock<IConfigurationHelper> mckconfigurationHelper = new Mock<IConfigurationHelper>();
            mckconfigurationHelper.Setup(x => x.GetSection("ACCESS_KEY")).Returns("?access_key=000baa3e87af3e0b078a3bfbb6249876");
            mckconfigurationHelper.Setup(x => x.GetSection("BASE_URL")).Returns("http://apilayer.net/api/");

            IConversorService service = new ConversorService(new ConversorACLFactory(new MoedaFactory(), new RedisConnectorHelperFactory(mckcache.Object),mckconfigurationHelper.Object), new MoedaFactory());

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
        public void TestDeConversaoDeMoedaAPIExternaSemCotacaoNoCache()
        {
            byte[] resultmockNull = null;
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            Mock<IConfigurationHelper> mckconfigurationHelper = new Mock<IConfigurationHelper>();

            mckcache.Setup(x => x.Get("GetCotacaoComBaseNoDolarBRL")).Returns(resultmockNull);
            mckcache.Setup(x => x.Get("GetCotacaoComBaseNoDolarUSD")).Returns(resultmockNull);
            mckconfigurationHelper.Setup(x => x.GetSection("ACCESS_KEY")).Returns("?access_key=000baa3e87af3e0b078a3bfbb6249876");
            mckconfigurationHelper.Setup(x => x.GetSection("BASE_URL")).Returns("http://apilayer.net/api/");
                            

            IConversorService service = new ConversorService(new ConversorACLFactory(new MoedaFactory(), new RedisConnectorHelperFactory(mckcache.Object),mckconfigurationHelper.Object), new MoedaFactory());
            ConverterMoedaRequest request = new ConverterMoedaRequest()
            {
                SiglaMoedaOrigem = "BRL",
                MoedaParaConversao = "USD",
                ValorParaConversao = 1M
            };

            var result = service.ConverterMoeda(request);
 
            Assert.True(result != null);
            Assert.True(result.valor > 0 || result.valor <   0);
        }
        [Fact]
        public void TestDeConversaoDeMoedaAPIExternaComCotacaoNoCache()
        {
            IMoeda MoedaDolar = new Moeda("USD", 1);
            IMoeda MoedaReal = new Moeda("BRL", 3.85M);
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            Mock<IConfigurationHelper> mckconfigurationHelper = new Mock<IConfigurationHelper>();
            mckconfigurationHelper.Setup(x => x.GetSection("ACCESS_KEY")).Returns("?access_key=1503440cbd4d453ce74962abd00a82c2");
            mckconfigurationHelper.Setup(x => x.GetSection("BASE_URL")).Returns("http://apilayer.net/api/");

            RedisConnectorHelperFactory redisHelperFactory = new RedisConnectorHelperFactory(mckcache.Object);
            IRedisConnectorHelper redisHelper = redisHelperFactory.Create();

            mckcache.Setup(x => x.Get("GetCotacaoComBaseNoDolarBRL")).Returns(Serialize(MoedaReal));
            mckcache.Setup(x => x.Get("GetCotacaoComBaseNoDolarUSD")).Returns(Serialize(MoedaDolar));
            IConversorService service = new ConversorService(new ConversorACLFactory(new MoedaFactory(), new RedisConnectorHelperFactory(mckcache.Object),mckconfigurationHelper.Object), new MoedaFactory());
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
        private static byte[] Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            BinaryFormatter objBinaryFormatter = new BinaryFormatter();
            using (MemoryStream objMemoryStream = new MemoryStream())
            {
                objBinaryFormatter.Serialize(objMemoryStream, obj);
                byte[] objDataAsByte = objMemoryStream.ToArray();
                return objDataAsByte;
            }
        }


    }
}
