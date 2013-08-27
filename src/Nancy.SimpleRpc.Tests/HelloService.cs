namespace Nancy.SimpleRpc.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Nancy.SimpleRpc;

    public class HelloService : IService<HelloRequest, HelloResponse>
    {
        public Task<HelloResponse> Execute(HelloRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HelloResponse { Result = "Hello, " + request.Name });
        }
    }
}