using ConversorDeMoedas.Domain;
using ConversorDeMoedas.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.ACL.Interface
{
    public interface IConversorACL
    {
        List<IMoeda> GetMoedas();
        IMoeda GetCotacaoComBaseNoDolar(String SiglasDaMoeda);
        string GetRedis(string key);
        void SetRedis();
    }
}
