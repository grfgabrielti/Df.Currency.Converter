using ConversorDeMoedas.ACL.Interface;
using System;
using Xunit;
using ConversorDeMoedas.ACL;
using ConversorDeMoedas.Domain.Factory;
using ConversorDeMoedas.Domain.Interface;
using System.Collections.Generic;

namespace ConversorDeMoedas.ACL.Test
{
    public class ConversorACLTest
    {
        [Fact]
        public void TestQueDeveRetornarListaComAsMoedasSemValorSomenteSiglaENome()
        {
            IConversorACL conversorACL = new ConversorACL(new MoedaFactory());
            List<IMoeda> result = conversorACL.GetMoedas();
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void TestQueRetornaACotacaoDaMoedaDesejadaComBaseNoDolar()
        {
            String SiglaMoedaDesejada = "USD";
            IConversorACL conversorACL = new ConversorACL(new MoedaFactory());
            IMoeda DolarResult = conversorACL.GetCotacaoComBaseNoDolar(SiglaMoedaDesejada);
            Assert.True(DolarResult.Valor.Equals(1));

            String OutraMoedaQueNaoTemValorDoDolar = "BRL";
            IMoeda RealResult = conversorACL.GetCotacaoComBaseNoDolar(OutraMoedaQueNaoTemValorDoDolar);
            Assert.False(RealResult.Valor.Equals(DolarResult.Valor));
        }
    }
}
