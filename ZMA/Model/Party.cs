namespace ZMA.Model;

public class Party
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public Host? Host { get; init; }
    public string? Details { get; set; }
    public string Category { get; set; }
    public DateTime Date { get; set; }
    public ICollection<Song> Queue { get; } = new List<Song>();
}