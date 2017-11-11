namespace Stork.Web.Providers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.Owin.Security;
    using WebApi.Identity.Providers;

    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string AudiencePropertyKey = "audience";

        private readonly string issuer = string.Empty;

        public CustomJwtFormat(string issuer)
        {
            this.issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string audienceId = data.Properties.Dictionary.ContainsKey(AudiencePropertyKey) ? data.Properties.Dictionary[AudiencePropertyKey] : null;

            //string audienceId = ConfigurationManager.AppSettings["auth:jwt:AudienceId"];
            //string symmetricKeyAsBase64 = ConfigurationManager.AppSettings["auth:jwt:AudienceSecret"];

            if (string.IsNullOrWhiteSpace(audienceId)) { 
                throw new InvalidOperationException("AuthenticationTicket. Properties does not include audience");
            }

            Audience audience = AudiencesStore.FindAudience(audienceId);
            string symmetricKeyAsBase64 = audience.Base64Secret;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.Default.GetBytes(symmetricKeyAsBase64));
            var signingKey = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            var claims = data.Identity.Claims;
            var notbefore = data.Properties.IssuedUtc.Value.UtcDateTime;
            var expires = data.Properties.ExpiresUtc.Value.UtcDateTime;

            var token = new JwtSecurityToken(issuer, audienceId, claims, notbefore, expires, signingKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

        public bool IsTokenValid(string protectedText)
        {
            var defaultAudience = AudiencesStore.DefaultAudience;
//            Audience audience = AudiencesStore.FindAudience(audienceId);
//            string symmetricKeyAsBase64 = audience.Base64Secret;

            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.Default.GetBytes(defaultAudience.Base64Secret));

            var validationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = defaultAudience.ClientId,
                ValidateLifetime = true,
                ValidateIssuer = true,
                RequireExpirationTime = true,
                IssuerSigningKey = securityKey
            };

            Microsoft.IdentityModel.Tokens.SecurityToken token = new JwtSecurityToken();
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(protectedText, validationParameters, out token);
                DateTime now = DateTime.UtcNow;
                bool isValid = true;

                if (now > token.ValidTo)
                {
                    isValid = false;
                }

                if (now < token.ValidFrom)
                {
                    isValid = false;
                }

                return principal.Identity.IsAuthenticated & isValid;
            }
            catch { }

            return false;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}