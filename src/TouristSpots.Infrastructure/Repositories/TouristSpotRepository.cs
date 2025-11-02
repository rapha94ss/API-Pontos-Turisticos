using Microsoft.EntityFrameworkCore;
using TouristSpots.Domain.Entities;
using TouristSpots.Domain.Repositories;
using TouristSpots.Infrastructure.Data;

namespace TouristSpots.Infrastructure.Repositories;

public class TouristSpotRepository : ITouristSpotRepository
{
    private readonly AppDbContext _db;
    public TouristSpotRepository(AppDbContext db) => _db = db;

    public async Task<int> AddAsync(TouristSpot spot, CancellationToken ct = default)
    {
        _db.TouristSpots.Add(spot);
        await _db.SaveChangesAsync(ct);
        return spot.Id;
    }

    public Task<TouristSpot?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.TouristSpots.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<(IReadOnlyList<TouristSpot> Items, int Total)> SearchAsync(
        string? term, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _db.TouristSpots.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(term))
        {
            term = term.Trim();
            q = q.Where(x =>
                x.Name.Contains(term) ||
                x.Description.Contains(term) ||
                x.Location.Contains(term));
        }

        q = q.OrderByDescending(x => x.CreatedAt);

        var total = await q.CountAsync(ct);
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return (items, total);
    }
}
