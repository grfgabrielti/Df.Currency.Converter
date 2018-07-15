using ConversorDeMoedas.Domain.Interface;
using System;

namespace ConversorDeMoedas.Domain
{
    public class Moeda : IMoeda
    {
        public String SiglaMoeda { get; private set; }
        public String NomeMoeda { get; private set; }

        public Decimal Valor { get; private set; }


        public Moeda(String siglaMoeda, String nomeMoeda)
        {
            this.SiglaMoeda = siglaMoeda;
            this.NomeMoeda = nomeMoeda;
        }

        public Moeda(String siglaMoeda, decimal valor)
        {
            this.SiglaMoeda = siglaMoeda;
            this.Valor = valor;
        }

        public IMoeda ConverterParaDolar(IMoeda CotacaoDaMoedaEscolhidaEmDolar)
        {
            if (CotacaoDaMoedaEscolhidaEmDolar.Valor > 1)
                return new Moeda("USD", this.Valor / CotacaoDaMoedaEscolhidaEmDolar.Valor);
            else
                return new Moeda("USD", this.Valor * CotacaoDaMoedaEscolhidaEmDolar.Valor);
        }
        public Decimal ObterValorDaConversaoDeMoeda(IMoeda CotacaoEmDolarMoedaConvertida)
        {
            return (this.Valor * CotacaoEmDolarMoedaConvertida.Valor);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            var dinheiro = obj as IMoeda;

            if (ReferenceEquals(null, dinheiro))
                return false;

            return (this.SiglaMoeda == dinheiro.SiglaMoeda && this.Valor == dinheiro.Valor);
        }
        public override int GetHashCode()
        {
            return this.SiglaMoeda.GetHashCode() ^ this.Valor.GetHashCode();
        }
    }
}
