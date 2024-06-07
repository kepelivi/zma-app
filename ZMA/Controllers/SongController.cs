using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZMA.Model;
using ZMA.Services.Repositories;
using Host = ZMA.Model.Host;
using ILogger = ZMA.Utility.ILogger;

namespace ZMA.Controllers;

[ApiController]
[Route("[controller]")]
public class SongController(ISongRepository songRepository, UserManager<Host> userManager, ILogger logger) : ControllerBase
{
    
    [HttpPost("RequestSong")]
    public async Task<ActionResult> RequestSong([Required] string title, [Required] Guid partyId)
    {
        try
        {
            var song = new Song() { Title = title, RequestTime = DateTime.Now, Accepted = false };
            
            await songRepository.RequestSong(song, partyId);

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
            await songRepository.AcceptSong(songId);

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
            return Ok(await songRepository.GetSongs(partyId));
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
            await songRepository.DeleteSong(partyId, songId);
            
            logger.LogInfo("Host deleted song.");

            return Ok("Song denied");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return NotFound("Deleting song failed.");
        }
    }
}