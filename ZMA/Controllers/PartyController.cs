using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZMA.Model;
using ZMA.Services.Repositories;
using Host = ZMA.Model.Host;
using ILogger = ZMA.Utility.ILogger;

namespace ZMA.Controllers;

[ApiController]
[Route("[controller]")]
public class PartyController(IPartyRepository partyRepository, UserManager<Host> userManager, ILogger logger) : ControllerBase
{
    [HttpGet("CreateParty"), Authorize(Roles = "Host")]
    public async Task<ActionResult> CreateParty([Required] string name, string? details, [Required] string category, [Required] DateTime date)
    {
        try
        {
            var host = await userManager.Users.SingleAsync(h => User.Identity != null && h.UserName == User.Identity.Name);

            var party = new Party {Name = name, Host = host, Date = date, Details = details, Category = category};
            
            partyRepository.CreateParty(party);

            logger.LogInfo($"New party {party.Name} created at {DateTime.Now.ToShortDateString()} for the date of {party.Date}");
            
            return Ok(party);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Creating party failed.");
        }
    }

    [HttpGet("GetParty"), Authorize(Roles = "Host")]
    public async Task<ActionResult<Party>> GetParty([Required] int Id)
    {
        try
        {
            var party = partyRepository.GetParty(Id);

            return Ok(party);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Retrieving party failed.");
        }
    }
}