using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Artists.Commands.ApproveArtist;

public class ApproveArtistCommand : IRequest
{
    public int ArtistId { get; set; }
}

public class ApproveArtistCommandHandler : IRequestHandler<ApproveArtistCommand>
{
    private readonly IDateTime _dateTime;

    private readonly IApplicationDbContext _context;

    public ApproveArtistCommandHandler(IDateTime datetime, IApplicationDbContext context)
    {
        _dateTime = datetime;
        _context = context;
    }

    public async Task<Unit> Handle(ApproveArtistCommand request, CancellationToken cancellationToken)
    {
        Artist artist = await _context
            .Artists
            .Where(a => a.Id == request.ArtistId)
            .SingleAsync();

        artist.ModifiedAt = _dateTime.Now;
        artist.IsApproved = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}