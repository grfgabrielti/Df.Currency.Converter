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
using Microsoft.Extensions.Configuration;

namespace ConversorDeMoedas.ACL
{
    public class ConversorACL : IConversorACL
    {
        private Object GetMoedasLock = new Object();
        private Object GetCotacaoComBaseNoDolarLock = new Object();
        private IConfiguration Configuration { get; }

        IMoedaFactory moedaFactory;
        IRedisConnectorHelperFactory redisConnectorHelperFactory;

        public ConversorACL(IMoedaFactory moedaFactory, IRedisConnectorHelperFactory redisConnectorHelperFactory,IConfiguration Configuration)
        {
            this.moedaFactory = moedaFactory;
            this.redisConnectorHelperFactory = redisConnectorHelperFactory;
            this.Configuration = Configuration;
        }

        public List<IMoeda> GetMoedas()
        {

            IRedisConnectorHelper redisConnectorHelper = redisConnectorHelperFactory.Create();
            String NomeCacheObject = "GetMoedasList";
            var cacheValue = redisConnectorHelper.Get<List<IMoeda>>(NomeCacheObject);

            if (cacheValue == null)
            {
                lock (GetMoedasLock)
                {
                    cacheValue = redisConnectorHelper.Get<List<IMoeda>>(NomeCacheObject);

                    if (cacheValue == null)
                    {

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(Configuration.GetSection("BASE_URL").Value);
                        HttpResponseMessage response = client.GetAsync("list" + Configuration.GetSection("ACCESS_KEY").Value).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var retorno = JsonConvert.DeserializeXNode(response.Content.ReadAsStringAsync().Result, "Root");
                            var resultado = retorno.Root.Element("currencies").Elements().Select(c => moedaFactory.Create(c.Name.ToString(), c.Value)).ToList();
                            redisConnectorHelper.Set(NomeCacheObject, resultado, 120);
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
            IRedisConnectorHelper redisConnectorHelper = redisConnectorHelperFactory.Create();
            String NomeCacheObject = "GetCotacaoComBaseNoDolar" + SiglasDaMoeda;
            var cacheValue = redisConnectorHelper.Get<IMoeda>(NomeCacheObject);

            if (cacheValue == null)
            {
                lock (GetMoedasLock)
                {
                    cacheValue = redisConnectorHelper.Get<IMoeda>(NomeCacheObject);

                    if (cacheValue == null)
                    {

                        HttpClient client = new HttpClient();
                        String baseurl = Configuration.GetSection("BASE_URL").Value;
                        client.BaseAddress = new Uri(baseurl);
                        String accesskey = Configuration.GetSection("ACCESS_KEY").Value;
                        HttpResponseMessage response = client.GetAsync("live" + accesskey + "&currencies=" + SiglasDaMoeda).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var retorno = JsonConvert.DeserializeXNode(response.Content.ReadAsStringAsync().Result, "Root");
                            var resultado = retorno.Root.Element("quotes").Elements().Select(c => moedaFactory.Create(Convert.ToString(c.Name), Convert.ToDecimal(c.Value))).First();
                            redisConnectorHelper.Set(NomeCacheObject, resultado, 5);
                            return resultado;
                        }
                        throw new Exception("Não foi possivel obter a cotacao da moeda desejada");
                    }
                }
            }
            return cacheValue;
        }
    }
}
