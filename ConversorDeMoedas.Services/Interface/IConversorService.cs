using ConversorDeMoedas.Domain;
using ConversorDeMoedas.Domain.Interface;
using ConversorDeMoedas.Services.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Services.Interface
{
    public interface IConversorService
    {
        List<IMoeda> GetMoedas();
        IMoeda ConverterMoeda(ConverterMoedaRequest converterMoedaRequest);
    }
}
