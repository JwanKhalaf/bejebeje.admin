namespace bejebeje.admin.Domain.Entities;

public class Lyric : AuditableEntity
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }

    public string UserId { get; set; }

    public IEnumerable<LyricSlug> Slugs { get; set; } = new List<LyricSlug>();
    
    public bool IsVerified { get; set; }
    
    public DateTime? VerifiedAt { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsApproved { get; set; }

    public int ArtistId { get; set; }

    public Artist Artist { get; set; } = null!;
}
