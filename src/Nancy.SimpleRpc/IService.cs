namespace Nancy.SimpleRpc
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IService<in TRequest>
        where TRequest: new()
    {
        Task<object> Execute(TRequest request, CancellationToken cancellationToken);
    }
}