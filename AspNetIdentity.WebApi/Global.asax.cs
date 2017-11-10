namespace AspNetIdentity.WebApi
{
    using System.Web.Http;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()  
        {
            Bootstrapper.Run();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
