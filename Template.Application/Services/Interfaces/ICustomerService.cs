using Template.Application.ViewModels.Customer;

namespace Template.Application.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerAddressViewModel>> GetAllAsync();
    Task<CustomerViewModel> GetByIdAsync(CustomerIdViewModel customerVM);
    Task<CustomerAddressViewModel> GetAddressByIdAsync(CustomerIdViewModel customerVM);
    Task<CustomerAddressViewModel> GetAddressByNameAsync(CustomerNameViewModel customerVM);
    Task RemoveAsync(CustomerViewModel customerVM);
    Task UpdateAsync(CustomerViewModel customerVM);
    Task<CustomerViewModel> AddAsync(CustomerViewModel customerVM);
}
