namespace AspNetIdentity.WebApi.Identity
{
    using System.Security.Claims;
    using System.Web;

    public class UserContext : IUserContext
    {
        public UserContext()
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;

            // fallback to default when there isn't name or usename claims in the token (fixed automated API tests)
            this.UserName = identity?.FindFirst("name")?.Value ?? "unknown";

            this.UserIdentity = identity?.Name ?? "unknown";
        }

        public string UserName { get; private set; }

        public string UserIdentity { get; private set; }
    }
}
