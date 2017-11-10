namespace AspNetIdentity.WebApi.Services
{
    using System;

    public class UserService : IUserService
    {
        public long GetUserIdByName(string name)
        {
            return DateTime.Now.Ticks;
        }
    }
}