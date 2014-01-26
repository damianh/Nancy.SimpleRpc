namespace Nancy.SimpleRpc
{
    using System;

    public class DelegateRpcServiceResolver : IRpcServiceResolver
    {
        private readonly Func<Type, object> _getService;

        public DelegateRpcServiceResolver(Func<Type, object> getService)
        {
            _getService = getService;
        }

        public IRpcService<TRequest,TResponse> GetService<TRequest, TResponse>()
            where TRequest : class, new()
            where TResponse : class, new()
        {
            return (IRpcService<TRequest, TResponse>)_getService(typeof(IRpcService<TRequest, TResponse>));
        }
    }
}