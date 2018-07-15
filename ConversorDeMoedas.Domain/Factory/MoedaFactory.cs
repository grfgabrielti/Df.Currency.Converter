using ConversorDeMoedas.Domain.Interface;
using ConversorDeMoedas.Domain.Interface.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Domain.Factory
{
    public class MoedaFactory : IMoedaFactory
    {
        public IMoeda Create(String Siglas, Decimal Valor)
        {
            return new Moeda(Siglas, Valor);
        }

        public IMoeda Create(String Siglas, String NomeMoeda)
        {
            return new Moeda(Siglas, NomeMoeda);
        }
    }
}
