using ZMA.Model;

namespace ZMA.Services.Repositories;

public interface IPartyRepository
{
    void CreateParty(Party party);
    Party GetParty(Guid id);
    ICollection<Party> GetParties();
    void RequestSong(Song song, Guid partyId);
    void AcceptSong(int songId);
    ICollection<Song> GetSongs(Guid partyId);
    void DeleteParty(Party party);
    void UpdateParty(Guid partyId, string name, string details, string category, DateTime date);
    void DeleteSong(Guid partyId, int songId);
}