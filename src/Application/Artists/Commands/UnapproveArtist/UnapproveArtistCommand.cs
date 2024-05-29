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
    private readonly IDateTime _dateTime;

    private readonly IApplicationDbContext _context;

    public UnapproveArtistCommandHandler(IDateTime datetime, IApplicationDbContext context)
    {
        _dateTime = datetime;
        _context = context;
    }

    public async Task<Unit> Handle(UnapproveArtistCommand request, CancellationToken cancellationToken)
    {
        Artist artist = await _context
            .Artists
            .Where(a => a.Id == request.ArtistId)
            .SingleAsync();

        artist.ModifiedAt = _dateTime.Now;
        artist.IsApproved = false;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}