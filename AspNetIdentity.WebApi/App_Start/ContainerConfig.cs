namespace AspNetIdentity.WebApi
{
    using System.Reflection;
    using System.Web.Http;

    using AspNetIdentity.WebApi.DI;
    using AspNetIdentity.WebApi.Models;

    using Autofac;
    using Autofac.Integration.WebApi;

    internal static class ContainerConfig
    {
        internal static IContainer Container;

        internal static IContainer Initialize(HttpConfiguration config)
        {
            return Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        private static IContainer Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            return container;
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register your Web API controllers.  
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<XAppDbContext>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterModule<ModelsModule>();
            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule<IdentityModule>();
            
            //Set the dependency resolver to be Autofac.  
            Container = builder.Build();

            return Container;
        }
    }
}
