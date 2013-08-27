namespace Nancy.SimpleRpc
{
    public interface IServiceResolver
    {
        IService<TRequest, TResponse> GetService<TRequest, TResponse>() where TRequest : new() where TResponse : new();
    }
}