using System.ComponentModel.DataAnnotations;

namespace bejebeje.admin.WebUI.ViewModels;

public class CreateArtistNameViewModel
{
    [Required]
    public string FirstName { get; set; }

    public string LastName { get; set; }
}