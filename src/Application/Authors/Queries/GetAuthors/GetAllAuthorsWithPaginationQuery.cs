using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Extensions;
using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Application.Common.Models;
using bejebeje.admin.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Authors.Queries.GetAuthors;

[BindProperties]
public class GetAllAuthorsWithPaginationQuery : IRequest<PaginatedList<AuthorDto>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string SearchTerm { get; set; } = string.Empty;
}

public class GetAllAuthorsHandler : IRequestHandler<GetAllAuthorsWithPaginationQuery, PaginatedList<AuthorDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAuthorsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AuthorDto>> Handle(
        GetAllAuthorsWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        PaginatedList<AuthorDto> result;

        IQueryable<Author> authors = _context.Authors
            .AsNoTracking();

        if (string.IsNullOrEmpty(request.SearchTerm))
        {
            result = await authors
                .OrderBy(a => a.FirstName)
                .ProjectTo<AuthorDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
        else
        {
            string search = request.SearchTerm.NormalizeStringForUrl();
            string pattern = $"%{search}%";

            result = await authors
                .Where(a =>
                    EF.Functions.Like(a.FirstName, pattern) ||
                    EF.Functions.Like(a.LastName, pattern) ||
                    a.Slugs.Any(y => EF.Functions.Like(y.Name, pattern)))
                .OrderBy(a => a.FirstName)
                .ProjectTo<AuthorDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }

        result.SearchTerm = request.SearchTerm;

        return result;
    }
}