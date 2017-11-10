namespace AspNetIdentity.WebApi.Services
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using AspNetIdentity.WebApi.Models;

    public class UserService : IUserService
    {
        private readonly XAppDbContext db;

        public UserService(XAppDbContext dbContext)
        {
            this.db = dbContext;
        }
        
        public long GetUserIdByName(string name)
        {
            var user = this.GetUserAsync(name);
            return user?.Id ?? Int64.MinValue;
        }

        public Task<XUser> GetUserAsync(string name)
        {
            return this.db.Users.FirstOrDefaultAsync(u => u.UserName == name);
        }
    }
}