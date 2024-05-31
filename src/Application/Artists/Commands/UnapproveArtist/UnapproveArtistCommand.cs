using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Artists.Commands.UnapproveArtist;

public class UnapproveArtistCommand : IRequest
{
    public int ArtistId { get; set; }
}

public class UnapproveArtistCommandHandler : IRequestHandler<UnapproveArtistCommand>
{
    private readonly IApplicationDbContext _context;

    public UnapproveArtistCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UnapproveArtistCommand request, CancellationToken cancellationToken)
    {
        Artist artist = await _context
            .Artists
            .Where(a => a.Id == request.ArtistId)
            .SingleAsync();

        artist.IsApproved = false;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}