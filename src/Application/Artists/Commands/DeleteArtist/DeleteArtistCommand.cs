using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Artists.Commands.DeleteArtist;

public class DeleteArtistCommand : IRequest
{
    public int ArtistId { get; set; }
}

public class DeleteArtistCommandHandler : IRequestHandler<DeleteArtistCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteArtistCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteArtistCommand command, CancellationToken cancellationToken)
    {
        Artist artist = await _context.Artists
            .Where(l => l.Id == command.ArtistId)
            .SingleAsync(cancellationToken);

        artist.IsDeleted = true;
        artist.IsApproved = false;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}