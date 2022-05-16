using bejebeje.admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Artist> Artists { get; }

    DbSet<Lyric> Lyrics { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
