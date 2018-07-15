using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ConversorDeMoedas.UI.Core.Models.Conversor;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConversorDeMoedas.UI.Core.Controllers
{
    public class ConversorController : Controller
    {
        private String BASE_URL = "http://localhost:1234/API/";

        public IActionResult Index()
        {
       

            return View();
        }



        public List<ConversorViewModel> sla()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            HttpResponseMessage response = client.GetAsync("Conversor").Result;
            List<ConversorViewModel> conversorViewModels = new List<ConversorViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var retorno = JsonConvert.DeserializeObject<List<ConversorViewModel>>(response.Content.ReadAsStringAsync().Result);
                conversorViewModels = retorno;
            }
            return conversorViewModels;
        }
    }
}