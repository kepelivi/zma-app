using Microsoft.AspNetCore.Mvc;
using ZMA.Contracts;
using ZMA.Services.Authentication;
using ILogger = ZMA.Utility.ILogger;

namespace ZMA.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authenticationService, ILogger logger, IConfiguration config) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<ActionResult<RegistrationRes>> RegisterHost(RegistrationReq request)
    {
        var result = await authenticationService.RegisterAsync(request.Email, request.Username, request.Name, request.Password,
            config["HostRole"] ?? Environment.GetEnvironmentVariable("HOSTROLE"));
        
        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }
        
        logger.LogInfo($"New host registered at {DateTime.Now.ToShortDateString()} - email: {result.Email}, username: {result.UserName}");

        return CreatedAtAction(nameof(RegisterHost), new RegistrationRes(result.Email, result.UserName));
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult> Authenticate([FromBody] AuthReq request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await authenticationService.LoginAsync(request.Email, request.Password);
        
        if (!result.Success)
        {
            AddErrors(result);
            return BadRequest(ModelState);
        }
        
        logger.LogInfo($"Host logged in - email: {result.Email}, username: {result.UserName}");
        
        var secure = config.GetValue<bool>("CookieSettings:Secure", true);
        var sameSite = config.GetValue("CookieSettings:SameSite", "None");

        Response.Cookies.Append("Host", result.Token, new CookieOptions()
        {
            HttpOnly = false,
            Secure = secure,
            SameSite = Enum.TryParse<SameSiteMode>(sameSite, out var sameSiteMode) ? sameSiteMode : SameSiteMode.None
        });

        return Ok();
    }
    
    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("User");
        return Ok();
    }
    
    private void AddErrors(AuthResult result)
    {
        foreach (var error in result.ErrorMessages)
        {
            ModelState.AddModelError(error.Key, error.Value);
        }
    }
}