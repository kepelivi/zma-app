using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;
using ZMA.Contracts;
using ZMA.Model;

namespace ZMAIntegrationTest.Controllers;

[Collection("IntegrationTests")]
public class PartyControllerTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ZMAWebApplicationFactory _app;
    private readonly HttpClient _client;
    private readonly RegistrationReq _hostRegReq;
    private readonly AuthReq _hostAuthReq;
    
    public PartyControllerTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _app = new ZMAWebApplicationFactory();
        _client = _app.CreateClient();
        _hostRegReq = new RegistrationReq("host@gmail.com", "host", "Host", "Host!12346");
        _hostAuthReq = new AuthReq("host@gmail.com", "Host!12346");
    }

    [Fact]
    public async Task CreateParty_SuccessFullyCreatesParty()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();
        
        var createParty = await _client.PostAsync(
            $"/Party/CreateParty?name=test&details=test&category=test&date=2022-12-12",
            null);
        
        if (!createParty.IsSuccessStatusCode)
        {
            var errorContent = await createParty.Content.ReadAsStringAsync();
            _testOutputHelper.WriteLine($"Error Content: {errorContent}");
        }
        
        createParty.EnsureSuccessStatusCode();

        var createdParty = await createParty.Content.ReadFromJsonAsync<Party>();
        
        Assert.Equal("test", createdParty.Name);
    }

    [Fact]
    public async Task GetParty_ReturnsCorrectParty()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();
        
        var createParty = await _client.PostAsync(
            $"/Party/CreateParty?name=test&details=test&category=test&date=2022-12-12",
            null);
        
        createParty.EnsureSuccessStatusCode();

        var getParties = await _client.GetAsync("/Party/GetParties");
        getParties.EnsureSuccessStatusCode();

        var parties = await getParties.Content.ReadFromJsonAsync<ICollection<Party>>();

        var id = parties.Single(p => p.Name == "test").Id;

        var getParty = await _client.GetAsync($"/Party/GetParty?id={id}");
        getParty.EnsureSuccessStatusCode();

        var result = await getParty.Content.ReadFromJsonAsync<Party>();
        
        Assert.Equal("test", result.Name);
    }

    [Fact]
    public async Task GetParties_ReturnsAllParties()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();
        
        var createParty = await _client.PostAsync(
            $"/Party/CreateParty?name=test&details=test&category=test&date=2022-12-12",
            null);
        createParty.EnsureSuccessStatusCode();

        var getParties = await _client.GetAsync("/Party/GetParties");
        getParties.EnsureSuccessStatusCode();

        var parties = await getParties.Content.ReadFromJsonAsync<ICollection<Party>>();
        
        Assert.Equal(1, parties.Count);
    }

    [Fact]
    public async Task DeleteParty_SuccessfullyDeletesParty()
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

        var deleteParty = await _client.DeleteAsync($"/Party/DeleteParty?partyId={id}");
        deleteParty.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateParty_SuccessfullyUpdatesParty()
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

        var updateParty =
            await _client.PatchAsync(
                $"/Party/UpdateParty?partyId={id}&name=test1&details=test&category=test&date=2022-12-12",
                null);
        updateParty.EnsureSuccessStatusCode();

        var getParty = await _client.GetAsync($"/Party/GetParty?id={id}");

        var result = await getParty.Content.ReadFromJsonAsync<Party>();
        
        Assert.Equal("test1", result.Name);
    }

    [Fact]
    public async Task GetParty_ThrowsNotFoundWhen_ProvidingWrongId()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();
        
        var createParty = await _client.PostAsync(
            "/Party/CreateParty?name=test&details=test&category=test&date=2022-12-12",
            null);
        createParty.EnsureSuccessStatusCode();

        var getParty = await _client.GetAsync($"/Party/GetParty?id={Guid.NewGuid()}");

        
        Assert.Equal(HttpStatusCode.NotFound, getParty.StatusCode);
    }

    [Fact]
    public async Task GetParties_ThrowsExceptionWhen_Empty()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();

        var getParties = await _client.GetAsync("/Party/GetParties");

        Assert.Equal(HttpStatusCode.NotFound, getParties.StatusCode);
    }

    [Fact]
    public async Task CreateParty_ReturnsUnauthorizedWhen_ThereIsNoHost()
    {
        var createParty = await _client.PostAsync(
            "/Party/CreateParty?name=test&details=test&category=test&date=2022-12-12",
            null);
        
        Assert.Equal(HttpStatusCode.Unauthorized, createParty.StatusCode);
    }
}