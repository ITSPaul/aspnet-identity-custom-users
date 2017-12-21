namespace AspNetIdentity.WebApi
{
    using System;

    using AspNetIdentity.WebApi.Identity.Providers;
    using AspNetIdentity.WebApi.Models;
    using AspNetIdentity.WebApi.Models.Auth.Identity;

    using Autofac;

    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;
    using Microsoft.Owin.Cors;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.DataHandler;
    using Microsoft.Owin.Security.DataProtection;
    using Microsoft.Owin.Security.Infrastructure;
    using Microsoft.Owin.Security.OAuth;

    using Owin;

    public partial class Startup
    {
        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }

        public static void ConfigureAuth(IAppBuilder app, IContainer container)
        {
            // Configure the db context and user manager to use a single instance per request
            //app.CreatePerOwinContext(XAppDbContext.Create);
            //app.CreatePerOwinContext<XUserManager>(XUserManager.Create);
            //app.CreatePerOwinContext<XRoleManager>(XRoleManager.Create);

            app.UseCors(CorsOptions.AllowAll);

            DataProtectionProvider = app.GetDataProtectionProvider();

            ConfigureOAuthTokenGeneration(app, container);

            //// Api controllers with an [Authorize] attribute will be validated with JWT
            ConfigureOAuthTokenConsumption(app, container);
        }

        private static void ConfigureOAuthTokenGeneration(IAppBuilder app, IContainer container)
        {
            var issuer = JwtConfigurationProvider.Issuer;
            string audienceId = AudiencesStore.DefaultAudience.AudienceId;

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
#if DEBUG
                AllowInsecureHttp = true,
#endif
                TokenEndpointPath = new PathString("/auth/token"),
                AccessTokenExpireTimeSpan = JwtConfigurationProvider.AccessTokenExpireTimeSpan,
                AuthorizationCodeExpireTimeSpan = JwtConfigurationProvider.AccessTokenExpireTimeSpan,
                Provider = container.Resolve<IOAuthAuthorizationServerProvider>(),
                AccessTokenFormat = new JwtTokenFormat(new JwtTokenOptions(audienceId, issuer)),
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = "Bearer",
                RefreshTokenFormat = new TicketDataFormat(app.CreateDataProtector(typeof(OAuthAuthorizationServerMiddleware).Namespace, "Refresh_Token", "v1")),
                RefreshTokenProvider = container.Resolve<IAuthenticationTokenProvider>()
            });
        }

        private static void ConfigureOAuthTokenConsumption(IAppBuilder app, IContainer container)
        {
            var issuer = JwtConfigurationProvider.Issuer;
            string audienceId = AudiencesStore.DefaultAudience.AudienceId;

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                AccessTokenFormat = new JwtTokenFormat(new JwtTokenOptions(audienceId, issuer)),
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = "Bearer",
                Description = new AuthenticationDescription()
            });
        }
    }
}
