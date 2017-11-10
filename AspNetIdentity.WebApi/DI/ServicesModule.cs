namespace AspNetIdentity.WebApi.DI
{
    using AspNetIdentity.WebApi.Services;

    using Autofac;

    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
        }
    }
}
