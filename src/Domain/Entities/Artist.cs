namespace bejebeje.admin.Domain.Entities;

public class Artist : AuditableEntity
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string FullName { get; set; }

    public IList<ArtistSlug> Slugs { get; set; } = new List<ArtistSlug>();

    public bool IsApproved { get; set; }

    public string UserId { get; set; }

    public bool IsDeleted { get; set; }

    public bool HasImage { get; set; }

    public char Sex { get; set; }

    public IList<Lyric> Lyrics { get; private set; } = new List<Lyric>();
}
