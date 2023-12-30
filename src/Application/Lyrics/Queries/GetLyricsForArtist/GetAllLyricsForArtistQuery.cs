using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Queries.GetLyricsForArtist;

[BindProperties]
public class GetAllLyricsForArtistQuery : IRequest<GetLyricsForArtistDto>
{
    public int ArtistId { get; set; }
}

public class GetAllLyricsForArtistQueryHandler : IRequestHandler<GetAllLyricsForArtistQuery, GetLyricsForArtistDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllLyricsForArtistQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetLyricsForArtistDto> Handle(
        GetAllLyricsForArtistQuery request,
        CancellationToken cancellationToken)
    {
        GetLyricsForArtistDto result = new GetLyricsForArtistDto();

        ArtistDto artistDto = await _context.Artists
            .AsNoTracking()
            .Where(a => a.Id == request.ArtistId)
            .ProjectTo<ArtistDto>(_mapper.ConfigurationProvider)
            .SingleAsync(cancellationToken: cancellationToken);

        result.Artist = artistDto;
        
         List<LyricDto> lyrics = await _context.Lyrics
            .AsNoTracking()
            .Where(l => l.ArtistId == request.ArtistId)
            .OrderBy(l => l.Title)
            .ProjectTo<LyricDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);;

         result.Lyrics = lyrics;

        return result;
    }
}