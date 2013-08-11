namespace Nancy.SimpleRpc.Tests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Nancy.Bootstrapper;
    using Nancy.SimpleRpc.Client;
    using Nancy.SimpleRpc.Testing;
    using Nancy.TinyIoc;
    using Xunit;

    public class HelloServiceTests
    {
        [Fact]
        public async Task Can_invoke_service()
        {
            var host = new TestServiceHost("http://example.com", new HelloServiceBootstrapper());
            HelloResponse response = await host.Client.Send<HelloResponse>(new HelloRequest {Name = "World!"});
            response.Result.Should().Be("Hello, World!");
        }

        private class HelloServiceBootstrapper : DefaultNancyBootstrapper
        {
            protected override void ConfigureApplicationContainer(TinyIoCContainer container)
            {
                base.ConfigureApplicationContainer(container);
                
                container.Register<IServiceResolver>((tinyIoCContainer, _) => new DelegateServiceResolver(tinyIoCContainer.Resolve));
                container.Register<IService<HelloRequest>, HelloService>();
            }
        }
    }
}