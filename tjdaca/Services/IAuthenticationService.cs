namespace tjdaca.Services
{
    public interface IAuthenticationService
    {
        Task<bool> Login(string username, string password);
    }
}
