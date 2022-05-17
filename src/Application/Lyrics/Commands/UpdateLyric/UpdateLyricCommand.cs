using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;

namespace bejebeje.admin.Application.Lyrics.Commands.UpdateLyric;

public class UpdateLyricCommand : IRequest
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }

    public bool IsApproved { get; set; }

    public bool IsVerified { get; set; }

    public bool IsDeleted { get; set; }
}

public class UpdateLyricCommandHandler : IRequestHandler<UpdateLyricCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLyricCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateLyricCommand request, CancellationToken cancellationToken)
    {
        Lyric entity = await _context
            .Lyrics
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Lyric), request.Id);
        }

        entity.Title = request.Title;
        entity.Body = request.Body;
        entity.IsApproved = request.IsApproved;
        entity.IsDeleted = request.IsDeleted;
        entity.IsVerified = request.IsVerified;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
