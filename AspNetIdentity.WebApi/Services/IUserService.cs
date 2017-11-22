namespace AspNetIdentity.WebApi.Services
{
    using System.Threading.Tasks;

    using AspNetIdentity.WebApi.Models;

    public interface IUserService
    {
        Task<long> GetUserIdByNameAsync(string name);

        Task<XUser> GetUserAsync(string name);
    }
}