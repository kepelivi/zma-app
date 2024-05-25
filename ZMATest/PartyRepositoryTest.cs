using Microsoft.EntityFrameworkCore;
using ZMA.Data;
using ZMA.Model;
using ZMA.Services.Repositories;

namespace ZMATest;

public class PartyRepositoryTest
{
    private ZMAContext _context;
    private IPartyRepository _partyRepository;
    private readonly Guid _testId = Guid.NewGuid();
    private readonly Host _testHost = new Host() {Name = "c"};
    
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ZMAContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ZMAContext(options);
        _context.Parties.AddRange(new List<Party>
        {
            new Party { Id = _testId, Category = "test", Date = DateTime.Now, Details = "test", Name = "test", Host = _testHost},
            new Party { Id = Guid.NewGuid(), Category = "test2", Date = DateTime.Now, Details = "test222", Name = "TEST2", Host = _testHost}
        });
        _context.SaveChanges();
        
        _context.Users.AddRange(new List<Host>()
        {
            _testHost
        });

        _partyRepository = new PartyRepository(_context);
    }

    [Test]
    public void GetParties_ReturnsAllParties()
    {
        var result = _partyRepository.GetParties().ToList();
        
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("test", result[0].Category);
    }

    [Test]
    public void GetParty_ReturnsCorrectPartyBasedOnId()
    {
        var result = _partyRepository.GetParty(_testId);
        
        Assert.AreEqual(_testId, result.Id);
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}