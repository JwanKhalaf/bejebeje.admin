using bejebeje.admin.Application.Artists.Queries.GetArtists;

namespace bejebeje.admin.WebUI.ViewModels.Artists.Queries.GetArtists;

public class ArtistsViewModel
{
    public IList<ArtistDto> Artists { get; set; } = new List<ArtistDto>();
}
