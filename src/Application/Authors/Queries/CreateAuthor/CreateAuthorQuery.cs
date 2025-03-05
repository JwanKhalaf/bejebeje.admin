using System.ComponentModel.DataAnnotations;
using bejebeje.admin.Application.Common.Interfaces;
using MediatR;

namespace bejebeje.admin.Application.Authors.Queries.CreateAuthor;

public class CreateAuthorQuery : IRequest<CreateAuthorQueryViewModel>
{
}

public class CreateAuthorQueryViewModel
{
    [Display(Name = "First name")] public string FirstName { get; set; }

    [Display(Name = "Last name")] public string LastName { get; set; }

    public string Biography { get; set; }

    public string Sex { get; set; }
}

public class CreateAuthorQueryHandler : IRequestHandler<CreateAuthorQuery, CreateAuthorQueryViewModel>
{
    private readonly IApplicationDbContext _context;

    public CreateAuthorQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateAuthorQueryViewModel> Handle(CreateAuthorQuery query, CancellationToken cancellationToken)
    {
        CreateAuthorQueryViewModel viewModel = new CreateAuthorQueryViewModel();

        return viewModel;
    }
}