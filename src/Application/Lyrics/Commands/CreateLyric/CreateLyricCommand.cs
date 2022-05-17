using bejebeje.admin.Application.Common.Interfaces;
using bejebeje.admin.Domain.Entities;
using MediatR;

namespace bejebeje.admin.Application.Lyrics.Commands.CreateLyric;

public class CreateLyricCommand : IRequest<int>
{
    public string Title { get; set; }

    public string Body { get; set; }

    public string UserId { get; set; }

    public int ArtistId { get; set; }
    
    public bool IsVerified { get; set; }
}

public class CreateLyricCommandHandler : IRequestHandler<CreateLyricCommand, int>
{
    private readonly ICurrentUserService _currentUserService;
    
    private readonly IApplicationDbContext _context;

    public CreateLyricCommandHandler(ICurrentUserService userService, IApplicationDbContext context)
    {
        _currentUserService = userService;
        _context = context;
    }

    public async Task<int> Handle(CreateLyricCommand request, CancellationToken cancellationToken)
    {
        var entity = new Lyric
        {
            Title = request.Title,
            Body = request.Body,
            UserId = _currentUserService.UserId,
            ArtistId = request.ArtistId,
            IsVerified = request.IsVerified
        };

        _context.Lyrics.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
