using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using DeviceManagement.API.Dtos;

namespace DeviceManagement.API.Repository
{
    public interface IRepository<TModel>
    where TModel: class
    {
        Task<IQueryable<TModel>> Find(Expression<Func<TModel, bool>> expression);
        Task<TModel> GetByIdAsync(Guid id);
        Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken);

        Task<bool> SaveChangesAsync();

        Task<Response<TModel>> CreateAsync(TModel model);

        void UpdateAsync(TModel model);
        void DeleteAsync(TModel model);

        Task<int> GetCountAsync();
    }
}