using AutoMapper;
using bejebeje.admin.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Dashboard.Queries;

public class GetDashboardQuery : IRequest<DashboardViewModel>
{
}

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDashboardQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<DashboardViewModel> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        DashboardViewModel viewModel = new DashboardViewModel();

        int numberOfArtists = await _context.Artists.AsNoTracking().Where(x => x.IsApproved && !x.IsDeleted).CountAsync();

        int numberOfLyrics = await _context.Lyrics.AsNoTracking().Where(x => !x.IsDeleted && x.IsApproved).CountAsync();
        

        viewModel.NumberOfArtists = numberOfArtists;
        viewModel.NumberOfLyrics = numberOfLyrics;

        return viewModel;
    }
}