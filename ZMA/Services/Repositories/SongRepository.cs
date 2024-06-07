using Microsoft.EntityFrameworkCore;
using ZMA.Data;
using ZMA.Model;

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
    
    public async Task<Song> RequestSong(Song song, Guid partyId)
    {
        var party = await _dbContext.Parties.Include(p => p.Queue).FirstOrDefaultAsync(p => p.Id == partyId);

        if (party == null)
        {
            throw new Exception("Party not found.");
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
        var party = await _dbContext.Parties.Include(party => party.Queue).SingleAsync(p => p.Id == partyId);

        if (party == null)
        {
            throw new Exception("Party doesn't exist");
        }

        var songs = await _dbContext.Songs.ToListAsync();

        if (songs == null)
        {
            throw new Exception("Queue is empty");
        }

        return songs;
    }
    
    public async Task DeleteSong(Guid partyId, int songId)
    {
        var party = await _dbContext.Parties.Include(party => party.Queue).SingleAsync(p => p.Id == partyId);

        var songToDelete = await _dbContext.Songs.SingleAsync(s => s.Id == songId);

        party.Queue.Remove(songToDelete);

        _dbContext.Entry(songToDelete).State = EntityState.Deleted;

        await _dbContext.SaveChangesAsync();
    }
}