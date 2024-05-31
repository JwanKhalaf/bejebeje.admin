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
public class GetUnapprovedLyricsWithPaginationQuery : IRequest<PaginatedList<LyricDto>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string SearchTerm { get; set; } = string.Empty;
}

public class
    GetUnapprovedLyricsWithPaginationQueryHandler : IRequestHandler<GetUnapprovedLyricsWithPaginationQuery,
    PaginatedList<LyricDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUnapprovedLyricsWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<LyricDto>> Handle(
        GetUnapprovedLyricsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        PaginatedList<LyricDto> result;

        IQueryable<Lyric> lyrics = _context.Lyrics
            .AsNoTracking();

        if (string.IsNullOrEmpty(request.SearchTerm))
        {
            result = await lyrics
                .AsNoTracking()
                .Where(l => !l.IsApproved && !l.IsDeleted)
                .OrderBy(l => l.Title)
                .ProjectTo<LyricDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
        else
        {
            string search = request.SearchTerm.ToLowerInvariant();
            string pattern = $"%{search}%";

            result = await lyrics
                .Where(l => (!l.IsApproved && !l.IsDeleted) && (
                    EF.Functions.Like(l.Title, pattern) ||
                    l.Slugs.Any(y => EF.Functions.Like(y.Name, pattern))))
                .OrderBy(l => l.Title)
                .ProjectTo<LyricDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }

        return result;
    }
}