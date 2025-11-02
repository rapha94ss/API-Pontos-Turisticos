using TouristSpots.Domain.Entities;

namespace TouristSpots.Domain.Repositories;

public interface ITouristSpotRepository
{
    Task<int> AddAsync(TouristSpot spot, CancellationToken ct = default);
    Task<TouristSpot?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<(IReadOnlyList<TouristSpot> Items, int Total)> SearchAsync(
        string? term, int page, int pageSize, CancellationToken ct = default);
}
