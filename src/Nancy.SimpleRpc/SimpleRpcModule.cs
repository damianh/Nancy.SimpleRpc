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
        private readonly IRpcServiceResolver _rpcServiceResolver;

        public SimpleRpcModule(IRpcServiceResolver rpcServiceResolver)
        {
            _rpcServiceResolver = rpcServiceResolver;
        }

        protected void RegisterRpcServices()
        {
            MethodInfo addEndpointMethod = typeof(SimpleRpcModule).GetMethod("AddEndpoint", BindingFlags.NonPublic | BindingFlags.Instance);
            var assembly = Assembly.GetCallingAssembly();
            IEnumerable<Type> services = assembly
                   .GetExportedTypes()
                   .Where(t => t.IsAssignableToGenericType(typeof(IRpcService<,>))
                       && !t.IsInterface
                       && !t.IsAbstract);

            foreach (Type service in services)
            {
                var typeArguments =
                    service.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRpcService<,>))
                        .SelectMany(i => i.GetGenericArguments())
                        .ToArray();

                addEndpointMethod
                    .MakeGenericMethod(typeArguments)
                    .Invoke(this, new object[] { _rpcServiceResolver });
            }
        }

        private void AddEndpoint<TRequest, TResponse>(IRpcServiceResolver rpcServiceResolver)
            where TRequest : class, new()
            where TResponse : class, new()
        {
            Post[typeof(TRequest).Name, true] = async (ctx, ct) =>
            {
                IRpcService<TRequest, TResponse> rpcService = rpcServiceResolver.GetRpcService<TRequest, TResponse>();
                var request = this.Bind<TRequest>();
                return await rpcService.Execute(request, ct);
            };
        }
    }
}