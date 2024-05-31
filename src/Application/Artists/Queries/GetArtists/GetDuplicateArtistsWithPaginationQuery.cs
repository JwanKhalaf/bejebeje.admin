using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Artists.Queries.GetArtists;

public class GetDuplicateArtistsWithPaginationQuery : IRequest<PaginatedList<ArtistDto>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string SearchTerm { get; set; }

    public GetDuplicateArtistsWithPaginationQuery(string searchTerm, int pageNumber, int pageSize)
    {
        SearchTerm = searchTerm;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class
    GetDuplicateArtistsQueryHandler : IRequestHandler<GetDuplicateArtistsWithPaginationQuery,
    PaginatedList<ArtistDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDuplicateArtistsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ArtistDto>> Handle(
        GetDuplicateArtistsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        PaginatedList<ArtistDto> artists = await _context.Artists
            .FromSqlRaw(
                $"SELECT a1.id AS id, a1.first_name AS first_name, a1.last_name AS last_name, a1.full_name, a1.has_image, a1.is_approved, a1.is_deleted, a1.created_at, a1.modified_at FROM artists a1 JOIN artists a2 ON a1.id < a2.id WHERE a1.is_deleted = false AND a2.is_deleted = false AND similarity(a1.first_name, a2.first_name) >= 0.5 AND similarity(a1.last_name, a2.last_name) >= 0.5")
            .ProjectTo<ArtistDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return artists;
    }
}