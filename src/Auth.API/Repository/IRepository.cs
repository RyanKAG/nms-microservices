using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Auth.API.Dtos;

namespace Auth.API.Repository
{
    public interface IRepository<TModel>
    where TModel: class
    {
        Task<IQueryable<TModel>> Find(Expression<Func<TModel, bool>> expression);
        Task<TModel> GetByIdAsync(Guid id);
        Task<IEnumerable<TModel>> GetByListOfIdsAsync(IEnumerable<Guid> ids);
        Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken);

        Task<bool> SaveChangesAsync();

        Task<Response<TModel>> CreateAsync(TModel model);

        void UpdateAsync(TModel model);
        void Delete(TModel model);

        Task<int> GetCountAsync();
    }
}