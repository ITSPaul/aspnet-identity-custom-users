namespace AspNetIdentity.WebApi.Services
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using AspNetIdentity.WebApi.Models;
    using Models.Auth.Identity;

    public class UserService : IUserService
    {
        private readonly XAppDbContext db;
        private readonly XUserManager userManager;

        public UserService(XAppDbContext dbContext, XUserManager userManager)
        {
            this.db = dbContext;
            this.userManager = userManager;
        }
        
        public async Task<long> GetUserIdByNameAsync(string name)
        {
            var user = await this.GetUserAsync(name);
            return user?.Id ?? Int64.MinValue;
        }

        public async Task<XUser> GetUserAsync(string name)
        {
            return await this.userManager.FindByNameAsync(name);
//            return this.db.Users.FirstOrDefaultAsync(u => u.UserName == name);
        }
    }
}