﻿using bejebeje.admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace bejebeje.admin.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Artist> Artists { get; }

    DbSet<ArtistSlug> ArtistSlugs { get; }

    DbSet<Lyric> Lyrics { get; }

    DbSet<LyricSlug> LyricSlugs { get; }
    
    DbSet<Author> Authors { get; }
    
    DbSet<AuthorSlug> AuthorSlugs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}