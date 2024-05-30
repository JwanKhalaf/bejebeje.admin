using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Lyrics.Commands.UpdateLyric;

public class UpdateLyricViewModel : IMapFrom<Lyric>
{
    public int LyricId { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Lyric, UpdateLyricViewModel>()
            .ForMember(x => x.LyricId, opt => opt.MapFrom(x => x.Id));
    }
}

public class UpdateLyricQuery : IRequest<UpdateLyricViewModel>
{
    public int LyricId { get; set; }
}

public class UpdateLyricQueryHandler : IRequestHandler<UpdateLyricQuery, UpdateLyricViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateLyricQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UpdateLyricViewModel> Handle(UpdateLyricQuery query, CancellationToken cancellationToken)
    {
        UpdateLyricViewModel updateLyricViewModel = await _context.Lyrics
            .Where(l => l.Id == query.LyricId)
            .ProjectTo<UpdateLyricViewModel>(_mapper.ConfigurationProvider)
            .SingleAsync(cancellationToken);

        return updateLyricViewModel;
    }
}