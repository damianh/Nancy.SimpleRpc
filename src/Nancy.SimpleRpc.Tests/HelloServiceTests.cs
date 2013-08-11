namespace Nancy.SimpleRpc.Tests
{
    using System.Threading.Tasks;
    using Nancy.SimpleRpc.Client;
    using FluentAssertions;
    using Nancy.TinyIoc;
    using Testing;
    using Xunit;

    public class HelloServiceTests
    {
        [Fact]
        public async Task Can_invoke_service()
        {
            var host = new TestServiceHost("http://example.com", new HelloServiceBootstrapper());
            HelloResponse response = await host.Client.Send<HelloResponse>(new HelloRequest { Name = "World!" });
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