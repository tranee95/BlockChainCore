using System;
using Microsoft.AspNetCore.Mvc;
using blockchainAPI.Service;
using Common;
using Newtonsoft.Json;

namespace blockchainAPI.Controllers 
{
    [Route("[controller]")]
    public class MineController : Controller
    {
        private IPipeService service;

        public MineController(IPipeService _service)
        {
            this.service = _service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                Console.WriteLine("Get: Mine...");
                var requestData = new PipeRequest
                    {
                        Type = "mine",
                        Value = string.Empty
                    };

                var pipeResult = service.RunClientPipe(JsonConvert.SerializeObject(requestData));
        
                Response.StatusCode = 200;
                var result = JsonConvert.DeserializeObject<BlockResult>(pipeResult);
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