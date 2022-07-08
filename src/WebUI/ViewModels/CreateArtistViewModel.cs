namespace bejebeje.admin.WebUI.ViewModels;

public class CreateArtistViewModel
{
    public int StepNumber { get; set; }
    
    public CreateArtistBandViewModel Band { get; set; }
    
    public CreateArtistNameViewModel Name { get; set; }
    
    public CreateArtistGenderViewModel Gender { get; set; }
}