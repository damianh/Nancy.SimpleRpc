namespace Nancy.SimpleRpc
{
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class RpcService<TRequest, TResponse> : IRpcService<TRequest, TResponse>
        where TRequest : new()
        where TResponse : new()
    {
        public abstract Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken);
    }
}