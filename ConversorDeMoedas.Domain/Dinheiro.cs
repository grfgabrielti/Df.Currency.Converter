using ConversorDeMoedas.Domain.Factory;
using ConversorDeMoedas.Domain.Interface;
using ConversorDeMoedas.Domain.Interface.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Domain
{
    public class Dinheiro
    {
        public String Moeda { get; private set; }
        public decimal Valor { get; private set; }

        public Dinheiro(String moeda, decimal valor)
        {
            this.Moeda = moeda;
            this.Valor = valor;
        }

        public Dinheiro ConverterParaDolar(Dinheiro CotacaoDolar)
        {
            if (CotacaoDolar.Valor > 1)
                return new Dinheiro("USD", this.Valor / CotacaoDolar.Valor);
            else
                return new Dinheiro("USD", this.Valor * CotacaoDolar.Valor);

        }

        public Boolean MoedaÉDola()
        {
            return (this.Moeda.Equals("USD"));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            var dinheiro = obj as Dinheiro;

            if (ReferenceEquals(null, dinheiro))
                return false;

            return (Moeda == dinheiro.Moeda && Valor == dinheiro.Valor);
        }


    }
}
