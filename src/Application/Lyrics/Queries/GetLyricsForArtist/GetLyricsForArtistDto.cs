using AutoMapper;
using bejebeje.admin.Application.Common.Enums;
using bejebeje.admin.Application.Common.Extensions;
using bejebeje.admin.Application.Common.Helpers;
using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Domain.Entities;

namespace bejebeje.admin.Application.Lyrics.Queries.GetLyricsForArtist;

public class GetLyricsForArtistDto
{
    public ArtistDto Artist { get; set; }
    
    public List<LyricDto> Lyrics { get; set; }
}

public class ArtistDto : IMapFrom<Artist>
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string ImageUrl { get; set; }

    public string ImageAlternateText { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Artist, ArtistDto>()
            .ForMember(a => a.Name, opt => opt.MapFrom(x => x.FullName.ToTitleCase()))
            .ForMember(a => a.ImageUrl, opt => opt.MapFrom(s => ImageUrlBuilder.BuildArtistImageUrl(s.HasImage, s.Id, ImageSize.Standard)))
            .ForMember(a => a.ImageAlternateText, opt => opt.MapFrom( s => ImageUrlBuilder.GetArtistImageAlternateText(s.HasImage, s.FullName)));
    }
}

public class LyricDto : IMapFrom<Lyric>
{
    public int Id { get; set; }

    public string Title { get; set; }

    public bool IsApproved { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public bool IsVerified { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime? ModifiedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Lyric, LyricDto>();
    }
}