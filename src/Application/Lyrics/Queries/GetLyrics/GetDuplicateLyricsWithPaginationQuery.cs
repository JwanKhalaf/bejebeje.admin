using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Queries.GetLyrics;

public class GetDuplicateLyricsWithPaginationQuery : IRequest<PaginatedList<LyricDto>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string SearchTerm { get; set; } = string.Empty;
}

public class
    GetDuplicateLyricsWithPaginationQueryHandler : IRequestHandler<GetDuplicateLyricsWithPaginationQuery,
    PaginatedList<LyricDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDuplicateLyricsWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<LyricDto>> Handle(
        GetDuplicateLyricsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        PaginatedList<LyricDto> lyrics = await _context
            .Lyrics
            .FromSqlRaw(
                $"SELECT t.id, t.title, t.is_deleted, t.is_approved, t.is_verified, a.full_name, a.has_image, a.id as artist_id, t.created_at, t.modified_at FROM (SELECT l1.id, l1.artist_id, l1.created_at, l1.is_approved, l1.is_deleted, l1.is_verified, l1.modified_at, l1.title FROM lyrics l1 INNER JOIN lyrics l2 ON l1.id < l2.id AND l1.artist_id = l2.artist_id WHERE similarity(lower(l1.title), lower(l2.title)) >= 0.6 AND l1.artist_id = l2.artist_id AND l1.is_deleted = false AND l2.is_deleted = false ORDER BY l1.title) AS t INNER JOIN artists AS a ON t.artist_id = a.id ORDER BY t.title")
            .ProjectTo<LyricDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return lyrics;
    }
}