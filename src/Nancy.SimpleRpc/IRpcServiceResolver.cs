namespace Nancy.SimpleRpc
{
    public interface IRpcServiceResolver
    {
        IRpcService<TRequest, TResponse> GetService<TRequest, TResponse>()
            where TRequest : class, new()
            where TResponse : class, new();
    }
}