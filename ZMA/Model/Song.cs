namespace ZMA.Model;

public class Song
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public DateTime RequestTime { get; init; }
    public bool Accepted { get; set; }
}