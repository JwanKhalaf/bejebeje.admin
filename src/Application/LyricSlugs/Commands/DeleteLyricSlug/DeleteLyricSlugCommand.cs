using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;

namespace bejebeje.admin.Application.LyricSlugs.Commands.DeleteLyricSlug;

public class DeleteLyricSlugCommand : IRequest
{
    public int LyricId { get; set; }

    public int LyricSlugId { get; set; }
}

public class DeleteLyricSlugCommandHandler : IRequestHandler<DeleteLyricSlugCommand>
{
    private readonly IDateTime _dateTime;
    private readonly IApplicationDbContext _context;

    public DeleteLyricSlugCommandHandler(
        IDateTime dateTime,
        IApplicationDbContext context)
    {
        _dateTime = dateTime;
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteLyricSlugCommand command,
        CancellationToken cancellationToken)
    {
        LyricSlug? slug = await _context.LyricSlugs.FindAsync(command.LyricSlugId, cancellationToken);

        if (slug == null) throw new NullReferenceException(nameof(command.LyricSlugId));

        slug.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}