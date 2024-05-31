using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Commands.UnapproveLyric;

public class UnapproveLyricCommand : IRequest
{
    public int LyricId { get; set; }
}

public class UnapproveLyricCommandHandler : IRequestHandler<UnapproveLyricCommand>
{
    private readonly IApplicationDbContext _context;

    public UnapproveLyricCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UnapproveLyricCommand command, CancellationToken cancellationToken)
    {
        Lyric entity = await _context
            .Lyrics
            .Where(l => l.Id == command.LyricId)
            .SingleAsync(cancellationToken);

        entity.IsApproved = false;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}