using Domain.BaseModels;

namespace Application.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
    }
}