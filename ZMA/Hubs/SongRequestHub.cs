using Microsoft.AspNetCore.SignalR;
using ZMA.Model;

namespace ZMA.Hubs;

public class SongRequestHub : Hub
{
    public async Task SendSongRequestUpdate(Song song)
    {
        await Clients.All.SendAsync("receiveSongRequestUpdate", song);
    }
}