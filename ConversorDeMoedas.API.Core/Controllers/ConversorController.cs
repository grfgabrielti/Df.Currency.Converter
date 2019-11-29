using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConversorDeMoedas.Services.Interface;
using ConversorDeMoedas.Services.Interface.Factory;
using ConversorDeMoedas.Services.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConversorDeMoedas.API.Core.Controllers
{
    [Produces("application/json")]
    [Route("api/Conversor")]
    public class ConversorController : Controller
    {
        IConversorService conversorService;
        public ConversorController(IConversorService conversorService)
        {
            this.conversorService = conversorService;
        }

        [HttpGet]
        public dynamic Get()
        {
            return conversorService.GetMoedas();
        }

        [HttpGet]
        [Route("ConverterMoeda/{SiglaMoedaOrigem}/{MoedaParaConversao}/{ValorParaConversao}")]
        public dynamic ConverterMoeda(String SiglaMoedaOrigem, String MoedaParaConversao, Decimal ValorParaConversao)
        {
            ConverterMoedaRequest request = new ConverterMoedaRequest()
            {
                SiglaMoedaOrigem = SiglaMoedaOrigem,
                MoedaParaConversao = MoedaParaConversao,
                ValorParaConversao = ValorParaConversao
            };

            return conversorService.ConverterMoeda(request);
        }

        [HttpGet]
        [Route("ConverterMoeda/GetRedis/{key}")]
        public dynamic TestRedis(string key)
        {
            return conversorService.TestRedis(key);
        }

        [HttpGet]
        [Route("ConverterMoeda/SetRedis")]
        public dynamic Setredis()
        {
            conversorService.TestSetRedis();
            return "Sucesso";
        }

    }
}