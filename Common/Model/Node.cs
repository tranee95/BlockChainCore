using System.Collections.Generic;

namespace Common
{
    public class Node
    {
        public string Address { get; set; }
        public string Identifier {get; set; }
    }

    public static class DefaulNodeList
    {
        public static List<Node> Nodes
        {
            get
            {
                return new List<Node>
                {
                    new Node{
                        Address = "015d891a.ngrok.io",
                        Identifier = "valeraNode" 
                    },
                    new Node{
                        Address = "192.168.1.33",
                        Identifier = "TestIdentifier"
                    },
                    new Node{
                        Address = "192.168.1.239",
                        Identifier = "TestIdentifier"
                    }
                };                
            }
        }
    }
}