// ReSharper disable CheckNamespace
namespace Nancy.SimpleRpc.Testing
// ReSharper restore CheckNamespace
{
    using System.Threading;
    using System.Threading.Tasks;
    using Client;
    using Nancy.Bootstrapper;
    using global::Owin.Testing;

    public class TestServiceClient : IServiceClient
    {
        private readonly string _baseUri;
        private readonly OwinTestServer _owinTestServer;

        public TestServiceClient(string baseUri, INancyBootstrapper bootstrapper)
        {
            _baseUri = baseUri;
            _owinTestServer = OwinTestServer.Create(builder => new Startup(bootstrapper).Configuration(builder));
        }

        public Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken) where TResponse : new()
        {
            var serviceClient = new ServiceClient(_baseUri, _owinTestServer.CreateHandler());
            return serviceClient.Send<TRequest, TResponse>(request, cancellationToken);
        }
    }
}