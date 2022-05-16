using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Application.Common.Security;
using MediatR;

namespace bejebeje.admin.Application.TodoLists.Commands.PurgeTodoLists;

[Authorize(Roles = "Administrator")]
[Authorize(Policy = "CanPurge")]
public class PurgeTodoListsCommand : IRequest
{
}

public class PurgeTodoListsCommandHandler : IRequestHandler<PurgeTodoListsCommand>
{
    private readonly IApplicationDbContext _context;

    public PurgeTodoListsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
    {
        _context.Artists.RemoveRange(_context.Artists);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
