namespace AspNetIdentity.WebApi
{
    using System.Configuration;

    public static class AppSettings
    {
        public static string AuthCustomJwtFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["auth:jwt:CustomJwtFormat"];
            }
        }
    }
}
