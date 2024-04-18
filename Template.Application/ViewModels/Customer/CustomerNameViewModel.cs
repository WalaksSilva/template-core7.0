using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Template.Application.ViewModels.Customer;

public class CustomerNameViewModel
{
    public CustomerNameViewModel()
    {

    }
    public CustomerNameViewModel(string name)
    {
        Name = name;
    }

    [FromRoute(Name = "name")]
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string Name { get; set; }
}
