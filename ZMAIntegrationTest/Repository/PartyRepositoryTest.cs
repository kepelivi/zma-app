using Microsoft.EntityFrameworkCore;
using ZMA.Data;
using ZMA.Model;
using ZMA.Services.Repositories;

namespace ZMAIntegrationTest;

[Collection("IntegrationTests")]
public class PartyRepositoryTest : IDisposable
{
    public ZMAContext Context { get; private set; }
    public Guid TestId;
    public Host TestHost = new() {Name = "c", Id = Guid.NewGuid().ToString()};
    public IPartyRepository _partyRepository;

    public PartyRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<ZMAContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ZMAContext(options);
        
        SeedDatabase();

        _partyRepository = new PartyRepository(Context);
    }

    private void SeedDatabase()
    {
        Context.Users.Add(TestHost);
        Context.SaveChanges();

        var party = new Party
        {
            Category = "test", Date = DateTime.Now, Details = "test", Name = "test", Host = TestHost
        };

        Context.Parties.Add(party);
        Context.SaveChanges();
        
        TestId = party.Id;
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }

    [Fact]
    public async Task GetParties_ReturnsAllParties()
    {
        var result = await _partyRepository.GetParties();
        
        Assert.Equal("test", result.ToList()[0].Category);
    }

    [Fact]
    public async Task CreateParty_AddsNewPartyToDb()
    {
        var party = new Party()
            { Category = "test1", Date = DateTime.Now, Details = "test", Name = "test1", Host = TestHost };
        
        var result = await _partyRepository.CreateParty(party);
        
        Assert.Equal("test1", result.Name);
    }

    [Fact]
    public async Task GetParty_ReturnsCorrectPartyBasedOnId()
    {
        var result = await _partyRepository.GetParty(TestId);
        
        Assert.Equal(TestId, result.Id);
    }

    [Fact]
    public async Task UpdateParty_SuccessfullyUpdatesPartyEntity()
    {
        var result = _partyRepository.UpdateParty(TestId, "testt", "test", "test", DateTime.Now);
        
        Assert.Equal(true, result.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task DeleteParty_SuccessfullyDeletesPartyEntity()
    {
        var party = new Party() { Category = "test2", Date = DateTime.Now, Details = "test", Name = "test1", Host = TestHost };
        var createdParty = await _partyRepository.CreateParty(party);

        var result = _partyRepository.DeleteParty(createdParty);
        
        Assert.Equal(true, result.IsCompletedSuccessfully);
    }
}