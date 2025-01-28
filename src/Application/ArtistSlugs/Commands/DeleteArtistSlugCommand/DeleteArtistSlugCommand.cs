using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;

namespace bejebeje.admin.Application.ArtistSlugs.Commands.DeleteArtistSlugCommand;

public class DeleteArtistSlugCommand : IRequest
{
    public int ArtistId { get; set; }

    public int ArtistSlugId { get; set; }
}

public class DeleteArtistSlugCommandHandler : IRequestHandler<DeleteArtistSlugCommand>
{
    private readonly IDateTime _dateTime;
    private readonly IApplicationDbContext _context;

    public DeleteArtistSlugCommandHandler(
        IDateTime dateTime,
        IApplicationDbContext context)
    {
        _dateTime = dateTime;
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteArtistSlugCommand command,
        CancellationToken cancellationToken)
    {
        ArtistSlug? slug = await _context.ArtistSlugs.FindAsync(command.ArtistSlugId, cancellationToken);

        if (slug == null) throw new NullReferenceException(nameof(command.ArtistSlugId));

        slug.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}