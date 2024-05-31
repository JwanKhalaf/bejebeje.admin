using bejebeje.admin.Application.Common.Extensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;

namespace bejebeje.admin.Application.Artists.Commands.CreateArtist;

public class CreateArtistCommand : IRequest<int>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Sex { get; set; }
}

public class CreateArtistCommandHandler : IRequestHandler<CreateArtistCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateArtistCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
    {
        Artist entity = new Artist
        {
            FirstName = request.FirstName.Standardize(),
            LastName = request.LastName.Standardize(),
            FullName = string.IsNullOrEmpty(request.FirstName)
                ? request.FirstName
                : $"{request.FirstName} {request.LastName}",
            Sex = char.Parse(request.Sex)
        };

        _context.Artists.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}