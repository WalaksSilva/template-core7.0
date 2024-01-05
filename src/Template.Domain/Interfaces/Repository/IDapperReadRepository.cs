using System.Threading.Tasks;

namespace Template.Domain.Interfaces.Repository;

public interface IDapperReadRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
}
