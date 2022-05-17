namespace bejebeje.admin.Domain.Entities;

public class LyricSlug : AuditableEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsDeleted { get; set; }

    public int LyricId { get; set; }

    public Lyric Lyric { get; set; } = null!;
}