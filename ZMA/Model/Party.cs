namespace ZMA.Model;

public class Party
{
    public int Id { get; init; }
    public Host? Host { get; init; }
    public DateTime Date { get; init; }
    public ICollection<Song> Queue { get; } = new List<Song>();
}