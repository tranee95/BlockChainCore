using System.Collections.Generic;

namespace Common 
{
    public class DsSelf
    {
        public List<Block> Chain { get; set;}
        public List<Transaction> CurrentTransactions { get; set; } 
        public List<Node> Node { get; set; }  
    }
}