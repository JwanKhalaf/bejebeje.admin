using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Artists.Queries.GetArtist;

[BindProperties]
public class GetArtistQuery(int artistId) : IRequest<ArtistDto>
{
    public int ArtistId { get; } = artistId;
}

public class GetArtistQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetArtistQuery, ArtistDto>
{
    public async Task<ArtistDto> Handle(
        GetArtistQuery request,
        CancellationToken cancellationToken)
    {
        ArtistDto result = await context.Artists
            .AsNoTracking()
            .Include(a => a.Lyrics)
            .Where(a => a.Id == request.ArtistId)
            .ProjectTo<ArtistDto>(mapper.ConfigurationProvider)
            .SingleAsync(cancellationToken: cancellationToken);

        return result;
    }
}