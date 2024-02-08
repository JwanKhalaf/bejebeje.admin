using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Queries.GetLyricDetail;

[BindProperties]
public class GetLyricDetailQuery : IRequest<GetLyricDetailDto>
{
    public int LyricId { get; set; }
}

public class GetLyricDetailQueryHandler : IRequestHandler<GetLyricDetailQuery, GetLyricDetailDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLyricDetailQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetLyricDetailDto> Handle(
        GetLyricDetailQuery request,
        CancellationToken cancellationToken)
    {
        GetLyricDetailDto result = await _context.Lyrics
            .AsNoTracking()
            .Include(l => l.Artist)
            .Where(a => a.Id == request.LyricId)
            .ProjectTo<GetLyricDetailDto>(_mapper.ConfigurationProvider)
            .SingleAsync(cancellationToken: cancellationToken);

        return result;
    }
}