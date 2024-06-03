using Microsoft.EntityFrameworkCore;
using ZMA.Data;
using ZMA.Model;
using ZMA.Services.Repositories;
using ZMAIntegrationTest.Fixtures;

namespace ZMAIntegrationTest;

[Collection("IntegrationTests")]
public class PartyRepositoryTest : IClassFixture<DatabaseFixture>
{
    private ZMAContext _context;
    private IPartyRepository _partyRepository;
    private readonly DatabaseFixture _fixture = new();

    public PartyRepositoryTest(DatabaseFixture fixture)
    {
        _context = fixture.Context;
        _partyRepository = new PartyRepository(_context);
    }

    [Fact]
    public async Task GetParties_ReturnsAllParties()
    {
        var result = await _partyRepository.GetParties();
        
        Assert.Equal(2, result.Count);
        Assert.Equal("test", result.ToList()[0].Category);
    }

    [Fact]
    public async Task CreateParty_AddsNewPartyToDb()
    {
        var party = new Party()
            { Category = "test1", Date = DateTime.Now, Details = "test", Name = "test1", Host = _fixture.TestHost };
        
        var result = await _partyRepository.CreateParty(party);
        
        Assert.Equal("test1", result.Name);
    }

    [Fact]
    public async Task GetParty_ReturnsCorrectPartyBasedOnId()
    {
        var result = await _partyRepository.GetParty(_fixture.TestId);
        
        Assert.Equal(_fixture.TestId, result.Id);
    }

    [Fact]
    public async Task RequestSong_AddsSongToDatabase()
    {
        var song = new Song() { Id = 2, Title = "test2", Accepted = false, RequestTime = DateTime.Now };

        var result = await _partyRepository.RequestSong(song, _fixture.TestId);
        
        Assert.Equal(song.Id, result.Id);
    }

    [Fact]
    public async Task UpdateParty_SuccessfullyUpdatesPartyEntity()
    {
        var result = _partyRepository.UpdateParty(_fixture.TestId, "testt", "test", "test", DateTime.Now);
        
        Assert.Equal(true, result.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task DeleteParty_SuccessfullyDeletesPartyEntity()
    {
        var party = new Party() { Category = "test2", Date = DateTime.Now, Details = "test", Name = "test1", Host = _fixture.TestHost };
        var createdParty = await _partyRepository.CreateParty(party);

        var result = _partyRepository.DeleteParty(createdParty);
        
        Assert.Equal(true, result.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task GetSongs_ReturnsAllSongs()
    {
        var result = await _partyRepository.GetSongs(_fixture.TestId);
        
        Assert.Equal(1, result.ToList().Count);
    }

    [Fact]
    public async Task DeleteSong_SuccessfullyDeletesSongEntity()
    {
        
    }
}