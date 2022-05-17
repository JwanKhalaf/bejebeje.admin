using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Artists.Queries.GetArtists;

public class GetArtistsQuery : IRequest<ArtistsViewModel>
{
}

public class GetArtistsQueryHandler : IRequestHandler<GetArtistsQuery, ArtistsViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetArtistsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ArtistsViewModel> Handle(GetArtistsQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Artists
            .AsNoTracking()
            .ProjectTo<ArtistDto>(_mapper.ConfigurationProvider)
            .OrderBy(t => t.Title)
            .ToListAsync(cancellationToken);
        
        return new ArtistsViewModel
        {
            Artists = result 
        };
    }
}
