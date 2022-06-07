using MediatR;

namespace bejebeje.admin.Application.Artists.Queries.CreateArtist;

public class CreateArtistQuery : IRequest<CreateArtistGenderDto>
{
}

public class CreateArtistQueryHandler : IRequestHandler<CreateArtistQuery, CreateArtistGenderDto>
{
    public async Task<CreateArtistGenderDto> Handle(
        CreateArtistQuery request,
        CancellationToken cancellationToken)
    {
        CreateArtistGenderDto model = new CreateArtistGenderDto();

        return model;
    }
}