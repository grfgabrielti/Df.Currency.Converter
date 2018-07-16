using ConversorDeMoedas.Domain;
using ConversorDeMoedas.Domain.Interface;
using ConversorDeMoedas.Services.Request;
using ConversorDeMoedas.Services.Request.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Services.Interface
{
    public interface IConversorService
    {
        List<IMoeda> GetMoedas();
        ConverterMoedaResult ConverterMoeda(ConverterMoedaRequest converterMoedaRequest);
    }
}
