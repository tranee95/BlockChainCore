using System;
using System.Collections.Generic;

namespace Common
{
    [Serializable()]
    public class Block
    {
        public int Index { get; set; }
        public int Timestamp { get; set; }
        public List<Transaction> Transaction { get; set; }
        public int Proof { get; set; }
        public string PreviousHash { get; set; }
    }

    public class BlockResult : Block 
    {
        public string Message { get; set; }
    }
}