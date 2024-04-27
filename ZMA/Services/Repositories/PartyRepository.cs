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
}