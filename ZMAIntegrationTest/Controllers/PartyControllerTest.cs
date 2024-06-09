using System.Net;
using System.Net.Http.Json;
using ZMA.Contracts;
using ZMA.Model;

namespace ZMAIntegrationTest.Controllers;

[Collection("IntegrationTests")]
public class PartyControllerTest
{
    private readonly ZMAWebApplicationFactory _app;
    private readonly HttpClient _client;
    private readonly RegistrationReq _hostRegReq;
    private readonly AuthReq _hostAuthReq;
    
    public PartyControllerTest()
    {
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

        var party = new Party() { Name = "test", Category = "test", Date = DateTime.Now, Details = "test" };
        var content = JsonContent.Create(party);
        
        var createParty = await _client.PostAsync(
            $"/Party/CreateParty?name={party.Name}&details={party.Details}&category={party.Category}&date={party.Date}",
            content);
        createParty.EnsureSuccessStatusCode();

        var createdParty = await createParty.Content.ReadFromJsonAsync<Party>();
        
        Assert.Equal(party.Name, createdParty.Name);
    }

    [Fact]
    public async Task GetParty_ReturnsCorrectParty()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();

        var party = new Party() { Id = Guid.NewGuid(), Name = "test", Category = "test", Date = DateTime.Now, Details = "test" };
        var content = JsonContent.Create(party);
        
        var createParty = await _client.PostAsync(
            $"/Party/CreateParty?name={party.Name}&details={party.Details}&category={party.Category}&date={party.Date}",
            content);
        createParty.EnsureSuccessStatusCode();

        var getParties = await _client.GetAsync("/Party/GetParties");
        getParties.EnsureSuccessStatusCode();

        var parties = await getParties.Content.ReadFromJsonAsync<ICollection<Party>>();

        var id = parties.Single(p => p.Name == party.Name).Id;

        var getParty = await _client.GetAsync($"/Party/GetParty?id={id}");
        getParty.EnsureSuccessStatusCode();

        var result = await getParty.Content.ReadFromJsonAsync<Party>();
        
        Assert.Equal(party.Name, result.Name);
    }

    [Fact]
    public async Task GetParties_ReturnsAllParties()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();

        var party = new Party() { Id = Guid.NewGuid(), Name = "test", Category = "test", Date = DateTime.Now, Details = "test" };
        var content = JsonContent.Create(party);
        
        var createParty = await _client.PostAsync(
            $"/Party/CreateParty?name={party.Name}&details={party.Details}&category={party.Category}&date={party.Date}",
            content);
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

        var party = new Party() { Id = Guid.NewGuid(), Name = "test", Category = "test", Date = DateTime.Now, Details = "test" };
        var content = JsonContent.Create(party);
        
        var createParty = await _client.PostAsync(
            $"/Party/CreateParty?name={party.Name}&details={party.Details}&category={party.Category}&date={party.Date}",
            content);
        createParty.EnsureSuccessStatusCode();

        var getParties = await _client.GetAsync("/Party/GetParties");
        getParties.EnsureSuccessStatusCode();

        var parties = await getParties.Content.ReadFromJsonAsync<ICollection<Party>>();
        
        var id = parties.Single(p => p.Name == party.Name).Id;

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

        var party = new Party() { Id = Guid.NewGuid(), Name = "test", Category = "test", Date = DateTime.Now, Details = "test" };
        var content = JsonContent.Create(party);
        
        var createParty = await _client.PostAsync(
            $"/Party/CreateParty?name={party.Name}&details={party.Details}&category={party.Category}&date={party.Date}",
            content);
        createParty.EnsureSuccessStatusCode();

        var getParties = await _client.GetAsync("/Party/GetParties");
        getParties.EnsureSuccessStatusCode();

        var parties = await getParties.Content.ReadFromJsonAsync<ICollection<Party>>();
        
        var id = parties.Single(p => p.Name == party.Name).Id;
        
        var newParty = new Party() { Id = Guid.NewGuid(), Name = "test1", Category = "test", Date = DateTime.Now, Details = "test" };
        var patchContent = JsonContent.Create(newParty);

        var updateParty =
            await _client.PatchAsync(
                $"/Party/UpdateParty?partyId={id}&name={newParty.Name}&details={newParty.Details}&category={newParty.Category}&date={newParty.Date}",
                patchContent);
        updateParty.EnsureSuccessStatusCode();

        var getParty = await _client.GetAsync($"/Party/GetParty?id={id}");

        var result = await getParty.Content.ReadFromJsonAsync<Party>();
        
        Assert.Equal(newParty.Name, result.Name);
    }

    [Fact]
    public async Task GetParty_ThrowsNotFoundWhen_ProvidingWrongId()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();

        var party = new Party() { Id = Guid.NewGuid(), Name = "test", Category = "test", Date = DateTime.Now, Details = "test" };
        var content = JsonContent.Create(party);
        
        var createParty = await _client.PostAsync(
            $"/Party/CreateParty?name={party.Name}&details={party.Details}&category={party.Category}&date={party.Date}",
            content);
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
    public async Task CreateParty_FailsWhen_RequiredParameterIsNotProvided()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();

        var party = new Party() { Category = "test", Date = DateTime.Now, Details = "test" };
        var content = JsonContent.Create(party);
        
        var createParty = await _client.PostAsync(
            $"/Party/CreateParty?name={party.Name}&details={party.Details}&category={party.Category}&date={party.Date}",
            content);
        
        Assert.Equal(HttpStatusCode.BadRequest, createParty.StatusCode);
    }

    [Fact]
    public async Task CreateParty_ReturnsUnauthorizedWhen_ThereIsNoHost()
    {
        var party = new Party() { Category = "test", Date = DateTime.Now, Details = "test" };
        var content = JsonContent.Create(party);
        
        var createParty = await _client.PostAsync(
            $"/Party/CreateParty?name={party.Name}&details={party.Details}&category={party.Category}&date={party.Date}",
            content);
        
        Assert.Equal(HttpStatusCode.Unauthorized, createParty.StatusCode);
    }
}