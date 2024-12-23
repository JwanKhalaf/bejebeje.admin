using System.ComponentModel.DataAnnotations;
using bejebeje.admin.Application.Common.Extensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using bejebeje.admin.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.ArtistSlugs.Commands;

// get
public class CreateArtistSlugQuery : IRequest<CreateArtistSlugQueryViewModel>
{
    public int ArtistId { get; set; }
}

public class CreateArtistSlugQueryViewModel
{
    public int ArtistId { get; set; }

    [Required] public string Name { get; set; }

    [Display(Name = "Is primary?")] public bool IsPrimary { get; set; }
}

public class CreateArtistSlugQueryHandler : IRequestHandler<CreateArtistSlugQuery, CreateArtistSlugQueryViewModel>
{
    private readonly IApplicationDbContext _context;

    public CreateArtistSlugQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateArtistSlugQueryViewModel> Handle(
        CreateArtistSlugQuery query,
        CancellationToken cancellationToken)
    {
        CreateArtistSlugQueryViewModel viewModel = new CreateArtistSlugQueryViewModel { ArtistId = query.ArtistId };

        return viewModel;
    }
}

// post
public class CreateArtistSlugCommand : IRequest
{
    public int ArtistId { get; set; }

    [Required] public string Name { get; set; }

    public bool IsPrimary { get; set; }
}

public class CreateArtistSlugCommandHandler : IRequestHandler<CreateArtistSlugCommand>
{
    private readonly IDateTime _dateTime;
    private readonly IApplicationDbContext _context;

    public CreateArtistSlugCommandHandler(
        IDateTime dateTime,
        IApplicationDbContext context)
    {
        _dateTime = dateTime;
        _context = context;
    }

    public async Task<Unit> Handle(
        CreateArtistSlugCommand command,
        CancellationToken cancellationToken)
    {
        // normalise the slug name
        string normalizedSlugName = command.Name.NormalizeStringForUrl();

        // check if the slug already exists for the artist
        bool slugExists = await _context.ArtistSlugs
            .AnyAsync(slug =>
                    slug.Name == normalizedSlugName &&
                    slug.ArtistId == command.ArtistId &&
                    !slug.IsDeleted,
                cancellationToken);

        if (slugExists)
        {
            throw new ArtistSlugAlreadyExistsException(normalizedSlugName, command.ArtistId);
        }

        ArtistSlug slug = new ArtistSlug
        {
            Name = normalizedSlugName,
            IsPrimary = command.IsPrimary,
            IsDeleted = false,
            CreatedAt = _dateTime.Now,
            ModifiedAt = null,
            ArtistId = command.ArtistId
        };

        if (command.IsPrimary)
        {
            var artist = await _context
                .Artists
                .Include(x => x.Slugs)
                .Where(x => x.Id == command.ArtistId)
                .SingleAsync(cancellationToken);

            foreach (var item in artist.Slugs)
            {
                item.IsPrimary = false;
            }

            artist.Slugs.Add(slug);

            _context.Entry(artist).State = EntityState.Modified;
        }
        else
        {
            _context.ArtistSlugs.Add(slug);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}