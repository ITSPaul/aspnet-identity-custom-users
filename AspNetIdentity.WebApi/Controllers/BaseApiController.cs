namespace AspNetIdentity.WebApi.Controllers
{
    using Microsoft.AspNet.Identity;
    using System.Web.Http;
    using System.Net.Http;
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

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
