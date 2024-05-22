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
        var party = _dbContext.Parties.Include(p => p.Queue).Single(p => p.Id == id);

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

    public void AcceptSong(int songId)
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
        
        song.Accepted = true;
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

    public void DeleteParty(Party party)
    {
        _dbContext.Parties.Remove(party);
        _dbContext.SaveChanges();
    }

    public void UpdateParty(Guid partyId, string name, string details, string category, DateTime date)
    {
        var partyToUpdate = _dbContext.Parties.Single(p => p.Id == partyId);

        if (partyToUpdate == null)
        {
            throw new Exception("Party doesn't exist");
        }

        partyToUpdate.Name = name;
        partyToUpdate.Details = details;
        partyToUpdate.Category = category;
        partyToUpdate.Date = date;

        _dbContext.SaveChanges();
    }

    public void DeleteSong(Guid partyId, int songId)
    {
        var party = _dbContext.Parties.Include(party => party.Queue).Single(p => p.Id == partyId);

        var songToDelete = party.Queue.Single(song => song.Id == songId);

        party.Queue.Remove(songToDelete);

        _dbContext.Entry(songToDelete).State = EntityState.Deleted;

        _dbContext.SaveChanges();
    }
}