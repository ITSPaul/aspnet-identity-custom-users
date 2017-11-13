namespace AspNetIdentity.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    using AspNetIdentity.WebApi.Models;
    using AspNetIdentity.WebApi.Services;

    [RoutePrefix("api/values")]
    public class ValuesController : BaseApiController
    {
        private readonly IUserService userService;

        public ValuesController(IUserService userService)
        {
            this.userService = userService;
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [Route("Protected/{id}")]
        public IHttpActionResult Protected([FromUri]string id)
        {
            return Ok(new { id });
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
