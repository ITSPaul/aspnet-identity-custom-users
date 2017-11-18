namespace AspNetIdentity.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    [Authorize]
    [RoutePrefix("api/secrets")]
    public class SecretsController : ApiController
    {
        // GET api/secrets
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "secret value1", "secret value2" };
        }
    }
}
