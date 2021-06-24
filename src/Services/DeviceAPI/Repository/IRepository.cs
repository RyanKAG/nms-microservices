using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DeviceAPI.Dtos;

namespace DeviceAPI.Repository
{
    public interface IRepository<TModel>
    {
        Task<IQueryable<TModel>> Find(Expression<Func<TModel, bool>> expression);
        Task<TModel> GetByIdAsync(int id);
        Task<IEnumerable<TModel>> GetAllAsync();

        Task<bool> SaveChangesAsync();

        Task<Response<TModel>> CreateAsync(TModel model);

        Task<Response<TModel>> UpdateAsync(TModel model);
        Task<Response<TModel>> DeleteAsync(TModel model);

        Task<int> GetCountAsync();
    }
}