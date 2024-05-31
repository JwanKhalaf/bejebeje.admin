using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Commands.UpdateLyric;

public class UpdateLyricCommand : IRequest
{
    public int LyricId { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }
}

public class UpdateLyricCommandHandler : IRequestHandler<UpdateLyricCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLyricCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateLyricCommand command, CancellationToken cancellationToken)
    {
        Lyric entity = await _context
            .Lyrics
            .Where(l => l.Id == command.LyricId)
            .SingleAsync(cancellationToken);

        entity.Title = command.Title;
        entity.Body = command.Body;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}