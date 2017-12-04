namespace AspNetIdentity.WebApi
{
    using System.Reflection;
    using System.Web.Http;

    using AspNetIdentity.WebApi.DI;
    using AspNetIdentity.WebApi.Models;

    using Autofac;
    using Autofac.Integration.WebApi;
    using Identity.Providers;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security.Infrastructure;
    using Microsoft.Owin.Security.OAuth;
    using Models.Auth.Identity;

    internal static class ContainerConfig
    {
        internal static IContainer Container;

        internal static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        private static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register your Web API controllers.  
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<XAppDbContext>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterModule<ModelsModule>();
            builder.RegisterModule<ServicesModule>();

            RegisterUserIdentityServices(builder);
            
            //Set the dependency resolver to be Autofac.  
            Container = builder.Build();

            return Container;
        }

        private static void RegisterUserIdentityServices(ContainerBuilder builder)
        {
            builder.Register<UserStore<XUser, XRole, long, XLogin, XUserRole, XClaim>>(
                    c => new UserStore<XUser, XRole, long, XLogin, XUserRole, XClaim>(c.Resolve<XAppDbContext>()))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.Register<RoleStore<XRole, long, XUserRole>>(
                    c => new RoleStore<XRole, long, XUserRole>(c.Resolve<XAppDbContext>()))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Register<IdentityFactoryOptions<XUserManager>>(c => new IdentityFactoryOptions<XUserManager>()
            {
                DataProtectionProvider = Startup.DataProtectionProvider //new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("AspNetIdentityTest")
            }).InstancePerLifetimeScope();

            builder.Register<IdentityFactoryOptions<XRoleManager>>(c => new IdentityFactoryOptions<XRoleManager>()
            {
                DataProtectionProvider = Startup.DataProtectionProvider //new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("AspNetIdentityTest")
            }).InstancePerLifetimeScope();

            builder.RegisterType<XUserManager>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<XRoleManager>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<CustomOAuthProvider>().As<IOAuthAuthorizationServerProvider>().InstancePerLifetimeScope();
            builder.RegisterType<WebApi.Identity.Providers.RefreshTokenProvider>().As<IAuthenticationTokenProvider>()
                .InstancePerLifetimeScope();
        }
    }
}
