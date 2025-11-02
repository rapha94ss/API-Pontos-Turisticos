namespace TouristSpots.Application.Dtos;

public record TouristSpotDto(
    int Id, string Name, string Description, string Location, string City, string State, DateTime CreatedAt);
