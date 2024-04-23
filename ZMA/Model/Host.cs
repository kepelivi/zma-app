using Microsoft.AspNetCore.Identity;

namespace ZMA.Model;

public class Host : IdentityUser
{
    public string Name { get; init; }
}