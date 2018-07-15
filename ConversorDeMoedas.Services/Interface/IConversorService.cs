using ConversorDeMoedas.Domain;
using ConversorDeMoedas.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Services.Interface
{
    public interface IConversorService
    {
        List<IMoeda> GetMoedas();
        IMoeda ConverterMoeda(String MoedaOrigemSiglas, String MoedaParaConversao, Decimal ValorParaConversao);
    }
}
