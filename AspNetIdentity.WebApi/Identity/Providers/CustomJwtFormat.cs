namespace AspNetIdentity.WebApi.Identity.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;

    using Microsoft.IdentityModel.Tokens;
    using Microsoft.Owin.Security;

    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string AudiencePropertyKey = "audience";

        private const string IssuedAtClaimName = "iat";

        private const string ExpiryClaimName = "exp";

        private const string JwtIdClaimName = "jti";

        private readonly string issuer = string.Empty;

        private static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly List<string> allowedAudiences = new List<string>();

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

            string audienceId = AudiencesStore.DefaultAudience.AudienceId; //data.Properties.Dictionary.ContainsKey(AudiencePropertyKey) ? data.Properties.Dictionary[AudiencePropertyKey] : null;
            //string audienceId = data.Properties.Dictionary.ContainsKey(AudiencePropertyKey) ? data.Properties.Dictionary[AudiencePropertyKey] : null;

            //string audienceId = ConfigurationManager.AppSettings["auth:jwt:AudienceId"];
            //string symmetricKeyAsBase64 = ConfigurationManager.AppSettings["auth:jwt:AudienceSecret"];

            if (string.IsNullOrWhiteSpace(audienceId)) { 
                throw new InvalidOperationException("AuthenticationTicket. Properties does not include audience");
            }

            Audience audience = AudiencesStore.FindAudience(audienceId);
            var securityKey = GetSymmetricSecurityKey(audience);
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            var claims = data.Identity.Claims;
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            if (!issued.HasValue || !expires.HasValue)
            {
                return null;
            }

            var token = new JwtSecurityToken(issuer, audienceId, claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingCredentials);
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
                ValidAudience = defaultAudience.AudienceId,
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
            if (string.IsNullOrWhiteSpace(protectedText))
            {
                throw new ArgumentNullException("protectedText");
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(protectedText) as JwtSecurityToken;

            if (token == null)
            {
                throw new ArgumentOutOfRangeException("protectedText", "Invalid JWT Token");
            }

            Audience audience = AudiencesStore.DefaultAudience;
            string audienceId = audience.AudienceId;
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = GetSymmetricSecurityKey(audience),
                ValidAudiences = new[] { audienceId },
                ValidateIssuer = true,
                ValidIssuer = this.issuer,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;

            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(protectedText, validationParameters, out validatedToken);
            var claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;

            var authenticationExtra = new AuthenticationProperties(new Dictionary<string, string>());
            if (claimsIdentity.Claims.Any(c => c.Type == ExpiryClaimName))
            {
                string expiryClaim = (from c in claimsIdentity.Claims where c.Type == ExpiryClaimName select c.Value).Single();
                authenticationExtra.ExpiresUtc = epoch.AddSeconds(Convert.ToInt64(expiryClaim, CultureInfo.InvariantCulture));
            }

            if (claimsIdentity.Claims.Any(c => c.Type == IssuedAtClaimName))
            {
                string issued = (from c in claimsIdentity.Claims where c.Type == IssuedAtClaimName select c.Value).Single();
                authenticationExtra.IssuedUtc = epoch.AddSeconds(Convert.ToInt64(issued, CultureInfo.InvariantCulture));
            }

            var returnedIdentity = new ClaimsIdentity(claimsIdentity.Claims, "JWT");

            return new AuthenticationTicket(returnedIdentity, authenticationExtra);
        }

        private static SymmetricSecurityKey GetSymmetricSecurityKey(Audience audience)
        {
            return new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.Default.GetBytes(audience.Base64Secret));
        }
    }
}