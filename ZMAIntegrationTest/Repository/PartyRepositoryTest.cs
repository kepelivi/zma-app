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
    public async Task RequestSong_AddsSongToDatabase()
    {
        var song = new Song() { Id = 2, Title = "test2", Accepted = false, RequestTime = DateTime.Now };

        var result = await _partyRepository.RequestSong(song, TestId);
        
        Assert.Equal(song.Id, result.Id);
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

    [Fact]
    public async Task GetSongs_ReturnsAllSongs()
    {
        var song = new Song() { Id = 3, Title = "test2", Accepted = false, RequestTime = DateTime.Now };
        var song2 = new Song() { Id = 4, Title = "test2", Accepted = false, RequestTime = DateTime.Now };

        await _partyRepository.RequestSong(song, TestId);
        await _partyRepository.RequestSong(song2, TestId);
        
        var result = await _partyRepository.GetSongs(TestId);
        
        Assert.Equal(2, result.ToList().Count);
    }

    [Fact]
    public async Task DeleteSong_SuccessfullyDeletesSongEntity()
    {
        var song = new Song() { Id = 4, Title = "test2", Accepted = false, RequestTime = DateTime.Now };

        await _partyRepository.RequestSong(song, TestId);

        var result = _partyRepository.DeleteSong(TestId, song.Id);
        
        Assert.Equal(true, result.IsCompletedSuccessfully);
    }

    [Fact]
    public async void AcceptSong_SuccessfullySetsAcceptedProperty()
    {
        var song = new Song() { Id = 5, Title = "test2", Accepted = false, RequestTime = DateTime.Now };

        await _partyRepository.RequestSong(song, TestId);

        await _partyRepository.AcceptSong(song.Id);

        var parties = await _partyRepository.GetSongs(TestId);

        var result = parties.ToList()[0].Accepted;
        
        Assert.Equal(true, result);
    }
}