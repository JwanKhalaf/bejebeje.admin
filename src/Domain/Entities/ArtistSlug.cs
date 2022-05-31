using System.ComponentModel.DataAnnotations.Schema;

namespace bejebeje.admin.Domain.Entities;

[Table("artist_slugs")]
public class ArtistSlug : AuditableEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsDeleted { get; set; }

    public int ArtistId { get; set; }
    
    public Artist Artist { get; set; } = null!;
}