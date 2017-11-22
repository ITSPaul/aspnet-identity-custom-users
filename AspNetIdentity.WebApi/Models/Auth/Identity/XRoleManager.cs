namespace AspNetIdentity.WebApi.Models.Auth.Identity
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;

    public class XRoleManager : RoleManager<XRole, long>
    {
        public XRoleManager(IRoleStore<XRole, long> roleStore, IdentityFactoryOptions<XRoleManager> options) : base(roleStore)
        {
        }

        public static XRoleManager Create(IdentityFactoryOptions<XRoleManager> options, IOwinContext context)
        {
            return new XRoleManager(new RoleStore<XRole, long, XUserRole>(context.Get<XAppDbContext>()), options);
        }
    }
}
