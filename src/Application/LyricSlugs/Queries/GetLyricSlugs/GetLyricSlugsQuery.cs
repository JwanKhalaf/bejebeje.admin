using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.LyricSlugs.Queries.GetLyricSlugs;

public class GetLyricSlugsQuery : IRequest<GetLyricSlugsQueryViewModel>
{
    public int LyricId { get; set; }
}

public class GetLyricSlugsQueryViewModel
{
    public int LyricId { get; set; }
    
    public List<GetLyricSlugsQueryViewModelItem> LyricSlugs { get; set; }
}

public class GetLyricSlugsQueryViewModelItem : IMapFrom<LyricSlug>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsDeleted { get; set; }
}

public class GetLyricSlugsQueryHandler : IRequestHandler<GetLyricSlugsQuery, GetLyricSlugsQueryViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLyricSlugsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetLyricSlugsQueryViewModel> Handle(
        GetLyricSlugsQuery request,
        CancellationToken cancellationToken)
    {
        List<GetLyricSlugsQueryViewModelItem> slugs = await _context
            .LyricSlugs
            .AsNoTracking()
            .Where(x => x.LyricId == request.LyricId)
            .ProjectTo<GetLyricSlugsQueryViewModelItem>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        GetLyricSlugsQueryViewModel result =
            new GetLyricSlugsQueryViewModel { LyricId = request.LyricId, LyricSlugs = slugs };

        return result;
    }
}