namespace bejebeje.admin.Domain.Entities;

public class ArtistSlug : AuditableEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsDeleted { get; set; }

    public int ArtistId { get; set; }
    
    public Artist Artist { get; set; } = null!;
}