using ZMA.Contracts;

namespace ZMA.Services.Authentication;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(string email, string username, string name, string password, string role);
    Task<AuthResult> LoginAsync(string email, string password);
}