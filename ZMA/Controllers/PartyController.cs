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
    [HttpPost("CreateParty"), Authorize(Roles = "Host")]
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
    public async Task<ActionResult<Party>> GetParty([Required] Guid id)
    {
        try
        {
            var party = partyRepository.GetParty(id);

            return Ok(party);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Retrieving party failed.");
        }
    }

    [HttpGet("GetParties"), Authorize(Roles = "Host")]
    public async Task<ActionResult<ICollection<Party>>> GetParties()
    {
        try
        {
            return Ok(partyRepository.GetParties());
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Retrieving parties failed.");
        }
    }

    [HttpPost("RequestSong")]
    public async Task<ActionResult> RequestSong([Required] string title, [Required] Guid partyId)
    {
        try
        {
            var song = new Song() { Title = title, RequestTime = DateTime.Now, Accepted = false };
            
            partyRepository.RequestSong(song, partyId);

            return Ok("Successfully requested song.");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Requesting song failed.");
        }
    }

    [HttpPatch("AcceptSong"), Authorize(Roles = "Host")]
    public async Task<ActionResult> AcceptSong([Required] int songId)
    {
        try
        {
            partyRepository.AcceptSong(songId);

            return Ok("Song accepted by host.");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Accepting song failed.");
        }
    }

    [HttpGet("GetSongs"), Authorize(Roles = "Host")]
    public async Task<ActionResult<ICollection<Song>>> GetSongs([Required] Guid partyId)
    {
        try
        {
            return Ok(partyRepository.GetSongs(partyId));
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Retrieving songs failed.");
        }
    }

    [HttpDelete("DenySong"), Authorize(Roles = "Host")]
    public async Task<ActionResult> DenySong([Required] Guid partyId, [Required] int songId)
    {
        try
        {
            partyRepository.DeleteSong(partyId, songId);
            
            logger.LogInfo("Host deleted song.");

            return Ok("Song denied");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Deleting song failed.");
        }
    }

    [HttpDelete("DeleteParty"), Authorize(Roles = "Host")]
    public async Task<ActionResult> DeleteParty([Required] Guid partyId)
    {
        try
        {
            var party = partyRepository.GetParty(partyId);
            
            partyRepository.DeleteParty(party);
            
            logger.LogInfo($"Party deleted - Name: {party.Name}, Date: {party.Date}");

            return Ok("Party deleted.");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Deleting party failed.");
        }
    }

    [HttpPatch("UpdateParty"), Authorize(Roles = "Host")]
    public async Task<ActionResult> UpdateParty([Required] Guid partyId, string name, string details, string category,
        DateTime date)
    {
        try
        {
            partyRepository.UpdateParty(partyId, name, details, category, date);
            
            logger.LogInfo($"Party updated - Id: {partyId}");

            return Ok("Party updated.");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Updating party failed.");
        }
    }
}