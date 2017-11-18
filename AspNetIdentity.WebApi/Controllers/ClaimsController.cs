namespace AspNetIdentity.WebApi.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Http;

    [RoutePrefix("api/claims")]
    public class ClaimsController : ApiController
    {
        // GET api/claims
        [Authorize]
        [HttpGet]
        public object Get()
        {
            var identity = this.User.Identity as ClaimsIdentity;

            var claims = from c in identity.Claims
                         select new
                                    {
                                        subject = c.Subject.Name,
                                        type = c.Type,
                                        value = c.Value
                                    };

            return Ok(claims);
        }

    }
}
