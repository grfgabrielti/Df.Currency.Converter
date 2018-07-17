using ConversorDeMoedas.Domain.Interface;
using System;

namespace ConversorDeMoedas.Domain
{
    [Serializable]
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
            IMoeda result = null;
            if (CotacaoDaMoedaEscolhidaEmDolar.Valor <= 0)
                throw new Exception("Nao e possivel realizar o calculo sem a cotacao da moeda em dolar.");

            result = new Moeda("USD", this.Valor / CotacaoDaMoedaEscolhidaEmDolar.Valor);
            return result;

        }
        public Decimal ObterValorDaConversaoDeMoeda(IMoeda CotacaoEmDolarMoedaConvertida)
        {
            var retorno = (this.Valor * CotacaoEmDolarMoedaConvertida.Valor);
            return retorno;
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
            int resultado =  this.SiglaMoeda.GetHashCode() ^ this.Valor.GetHashCode();
            return resultado;
        }
    }
}
