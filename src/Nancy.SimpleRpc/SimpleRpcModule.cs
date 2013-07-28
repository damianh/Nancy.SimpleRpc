namespace Nancy.SimpleRpc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Nancy.Extensions;
    using Nancy.ModelBinding;

    public abstract class SimpleRpcModule : NancyModule
    {
        private readonly IServiceResolver _serviceResolver;
        private readonly Assembly _serviceAssembly;

        protected SimpleRpcModule(IServiceResolver serviceResolver, Assembly serviceAssembly)
            : this(serviceResolver, serviceAssembly, string.Empty)
        {}

        protected SimpleRpcModule(IServiceResolver serviceResolver, Assembly serviceAssembly, string modulePath) : base(modulePath)
        {
            _serviceResolver = serviceResolver;
            _serviceAssembly = serviceAssembly;
            IEnumerable<Type> services = _serviceAssembly.GetExportedTypes().Where(t => t.IsAssignableToGenericType(typeof (IService<,>)));

            foreach (Type service in services)
            {
                var typeArguments = service.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IService<,>))
                    .SelectMany(i => i.GetGenericArguments())
                    .ToArray();
                MethodInfo methodInfo = typeof(SimpleRpcModule).GetMethod("Handle", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo handleMethod = methodInfo.MakeGenericMethod(typeArguments);
                handleMethod.Invoke(this, new object[] { });
            }
        }

        private void Handle<TRequest, TResponse>()
            where TResponse : new()
            where TRequest : new()
        {
            Post[typeof(TRequest).Name, true] = async (ctx, ct) =>
            {
                IService<TRequest, TResponse> service = _serviceResolver.GetService<TRequest, TResponse>();
                var request = this.Bind<TRequest>();
                return await service.Execute(request, ct);
            };
        }
    }
}