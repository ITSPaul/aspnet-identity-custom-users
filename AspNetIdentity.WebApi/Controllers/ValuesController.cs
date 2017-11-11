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

        [HttpGet]
        [Route("user/{username?}")]
        public async Task<IHttpActionResult> GetUser(string username = "krzyhook")
        {
            var user = await this.userService.GetUserAsync(username);  //this.userService.GetUserIdByName(string.Empty);
            if (user == null)
            {
                return this.NotFound();
            }
            var u2 = await this.AppUserManager.FindByNameAsync("SuperPowerUser");
            var r = await this.AppRoleManager.FindByNameAsync("admin");
            var result = new
            {
                username = user.UserName,
                firstName = user.FirstName,
                lastName = user.LastName,
                date = user.CreationDate,
                email = u2.Email,
                role = r.Name
            };


            return this.Ok(result);
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
