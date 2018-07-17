using ConversorDeMoedas.ACL.Interface;
using ConversorDeMoedas.Domain;
using ConversorDeMoedas.Domain.Interface;
using ConversorDeMoedas.Domain.Interface.Factory;
using ConversorDeMoedas.Infrastructure.Interface;
using ConversorDeMoedas.Infrastructure.Interface.Factory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;

namespace ConversorDeMoedas.ACL
{
    public class ConversorACL : IConversorACL
    {
        private Object farol = new Object();
        String ACCESS_KEY = "?access_key=c33c35cf4405c47d42a77c2b6e2eb3d1";
        String BASE_URL = "http://apilayer.net/api/";
        IMoedaFactory moedaFactory;
        IRedisConnectorHelperFactory redisConnectorHelperFactory;
        public ConversorACL(IMoedaFactory moedaFactory, IRedisConnectorHelperFactory redisConnectorHelperFactory)
        {
            this.moedaFactory = moedaFactory;
            this.redisConnectorHelperFactory = redisConnectorHelperFactory;
        }

        public List<IMoeda> GetMoedas()
        {

            IRedisConnectorHelper redisConnectorHelper = redisConnectorHelperFactory.Create();
            var cacheValue = redisConnectorHelper.Get<List<IMoeda>>("GetMoedasList");

            if (cacheValue == null)
            {
                lock (farol)
                {
                    cacheValue = redisConnectorHelper.Get<List<IMoeda>>("GetMoedasList");

                    if (cacheValue == null)
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(BASE_URL);
                        HttpResponseMessage response = client.GetAsync("list" + ACCESS_KEY).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var retorno = JsonConvert.DeserializeXNode(response.Content.ReadAsStringAsync().Result, "Root");
                            var resultado = retorno.Root.Element("currencies").Elements().Select(c => moedaFactory.Create(c.Name.ToString(), c.Value)).ToList();
                            redisConnectorHelper.Set("GetMoedasList", resultado, 60);
                            return resultado;
                        }
                        else
                        {
                            throw new Exception("Não foi possivel obter as moedas");
                        }
                    }
                }
            }
            return cacheValue;

        }
        public IMoeda GetCotacaoComBaseNoDolar(String SiglasDaMoeda)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            HttpResponseMessage response = client.GetAsync("live" + ACCESS_KEY + "&currencies=" + SiglasDaMoeda).Result;
            if (response.IsSuccessStatusCode)
            {
                var retorno = JsonConvert.DeserializeXNode(response.Content.ReadAsStringAsync().Result, "Root");
                var resultado = retorno.Root.Element("quotes").Elements().Select(c => moedaFactory.Create(Convert.ToString(c.Name), Convert.ToDecimal(c.Value))).First();
                return resultado;
            }
            throw new Exception("Não foi possivel obter a cotacao da moeda desejada");
        }
    }
}
