using System.ComponentModel.DataAnnotations.Schema;

namespace bejebeje.admin.Domain.Entities;

public class Lyric : AuditableEntity
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }

    public string UserId { get; set; }

    public IList<LyricSlug> Slugs { get; set; } = new List<LyricSlug>();
    
    public bool IsVerified { get; set; }
    
    public DateTime? VerifiedAt { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsApproved { get; set; }

    public int ArtistId { get; set; }

    [Column("youtube_link")]
    public string? YouTubeLink { get; set; }

    public Artist Artist { get; set; } = null!;
}
