using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;

namespace bejebeje.admin.Application.Lyrics.Commands.DeleteLyric;

public class DeleteLyricCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteLyricCommandHandler : IRequestHandler<DeleteLyricCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteLyricCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteLyricCommand request, CancellationToken cancellationToken)
    {
        Lyric entity = await _context
            .Lyrics
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Lyric), request.Id);
        }

        entity.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
