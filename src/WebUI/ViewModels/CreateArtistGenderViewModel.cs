using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bejebeje.admin.WebUI.ViewModels;

public class CreateArtistGenderViewModel
{
    [Display(Name = "Sex")] public string SelectedGender { get; set; }

    public List<SelectListItem> GenderOptions = new List<SelectListItem>
    {
        new SelectListItem { Value = "m", Text = "Male" }, new SelectListItem { Value = "f", Text = "Female" },
    };
}
