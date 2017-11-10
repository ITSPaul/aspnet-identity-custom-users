namespace AspNetIdentity.WebApi.Services
{
    public interface IUserService
    {
        long GetUserIdByName(string name);
    }
}