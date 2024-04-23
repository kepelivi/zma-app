using Microsoft.AspNetCore.Identity;

namespace ZMA.Services.Authentication;

public interface ITokenService
{
    public string CreateToken(IdentityUser user, string role);
}