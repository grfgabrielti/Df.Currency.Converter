using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Services.Request
{
    public class ConverterMoedaRequest
    {
        public String SiglaMoedaOrigem { get; set; }
        public String MoedaParaConversao { get; set; }
        public Decimal ValorParaConversao { get; set; }
    }
}
