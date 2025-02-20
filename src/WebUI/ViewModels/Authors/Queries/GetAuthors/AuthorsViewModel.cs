using bejebeje.admin.Application.Artists.Queries.GetArtists;
using bejebeje.admin.Application.Authors.Queries.GetAuthors;

namespace bejebeje.admin.WebUI.ViewModels.Authors.Queries.GetAuthors;

public class AuthorsViewModel
{
    public IList<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
}