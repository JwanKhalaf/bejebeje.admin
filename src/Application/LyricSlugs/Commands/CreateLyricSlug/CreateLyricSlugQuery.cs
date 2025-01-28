using System.ComponentModel.DataAnnotations;
using bejebeje.admin.Application.Common.Extensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using bejebeje.admin.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.LyricSlugs.Commands.CreateLyricSlug;

// get
public class CreateLyricSlugQuery : IRequest<CreateLyricSlugQueryViewModel>
{
    public int LyricId { get; set; }
}

public class CreateLyricSlugQueryViewModel
{
    public int LyricId { get; set; }

    [Required] public string Name { get; set; }

    [Display(Name = "Is primary?")] public bool IsPrimary { get; set; }
}

public class CreateLyricSlugQueryHandler : IRequestHandler<CreateLyricSlugQuery, CreateLyricSlugQueryViewModel>
{
    private readonly IApplicationDbContext _context;

    public CreateLyricSlugQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateLyricSlugQueryViewModel> Handle(
        CreateLyricSlugQuery query,
        CancellationToken cancellationToken)
    {
        CreateLyricSlugQueryViewModel viewModel = new CreateLyricSlugQueryViewModel { LyricId = query.LyricId };

        return viewModel;
    }
}

// post
public class CreateLyricSlugCommand : IRequest
{
    public int LyricId { get; set; }

    [Required] public string Name { get; set; }

    public bool IsPrimary { get; set; }
}

public class CreateLyricSlugCommandHandler : IRequestHandler<CreateLyricSlugCommand>
{
    private readonly IDateTime _dateTime;
    private readonly IApplicationDbContext _context;

    public CreateLyricSlugCommandHandler(
        IDateTime dateTime,
        IApplicationDbContext context)
    {
        _dateTime = dateTime;
        _context = context;
    }

    public async Task<Unit> Handle(
        CreateLyricSlugCommand command,
        CancellationToken cancellationToken)
    {
        // normalise the slug name
        string normalizedSlugName = command.Name.NormalizeStringForUrl();

        // check if the slug already exists for the lyric
        bool slugExists = await _context.LyricSlugs
            .AnyAsync(slug =>
                    slug.Name == normalizedSlugName &&
                    slug.LyricId == command.LyricId &&
                    !slug.IsDeleted,
                cancellationToken);

        if (slugExists)
        {
            throw new LyricSlugAlreadyExistsException(normalizedSlugName, command.LyricId);
        }

        LyricSlug slug = new LyricSlug
        {
            Name = normalizedSlugName,
            IsPrimary = command.IsPrimary,
            IsDeleted = false,
            CreatedAt = _dateTime.Now,
            ModifiedAt = null,
            LyricId = command.LyricId
        };

        if (command.IsPrimary)
        {
            var lyric = await _context
                .Lyrics
                .Include(x => x.Slugs)
                .Where(x => x.Id == command.LyricId)
                .SingleAsync(cancellationToken);

            foreach (var item in lyric.Slugs)
            {
                item.IsPrimary = false;
            }

            lyric.Slugs.Add(slug);

            _context.Entry(lyric).State = EntityState.Modified;
        }
        else
        {
            _context.LyricSlugs.Add(slug);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}