namespace Nancy.SimpleRpc.Client
{
    using System.Threading;
    using System.Threading.Tasks;

    public static class ServiceClientExtensions
    {
        public static Task<TResponse> Send<TResponse>(this IServiceClient serviceClient, object request) where TResponse : new()
        {
            return serviceClient.Send<TResponse>(request, CancellationToken.None);
        }
    }
}