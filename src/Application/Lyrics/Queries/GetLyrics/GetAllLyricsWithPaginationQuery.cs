using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Application.Common.Models;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Queries.GetLyrics;

[BindProperties]
public class GetAllLyricsWithPaginationQuery : IRequest<PaginatedList<LyricDto>>
{
    public int PageNumber { get; set; } = 1;
    
    public int PageSize { get; set; } = 10;
    
    public string SearchTerm { get; set; } = string.Empty;
}

public class GetAllLyricsWithPaginationQueryHandler : IRequestHandler<GetAllLyricsWithPaginationQuery, PaginatedList<LyricDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllLyricsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<LyricDto>> Handle(GetAllLyricsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        PaginatedList<LyricDto> result;

        IQueryable<Lyric> lyrics = _context.Lyrics
            .AsNoTracking();

        if (string.IsNullOrEmpty(request.SearchTerm))
        {
            result = await lyrics
                .OrderBy(t => t.Title)
                .ProjectTo<LyricDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
        else
        {
            string search = request.SearchTerm.ToLowerInvariant();
            string pattern = $"%{search}%";
            
            result = await lyrics
                .Where(x => 
                    EF.Functions.Like(x.Title, pattern) ||
                    x.Slugs.Any(y => EF.Functions.Like(y.Name, pattern)))
                .OrderBy(t => t.Title)
                .ProjectTo<LyricDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }

        return result;
    }
}
