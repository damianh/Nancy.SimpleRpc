namespace Nancy.SimpleRpc.Testing
{
    using Nancy.Bootstrapper;
    using global::Owin;

    internal class Startup
    {
        private readonly INancyBootstrapper _bootstrapper;

        public Startup(INancyBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        public void Configuration(IAppBuilder builder)
        {
            builder.UseNancy(options => options.Bootstrapper = _bootstrapper);
        }
    }
}