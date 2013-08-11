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
        private static IEnumerable<Action<object>> _configureModuleActions;

        protected SimpleRpcModule(IServiceResolver serviceResolver)
            : this(serviceResolver, Assembly.GetCallingAssembly(), string.Empty)
        { }

        protected SimpleRpcModule(IServiceResolver serviceResolver, Assembly serviceAssembly)
            : this(serviceResolver, serviceAssembly, string.Empty)
        {}

        protected SimpleRpcModule(IServiceResolver serviceResolver, Assembly serviceAssembly, string modulePath) : base(modulePath)
        {
            _serviceResolver = serviceResolver;

            if (_configureModuleActions == null)
            {
                var configureActions = new List<Action<object>>();
                IEnumerable<Type> services = serviceAssembly.GetExportedTypes().Where(t => t.IsAssignableToGenericType(typeof (IService<>)));

                foreach (Type service in services)
                {
                    var typeArguments =
                        service.GetInterfaces()
                            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IService<>))
                            .SelectMany(i => i.GetGenericArguments())
                            .ToArray();
                    MethodInfo methodInfo = typeof (SimpleRpcModule).GetMethod("Handle", BindingFlags.NonPublic | BindingFlags.Instance);
                    MethodInfo handleMethod = methodInfo.MakeGenericMethod(typeArguments);
                    configureActions.Add(obj => handleMethod.Invoke(obj, new object[] { }));
                }
                _configureModuleActions = configureActions;
            }

            foreach (var configureModuleAction in _configureModuleActions)
            {
                configureModuleAction(this);
            }
        }

        private void Handle<TRequest>()
            where TRequest : new()
        {
            Post[typeof(TRequest).Name, true] = async (ctx, ct) =>
            {
                IService<TRequest> service = _serviceResolver.GetService<TRequest>();
                var request = this.Bind<TRequest>();
                return await service.Execute(request, ct);
            };
        }
    }
}