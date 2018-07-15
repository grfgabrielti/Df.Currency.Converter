using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Domain.Interface.Factory
{
    public interface IMoedaFactory
    {
        IMoeda Create(String Siglas, Decimal Valor);
        IMoeda Create(String Siglas, String NomeMoeda);
    }
}
