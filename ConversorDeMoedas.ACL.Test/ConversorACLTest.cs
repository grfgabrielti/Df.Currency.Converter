using ConversorDeMoedas.ACL.Interface;
using System;
using Xunit;
using ConversorDeMoedas.ACL;
using ConversorDeMoedas.Domain.Factory;
using ConversorDeMoedas.Domain.Interface;
using System.Collections.Generic;
using ConversorDeMoedas.Infrastructure.Factory;
using Microsoft.Extensions.Caching.Distributed;
using ConversorDeMoedas.Infrastructure.Interface.Factory;
using Moq;
using ConversorDeMoedas.Domain;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ConversorDeMoedas.Infrastructure.Interface;
using Microsoft.Extensions.Configuration;

namespace ConversorDeMoedas.ACL.Test
{
    public class ConversorACLTest
    {

        [Fact]
        public void TestQueDeveRetornarListaComAsMoedasSemValorSomenteSiglaENome()
        {
            List<IMoeda> mcklist = new List<IMoeda>();
            byte[] resultmockNull = null;
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            Mock<IConfigurationHelper> mckconfigurationHelper = new Mock<IConfigurationHelper>();
            mckconfigurationHelper.Setup(x => x.GetSection("ACCESS_KEY")).Returns("?access_key=1503440cbd4d453ce74962abd00a82c2");
            mckconfigurationHelper.Setup(x => x.GetSection("BASE_URL")).Returns("http://apilayer.net/api/");
            mckcache.Setup(x => x.Get("GetMoedasList")).Returns(resultmockNull);

            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            mckcache.Setup(x => x.Set("GetMoedasList", Serialize(mcklist), distributedCacheEntryOptions));
            IConversorACL conversorACL = new ConversorACL(new MoedaFactory(), new RedisConnectorHelperFactory(mckcache.Object),mckconfigurationHelper.Object);
            List<IMoeda> result = conversorACL.GetMoedas();
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void TestQueRetornaACotacaoDaMoedaDesejadaComBaseNoDolarSemInformacoesEmCache()
        {
            IMoeda MoedaDolarMck = new Moeda("USD", 1);
            byte[] resultmockNull = null;
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            Mock<IConfigurationHelper> mckconfigurationHelper = new Mock<IConfigurationHelper>();
            mckconfigurationHelper.Setup(x => x.GetSection("ACCESS_KEY")).Returns("?access_key=1503440cbd4d453ce74962abd00a82c2");
            mckconfigurationHelper.Setup(x => x.GetSection("BASE_URL")).Returns("http://apilayer.net/api/");

            mckcache.Setup(x => x.Get("GetCotacaoComBaseNoDolarUSD")).Returns(resultmockNull);
            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            mckcache.Setup(x => x.Set("GetCotacaoComBaseNoDolarUSD", Serialize(MoedaDolarMck), distributedCacheEntryOptions));
            IConversorACL conversorACL = new ConversorACL(new MoedaFactory(), new RedisConnectorHelperFactory(mckcache.Object),mckconfigurationHelper.Object);
            IMoeda DolarResult = conversorACL.GetCotacaoComBaseNoDolar(MoedaDolarMck.SiglaMoeda);
            Assert.True(DolarResult.Valor.Equals(1));

            String OutraMoedaQueNaoTemValorDoDolar = "BRL";
            mckcache.Setup(x => x.Get("GetCotacaoComBaseNoDolarBRL")).Returns(resultmockNull);
            mckcache.Setup(x => x.Set("GetCotacaoComBaseNoDolarBRL", Serialize(MoedaDolarMck), distributedCacheEntryOptions));

            IMoeda RealResult = conversorACL.GetCotacaoComBaseNoDolar(OutraMoedaQueNaoTemValorDoDolar);
            Assert.False(RealResult.Valor.Equals(DolarResult.Valor));
        }
        [Fact]
        public void TestQueRetornaACotacaoDaMoedaDesejadaComBaseNoDolarComInformacoesEmCache()
        {
            IMoeda MoedaDolar = new Moeda("USD", 1);
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            Mock<IConfigurationHelper> mckConfigurationHelper = new Mock<IConfigurationHelper>();

            RedisConnectorHelperFactory redisHelperFactory = new RedisConnectorHelperFactory(mckcache.Object);
            IRedisConnectorHelper redisHelper = redisHelperFactory.Create();
            mckcache.Setup(x => x.Get("GetCotacaoComBaseNoDolarUSD")).Returns(Serialize(MoedaDolar));

            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            mckcache.Setup(x => x.Set("GetCotacaoComBaseNoDolarUSD", Serialize(MoedaDolar), distributedCacheEntryOptions));

            IConversorACL conversorACL = new ConversorACL(new MoedaFactory(), new RedisConnectorHelperFactory(mckcache.Object),mckConfigurationHelper.Object);
            IMoeda DolarResult = conversorACL.GetCotacaoComBaseNoDolar(MoedaDolar.SiglaMoeda);
            Assert.True(DolarResult.Valor.Equals(1));
            IMoeda MoedaBRL = new Moeda("BRL", 3.86M);
            mckcache.Setup(x => x.Get("GetCotacaoComBaseNoDolarBRL")).Returns(Serialize(MoedaBRL));
            mckcache.Setup(x => x.Set("GetCotacaoComBaseNoDolarBRL", Serialize(MoedaBRL), distributedCacheEntryOptions));

            IMoeda RealResult = conversorACL.GetCotacaoComBaseNoDolar(MoedaBRL.SiglaMoeda);
            Assert.False(RealResult.Valor.Equals(DolarResult.Valor));
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
