using System;
using System.Text.Json.Serialization;
using Template.Application.ViewModels.Address;

namespace Template.Application.ViewModels.Customer;

public class CustomerViewModel
{
    [JsonConstructor]
    public CustomerViewModel(int id, int addressId, string name)
    {
        Id = id;
        AddressId = addressId;
        Name = name;
    }

    public int Id { get; set; }
    public int AddressId { get; set; }
    public string Name { get; set; }
    public DateTime DateCreated { get; set; }
    public AddressViewModel Address { get; set; }
    
    
}
