namespace Nancy.SimpleRpc.Client
{
    using System.Threading;
    using System.Threading.Tasks;

    public static class ServiceClientExtensions
    {
        public static Task<TResponse> Send<TRequest, TResponse>(this IServiceClient serviceClient, TRequest request) where TResponse : new()
        {
            return serviceClient.Send<TRequest, TResponse>(request, CancellationToken.None);
        }
    }
}