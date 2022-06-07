namespace bejebeje.admin.Application.Artists.Queries.CreateArtist;

public class CreateArtistGenderDto
{
    public Gender SelectedGender { get; set; }
    
    public List<SelectListItem> GenderOptions = new List<SelectListItem>
    {
        new SelectList(Enum.GetValues(typeof(Gender))
    };
}

public enum Gender
{
    Female,
    Male,
    NotApplicable
}