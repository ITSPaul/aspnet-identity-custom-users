using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetIdentity.WebApi.Identity.Providers
{
    using System.Configuration;

    public static class JwtConfigurationProvider
    {
        public static string Issuer
        {
            get { return ConfigurationManager.AppSettings["auth:jwt:Issuer"]; }
        }
        public static string AudienceName
        {
            get { return ConfigurationManager.AppSettings["auth:jwt:AudienceName"]; }
        }
        public static string AudienceId
        {
            get { return ConfigurationManager.AppSettings["auth:jwt:AudienceId"]; }
        }
        public static string AudienceBase64Secret
        {
            get { return ConfigurationManager.AppSettings["auth:jwt:AudienceSecret"]; }
        }
        public static TimeSpan AccessTokenExpireTimeSpan
        {
            get { return TimeSpan.FromDays(1); }
        }
        public static DateTimeOffset RefreshTokenExpiresUtc
        {
            get { return DateTime.UtcNow.AddHours(2); }
        }
    }
}
