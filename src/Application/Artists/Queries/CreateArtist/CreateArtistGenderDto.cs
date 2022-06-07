namespace bejebeje.admin.Application.Artists.Queries.CreateArtist;

public class CreateArtistGenderDto
{
    public Gender SelectedGender { get; set; }
}

public enum Gender
{
    Female,
    Male,
    NotApplicable
}