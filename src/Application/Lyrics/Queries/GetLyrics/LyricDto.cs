using AutoMapper;
using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Domain.Entities;

namespace bejebeje.admin.Application.Lyrics.Queries.GetLyrics;

public class LyricDto : IMapFrom<Lyric>
{
    public int Id { get; set; }

    public string Title { get; set; }

    public bool IsApproved { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime? ModifiedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Lyric, LyricDto>();
    }
}
