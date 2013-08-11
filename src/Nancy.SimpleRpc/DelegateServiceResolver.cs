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

        public IService<TRequest> GetService<TRequest>() where TRequest : new()
        {
            return (IService<TRequest>) _getService(typeof (IService<TRequest>));
        }
    }
}