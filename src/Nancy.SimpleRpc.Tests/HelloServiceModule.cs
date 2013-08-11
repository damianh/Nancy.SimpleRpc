namespace Nancy.SimpleRpc.Tests
{
    using Nancy.SimpleRpc;

    public class HelloServiceModule : SimpleRpcModule
    {
        public HelloServiceModule(IServiceResolver serviceResolver) : base(serviceResolver)
        {}
    }
}