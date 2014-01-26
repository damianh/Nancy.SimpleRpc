namespace Nancy.SimpleRpc.Tests
{
    using Nancy.SimpleRpc.Tests.Annotations;

    [UsedImplicitly]
    public class HelloServiceModule : SimpleRpcModule
    {
        public HelloServiceModule(IRpcServiceResolver rpcServiceResolver)
            : base(rpcServiceResolver)
        {
            RegisterRpcServices();
        }
    }
}