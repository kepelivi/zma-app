using ZMA.Model;

namespace ZMA.Services.Repositories;

public interface IPartyRepository
{
    Task<ICollection<Party>> GetParties();
    Task DeleteParty(Party party);
    Task UpdateParty(Guid partyId, string name, string details, string category, DateTime date);
    Task<Party> CreateParty(Party party);
    Task<Party> GetParty(Guid id);
    
}