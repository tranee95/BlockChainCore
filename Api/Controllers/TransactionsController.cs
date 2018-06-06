using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using blockchainAPI.Service;
using Common;

namespace blockchainAPI.Controllers
{
    [Route("[controller]/[action]")]
    public class TransactionsController : Controller
    {
        private IPipeService service;

        public TransactionsController(IPipeService _service)
        {
            this.service = _service;
        }

        [HttpPost]
        public IActionResult New(string sender, string recipient, double amount)
        {
            try
            {
                Console.WriteLine("Post: New Transaction...");
                if (string.IsNullOrEmpty(sender) || string.IsNullOrEmpty(recipient))
                {
                    Response.StatusCode = 400;
                    return Json("Missing values");
                } else 
                {                   
                    var requestData = new PipeRequest
                    {
                        Type = "newTransaction",
                        Value = JsonConvert.SerializeObject(new Transaction {
                            Sender = sender,
                            Recipient = recipient,
                            Amount = amount
                        })
                    };
 
                    Response.StatusCode = 201;
                    return Json(service.RunClientPipe(JsonConvert.SerializeObject(requestData)));
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json($"ERROR: {ex.Message}");
            }          
        }
    }
}