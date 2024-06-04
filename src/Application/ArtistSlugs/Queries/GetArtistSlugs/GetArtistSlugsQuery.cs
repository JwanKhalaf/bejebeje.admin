using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.ArtistSlugs.Queries.GetArtistSlugs;

// get
public class GetArtistSlugsQuery : IRequest<GetArtistSlugsQueryViewModel>
{
    public int ArtistId { get; set; }
}

public class GetArtistSlugsQueryViewModel
{
    public int ArtistId { get; set; }

    public List<GetArtistSlugsQueryViewModelItem> ArtistSlugs { get; set; }
}

public class GetArtistSlugsQueryViewModelItem : IMapFrom<ArtistSlug>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsDeleted { get; set; }
}

public class GetArtistSlugsQueryHandler : IRequestHandler<GetArtistSlugsQuery, GetArtistSlugsQueryViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetArtistSlugsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetArtistSlugsQueryViewModel> Handle(
        GetArtistSlugsQuery request,
        CancellationToken cancellationToken)
    {
        List<GetArtistSlugsQueryViewModelItem> slugs = await _context
            .ArtistSlugs
            .AsNoTracking()
            .Where(x => x.ArtistId == request.ArtistId)
            .ProjectTo<GetArtistSlugsQueryViewModelItem>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        GetArtistSlugsQueryViewModel result =
            new GetArtistSlugsQueryViewModel { ArtistId = request.ArtistId, ArtistSlugs = slugs };

        return result;
    }
}