using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Artists.Commands.UndeleteArtist;

public class UndeleteArtistCommand : IRequest
{
    public int ArtistId { get; set; }
}

public class UndeleteArtistCommandHandler : IRequestHandler<UndeleteArtistCommand>
{
    private readonly IApplicationDbContext _context;

    public UndeleteArtistCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UndeleteArtistCommand command, CancellationToken cancellationToken)
    {
        Artist artist = await _context.Artists
            .Where(l => l.Id == command.ArtistId)
            .SingleAsync(cancellationToken);

        artist.IsDeleted = false;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}