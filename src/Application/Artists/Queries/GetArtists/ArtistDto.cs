using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Domain.Entities;

namespace bejebeje.admin.Application.Artists.Queries.GetArtists;

public class ArtistDto : IMapFrom<Artist>
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public bool IsApproved { get; set; }

    public bool IsDeleted { get; set; }
}
