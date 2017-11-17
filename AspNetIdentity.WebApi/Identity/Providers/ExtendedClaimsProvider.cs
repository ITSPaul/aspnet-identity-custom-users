namespace AspNetIdentity.WebApi.Identity.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using AspNetIdentity.WebApi.Models;

    public static class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(XUser user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(CreateClaim("UserId", user.Id.ToString()));
            claims.Add(CreateClaim("FirstName", user.FirstName));
            claims.Add(CreateClaim("LastName", user.LastName));

            return claims;
        }

        public static IEnumerable<Claim> CreateRolesBasedOnClaims(ClaimsIdentity identity)
        {
            return Enumerable.Empty<Claim>();
            //            List<Claim> claims = new List<Claim>();
            //
            //            //TODO: correct implementation
            //            if (identity.HasClaim(c => c.Type == "FTE" && c.Value == "1") &&
            //                identity.HasClaim(ClaimTypes.Role, "Admin"))
            //            {
            //                claims.Add(new Claim(ClaimTypes.Role, "IncidentResolvers"));
            //            }
            //
            //            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

    }
}
