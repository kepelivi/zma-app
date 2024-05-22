using Microsoft.AspNetCore.Identity;
using Host = ZMA.Model.Host;

namespace ZMA.Services.Authentication;

public class AuthService(UserManager<Host> userManager, ITokenService tokenService) : IAuthService
{
    public async Task<AuthResult> RegisterAsync(string email, string username, string name, string password, string role)
    {
        var host = new Host() { UserName = username, Email = email, Name = name};
        var result = await userManager.CreateAsync(host, password);

        if (!result.Succeeded)
        {
            return FailedRegistration(result, email, username);
        }
        
        await userManager.AddToRoleAsync(host, role);
        var newUser = userManager.FindByEmailAsync(email);
        return new AuthResult(true, email, username, newUser.Result.Id,"");
    }

    private static AuthResult FailedRegistration(IdentityResult result, string email, string username)
    {
        var authResult = new AuthResult(false, email, username, "","");

        foreach (var error in result.Errors)
        {
            authResult.ErrorMessages.Add(error.Code, error.Description);
        }

        return authResult;
    }
    
    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var managedUser = await userManager.FindByEmailAsync(email);

        if (managedUser == null)
        {
            return InvalidEmail(email);
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(managedUser, password);
        
        if (!isPasswordValid)
        {
            return InvalidPassword(email, managedUser.UserName);
        }


        var roles = await userManager.GetRolesAsync(managedUser);
        var accessToken = tokenService.CreateToken(managedUser, roles[0]);

        return new AuthResult(true, managedUser.Email, managedUser.UserName,managedUser.Id, accessToken);
    }

    private static AuthResult InvalidEmail(string email)
    {
        var result = new AuthResult(false, email, "", "","");
        result.ErrorMessages.Add("Bad credentials", "Invalid email");
        return result;
    }

    private static AuthResult InvalidPassword(string email, string userName)
    {
        var result = new AuthResult(false, email, userName, "","");
        result.ErrorMessages.Add("Bad credentials", "Invalid password");
        return result;
    }
}