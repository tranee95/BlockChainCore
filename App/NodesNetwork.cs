using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Common;
using System.Linq;


    /*
    1. Отправка запроса на регистрацию ноды
    2. Получения списка нод от сети
    3. Обновление ноды 
    */

namespace blockchainApp
{
    public static class NodesNetwork
    {
        public static void RegisterNodeInNetwork(List<Node> nodes, string nodeAddress, string nodeIdentifier)
        {
            foreach (var item in nodes)
            {
                try
                {
                    var newNode = $"address={nodeAddress}&Identifier={nodeIdentifier}";
                    
                    var result = ReqestService.Post<Node>($"{item.Address}/nodes/register", newNode);
                    Console.WriteLine($"Node has registred on {item.Address}");
                }
                catch (Exception ex)
                {          
                    Console.WriteLine($"Fail to registred: {ex.Message}");
                }
            }
        }

        public static List<Node> GetNodesList(List<Node> nodes, string nodeAddress)
        {            
            var result = new List<Node>();

            foreach (var item in nodes)         
                result.Add(item);
                 
            foreach (var item in nodes)
            {
                if (item.Address != nodeAddress)
                {
                    try
                    {
                        var nodesList = ReqestService.Get<List<Node>>(item.Address, "nodes/allNode");                        
                        foreach (var node in nodesList)
                        {      
                            if (result.FirstOrDefault(s => s.Address == node.Address) == null)
                            {
                                result.Add(node);
                                Console.WriteLine($"New node added: address:{node.Address}  Identifier:{node.Identifier}");  
                            }                              
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return result;
        }   
    }
}