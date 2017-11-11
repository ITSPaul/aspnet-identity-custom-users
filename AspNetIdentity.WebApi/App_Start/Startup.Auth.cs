namespace AspNetIdentity.WebApi
{
    using AspNetIdentity.WebApi.Models;
    using AspNetIdentity.WebApi.Models.Auth.Identity;

    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security.Cookies;

    using Owin;

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(XAppDbContext.Create);
            app.CreatePerOwinContext<XUserManager>(XUserManager.Create);
            app.CreatePerOwinContext<XRoleManager>(XRoleManager.Create);

            // These two lines (app.UseCookieAuthentication and app.UseExternalSignInCookie) allows to use
            // this.Request.GetOwinContext().GetUserManager<XUserManager>() inside ApiControllers
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}
