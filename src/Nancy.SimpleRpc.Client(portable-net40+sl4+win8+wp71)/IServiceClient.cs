namespace Nancy.SimpleRpc.Client
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IServiceClient
    {
        Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken token) where TResponse : new();
    }
}