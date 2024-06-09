using Microsoft.EntityFrameworkCore;
using ZMA.Data;
using ZMA.Model;
using Exception = System.Exception;

namespace ZMA.Services.Repositories;

public class SongRepository : ISongRepository
{
    private readonly IConfigurationRoot _config;
    private readonly DbContextOptionsBuilder<ZMAContext> _optionsBuilder;
    private readonly ZMAContext _dbContext;
    
    public SongRepository(ZMAContext context)
    {
        _config =
            new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
        _optionsBuilder = new DbContextOptionsBuilder<ZMAContext>();
        _optionsBuilder.UseSqlServer(_config["ConnectionString"]);
        _dbContext = context;
    }
    
    public async Task<Song> RequestSong(Song song)
    {
        var party = await _dbContext.Parties.Include(p => p.Queue).FirstOrDefaultAsync(p => p.Id == song.PartyId);

        if (party == null)
        {
            throw new Exception("Party not found");
        }

        await _dbContext.Songs.AddAsync(song);
        await _dbContext.SaveChangesAsync();
        return song;
    }
    
    public async Task AcceptSong(int songId)
    {
        var song = await _dbContext.Songs.SingleAsync(s => s.Id == songId);

        if (song == null)
        {
            throw new Exception("Song not found.");
        }
        
        song.Accepted = true;
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<ICollection<Song>> GetSongs(Guid partyId)
    {
        var songs = await _dbContext.Songs.Where(s => s.PartyId == partyId).ToListAsync();

        if (songs == null || songs.Count == 0)
        {
            throw new Exception("Queue is empty");
        }

        return songs;
    }
    
    public async Task DeleteSong(int songId)
    {
        var songToDelete = await _dbContext.Songs.SingleAsync(s => s.Id == songId);

        _dbContext.Songs.Remove(songToDelete);

        _dbContext.Entry(songToDelete).State = EntityState.Deleted;

        await _dbContext.SaveChangesAsync();
    }
}