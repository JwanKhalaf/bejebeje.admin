using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Commands.VerifyLyric;

public class VerifyLyricCommand : IRequest
{
    public int LyricId { get; set; }
}

public class VerifyLyricCommandHandler : IRequestHandler<VerifyLyricCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifyLyricCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(VerifyLyricCommand command, CancellationToken cancellationToken)
    {
        Lyric entity = await _context
            .Lyrics
            .Where(l => l.Id == command.LyricId)
            .SingleAsync(cancellationToken);

        entity.IsVerified = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}