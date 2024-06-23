using Microsoft.AspNetCore.Identity;
using Host = ZMA.Model.Host;

namespace ZMA.Data;

public class AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<Host> userManager)
{
    private readonly IConfigurationRoot _config = new ConfigurationBuilder()
        .AddUserSecrets<AuthenticationSeeder>()
        .Build();

    public void AddRole()
    {
        var tAdmin = CreateHostRole();
        tAdmin.Wait();
    }

    private async Task CreateHostRole()
    {
        await roleManager.CreateAsync(new IdentityRole(_config["HostRole"]   != null ? _config["HostRole"] : Environment.GetEnvironmentVariable("HOSTROLE")));
    }

    public void AddHost()
    {
        var tHost = CreateHostIfNotExists();
        tHost.Wait();
    }

    private async Task CreateHostIfNotExists()
    {
        var adminInDb = await userManager.FindByEmailAsync("zma@gmail.com");
        if (adminInDb == null)
        {
            var admin = new Host() { UserName = "cappyhost", Email = "zma@gmail.com", Name = "Cappy"};
            var adminCreated = await userManager.CreateAsync(admin, _config["HostPassword"] != null ? _config["HostPassword"] : Environment.GetEnvironmentVariable("HOSTPASSWORD"));

            if (adminCreated.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, _config["HostRole"] != null ? _config["HostRole"] : Environment.GetEnvironmentVariable("HOSTROLE"));
            }
        }
    }
}