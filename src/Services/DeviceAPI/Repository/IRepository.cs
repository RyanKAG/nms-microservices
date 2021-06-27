using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DeviceAPI.Dtos;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DeviceAPI.Repository
{
    public interface IRepository<TModel>
    where TModel: class
    {
        Task<IQueryable<TModel>> Find(Expression<Func<TModel, bool>> expression);
        Task<TModel> GetByIdAsync(Guid id);
        Task<IEnumerable<TModel>> GetAllAsync();

        Task<bool> SaveChangesAsync();

        Task<Response<TModel>> CreateAsync(TModel model);

        void UpdateAsync(TModel model);
        void DeleteAsync(TModel model);

        Task<int> GetCountAsync();
    }
}