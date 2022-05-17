namespace bejebeje.admin.Application.Common.Extensions;

public static class BooleanExtensions
{
    public static string ToYesOrNo(this bool value)
    {
        return value ? "Yes" : "No";
    }
}