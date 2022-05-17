using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;

namespace bejebeje.admin.Application.Artists.Commands.UpdateArtist;

public class UpdateArtistCommand : IRequest
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public bool IsApproved { get; set; }

    public bool IsDeleted { get; set; }

    public char Sex { get; set; }
    
    public bool HasImage { get; set; }
}

public class UpdateArtistCommandHandler : IRequestHandler<UpdateArtistCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateArtistCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateArtistCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Artists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Artist), request.Id);
        }

        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.FullName = string.IsNullOrEmpty(request.FirstName)
            ? request.FirstName
            : $"{request.FirstName} {request.LastName}";
        entity.IsApproved = request.IsApproved;
        entity.IsDeleted = request.IsDeleted;
        entity.Sex = request.Sex;
        entity.HasImage = request.HasImage;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
