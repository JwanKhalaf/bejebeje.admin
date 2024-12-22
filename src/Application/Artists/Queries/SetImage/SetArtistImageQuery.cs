using System.ComponentModel.DataAnnotations;
using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;

namespace bejebeje.admin.Application.Artists.Queries.SetImage;

public class SetArtistImageQuery : IRequest<SetArtistImageQueryViewModel>
{
    public int ArtistId { get; set; }
}

public class SetArtistImageQueryViewModel
{
    public int ArtistId { get; set; }
}

public class SetArtistImageQueryHandler : IRequestHandler<SetArtistImageQuery, SetArtistImageQueryViewModel>
{
    private readonly IApplicationDbContext _context;

    public SetArtistImageQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SetArtistImageQueryViewModel> Handle(SetArtistImageQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Artists
            .FindAsync(new object[] { query.ArtistId }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Artist), query.ArtistId);
        }

        SetArtistImageQueryViewModel viewModel = new SetArtistImageQueryViewModel();

        return viewModel;
    }
}