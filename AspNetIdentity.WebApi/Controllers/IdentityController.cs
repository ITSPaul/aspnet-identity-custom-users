namespace AspNetIdentity.WebApi.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Http;

    using AspNetIdentity.WebApi.Services;
    using Models.Auth.Identity;

    [RoutePrefix("api/identity")]
    public class IdentityController : BaseApiController
    {
        private readonly IUserService userService;
        private readonly XUserManager userManager;
        private readonly XRoleManager roleManager;

        public IdentityController(IUserService userService, XUserManager userManager, XRoleManager roleManager)
        {
            this.userService = userService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [Authorize]
        [HttpGet]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUser(string username)
        {
            var userS = await this.userService.GetUserAsync(username);
            if (userS == null)
            {
                return this.NotFound();
            }
            var userM = await this.userManager.FindByNameAsync(username);
            var result = new
            {
                username = userS.UserName,
                firstName = userS.FirstName,
                lastName = userS.LastName,
                date = userS.CreationDate,
                email = userM.Email,
                id = userM.Id,
                same = userM.Id == userS.Id
            };

            return this.Ok(result);
        }

        [HttpGet]
        [Route("userUnprotected/{username}")]
        public async Task<IHttpActionResult> GetUserUnprotected(string username)
        {
            var userS = await this.userService.GetUserAsync(username);
            if (userS == null)
            {
                return this.NotFound();
            }
            var userM = await this.userManager.FindByNameAsync(username);
            var result = new
            {
                username = userS.UserName,
                firstName = userS.FirstName,
                lastName = userS.LastName,
                date = userS.CreationDate,
                email = userM.Email,
                id = userM.Id,
                same = userM.Id == userS.Id
            };

            return this.Ok(result);
        }

        [Route("role/{id}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(long id)
        {
            var role = await this.roleManager.FindByIdAsync(id);

            if (role != null)
            {
                return Ok(role);
            }

            return NotFound();
        }

        [Route("roles", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = this.roleManager.Roles;

            return Ok(roles?.Select(r => new {
                Id = r.Id,
                Name = r.Name
            }));
        }

        [Authorize]
        [Route("claims", Name = "GetClaims")]
        public IHttpActionResult GetClaims()
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
