using System.Threading.Tasks;
using Template.Domain.Models.Services;

namespace Template.Domain.Interfaces.Services;

public interface IViaCEPService
{
    Task<ViaCEP> GetByCEPAsync(string cep);
}
