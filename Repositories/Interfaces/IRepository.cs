using Core.Models;

namespace Repositories.Interfaces;

public interface IRepository<TModel> where TModel : IModel
{
    Task<List<TModel>> GetAllAsync();

    Task<TModel?> GetByIdAsync(string id);

    Task<string> SaveAsync(TModel model);

    Task UpdateAsync(TModel model);

    Task DeleteAsync(string id);
}