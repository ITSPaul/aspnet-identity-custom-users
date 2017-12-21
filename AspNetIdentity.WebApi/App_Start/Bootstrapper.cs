namespace AspNetIdentity.WebApi
{
    using System.Web.Http;
    using Owin;

    internal static class Bootstrapper
    {
        internal static HttpConfiguration Run(IAppBuilder app)
        {
            // When using OWIN, you don't use GlobalConfiguration - you create
            // an HttpConfiguration and set it up independently.
            var config = new HttpConfiguration();

            //Configure AutoFac
            var container = ContainerConfig.Initialize(config);
            app.UseAutofacMiddleware(container);
            Startup.ConfigureAuth(app, container);

            return config;
        }
    }
}
