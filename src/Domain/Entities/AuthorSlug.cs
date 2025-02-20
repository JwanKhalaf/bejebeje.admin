using System.ComponentModel.DataAnnotations.Schema;

namespace bejebeje.admin.Domain.Entities;

[Table("author_slugs")]
public class AuthorSlug : AuditableEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsDeleted { get; set; }

    public int AuthorId { get; set; }
    
    public Author Author { get; set; } = null!;
}