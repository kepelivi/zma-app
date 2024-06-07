using ZMA.Model;

namespace ZMA.Services.Repositories;

public interface ISongRepository
{
    Task<Song> RequestSong(Song song, Guid partyId);
    Task<ICollection<Song>> GetSongs(Guid partyId);
    Task DeleteSong(Guid partyId, int songId);
    Task AcceptSong(int songId);
}