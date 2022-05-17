using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Domain.Entities;

namespace bejebeje.admin.Application.Artists.Queries.GetArtists;

public class ArtistDto : IMapFrom<Artist>
{
    public ArtistDto()
    {
        Items = new List<LyricDto>();
    }

    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Colour { get; set; }

    public IList<LyricDto> Items { get; set; }
}
