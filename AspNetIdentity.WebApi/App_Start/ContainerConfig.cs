namespace AspNetIdentity.WebApi
{
    using System.Reflection;
    using System.Web.Http;

    using AspNetIdentity.WebApi.DI;

    using Autofac;
    using Autofac.Integration.WebApi;

    internal static class ContainerConfig
    {
        internal static IContainer Container;

        internal static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        internal static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register your Web API controllers.  
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule<ModelsModule>();
            builder.RegisterModule<ServicesModule>();

            //Set the dependency resolver to be Autofac.  
            Container = builder.Build();

            return Container;
        }

    }
}
