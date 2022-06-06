using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bejebeje.admin.WebUI.ViewModels;

public class SexViewModel
{
    [Display(Name = "Sex")] public string SelectedSex { get; set; }

    public List<SelectListItem> SexOptions = new List<SelectListItem>
    {
        new SelectListItem { Value = "m", Text = "Male" }, new SelectListItem { Value = "f", Text = "Female" },
    };
}
