namespace Nancy.SimpleRpc.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Nancy.SimpleRpc;
    using Nancy.SimpleRpc.Tests.Annotations;

    [UsedImplicitly]
    public class HelloRpcService : RpcService<HelloRequest, HelloResponse>
    {
        public override Task<HelloResponse> Execute(HelloRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HelloResponse { Result = "Hello, " + request.Name });
        }
    }
}