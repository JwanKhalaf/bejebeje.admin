using bejebeje.admin.Application.Common.Interfaces;
using Bejebeje.Shared.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Artists.Commands.ApproveArtist;

public class ApproveArtistCommand : IRequest
{
    public int ArtistId { get; set; }
}

public class ApproveArtistCommandHandler : IRequestHandler<ApproveArtistCommand>
{
    private readonly IApplicationDbContext _context;

    public ApproveArtistCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ApproveArtistCommand request, CancellationToken cancellationToken)
    {
        Artist artist = await _context
            .Artists
            .Where(a => a.Id == request.ArtistId)
            .SingleAsync();

        artist.IsApproved = true;

        await _context.SaveChangesAsync(cancellationToken);

    }
}