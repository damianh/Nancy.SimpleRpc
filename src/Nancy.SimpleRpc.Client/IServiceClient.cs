namespace Nancy.SimpleRpc.Client
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IServiceClient
    {
        Task<TResponse> Send<TResponse>(object request, CancellationToken token) where TResponse : new();
    }
}