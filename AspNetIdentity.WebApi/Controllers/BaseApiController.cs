namespace AspNetIdentity.WebApi.Controllers
{
    using System.Web.Http;
    using System.Web;

    using Microsoft.AspNet.Identity.Owin;

    using AspNetIdentity.WebApi.Models.Auth.Identity;

    public class BaseApiController : ApiController
    {
        private XUserManager appUserManager = null;
        private XRoleManager appRoleManager = null;

        protected XUserManager AppUserManager
        {
            get
            {
                if (this.appUserManager == null)
                {
                    //// This works only when there are following lines
                    //// - app.UseCookieAuthentication(new CookieAuthenticationOptions());
                    //// - app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
                    //// added in the Startup.Auth
                    //this.appUserManager = this.Request.GetOwinContext().GetUserManager<XUserManager>(); //using System.Net.Http;

                    this.appUserManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<XUserManager>();
                }

                return appUserManager;
            }
        }

        protected XRoleManager AppRoleManager
        {
            get
            {
                if (this.appRoleManager == null)
                {
                    //// This works only when there are following lines
                    //// - app.UseCookieAuthentication(new CookieAuthenticationOptions());
                    //// - app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
                    //// added in the Startup.Auth
                    //this.appRoleManager = this.Request.GetOwinContext().GetUserManager<XRoleManager>(); //using System.Net.Http;
                    this.appRoleManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<XRoleManager>();
                }

                return this.appRoleManager;
            }
        }
    }
}
