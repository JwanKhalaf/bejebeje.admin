using System.ComponentModel.DataAnnotations;
using bejebeje.admin.Application.Common.Extensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;

namespace bejebeje.admin.Application.Artists.Commands.CreateArtist;

// get
public class CreateArtistQuery : IRequest<CreateArtistQueryViewModel>
{
}

public class CreateArtistQueryViewModel
{
    [Display(Name = "First name")] public string FirstName { get; set; }

    [Display(Name = "Last name")] public string LastName { get; set; }

    public string Sex { get; set; }
}

public class CreateArtistQueryHandler : IRequestHandler<CreateArtistQuery, CreateArtistQueryViewModel>
{
    private readonly IApplicationDbContext _context;

    public CreateArtistQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateArtistQueryViewModel> Handle(CreateArtistQuery query, CancellationToken cancellationToken)
    {
        CreateArtistQueryViewModel viewModel = new CreateArtistQueryViewModel();

        return viewModel;
    }
}

// post
public class CreateArtistCommand : IRequest<int>
{
    [Required]
    [Display(Name = "First name")]
    public string FirstName { get; set; }

    [Display(Name = "Last name")] public string LastName { get; set; }

    [Required] [Display(Name = "Sex")] public string Sex { get; set; }
}

public class CreateArtistCommandHandler : IRequestHandler<CreateArtistCommand, int>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;

    public CreateArtistCommandHandler(
        ICurrentUserService currentUserService,
        IApplicationDbContext context)
    {
        _currentUserService = currentUserService;
        _context = context;
    }

    public async Task<int> Handle(
        CreateArtistCommand request,
        CancellationToken cancellationToken)
    {
        string fullName = string.IsNullOrEmpty(request.FirstName)
            ? request.FirstName
            : $"{request.FirstName} {request.LastName}";

        Artist entity = new Artist
        {
            FirstName = request.FirstName.Standardize(),
            LastName = request.LastName.Standardize(),
            FullName = fullName,
            Sex = char.Parse(request.Sex),
            UserId = _currentUserService.UserId,
            Slugs = new List<ArtistSlug>
            {
                new ArtistSlug() { Name = fullName.NormalizeStringForUrl(), IsPrimary = true, }
            }
        };

        _context.Artists.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}