using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Host = ZMA.Model.Host;
using ILogger = ZMA.Utility.ILogger;

namespace ZMA.Controllers;

public class HostController (UserManager<Host> userManager, ILogger logger) : ControllerBase
{
    [HttpGet("GetHost"), Authorize(Roles = "Host")]
    public async Task<ActionResult<Host>> GetHost()
    {
        try
        {
            var host = userManager.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
            
            logger.LogInfo($"Successfully retrieved host {host?.UserName}");

            return Ok(host);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Couldn't find host.");
        }
    }
}