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
            mckcache.Setup(x => x.Get("GetMoedasList")).Returns(resultmockNull);

            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            mckcache.Setup(x => x.Set("GetMoedasList", Serialize(mcklist), distributedCacheEntryOptions));
            IConversorACL conversorACL = new ConversorACL(new MoedaFactory(), new RedisConnectorHelperFactory(mckcache.Object));
            List<IMoeda> result = conversorACL.GetMoedas();
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void TestQueRetornaACotacaoDaMoedaDesejadaComBaseNoDolar()
        {

            String SiglaMoedaDesejada = "USD";

            IMoeda mckMoeda = new Moeda("USD", 1);
            byte[] resultmockNull = null;
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            mckcache.Setup(x => x.Get("GetCotacaoComBaseNoDolarUSD")).Returns(resultmockNull);
            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            mckcache.Setup(x => x.Set("GetCotacaoComBaseNoDolarUSD", Serialize(mckMoeda), distributedCacheEntryOptions));
            IConversorACL conversorACL = new ConversorACL(new MoedaFactory(), new RedisConnectorHelperFactory(mckcache.Object));
            IMoeda DolarResult = conversorACL.GetCotacaoComBaseNoDolar(SiglaMoedaDesejada);
            Assert.True(DolarResult.Valor.Equals(1));

            String OutraMoedaQueNaoTemValorDoDolar = "BRL";
            mckcache.Setup(x => x.Get("GetCotacaoComBaseNoDolarBRL")).Returns(resultmockNull);
            mckcache.Setup(x => x.Set("GetCotacaoComBaseNoDolarBRL", Serialize(mckMoeda), distributedCacheEntryOptions));

            IMoeda RealResult = conversorACL.GetCotacaoComBaseNoDolar(OutraMoedaQueNaoTemValorDoDolar);
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
