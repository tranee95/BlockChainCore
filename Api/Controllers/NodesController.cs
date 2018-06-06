
using System;
using Microsoft.AspNetCore.Mvc;
using blockchainAPI.Service;
using System.Collections.Generic;
using Common;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("[controller]/[action]")]
    public class NodesController : Controller
    {
        private IPipeService service;

        public NodesController(IPipeService _service)
        {
            this.service = _service;
        }

        [HttpPost]
        public IActionResult Register(string address, string identifier)
        {
            try
            {
                if (!string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(identifier))
                {
                    Console.WriteLine("Post: add new Node...");
            
                    var newNode = new Node
                    {
                        Address = address,
                        Identifier = identifier
                    };
                        
                    var requestData = new PipeRequest
                    {
                        Type = "registerNode",
                        Value = JsonConvert.SerializeObject(newNode)
                    };
                    
                    Response.StatusCode = 201;
                    var result = service.RunClientPipe(JsonConvert.SerializeObject(requestData));
                    return Json(JsonConvert.DeserializeObject<Node>(result));    
                } else
                {
                    Response.StatusCode = 400;
                    return Json("Missing values");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json($"ERROR: {ex.Message}");
            }
        }   

        [HttpGet]
        public IActionResult AllNode()
        {
            try
            {   
                Console.WriteLine("Get: all node...");

                var requestData = new PipeRequest
                {
                    Type = "allNode",
                    Value = string.Empty
                };

                var pipeResult = service.RunClientPipe(JsonConvert.SerializeObject(requestData));
                var allNode = JsonConvert.DeserializeObject<List<Node>>(pipeResult);

                if (allNode != null)
                {
                    Response.StatusCode = 200;
                    return Json(allNode);   
                } else {
                    Response.StatusCode = 400;
                    return Json($"Error: Please supply a valid list of nodes");
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