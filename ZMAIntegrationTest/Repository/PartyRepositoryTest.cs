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
    public void GetParties_ReturnsAllParties()
    {
        var result = _partyRepository.GetParties().ToList();
        
        Assert.Equal(2, result.Count);
        Assert.Equal("test", result[0].Category);
    }

    [Fact]
    public async Task CreateParty_AddsNewPartyToDb()
    {
        var party = new Party()
            { Category = "test1", Date = DateTime.Now, Details = "test", Name = "test1", Host = _fixture.TestHost };
        
        var result = await _partyRepository.CreatePartyAsync(party);
        
        Assert.Equal("test1", result.Name);
    }

    [Fact]
    public void GetParty_ReturnsCorrectPartyBasedOnId()
    {
        var result = _partyRepository.GetParty(_fixture.TestId);
        
        Assert.Equal(_fixture.TestId, result.Id);
    }

    [Fact]
    public async Task RequestSong_AddsSongToDatabase()
    {
        var song = new Song() { Id = 1, Accepted = false, RequestTime = DateTime.Now };

        var result = await _partyRepository.RequestSongAsync(song, _fixture.TestId);
        
        Assert.Equal(song.Id, result.Id);
    }
}