using System.IO.Pipes;

namespace blockchainAPI.Service
{
    public interface IPipeService
    {
        string RunClientPipe(string value);
    }
}