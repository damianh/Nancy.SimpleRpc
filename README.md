Nancy.SimpleRpc
===============

A simple message based RPC over HTTP library

1) Define your request and response models:
```csharp
public class HelloRequest
{
    public string Name { get; set; }
}

public class HelloResponse
{
    public string Result { get; set; }
}
```
2) Define your service:
```csharp
public class HelloService : IService<HelloRequest>
{
    public Task<object> Execute(HelloRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult<object>(new HelloResponse { Result = "Hello, " + request.Name });
    }
}
```
3) Register your service in Nancy's containers: _(TODO: Auto disco & registration)_
```csharp
private class HelloServiceBootstrapper : DefaultNancyBootstrapper
{
    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);
        container.Register<IServiceResolver>((tinyIoCContainer, _) => new DelegateServiceResolver(tinyIoCContainer.Resolve));
        container.Register<IService<HelloRequest>, HelloService>();
    }
}
```

4) Test your service:
```csharp
[Fact]
public async Task Can_invoke_service()
{
    var host = new TestServiceHost("http://example.com", new HelloServiceBootstrapper());
    HelloResponse response = await host.Client.Send<HelloResponse>(new HelloRequest {Name = "World!"});
    response.Result.Should().Be("Hello, World!");
}
```
