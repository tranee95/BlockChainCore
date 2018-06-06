using System;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Common;
using Newtonsoft.Json;

namespace blockchainApp
{
    public class Blockchain
    {
        public Self _Self;

        public Blockchain(string address, string nodeIdentifier)
        {
            try
            {
                _Self = new Self();
                _Self.Chain = new List<Block>();
                _Self.Chain.Add(new Block
                {
                    Index = 0,
                    Timestamp = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds,
                    Transaction = _Self.CurrentTransactions,
                    PreviousHash = string.Empty
                });

                _Self.CurrentTransactions = new List<Transaction>();
            
                if (!string.IsNullOrEmpty(address))
                    _Self.Node = CreateDefaultNodeList(address, nodeIdentifier);
                else throw new Exception("Node address not specified, please use -a:<address node>");      
            }
            catch (Exception ex)
            {
               Console.WriteLine($"ERROR: {ex.Message}");
            }       
        }

        public int NewTransaction(Self self, string sender, string recipinet, double ammout)
        {
            self.CurrentTransactions.Add(new Transaction
            {
                Sender = sender,
                Recipient = recipinet,
                Amount = ammout
            });

            return self.LastBlok().Index;
        }

        public Block NewBlock(Self self, int proof, string previousHash = null)
        {
            var block  = new Block
            {
                Index = self.Chain.Count + 1,
                Timestamp = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds,
                Transaction = self.CurrentTransactions,
                Proof = proof,
                PreviousHash = self.Hash(self.LastBlok())
            };

            self.CurrentTransactions = new List<Transaction>( );
            self.Chain.Add(block);
            return block ;
        }

        public int ProofOfWork(Self self, int lastProof)
        {
            int proof = 0;
            while (self.ValidProof(lastProof, proof))
                proof += 1;

            return proof;
        }

        public List<Node> RegisterNode(List<Node> nodes, string address, string Identifier)
        {
            nodes.Add(new Node{
                Address = address,
                Identifier = Identifier
            });

            return nodes;
        }

        public List<Node> AllNodes(Self self)
        {
            return self.Node;
        }

        private List<Node> CreateDefaultNodeList(string address, string nodeIdentifier)
        {
            var result = DefaulNodeList.Nodes;
            result.Add(new Node
            {
                Address = address,
                Identifier = nodeIdentifier
            });
            return result;
        }
    }
}