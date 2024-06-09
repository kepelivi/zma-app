using ZMA.Model;

namespace ZMA.Services.Repositories;

public interface ISongRepository
{
    Task<Song> RequestSong(Song song);
    Task<ICollection<Song>> GetSongs(Guid partyId);
    Task DeleteSong(int songId);
    Task AcceptSong(int songId);
}