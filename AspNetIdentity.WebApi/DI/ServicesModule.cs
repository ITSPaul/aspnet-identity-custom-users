namespace AspNetIdentity.WebApi.DI
{
    using AspNetIdentity.WebApi.Identity.Providers;
    using AspNetIdentity.WebApi.Services;

    using Autofac;

    using Microsoft.Owin.Security.Infrastructure;
    using Microsoft.Owin.Security.OAuth;

    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomOAuthProvider>().As<IOAuthAuthorizationServerProvider>().InstancePerLifetimeScope();
            builder.RegisterType<AspNetIdentity.WebApi.Identity.Providers.RefreshTokenProvider>().As<IAuthenticationTokenProvider>().InstancePerLifetimeScope();
        }
    }
}
