using ZMA.Model;

namespace ZMA.Services.Repositories;

public interface IPartyRepository
{
    void CreateParty(Party party);
}