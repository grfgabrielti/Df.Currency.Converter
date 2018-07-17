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

namespace ConversorDeMoedas.ACL.Test
{
    public class ConversorACLTest
    {
        private readonly IDistributedCache _cache;


    
        [Fact]
        public void TestQueDeveRetornarListaComAsMoedasSemValorSomenteSiglaENome()
        {

            IConversorACL conversorACL = new ConversorACL(new MoedaFactory(), new RedisConnectorHelperFactory(_cache));
            List<IMoeda> result = conversorACL.GetMoedas();
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void TestQueRetornaACotacaoDaMoedaDesejadaComBaseNoDolar()
        {
            //Mock<IDistributedCache> cache = new Mock<IDistributedCache>();

            //String SiglaMoedaDesejada = "USD";

            //IConversorACL conversorACL = new ConversorACL(new MoedaFactory(), new RedisConnectorHelperFactory());
            //IMoeda DolarResult = conversorACL.GetCotacaoComBaseNoDolar(SiglaMoedaDesejada);
            //Assert.True(DolarResult.Valor.Equals(1));

            //String OutraMoedaQueNaoTemValorDoDolar = "BRL";
            //IMoeda RealResult = conversorACL.GetCotacaoComBaseNoDolar(OutraMoedaQueNaoTemValorDoDolar);
            //Assert.False(RealResult.Valor.Equals(DolarResult.Valor));
        }
    }
}
