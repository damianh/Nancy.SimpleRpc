namespace Nancy.SimpleRpc
{
    using System;

    public class DelegateServiceResolver : IServiceResolver
    {
        private readonly Func<Type, object> _getService;

        public DelegateServiceResolver(Func<Type, object> getService)
        {
            _getService = getService;
        }

        public IService<TRequest, TResponse> GetService<TRequest, TResponse>() where TRequest : new() where TResponse : new()
        {
            return (IService<TRequest, TResponse>) _getService(typeof (IService<TRequest, TResponse>));
        }
    }
}