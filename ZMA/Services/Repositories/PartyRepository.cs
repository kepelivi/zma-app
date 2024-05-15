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
    
    public void CreateParty(Party party)
    {
        var host = _dbContext.Users.FirstOrDefault(h => party.Host != null && h.Id == party.Host.Id);

        if (host == null)
        {
            throw new Exception("Host not found for creating a party.");
        }

        var createdParty = new Party { Name = party.Name, Host = host, Date = party.Date, Category = party.Category, Details = party.Details};

        _dbContext.Parties.Add(createdParty);
        _dbContext.SaveChanges();
    }

    public Party GetParty(Guid id)
    {
        var party = _dbContext.Parties.Find(id);

        if (party == null)
        {
            throw new Exception("Party not found.");
        }

        return party;
    }

    public ICollection<Party> GetParties()
    {
        var parties = _dbContext.Parties.Include(party => party.Queue).ToList();

        if (parties.Count == 0)
        {
            throw new Exception("No parties found.");
        }
        
        return _dbContext.Parties.ToList();
    }

    public void RequestSong(Song song, Guid partyId)
    {
        var party = _dbContext.Parties.FirstOrDefault(p => p.Id == partyId);

        if (party == null)
        {
            throw new Exception("Party not found.");
        }
        
        party.Queue.Add(song);
        _dbContext.SaveChanges();
    }

    public void AcceptSong(int songId, bool accept)
    {
        var party = _dbContext.Parties.Include(party => party.Queue).FirstOrDefault(p => p.Queue.Any(s => s.Id == songId));

        if (party == null)
        {
            throw new Exception("Party not found.");
        }
        
        var song = party?.Queue.FirstOrDefault(s => s.Id == songId);

        if (song == null)
        {
            throw new Exception("Song not found.");
        }
        
        song.Accepted = accept;
        _dbContext.SaveChanges();
    }

    public ICollection<Song> GetSongs(Guid partyId)
    {
        var party = _dbContext.Parties.Include(party => party.Queue).Single(p => p.Id == partyId);

        if (party == null)
        {
            throw new Exception("Party doesn't exist");
        }

        var songs = party.Queue;

        if (songs == null)
        {
            throw new Exception("Queue is empty");
        }

        return songs.ToList();
    }
}