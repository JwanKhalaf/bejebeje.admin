using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Commands.ApproveLyric;

public class ApproveLyricCommand : IRequest
{
    public int LyricId { get; set; }
}

public class ApproveLyricCommandHandler : IRequestHandler<ApproveLyricCommand>
{
    private readonly IDateTime _dateTime;
    private readonly IApplicationDbContext _context;

    public ApproveLyricCommandHandler(
        IDateTime dateTime,
        IApplicationDbContext context)
    {
        _dateTime = dateTime;
        _context = context;
    }

    public async Task<Unit> Handle(ApproveLyricCommand command, CancellationToken cancellationToken)
    {
        Lyric entity = await _context
            .Lyrics
            .Where(l => l.Id == command.LyricId)
            .SingleAsync(cancellationToken);

        entity.IsApproved = true;
        entity.ModifiedAt = _dateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}