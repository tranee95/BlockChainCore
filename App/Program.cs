using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using Newtonsoft.Json;
using Common;
using System.Linq;

namespace blockchainApp 
{
    class Program 
    {
        static void Main(string[] args) 
        {
            var nodeAddress = string.Empty;
            var getNodelist = false;
            var registerNode = false;

            if (args.Any(s => s.Contains("-a:")))
            {
                var arg = args.FirstOrDefault(t => t.Trim().StartsWith("-a:"));
                arg = arg.Replace("-a:",string.Empty); 
                nodeAddress = arg;

                Console.WriteLine($"Node addres: {nodeAddress}");
            }

            if (args.Any(s => s.Contains("-f")))
                getNodelist = true;

            if (args.Any(s => s.Contains("-r")))
                registerNode = true;
           
            var nodeIdentifier = Guid.NewGuid().ToString().Replace("-", String.Empty);
            var blockchain = new Blockchain(nodeAddress, nodeIdentifier);

            if (getNodelist && !string.IsNullOrEmpty(nodeAddress))           
                blockchain._Self.Node = NodesNetwork.GetNodesList(blockchain._Self.Node, nodeAddress);         

            if (registerNode && !string.IsNullOrEmpty(nodeAddress))
                NodesNetwork.RegisterNodeInNetwork(blockchain._Self.Node, nodeAddress, nodeIdentifier);
            
            try 
            {
                while (true) 
                {
                    var server = new NamedPipeServerStream("BlockchainPipe");
                    Console.WriteLine("server WaitForConnection");

                    StreamReader reader = new StreamReader(server);
                    StreamWriter writer = new StreamWriter(server);

                    server.WaitForConnection();
                    Console.WriteLine("client connected to server");

                    var requestData = JsonConvert.DeserializeObject<PipeRequest>(reader.ReadLine());
                   
                    switch (requestData.Type) 
                    {
                        case "mine":
                            var lastBlok = blockchain._Self.LastBlok();
                            var lastProof = lastBlok.Proof;
                            var proof = blockchain.ProofOfWork(blockchain._Self, lastProof);

                            blockchain.NewTransaction(blockchain._Self, "0", nodeIdentifier, 1);

                            var block = blockchain.NewBlock(blockchain._Self, proof);   
                            var result = new BlockResult
                            {
                                Index = block.Index,
                                Timestamp = block.Timestamp,
                                Transaction = block.Transaction,
                                Proof = block .Proof,
                                PreviousHash = block .PreviousHash,
                                Message = "New Block Forged"   
                            };

                            Console.WriteLine(result.Message);
                            writer.WriteLine(JsonConvert.SerializeObject(result));

                            break;

                        case "newTransaction":   
                            var transactionData = JsonConvert.DeserializeObject<Transaction>(requestData.Value);
                            if (string.IsNullOrEmpty(transactionData.Sender) ||
                                string.IsNullOrEmpty(transactionData.Recipient) ||
                                transactionData.Amount == 0) {
                                writer.WriteLine("Missing values");
                            } else 
                            {
                                var index = blockchain.NewTransaction(blockchain._Self, transactionData.Sender,
                                    transactionData.Recipient, transactionData.Amount);
                                writer.WriteLine(index);
                            }

                            break;

                        case "fullChain":
                            var allChain = new Chain
                            {
                                Chains = blockchain._Self.Chain,
                                Length = blockchain._Self.Chain.Count
                            };
                            
                            writer.WriteLine(JsonConvert.SerializeObject(allChain));

                            break;
                        
                        case "registerNode": 

                            var newNode = JsonConvert.DeserializeObject<Node>(requestData.Value);

                            if (!blockchain._Self.Node.Contains(newNode))                           
                                blockchain._Self.Node = blockchain.RegisterNode(blockchain._Self.Node, newNode.Address, newNode.Identifier);
                            
                            writer.WriteLine(JsonConvert.SerializeObject(newNode));

                            break;
                        
                        case "allNode":
                            writer.WriteLine(JsonConvert.SerializeObject(blockchain._Self.Node));

                            break;
                    }

                    writer.Flush();
                    server.Close();
                }

            } catch (Exception ex) 
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}