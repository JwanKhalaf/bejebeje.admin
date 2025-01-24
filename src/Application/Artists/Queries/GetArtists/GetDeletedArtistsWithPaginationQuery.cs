using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Extensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Application.Common.Models;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Artists.Queries.GetArtists;

public class GetDeletedArtistsWithPaginationQuery : IRequest<PaginatedList<ArtistDto>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string SearchTerm { get; set; }

    public GetDeletedArtistsWithPaginationQuery(string searchTerm, int pageNumber, int pageSize)
    {
        SearchTerm = searchTerm;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class
    GetDeletedArtistsQueryHandler : IRequestHandler<GetDeletedArtistsWithPaginationQuery,
        PaginatedList<ArtistDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDeletedArtistsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ArtistDto>> Handle(
        GetDeletedArtistsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        PaginatedList<ArtistDto> result;

        IQueryable<Artist> artists = _context.Artists
            .AsNoTracking();

        if (string.IsNullOrEmpty(request.SearchTerm))
        {
            result = await artists
                .Where(a => a.IsDeleted)
                .OrderBy(a => a.FirstName)
                .ProjectTo<ArtistDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
        else
        {
            string search = request.SearchTerm.NormalizeStringForUrl();
            string pattern = $"%{search}%";

            result = await artists
                .Where(a =>
                    a.IsDeleted && (
                        EF.Functions.Like(a.FirstName, pattern) ||
                        EF.Functions.Like(a.LastName, pattern) ||
                        a.Slugs.Any(y => EF.Functions.Like(y.Name, pattern))))
                .OrderBy(a => a.FirstName)
                .ProjectTo<ArtistDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
        
        result.SearchTerm = request.SearchTerm;

        return result;
    }
}