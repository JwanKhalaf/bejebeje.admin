using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Artists.Queries.GetArtist;
using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Artists.Commands.UpdateArtist;

// get
public class UpdateArtistQuery : IRequest<UpdateArtistQueryViewModel>
{
    public int ArtistId { get; set; }
}

public class UpdateArtistQueryViewModel
{
    public int Id { get; set; }

    [Display(Name = "First name")] public string FirstName { get; set; }

    [Display(Name = "Last name")] public string LastName { get; set; }

    [Display(Name = "Sex")] public string? Sex { get; set; }
}

public class UpdateArtistQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<UpdateArtistQuery, UpdateArtistQueryViewModel>
{
    public async Task<UpdateArtistQueryViewModel> Handle(UpdateArtistQuery query, CancellationToken cancellationToken)
    {
        ArtistDto result = await context.Artists
            .AsNoTracking()
            .Where(a => a.Id == query.ArtistId)
            .ProjectTo<ArtistDto>(mapper.ConfigurationProvider)
            .SingleAsync(cancellationToken: cancellationToken);

        UpdateArtistQueryViewModel viewModel = new UpdateArtistQueryViewModel
        {
            Id = result.Id, FirstName = result.FirstName, LastName = result.LastName, Sex = result.Sex?.ToString()
        };

        return viewModel;
    }
}

// post
public class UpdateArtistCommand : IRequest
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string? LastName { get; set; }

    [RegularExpression("^(m|f)?$", ErrorMessage = "Invalid value for Sex.")]
    public string? Sex { get; set; }
}

public class UpdateArtistCommandHandler : IRequestHandler<UpdateArtistCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateArtistCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateArtistCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Artists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Artist), request.Id);
        }

        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.FullName = string.IsNullOrEmpty(request.FirstName)
            ? request.FirstName
            : $"{request.FirstName} {request.LastName}";
        entity.Sex = string.IsNullOrEmpty(request.Sex) ? null : request.Sex[0];

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}