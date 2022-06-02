using System.ComponentModel.DataAnnotations.Schema;

namespace bejebeje.admin.Domain.Entities;

[Table("lyric_slugs")]
public class LyricSlug : AuditableEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsDeleted { get; set; }

    public int LyricId { get; set; }

    public Lyric Lyric { get; set; } = null!;
}