using bejebeje.admin.Application.Common.Exceptions;
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
    private readonly IDateTime _dateTime;
    private readonly IApplicationDbContext _context;

    public UndeleteLyricCommandHandler(
        IDateTime dateTime,
        IApplicationDbContext context)
    {
        _dateTime = dateTime;
        _context = context;
    }

    public async Task<Unit> Handle(UndeleteLyricCommand command, CancellationToken cancellationToken)
    {
        Lyric entity = await _context
            .Lyrics
            .Where(l => l.Id == command.LyricId)
            .SingleAsync(cancellationToken);

        entity.IsDeleted = false;
        entity.ModifiedAt = _dateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}