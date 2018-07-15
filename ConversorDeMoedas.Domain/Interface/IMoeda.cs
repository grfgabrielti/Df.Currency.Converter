using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Domain.Interface
{
    public interface IMoeda
    {
        String SiglaMoeda { get; }
        Decimal Valor { get; }
        Decimal ObterValorDaConversaoDeMoeda(IMoeda CotacaoEmDolarMoedaConvertida);
        IMoeda ConverterParaDolar(IMoeda CotacaoDolar);
        bool Equals(object obj);
    }
}
