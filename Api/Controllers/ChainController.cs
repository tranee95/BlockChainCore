using System;
using Microsoft.AspNetCore.Mvc;
using blockchainAPI.Service;
using Common;
using Newtonsoft.Json;

namespace blockchainAPI.Controllers
{
    [Route("[controller]")]
    public class ChainController : Controller
    {
        private IPipeService service;

        public ChainController(IPipeService _service)
        {
            this.service = _service;
        }

        [HttpGet]
        public IActionResult Chain()
        {
            try
            {
                Console.WriteLine("Get: Full Chain...");
                var requestData = new PipeRequest
                    {
                        Type = "fullChain",
                        Value = string.Empty
                    };

                Response.StatusCode = 200;
                var pipeResult = service.RunClientPipe(JsonConvert.SerializeObject(requestData));
                var result = JsonConvert.DeserializeObject<Chain>(pipeResult);
                return Json(result);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json($"ERROR: {ex.Message}");
            }
        }
    }
}