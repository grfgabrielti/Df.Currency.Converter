using ConversorDeMoedas.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ConversorDeMoedas.Domain.Test
{
    public class MoedaTest
    {
        [Fact]
        public void TestQueDeveComparaDoisValoresIguais()
        {
            IMoeda moeda = new Moeda("R$", 10.00M);
            IMoeda outroDinheiro = new Moeda("R$", 10.00M);
            Assert.True(moeda.Equals(outroDinheiro));

            moeda = new Moeda("USD", 10.00M);
            outroDinheiro = new Moeda("BRL", 10.00M);
            Assert.False(moeda.Equals(outroDinheiro));
        }
        [Fact]
        public void TestPegaValorEmDolarDaMoedaSolicitada()
        {
            IMoeda moeda = new Moeda("R$", 10);
            IMoeda CotacaoEmDolarDoReal = new Moeda("R$", 3.86M);
            var val = moeda.Valor * CotacaoEmDolarDoReal.Valor;

            Decimal result = moeda.ObterValorDaConversaoDeMoeda(CotacaoEmDolarDoReal);
            Assert.True(val.Equals(result));

        }
        [Fact]
        public void TestPegaValorEmDolarDaMoedaSolicitadaValorNegativo()
        {
            IMoeda moeda = new Moeda("R$", 10.00M);
            IMoeda CotacaoEmDolarDoReal = new Moeda("R$", 0.84M);
            var valor = moeda.Valor * CotacaoEmDolarDoReal.Valor;
            Assert.True(valor.Equals(moeda.ObterValorDaConversaoDeMoeda(CotacaoEmDolarDoReal)));
        }
        [Fact]
        public void TesteQuePegaOHashCodeDoObjetoDeAcordoComOsDadosDele()
        {
            IMoeda moeda = new Moeda("R$", 10.00M);
            var hash = moeda.SiglaMoeda.GetHashCode() ^ moeda.Valor.GetHashCode();
            Assert.True(moeda.GetHashCode().Equals(hash));
        }
        [Fact]
        public void TesteQueConverteMoedaDesejadaComValorInferiorAoDolar()
        {
            IMoeda moeda = new Moeda("R$", 10.00M);
            IMoeda CotacaoEmDolarDoReal = new Moeda("R$", 3.86M);
            var valor = moeda.Valor / CotacaoEmDolarDoReal.Valor;

            var result = moeda.ConverterParaDolar(CotacaoEmDolarDoReal);
            Assert.True(valor.Equals(result.Valor));
        }
        [Fact]
        public void TesteQueConverteMoedaDesejadaComValorSuperiorAoDolar()
        {
            IMoeda moeda = new Moeda("R$", 10.00M);
            IMoeda CotacaoEmDolarDoReal = new Moeda("R$", 0.25M);
            var valor = moeda.Valor / CotacaoEmDolarDoReal.Valor;

            var result = moeda.ConverterParaDolar(CotacaoEmDolarDoReal);
            Assert.True(valor.Equals(result.Valor));
        }
        [Fact]
        public void TestPassandoValorZeroParaCotacaoDoDolar()
        {
            IMoeda moeda = new Moeda("R$", 10.00M);
            IMoeda CotacaoEmDolarDoReal = new Moeda("R$", 0);
            Assert.Throws<Exception>(() => moeda.ConverterParaDolar(CotacaoEmDolarDoReal));
        }
    }
}