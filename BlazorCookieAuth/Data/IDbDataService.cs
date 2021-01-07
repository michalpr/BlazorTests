namespace BlazorCookieAuth.Data
{
    public interface IDbDataService
    {
        UserItem ValidateUser(string username, string password);
    }
}
