namespace TouristSpots.Application.Requests;

public record CreateTouristSpotRequest(
    string Name, string Description, string Location, string City, string State);
