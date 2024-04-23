namespace ZMA.Model;

public class Song
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Artist { get; init; }
    public DateTime RequestTime { get; init; }
    public bool Accepted { get; init; }
}