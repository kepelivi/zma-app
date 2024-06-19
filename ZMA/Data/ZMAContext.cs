using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZMA.Model;
using Host = ZMA.Model.Host;

namespace ZMA.Data;

public class ZMAContext : IdentityDbContext<Host, IdentityRole, string>
{
    public DbSet<Party> Parties { get; set; }
    public DbSet<Song> Songs { get; set; }
    
    public ZMAContext (DbContextOptions<ZMAContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Party>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasOne(p => p.Host)
                .WithMany();
            entity.HasMany(p => p.Queue)
                .WithOne()
                .HasForeignKey(s => s.PartyId);
        });

        builder.Entity<Song>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasOne<Party>()
                .WithMany(p => p.Queue)
                .HasForeignKey(s => s.PartyId);
        });
    }
}