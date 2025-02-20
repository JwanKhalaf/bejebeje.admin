using AutoMapper;
using AutoMapper.QueryableExtensions;
using bejebeje.admin.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bejebeje.admin.Application.Authors.Queries.GetAuthor;

[BindProperties]
public class GetAuthorQuery(int authorId) : IRequest<AuthorDto>
{
    public int AuthorId { get; } = authorId;
}

public class GetAuthorQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetAuthorQuery, AuthorDto>
{
    public async Task<AuthorDto> Handle(
        GetAuthorQuery request,
        CancellationToken cancellationToken)
    {
        AuthorDto result = await context.Authors
            .AsNoTracking()
            .Include(a => a.Lyrics)
            .Where(a => a.Id == request.AuthorId)
            .ProjectTo<AuthorDto>(mapper.ConfigurationProvider)
            .SingleAsync(cancellationToken: cancellationToken);

        return result;
    }
}