using System;
using System.IO;
using System.IO.Pipes;

namespace blockchainAPI.Service 
{
    public class PipeService : IPipeService
    {
        public string RunClientPipe(string value)
        {       
            var client = new NamedPipeClientStream("BlockchainPipe");
            client.Connect();
            var reader = new StreamReader(client);
            var writer = new StreamWriter(client);

            writer.WriteLine(value);
            writer.Flush();
            
            return reader.ReadLine();
        }
    }
}