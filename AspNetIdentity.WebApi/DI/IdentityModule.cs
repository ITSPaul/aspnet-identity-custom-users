namespace AspNetIdentity.WebApi.DI
{
    using Autofac;
    using Identity.Providers;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security.Infrastructure;
    using Microsoft.Owin.Security.OAuth;
    using Models;
    using Models.Auth.Identity;

    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
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
