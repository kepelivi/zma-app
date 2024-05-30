using Microsoft.EntityFrameworkCore;
using ZMA.Data;
using ZMA.Model;
using Host = ZMA.Model.Host;

namespace ZMAIntegrationTest.Fixtures;

public class DatabaseFixture : IDisposable
{
    public ZMAContext Context { get; private set; }
    public Guid TestId;
    public Host TestHost = new() {Name = "c", Id = Guid.NewGuid().ToString()};

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<ZMAContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        Context = new ZMAContext(options);
        
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        Context.Users.Add(TestHost);
        Context.SaveChanges();

        var party = new Party
        {
            Category = "test", Date = DateTime.Now, Details = "test", Name = "test", Host = TestHost
        };

        Context.Parties.Add(party);
        Context.SaveChanges();
        TestId = party.Id;
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}