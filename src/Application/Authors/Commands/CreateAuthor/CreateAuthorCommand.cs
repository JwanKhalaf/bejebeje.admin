using System.ComponentModel.DataAnnotations;
using bejebeje.admin.Application.Common.Extensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;

namespace bejebeje.admin.Application.Authors.Commands.CreateAuthor;

public class CreateAuthorCommand : IRequest<int>
{
    [Required]
    [Display(Name = "First name")]
    public string FirstName { get; set; }

    [Display(Name = "Last name")] public string LastName { get; set; }

    public string Biography { get; set; }

    [Required] [Display(Name = "Sex")] public string Sex { get; set; }
}

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, int>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;

    public CreateAuthorCommandHandler(
        ICurrentUserService currentUserService,
        IApplicationDbContext context)
    {
        _currentUserService = currentUserService;
        _context = context;
    }

    public async Task<int> Handle(
        CreateAuthorCommand command,
        CancellationToken cancellationToken)
    {
        string fullName = string.IsNullOrEmpty(command.FirstName)
            ? command.FirstName
            : $"{command.FirstName} {command.LastName}";

        string sex = command.Sex.ToLower();

        if (!(sex.Equals("f") || sex.Equals("m")))
        {
            sex = "n";
        }

        Author entity = new Author
        {
            FirstName = command.FirstName.Standardize(),
            LastName = command.LastName.Standardize(),
            FullName = fullName.ToLower(),
            Biography = command.Biography,
            Sex = char.Parse(sex),
            UserId = _currentUserService.UserId,
            Slugs = new List<AuthorSlug>
            {
                new AuthorSlug() { Name = fullName.NormalizeStringForUrl(), IsPrimary = true, }
            }
        };

        _context.Authors.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}