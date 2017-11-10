namespace AspNetIdentity.WebApi.DI
{
    using Autofac;

    public class ModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
//            builder.RegisterType<SomeAppContext>().InstancePerLifetimeScope();
        }
    }
}
