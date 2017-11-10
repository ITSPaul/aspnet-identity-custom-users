namespace AspNetIdentity.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    using AspNetIdentity.WebApi.Services;

    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
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

        [HttpGet]
        [Route("user")]
        public long GetUser()
        {
            return this.userService.GetUserIdByName(string.Empty);
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
