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
            IMoeda moeda = new Moeda("R$", 10.00M);
            IMoeda CotacaoEmDolarDoReal = new Moeda("R$", 3.84M);
            var val = moeda.Valor * CotacaoEmDolarDoReal.Valor;
            Assert.True(val.Equals(moeda.ObterValorDaConversaoDeMoeda(CotacaoEmDolarDoReal)));

        }


        [Fact]
        public void TestPegaValorEmDolarDaMoedaSolicitadaValorNegativo()
        {
            IMoeda moeda = new Moeda("R$", 10.00M);
            IMoeda CotacaoEmDolarDoReal = new Moeda("R$", 0.84M);
            var val = moeda.Valor * CotacaoEmDolarDoReal.Valor;
            Assert.True(val.Equals(moeda.ObterValorDaConversaoDeMoeda(CotacaoEmDolarDoReal)));

        }

        [Fact]
        public void TestGethashCode()
        {
            IMoeda moeda = new Moeda("R$", 10.00M);
            var hash = moeda.SiglaMoeda.GetHashCode() ^ moeda.Valor.GetHashCode();
            Assert.True(moeda.GetHashCode().Equals(hash));
        }

    }
}