namespace AspNetIdentity.WebApi.Identity.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AspNetIdentity.WebApi.Models;
    using AspNetIdentity.WebApi.Models.Auth.Identity;
    using AspNetIdentity.WebApi.Services;

    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OAuth;

    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private IUserService userService;

        public CustomOAuthProvider(IUserService userService)
        {
            this.userService = userService;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var clientId = string.Empty;
            var clientSecret = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                return Task.FromResult<object>(null);
            }

            var audience = AudiencesStore.FindAudience(context.ClientId);

            if (audience == null)
            {
                context.SetError("invalid_clientId", $"Invalid client_id '{context.ClientId}'");
                return Task.FromResult<object>(null);
            }

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
//            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
//            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "authorization", "content-type", "set-cookie" });

            var userManager = context.OwinContext.GetUserManager<XUserManager>();
            var roleManager = context.OwinContext.Get<XRoleManager>();

//            XUser user = userManager.Users.Where(x => x.Password == context.Password && x.UserName == context.UserName).FirstOrDefault();
            XUser user = await userManager.FindAsync(context.UserName, context.Password);
            
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

//            if (!user.IsActive)
//            {
//                context.SetError("account_locked", "Your account has been locked");
//                return;
//            }

            user.SecurityStamp = Guid.NewGuid().ToString();


            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
//            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
            oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user));
            oAuthIdentity.AddClaims(ExtendedClaimsProvider.CreateRolesBasedOnClaims(oAuthIdentity));

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "audience", (context.ClientId == null) ? string.Empty : context.ClientId
                }
            });

            var ticket = new AuthenticationTicket(oAuthIdentity, props);
            context.Validated(ticket);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["audience"];
            var currentClient = context.ClientId;

            // enforce client binding of refresh token
            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // chance to change authentication ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            // newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }
}