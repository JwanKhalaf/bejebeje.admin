namespace bejebeje.admin.Domain.Exceptions;

public class ArtistNotFoundException : Exception
{
    public ArtistNotFoundException(string artistName)
        : base($"The artist \"{artistName}\" was not found.")
    {
    }
}
