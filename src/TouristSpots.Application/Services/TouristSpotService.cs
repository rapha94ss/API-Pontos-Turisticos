using TouristSpots.Application.Dtos;
using TouristSpots.Application.Requests;
using TouristSpots.Domain.Entities;
using TouristSpots.Domain.Repositories;

namespace TouristSpots.Application.Services;

public interface ITouristSpotService
{
    Task<int> CreateAsync(CreateTouristSpotRequest req, CancellationToken ct = default);
    Task<TouristSpotDto?> GetAsync(int id, CancellationToken ct = default);
    Task<(IReadOnlyList<TouristSpotDto> Items, int Total)> SearchAsync(string? term, int page, int pageSize, CancellationToken ct = default);
}

public class TouristSpotService : ITouristSpotService
{
    private readonly ITouristSpotRepository _repo;
    public TouristSpotService(ITouristSpotRepository repo) => _repo = repo;

    public async Task<int> CreateAsync(CreateTouristSpotRequest req, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) throw new ArgumentException("Name is required");
        if (string.IsNullOrWhiteSpace(req.Description) || req.Description.Length > 100)
            throw new ArgumentException("Description is required and must be <= 100 chars");
        if (string.IsNullOrWhiteSpace(req.Location)) throw new ArgumentException("Location is required");
        if (string.IsNullOrWhiteSpace(req.City)) throw new ArgumentException("City is required");
        if (string.IsNullOrWhiteSpace(req.State) || req.State.Length != 2)
            throw new ArgumentException("State (UF) must be 2 letters");

        var spot = new TouristSpot
        {
            Name = req.Name.Trim(),
            Description = req.Description.Trim(),
            Location = req.Location.Trim(),
            City = req.City.Trim(),
            State = req.State.Trim().ToUpperInvariant(),
            CreatedAt = DateTime.UtcNow
        };

        return await _repo.AddAsync(spot, ct);
    }

    public async Task<TouristSpotDto?> GetAsync(int id, CancellationToken ct = default)
    {
        var s = await _repo.GetByIdAsync(id, ct);
        return s is null ? null : new TouristSpotDto(s.Id, s.Name, s.Description, s.Location, s.City, s.State, s.CreatedAt);
    }

    public async Task<(IReadOnlyList<TouristSpotDto> Items, int Total)> SearchAsync(string? term, int page, int pageSize, CancellationToken ct = default)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var (items, total) = await _repo.SearchAsync(term, page, pageSize, ct);
        var dtos = items.Select(s => new TouristSpotDto(s.Id, s.Name, s.Description, s.Location, s.City, s.State, s.CreatedAt)).ToList();
        return (dtos, total);
    }
}
