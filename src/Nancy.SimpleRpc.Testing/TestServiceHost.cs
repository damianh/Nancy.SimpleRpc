namespace Nancy.SimpleRpc.Testing
{
    using Nancy.Bootstrapper;
    using Nancy.SimpleRpc.Client;
    using global::Owin.Testing;

    public class TestServiceHost
    {
        private readonly string _baseUri;
        private readonly OwinTestServer _owinTestServer;
        private readonly IServiceClient _serviceClient;

        public TestServiceHost(string baseUri, INancyBootstrapper bootstrapper)
        {
            _baseUri = baseUri;
            _owinTestServer = OwinTestServer.Create(builder => new Startup(bootstrapper).Configuration(builder));
            _serviceClient = new ServiceClient(_baseUri, _owinTestServer.CreateHandler());
        }

        public IServiceClient Client
        {
            get { return _serviceClient; }
        }
    }
}