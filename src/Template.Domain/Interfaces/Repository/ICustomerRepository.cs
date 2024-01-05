using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Domain.Models;
using Template.Domain.Models.Dapper;

namespace Template.Domain.Interfaces.Repository;

public interface ICustomerRepository : IEntityBaseRepository<Customer>, IDapperReadRepository<Customer>
{
    Task<IEnumerable<CustomerAddress>> GetAllAsync();
    Task<CustomerAddress> GetAddressByIdAsync(int id);
    Task<CustomerAddress> GetByNameAsync(string name);
}
