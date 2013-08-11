namespace Nancy.SimpleRpc
{
    public interface IServiceResolver
    {
        IService<TRequest> GetService<TRequest>() where TRequest : new();
    }
}