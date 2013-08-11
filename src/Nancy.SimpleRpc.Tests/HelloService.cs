namespace Nancy.SimpleRpc.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Nancy.SimpleRpc;

    public class HelloService : IService<HelloRequest>
    {
        public Task<object> Execute(HelloRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(new HelloResponse { Result = "Hello, " + request.Name });
        }
    }
}