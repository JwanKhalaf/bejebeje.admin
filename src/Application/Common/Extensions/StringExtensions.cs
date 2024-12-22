using System.Globalization;
using System.Text;

namespace bejebeje.admin.Application.Common.Extensions;

public static class StringExtensions
{
    private static readonly TextInfo _textInfo = new CultureInfo("ku-TR", false).TextInfo;
    
    public static string Standardize(this string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            return input.Trim().ToLowerInvariant();
        }

        return string.Empty;
    }
    
    public static string NormalizeStringForUrl(this string name)
    {
        string lowerCaseName = name.Trim().ToLowerInvariant();
        string normalizedString = lowerCaseName.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();

        foreach (char c in normalizedString)
        {
            switch (CharUnicodeInfo.GetUnicodeCategory(c))
            {
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.DecimalDigitNumber:
                    stringBuilder.Append(c);
                    break;
                case UnicodeCategory.SpaceSeparator:
                case UnicodeCategory.ConnectorPunctuation:
                case UnicodeCategory.DashPunctuation:
                    stringBuilder.Append('_');
                    break;
            }
        }
        string result = stringBuilder.ToString();
        return string.Join("-", result.Split(new char[] { '_' }
            , StringSplitOptions.RemoveEmptyEntries));
    }

    public static string ToTitleCase(this string value)
    {
        return string.IsNullOrEmpty(value) ? value : _textInfo.ToTitleCase(value);
    }

    public static string ToSentenceCase(this string name)
    {
        string value;

        if (name.Length == 0)
        {
            value = name;
        }
        else if (name.Length == 1)
        {
            value = Convert.ToString(char.ToUpper(name[0]));
        }
        else
        {
            value = char.ToUpper(name[0]) + name.Substring(1);
        }

        return value;
    }
}