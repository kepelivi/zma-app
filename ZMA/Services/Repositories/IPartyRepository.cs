using ZMA.Model;

namespace ZMA.Services.Repositories;

public interface IPartyRepository
{
    Task<ICollection<Party>> GetParties();
    Task AcceptSong(int songId);
    Task DeleteParty(Party party);
    Task UpdateParty(Guid partyId, string name, string details, string category, DateTime date);
    Task DeleteSong(Guid partyId, int songId);
    Task<Party> CreateParty(Party party);
    Task<Party> GetParty(Guid id);
    Task<Song> RequestSong(Song song, Guid partyId);
    Task<ICollection<Song>> GetSongs(Guid partyId);
}