using ConversorDeMoedas.ACL.Interface;
using ConversorDeMoedas.Domain;
using ConversorDeMoedas.Domain.Interface;
using ConversorDeMoedas.Domain.Interface.Factory;
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
        private String ACCESS_KEY = "?access_key=c33c35cf4405c47d42a77c2b6e2eb3d1";
        private String BASE_URL = "http://apilayer.net/api/";
        IMoedaFactory moedaFactory;

        public ConversorACL(IMoedaFactory moedaFactory)
        {
            this.moedaFactory = moedaFactory;
        }
        public List<IMoeda> GetMoedas()
        {
            var manager = new RedisManagerPool("localhost:6379");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            HttpResponseMessage response = client.GetAsync("list" + ACCESS_KEY).Result;
            if (response.IsSuccessStatusCode)
            {
                var retorno = JsonConvert.DeserializeXNode(response.Content.ReadAsStringAsync().Result, "Root");
               // return retorno.Root.Element("currencies").Elements().Select(c => moedaFactory.Create(c.Value, c.Name.LocalName)).ToList();
            }

            throw new Exception("Não foi possivel obter as moedas");
        }

        private string endpoint = "convert";
        private string access_key = "YOUR_ACCESS_KEY";
        private string from = "EUR";
        private string to = "GBP";
        private string amount = "10";

        public List<Dinheiro> ConverterMoeda()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            //HttpResponseMessage response = client.GetAsync("convert" + ACCESS_KEY+ "&from="+from+"&to="+ to+ "&amount=" + amount).Result;

            HttpResponseMessage response = client.GetAsync("live" + ACCESS_KEY + "&currencies=JPY,BRL,USD").Result;

            if (response.IsSuccessStatusCode)
            {
                var retorno = JsonConvert.DeserializeXNode(response.Content.ReadAsStringAsync().Result, "Root");
                return retorno.Root.Element("quotes").Elements().Select(c => new Dinheiro(Convert.ToString(c.Name), Convert.ToDecimal(c.Value))).ToList();
            }

            throw new Exception("Não foi possivel obter as moedas");
        }

        public IMoeda GetCotacaoComBaseNoDolar(String SiglasDaMoeda)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            HttpResponseMessage response = client.GetAsync("live" + ACCESS_KEY + "&currencies=" + SiglasDaMoeda).Result;

            if (response.IsSuccessStatusCode)
            {
                var retorno = JsonConvert.DeserializeXNode(response.Content.ReadAsStringAsync().Result, "Root");
                return retorno.Root.Element("quotes").Elements().Select(c => moedaFactory.Create(Convert.ToString(c.Name), Convert.ToDecimal(c.Value))).First();
            }
            throw new Exception("Não foi possivel obter a cotacao");
        }
    }
}
