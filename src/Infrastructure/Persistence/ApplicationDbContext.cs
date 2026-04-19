using System.Reflection;
using bejebeje.admin.Application.Common.Interfaces;
using Bejebeje.Shared.Domain;
using Bejebeje.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace bejebeje.admin.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IDateTime _dateTime;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTime dateTime) : base(options)
    {
        _dateTime = dateTime;
    }

    public DbSet<Artist> Artists => Set<Artist>();

    public DbSet<ArtistSlug> ArtistSlugs => Set<ArtistSlug>();

    public DbSet<Lyric> Lyrics => Set<Lyric>();

    public DbSet<LyricSlug> LyricSlugs => Set<LyricSlug>();
    
    public DbSet<Author> Authors => Set<Author>();

    public DbSet<AuthorSlug> AuthorSlugs => Set<AuthorSlug>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<IBaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = _dateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedAt = _dateTime.Now;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
    {
        return base.Entry(entity);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}