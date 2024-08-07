﻿using System.Net;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using ZMA.Contracts;
using ZMA.Model;

namespace ZMAIntegrationTest.Controllers;

[Collection("IntegrationTests")]
public class SongControllerTest
{
    private readonly ZMAWebApplicationFactory _app;
    private readonly HttpClient _client;
    private readonly RegistrationReq _hostRegReq;
    private readonly AuthReq _hostAuthReq;
    
    public SongControllerTest()
    {
        _app = new ZMAWebApplicationFactory();
        _client = _app.CreateClient();
        _hostRegReq = new RegistrationReq("host@gmail.com", "host", "Host", "Host!12346");
        _hostAuthReq = new AuthReq("host@gmail.com", "Host!12346");
    }

    [Fact]
    public async Task RequestSong_SuccessfullyAddsSong()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();
        
        var createParty = await _client.PostAsync(
            "/Party/CreateParty?name=test&details=test&category=test&date=2022-12-12",
            null);
        createParty.EnsureSuccessStatusCode();
        
        var getParties = await _client.GetAsync("/Party/GetParties");
        getParties.EnsureSuccessStatusCode();
        
        var parties = await getParties.Content.ReadFromJsonAsync<ICollection<Party>>();

        var id = parties.Single(p => p.Name == "test").Id;

        var song = new Song() { PartyId = id, Title = "test" };

        var requestSong = await _client.PostAsync(
            $"/Song/RequestSong?title={song.Title}&partyId={id}", null);
        requestSong.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task AcceptSong_SuccessfullyAcceptsSongRequest()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();
        
        var createParty = await _client.PostAsync(
            "/Party/CreateParty?name=test&details=test&category=test&date=2022-12-12",
            null);
        createParty.EnsureSuccessStatusCode();
        
        var getParties = await _client.GetAsync("/Party/GetParties");
        getParties.EnsureSuccessStatusCode();
        
        var parties = await getParties.Content.ReadFromJsonAsync<ICollection<Party>>();

        var id = parties.Single(p => p.Name == "test").Id;

        var song = new Song() { PartyId = id, Title = "test" };
        
        var requestSong = await _client.PostAsync(
            $"/Song/RequestSong?title={song.Title}&partyId={id}", null);
        requestSong.EnsureSuccessStatusCode();

        var requestedSong = await requestSong.Content.ReadFromJsonAsync<Song>();

        var acceptContent = JsonContent.Create(new {});
        
        var acceptSong = await _client.PatchAsync($"/Song/AcceptSong?songId={requestedSong.Id}", acceptContent);
        acceptSong.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetSongs_ReturnsAllSongs()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();

        var createParty = await _client.PostAsync(
            "/Party/CreateParty?name=test&details=test&category=test&date=2022-12-12",
            null);
        createParty.EnsureSuccessStatusCode();
        
        var getParties = await _client.GetAsync("/Party/GetParties");
        getParties.EnsureSuccessStatusCode();
        
        var parties = await getParties.Content.ReadFromJsonAsync<ICollection<Party>>();

        var id = parties.Single(p => p.Name == "test").Id;

        var song = new Song() { PartyId = id, Title = "test" };
        
        var requestSong = await _client.PostAsync(
            $"/Song/RequestSong?title={song.Title}&partyId={id}", null);
        requestSong.EnsureSuccessStatusCode();

        var getSongs = await _client.GetAsync($"Song/GetSongs?partyId={id}");
        getSongs.EnsureSuccessStatusCode();

        var songs = await getSongs.Content.ReadFromJsonAsync<ICollection<Song>>();
        
        Assert.Equal(1, songs.Count);
    }

    [Fact]
    public async Task GetSongs_ReturnsEmptyListWhen_SongsIsEmpty()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();
        
        var createParty = await _client.PostAsync(
            "/Party/CreateParty?name=test&details=test&category=test&date=2022-12-12",
            null);
        createParty.EnsureSuccessStatusCode();
        
        var getParties = await _client.GetAsync("/Party/GetParties");
        getParties.EnsureSuccessStatusCode();
        
        var parties = await getParties.Content.ReadFromJsonAsync<ICollection<Party>>();
        var id = parties.Single(p => p.Name == "test").Id;
        
        var getSongs = await _client.GetAsync($"Song/GetSongs?partyId={id}");

        var songs = await getSongs.Content.ReadFromJsonAsync<ICollection<Song>>();
        
        Assert.Equal(0, songs.Count);
    }

    [Fact]
    public async Task DenySong_DeletesSongFromDB()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();
        
        var createParty = await _client.PostAsync(
            "/Party/CreateParty?name=test&details=test&category=test&date=2022-12-12",
            null);
        createParty.EnsureSuccessStatusCode();
        
        var getParties = await _client.GetAsync("/Party/GetParties");
        getParties.EnsureSuccessStatusCode();
        
        var parties = await getParties.Content.ReadFromJsonAsync<ICollection<Party>>();

        var id = parties.Single(p => p.Name == "test").Id;

        var song = new Song() { PartyId = id, Title = "test" };

        var requestSong = await _client.PostAsync(
            $"/Song/RequestSong?title={song.Title}&partyId={id}", null);
        requestSong.EnsureSuccessStatusCode();
        
        var requestedSong = await requestSong.Content.ReadFromJsonAsync<Song>();

        var denySong = await _client.DeleteAsync($"Song/DenySong?songId={requestedSong.Id}");
        denySong.EnsureSuccessStatusCode();
    }
}