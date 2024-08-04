using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ZMA.Hubs;
using ZMA.Model;
using ZMA.Services.Repositories;
using Host = ZMA.Model.Host;
using ILogger = ZMA.Utility.ILogger;

namespace ZMA.Controllers;

[ApiController]
[Route("[controller]")]
public class SongController(ISongRepository songRepository, UserManager<Host> userManager,
    ILogger logger, IHubContext<SongRequestHub> hubContext) : ControllerBase
{
    
    [HttpPost("RequestSong")]
    public async Task<ActionResult> RequestSong([Required] string title, [Required] Guid partyId)
    {
        try
        {
            var song = new Song() { Title = title, RequestTime = DateTime.UtcNow, Accepted = false, PartyId = partyId};
            
            var requestedSong = await songRepository.RequestSong(song);

            await hubContext.Clients.All.SendAsync("receiveSongRequestUpdate", requestedSong);

            return Ok(requestedSong);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest("Requesting song failed.");
        }
    }
    
    
    [HttpPatch("AcceptSong"), Authorize(Roles = "Host")]
    public async Task<ActionResult> AcceptSong([Required] int songId)
    {
        try
        {
            await songRepository.AcceptSong(songId);

            return Ok("Song accepted by host.");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest("Accepting song failed.");
        }
    }
    
    [HttpGet("GetSongs"), Authorize(Roles = "Host")]
    public async Task<ActionResult<ICollection<Song>>> GetSongs([Required] Guid partyId)
    {
        try
        {
            return Ok(await songRepository.GetSongs(partyId));
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Retrieving songs failed.");
        }
    }

    [HttpDelete("DenySong"), Authorize(Roles = "Host")]
    public async Task<ActionResult> DenySong([Required] int songId)
    {
        try
        {
            await songRepository.DeleteSong(songId);
            
            logger.LogInfo("Host deleted song.");

            return Ok("Song denied");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return BadRequest("Deleting song failed.");
        }
    }
}