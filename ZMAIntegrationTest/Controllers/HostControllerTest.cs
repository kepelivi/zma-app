using System.Net;
using System.Net.Http.Json;
using ZMA.Contracts;
using ZMA.Model;

namespace ZMAIntegrationTest.Controllers;

[Collection("IntegrationTests")]
public class HostControllerTest
{
    private readonly ZMAWebApplicationFactory _app;
    private readonly HttpClient _client;
    private readonly RegistrationReq _hostRegReq;
    private readonly AuthReq _hostAuthReq;
    
    public HostControllerTest()
    {
        _app = new ZMAWebApplicationFactory();
        _client = _app.CreateClient();
        _hostRegReq = new RegistrationReq("host@gmail.com", "host", "Host", "Host!12346");
        _hostAuthReq = new AuthReq("host@gmail.com", "Host!12346");
    }

    [Fact]
    public async Task GetHost_ReturnsCurrentlyLoggedInHost()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();

        var getHost = await _client.GetAsync("/GetHost");
        getHost.EnsureSuccessStatusCode();

        var loggedInHost = await getHost.Content.ReadFromJsonAsync<Host>();
        
        Assert.NotNull(loggedInHost);
        Assert.Equal(_hostRegReq.Email, loggedInHost.Email);
    }

    [Fact]
    public async Task GetHost_ReturnsUnauthorizedWhen_UserIsNotLoggedIn()
    {
        var getHost = await _client.GetAsync("/GetHost");
        
        Assert.Equal(HttpStatusCode.Unauthorized, getHost.StatusCode);
    }
}