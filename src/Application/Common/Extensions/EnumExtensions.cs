using bejebeje.admin.Application.Common.Enums;

namespace bejebeje.admin.Application.Common.Extensions;

public static class EnumExtensions
{
    private const string ExtraSmall = "xsm";

    private const string Small = "sm";

    private const string Standard = "s";

    public static string GetCorrespondingFolder(this ImageSize imageSize)
    {
        return imageSize switch
        {
            ImageSize.ExtraSmall => ExtraSmall,
            ImageSize.Small => Small,
            _ => Standard
        };
    }
}