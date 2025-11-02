namespace TouristSpots.Domain.Entities;

public class TouristSpot
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!; // <= 100 chars
    public string Location { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!; // UF (2 letras)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
