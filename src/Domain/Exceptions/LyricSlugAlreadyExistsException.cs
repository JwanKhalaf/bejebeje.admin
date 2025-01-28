namespace bejebeje.admin.Domain.Exceptions;

public class LyricSlugAlreadyExistsException : Exception
{
    public LyricSlugAlreadyExistsException(string slugName, int lyricId)
        : base($"A lyric slug with the name '{slugName}' already exists for lyric with Id: {lyricId}.")
    {
    }
}