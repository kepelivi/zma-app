using ZMA.Model;

namespace ZMA.Services.Repositories;

public interface IPartyRepository
{
    void CreateParty(Party party);
    Party GetParty(Guid id);
    ICollection<Party> GetParties();
    void RequestSong(Song song, Guid partyId);
    void AcceptSong(int songId, bool accept);
}