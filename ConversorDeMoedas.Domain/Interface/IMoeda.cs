using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Domain.Interface
{
    public interface IMoeda
    {
        String SiglaMoeda { get; }
        String NomeMoeda { get; }
        Decimal Valor { get; }
        Decimal ObterValorDaConversaoDeMoeda(IMoeda CotacaoEmDolarMoedaConvertida);
        IMoeda ConverterParaDolar(IMoeda CotacaoDaMoedaEscolhidaEmDolar);
        bool Equals(object obj);
    }
}
