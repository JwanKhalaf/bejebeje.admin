using System.ComponentModel.DataAnnotations;
using bejebeje.admin.Application.Common.Enums;
using bejebeje.admin.Application.Common.Extensions;
using bejebeje.admin.Application.Common.Helpers;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Commands.CreateLyric;

public class CreateLyricQuery : IRequest<CreateLyricQueryViewModel>
{
    public int ArtistId { get; set; }
}

public class CreateLyricQueryViewModel : IRequest
{
    [Required] public string Title { get; set; }

    [Required] public string Body { get; set; }

    public int ArtistId { get; set; }

    public string ArtistName { get; set; }

    public string ArtistImageUrl { get; set; }

    public string ArtistImageAlternateText { get; set; }
}

public class CreateLyricCommand : IRequest
{
    public string Title { get; set; }

    public string Body { get; set; }

    public int ArtistId { get; set; }
}

public class CreateLyricQueryHandler : IRequestHandler<CreateLyricQuery, CreateLyricQueryViewModel>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;

    public CreateLyricQueryHandler(
        ICurrentUserService userService,
        IApplicationDbContext context)
    {
        _currentUserService = userService;
        _context = context;
    }

    public async Task<CreateLyricQueryViewModel> Handle(CreateLyricQuery query, CancellationToken cancellationToken)
    {
        Artist artist = await _context
            .Artists
            .AsNoTracking()
            .Where(a => a.Id == query.ArtistId)
            .SingleAsync(cancellationToken);

        CreateLyricQueryViewModel command =
            new CreateLyricQueryViewModel
            {
                ArtistId = query.ArtistId,
                ArtistName = artist.FullName.ToTitleCase(),
                ArtistImageUrl = ImageUrlBuilder.BuildImageUrl(artist.HasImage, artist.Id, ImageSize.Standard),
                ArtistImageAlternateText = ImageUrlBuilder.GetImageAlternateText(artist.HasImage, artist.FullName)
            };

        return command;
    }
}

public class CreateLyricCommandHandler : IRequestHandler<CreateLyricCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;

    public CreateLyricCommandHandler(
        ICurrentUserService userService,
        IApplicationDbContext context)
    {
        _currentUserService = userService;
        _context = context;
    }

    public async Task<Unit> Handle(CreateLyricCommand request, CancellationToken cancellationToken)
    {
        var entity = new Lyric
        {
            Title = request.Title,
            Body = request.Body,
            UserId = _currentUserService.UserId,
            ArtistId = request.ArtistId,
            Slugs = new List<LyricSlug>
            {
                new LyricSlug { Name = request.Title.NormalizeStringForUrl(), IsPrimary = true }
            }
        };

        _context.Lyrics.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}