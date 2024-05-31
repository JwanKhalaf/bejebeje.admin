using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Commands.UndeleteLyric;

public class UndeleteLyricCommand : IRequest
{
    public int LyricId { get; set; }
}

public class UndeleteLyricCommandHandler : IRequestHandler<UndeleteLyricCommand>
{
    private readonly IApplicationDbContext _context;

    public UndeleteLyricCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UndeleteLyricCommand command, CancellationToken cancellationToken)
    {
        Lyric entity = await _context
            .Lyrics
            .Where(l => l.Id == command.LyricId)
            .SingleAsync(cancellationToken);

        entity.IsDeleted = false;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}