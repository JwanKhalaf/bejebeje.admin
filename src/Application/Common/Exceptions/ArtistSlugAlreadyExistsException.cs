namespace bejebeje.admin.Application.Common.Exceptions;

public class ArtistSlugAlreadyExistsException : Exception
{
    public ArtistSlugAlreadyExistsException(string slugName, int artistId)
        : base($"An artist slug with the name '{slugName}' already exists for artist with Id: {artistId}.")
    {
    }
}
