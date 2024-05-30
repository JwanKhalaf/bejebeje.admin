using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Commands.DeleteLyric;

public class DeleteLyricCommand : IRequest
{
    public int LyricId { get; set; }
}

public class DeleteLyricCommandHandler : IRequestHandler<DeleteLyricCommand>
{
    private readonly IDateTime _dateTime;
    private readonly IApplicationDbContext _context;

    public DeleteLyricCommandHandler(
        IDateTime dateTime,
        IApplicationDbContext context)
    {
        _dateTime = dateTime;
        _context = context;
    }

    public async Task<Unit> Handle(DeleteLyricCommand command, CancellationToken cancellationToken)
    {
        Lyric entity = await _context
            .Lyrics
            .Where(l => l.Id == command.LyricId)
            .SingleAsync(cancellationToken);

        entity.IsDeleted = true;
        entity.IsApproved = false;
        entity.IsVerified = false;
        entity.ModifiedAt = _dateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}