using AutoMapper;
using bejebeje.admin.Application.Common.Enums;
using bejebeje.admin.Application.Common.Extensions;
using bejebeje.admin.Application.Common.Helpers;
using bejebeje.admin.Application.Common.Mappings;
using bejebeje.admin.Domain.Entities;

namespace bejebeje.admin.Application.Artists.Queries.GetArtists;

public class ArtistDto : IMapFrom<Artist>
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string ImageUrl { get; set; }

    public string ImageAlternateText { get; set; }

    public bool IsApproved { get; set; }

    public bool IsDeleted { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Artist, ArtistDto>()
            .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.FirstName.ToTitleCase()))
            .ForMember(x => x.LastName, opt =>
            {
                opt.Condition(x => !string.IsNullOrEmpty(x.LastName));
                opt.MapFrom(x => x.LastName.ToTitleCase());
            })
            .ForMember(x => x.ImageUrl, opt => opt.MapFrom(s => ImageUrlBuilder.BuildImageUrl(s.HasImage, s.Id, ImageSize.Small)))
            .ForMember(x => x.ImageAlternateText, opt => opt.MapFrom( s => ImageUrlBuilder.GetImageAlternateText(s.HasImage, s.FullName)));
    }
}