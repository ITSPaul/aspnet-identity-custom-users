namespace AspNetIdentity.WebApi.Identity
{
    public interface IUserContext
    {
        string UserName { get; }

        string UserIdentity { get; }
    }
}
