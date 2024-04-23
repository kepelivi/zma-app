using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZMA.Model;
using Host = ZMA.Model.Host;

namespace ZMA.Data;

public class ZMAContext : IdentityDbContext<Host, IdentityRole, string>
{
    public DbSet<Party> Parties { get; set; }

    public ZMAContext()
    {
    }
    
    public ZMAContext (DbContextOptions<ZMAContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Party>()
            .HasOne(p => p.Host)
            .WithMany();

        builder.Entity<Party>()
            .HasMany(p => p.Queue)
            .WithOne();
    }
}