using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Commands.UnverifyLyric;

public class UnverifyLyricCommand : IRequest
{
    public int LyricId { get; set; }
}

public class UnverifyLyricCommandHandler : IRequestHandler<UnverifyLyricCommand>
{
    private readonly IApplicationDbContext _context;

    public UnverifyLyricCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UnverifyLyricCommand command, CancellationToken cancellationToken)
    {
        Lyric entity = await _context
            .Lyrics
            .Where(l => l.Id == command.LyricId)
            .SingleAsync(cancellationToken);

        entity.IsVerified = false;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}