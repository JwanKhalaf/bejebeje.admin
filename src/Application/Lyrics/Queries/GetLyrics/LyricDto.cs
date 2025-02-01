using AutoMapper;
using bejebeje.admin.Application.Common.Enums;
using bejebeje.admin.Application.Common.Extensions;
using bejebeje.admin.Application.Common.Helpers;
using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Domain.Entities;

namespace bejebeje.admin.Application.Lyrics.Queries.GetLyrics;

public class LyricDto : IMapFrom<Lyric>
{
    public int Id { get; set; }

    public string Title { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsApproved { get; set; }
    
    public bool IsVerified { get; set; }

    public string ArtistName { get; set; }
    
    public string ArtistImageUrl { get; set; }

    public string ArtistImageAlternateText { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime? ModifiedAt { get; set; }
    
    public string YouTubeLink { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Lyric, LyricDto>()
            .ForMember(x => x.ArtistName, opt => opt.MapFrom(x => x.Artist.FullName.ToTitleCase()))
            .ForMember(x => x.ArtistImageUrl, opt => opt.MapFrom(s => ImageUrlBuilder.BuildImageUrl(s.Artist.HasImage, s.Artist.Id, ImageSize.Small)))
            .ForMember(x => x.ArtistImageAlternateText, opt => opt.MapFrom( s => ImageUrlBuilder.GetImageAlternateText(s.Artist.HasImage, s.Artist.FullName)));
    }
}
