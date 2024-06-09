using System.Net;
using System.Net.Http.Json;
using ZMA.Contracts;
using ZMA.Model;

namespace ZMAIntegrationTest.Controllers;

[Collection("IntegrationTests")]
public class AuthControllerTest
{
    private readonly ZMAWebApplicationFactory _app;
    private readonly HttpClient _client;
    private readonly RegistrationReq _hostRegReq;
    private readonly AuthReq _hostAuthReq;
    
    public AuthControllerTest()
    {
        _app = new ZMAWebApplicationFactory();
        _client = _app.CreateClient();
        _hostRegReq = new RegistrationReq("host@gmail.com", "host", "Host", "Host!12346");
        _hostAuthReq = new AuthReq("host@gmail.com", "Host!12346");
    }

    [Fact]
    public async Task Register_SuccessfullyRegistersNewHost()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();

        var response = await register.Content.ReadFromJsonAsync<RegistrationRes>();
        
        Assert.Equal(_hostRegReq.Email, response.Email);
    }

    [Fact]
    public async Task Register_ReturnsBadRequestWhen_SentAnInvalidRequest()
    {
        var invalidReq = new RegistrationReq("host@gmail.com", "host", "Host", "invalidpwd");
        
        var register = await _client.PostAsJsonAsync("/Auth/Register", invalidReq);
        
        Assert.Equal(HttpStatusCode.BadRequest, register.StatusCode);
    }

    [Fact]
    public async Task Login_SuccessfullyLogsInUserWith_ValidData()
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
    public async Task Login_ReturnsBadRequestWhen_HostIsNotRegistered()
    {
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        
        Assert.Equal(HttpStatusCode.BadRequest, login.StatusCode);
    }

    [Fact]
    public async Task Logout_SuccessfullyLogsOutHost()
    {
        var register = await _client.PostAsJsonAsync("/Auth/Register", _hostRegReq);
        register.EnsureSuccessStatusCode();
        
        var login = await _client.PostAsJsonAsync("/Auth/Login", _hostAuthReq);
        login.EnsureSuccessStatusCode();

        var content = JsonContent.Create(new { });

        var logout = await _client.PostAsync("/Auth/Logout", content);
        logout.EnsureSuccessStatusCode();
    }
}