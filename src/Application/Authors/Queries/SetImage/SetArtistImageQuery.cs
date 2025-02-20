using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;

namespace bejebeje.admin.Application.Authors.Queries.SetImage;

public class SetAuthorImageQuery : IRequest<SetAuthorImageQueryViewModel>
{
    public int AuthorId { get; set; }
}

public class SetAuthorImageQueryViewModel
{
    public int AuthorId { get; set; }
}

public class SetAuthorImageQueryHandler : IRequestHandler<SetAuthorImageQuery, SetAuthorImageQueryViewModel>
{
    private readonly IApplicationDbContext _context;

    public SetAuthorImageQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SetAuthorImageQueryViewModel> Handle(SetAuthorImageQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Authors
            .FindAsync(new object[] { query.AuthorId }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Author), query.AuthorId);
        }

        SetAuthorImageQueryViewModel viewModel = new SetAuthorImageQueryViewModel();

        return viewModel;
    }
}