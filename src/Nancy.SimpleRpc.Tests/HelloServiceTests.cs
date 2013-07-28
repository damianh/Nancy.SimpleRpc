namespace Nancy.SimpleRpc.Tests
{
    using System.Threading.Tasks;
    using Client;
    using FluentAssertions;
    using Testing;
    using TinyIoc;
    using Xunit;

    public class HelloServiceTests
    {
        [Fact]
        public async Task Can_invoke_service()
        {
            IServiceClient client = new TestServiceClient("http://example.com", new HelloServiceBootstrapper());
            HelloResponse response = await client.Send<HelloRequest, HelloResponse>(new HelloRequest { Name = "World!" });
            response.Result.Should().Be("Hello, World!");
        }

        // Would rather not have this. If IServiceResolver -> DelegateServiceResolver could automatically be wired up, that would be great.
        private class HelloServiceBootstrapper : DefaultNancyBootstrapper
        {
            protected override void ConfigureApplicationContainer(TinyIoCContainer container)
            {
                base.ConfigureApplicationContainer(container);
                container.Register<IServiceResolver>((tinyIoCContainer, _) => new DelegateServiceResolver(tinyIoCContainer.Resolve));

                // Shouldn't TinyIoC automagically discover this?
                container.Register<IService<HelloRequest, HelloResponse>, HelloService>();
            }
        }
    }
}