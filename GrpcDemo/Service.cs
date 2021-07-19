using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
namespace GrpcDemo
{
    public class Service : Greeter.GreeterBase
    {
        private readonly ILogger<Service> _logger;
        public Service(ILogger<Service> logger)
        {
            _logger = logger;
        }
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}