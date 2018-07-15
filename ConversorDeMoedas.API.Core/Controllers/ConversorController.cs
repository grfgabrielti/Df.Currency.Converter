using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConversorDeMoedas.Services.Interface;
using ConversorDeMoedas.Services.Interface.Factory;
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
        [Route("ConverterMoeda/{MoedaOrigem}/{MoedaDestino}/{ValorMoedaOrigem}")]
        public dynamic ConverterMoeda(String MoedaOrigem, String MoedaDestino, Decimal ValorMoedaOrigem)
        {
            return conversorService.ConverterMoeda(MoedaOrigem, MoedaDestino , ValorMoedaOrigem);
        }
    }

    public class RequestGetConversão
    {
        public Int32 Id { get; set; }
    }
}