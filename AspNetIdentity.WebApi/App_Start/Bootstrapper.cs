namespace AspNetIdentity.WebApi
{
    using System.Web.Http;

    internal static class Bootstrapper
    {
        internal static void Run()
        {
            //Configure AutoFac  
            ContainerConfig.Initialize(GlobalConfiguration.Configuration);
        }

    }
}
