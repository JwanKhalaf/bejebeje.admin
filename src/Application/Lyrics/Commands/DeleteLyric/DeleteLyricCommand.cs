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
    private readonly IApplicationDbContext _context;

    public DeleteLyricCommandHandler(
        IApplicationDbContext context)
    {
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

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}