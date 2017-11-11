[assembly: Microsoft.Owin.OwinStartup(typeof(AspNetIdentity.WebApi.Startup))]

namespace AspNetIdentity.WebApi
{
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Web.Http;

    using Newtonsoft.Json.Serialization;

    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();

            this.ConfigureAuth(app);

            //ConfigureWebApi(httpConfig);
            app.UseWebApi(httpConfig);
        }

        private static void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
