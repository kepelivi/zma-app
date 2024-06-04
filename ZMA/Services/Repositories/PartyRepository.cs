using Microsoft.EntityFrameworkCore;
using ZMA.Data;
using ZMA.Model;

namespace ZMA.Services.Repositories;

public class PartyRepository : IPartyRepository
{
    private readonly IConfigurationRoot _config;
    private readonly DbContextOptionsBuilder<ZMAContext> _optionsBuilder;
    private readonly ZMAContext _dbContext;
    
    public PartyRepository(ZMAContext context)
    {
        _config =
            new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
        _optionsBuilder = new DbContextOptionsBuilder<ZMAContext>();
        _optionsBuilder.UseSqlServer(_config["ConnectionString"]);
        _dbContext = context;
    }

    public async Task<Party> CreateParty(Party party)
    {
        var host = await _dbContext.Users.FirstOrDefaultAsync(h => party.Host != null && h.Id == party.Host.Id);

        if (host == null)
        {
            throw new Exception("Host not found for creating a party.");
        }

        var createdParty = new Party { Name = party.Name, Host = host, Date = party.Date, Category = party.Category, Details = party.Details};

        await _dbContext.Parties.AddAsync(createdParty);
        await _dbContext.SaveChangesAsync();
        return createdParty;
    }
    
    public async Task<Party> GetParty(Guid id)
    {
        var party = await _dbContext.Parties.SingleAsync(p => p.Id == id);

        if (party == null)
        {
            throw new Exception("Party not found.");
        }

        return party;
    }

    public async Task<ICollection<Party>> GetParties()
    {
        var parties = await _dbContext.Parties.Include(party => party.Queue).ToListAsync();

        if (parties.Count == 0)
        {
            throw new Exception("No parties found.");
        }
        
        return parties;
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

    public async Task DeleteParty(Party party)
    {
        _dbContext.Parties.Remove(party);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateParty(Guid partyId, string name, string details, string category, DateTime date)
    {
        var partyToUpdate = await _dbContext.Parties.SingleAsync(p => p.Id == partyId);

        if (partyToUpdate == null)
        {
            throw new Exception("Party doesn't exist");
        }

        partyToUpdate.Name = name;
        partyToUpdate.Details = details;
        partyToUpdate.Category = category;
        partyToUpdate.Date = date;

        await _dbContext.SaveChangesAsync();
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